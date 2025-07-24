using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NSwag;
using Microsoft.AspNetCore.Identity;
using Rift.LLM;
using Rift.Models;
using Rift.Services;
using Rift.Repositories;
using Rift.App.Clients;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiDocument(options =>
{
    options.PostProcess = document =>
    {
        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Rift API",
            Description = "The backend API for the ONC chatbot",
        };
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
    var dbConnStr = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

    if (string.IsNullOrEmpty(dbHost))
    {
        dbConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    options.UseNpgsql(dbConnStr);
});

builder.Services.AddDbContext<FileDbContext>(options =>
{
    var fileDbConnectionString = builder.Configuration.GetConnectionString("FileDbConnection");
    var fileDbHost = Environment.GetEnvironmentVariable("FILEDB_HOST");
    var fileDbPort = Environment.GetEnvironmentVariable("FILEDB_PORT");
    var fileDbUser = Environment.GetEnvironmentVariable("FILEDB_USER");
    var fileDbPassword = Environment.GetEnvironmentVariable("FILEDB_PASSWORD");
    var fileDbName = Environment.GetEnvironmentVariable("FILEDB_DB");
    var fileDbConnStr = $"Host={fileDbHost};Port={fileDbPort};Database={fileDbName};Username={fileDbUser};Password={fileDbPassword}";

    if (string.IsNullOrEmpty(fileDbHost))
    {
        fileDbConnStr = builder.Configuration.GetConnectionString("FileDbConnection");
    }

    options.UseNpgsql(fileDbConnStr);
});

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration["Jwt:Audience"] ?? "http://localhost:3000")
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddHttpClient<ChromaDBClient>();
builder.Services.AddHttpClient<ReRankerClient>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IMessageEdgeRepository, MessageEdgeRepository>();
builder.Services.AddScoped<IMessageEdgeService, MessageEdgeService>();

builder.Services.AddScoped<ICompanyTokenRepository, CompanyTokenRepository>();
builder.Services.AddScoped<ICompanyTokenService, CompanyTokenService>();

builder.Services.AddScoped<IMessageFilesRepository, MessageFilesRepository>();
builder.Services.AddScoped<IMessageFilesService, MessageFilesService>();

builder.Services.AddScoped<IFileMetricsService, FileMetricsService>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IGeminiTitleService, GeminiTitleService>();

builder.Services.AddScoped<IRAGService, RAGService>();
builder.Services.AddScoped<ReRankerClient>();
builder.Services.AddScoped<ResponseProcessor>();
builder.Services.AddScoped(provider =>
{
    var systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "sys_prompt_large_llm.md"));
    return new PromptBuilder(systemPrompt);
});

var llmProviderName = builder.Configuration["LLmSettings:Provider"];
builder.Services.AddHttpClient<OncAPI>(client =>
{
    client.BaseAddress = new Uri($"https://data.oceannetworks.ca/api/");
});
builder.Services.AddScoped<OncFunctionParser>();

switch (llmProviderName)
{
    case "TogetherAI":
        builder.Services.AddScoped<ILlmProvider, TogetherAI>();
        break;
    case "HuggingFace":
        builder.Services.AddScoped<ILlmProvider, HuggingFace>();
        break;
    case "GoogleGemma":
        builder.Services.AddScoped<ILlmProvider, GoogleGemma>();
        break;
    default:
        throw new Exception($"Unsupported LLM provider: {llmProviderName}");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JWT key is not configured.");
    }
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // If the Authorization header is missing, try to get token from cookie
            var token = context.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

// Add Authorization
builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PerOncApiToken", context =>
    {
        var httpContext = context as HttpContext;
        var scopedServices = httpContext?.RequestServices;

        string? oncApiToken = httpContext?.Request.Query["token"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(oncApiToken))
        {
            Console.WriteLine("ONCApiToken not found in JWT or headers/query.");
            return RateLimitPartition.GetNoLimiter("no-token");
        }

        var dbContext = scopedServices?.GetRequiredService<ApplicationDbContext>();
        bool tokenExists = dbContext?.CompanyAPITokens.Any(t => t.ONCApiToken == oncApiToken) ?? false;

        if (!tokenExists)
        {
            Console.WriteLine($"ONCApiToken '{oncApiToken}' not found in DB.");
            return RateLimitPartition.GetNoLimiter("not-limited");
        }

        Console.WriteLine($"Applying rate limiting for token: {oncApiToken}");

        return RateLimitPartition.GetTokenBucketLimiter(oncApiToken, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 10, // Maximum tokens allowed per period. keep this the same as TokensPerPeriod
            TokensPerPeriod = 10, // How many tokens are added per period
            ReplenishmentPeriod = TimeSpan.FromHours(1), // How often are tokens replenished
            AutoReplenishment = true, // Disable to manually control replenishment
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
        });

    });
});


var app = builder.Build();

// Automatically apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var appDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    appDb.Database.Migrate();

    var fileDb = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    fileDb.Database.Migrate();

    var services = scope.ServiceProvider;
    await SeedRoles.SeedRolesAndAdminAsync(services);
    var seedFiles = new SeedFiles(chromaDbClient: services.GetRequiredService<ChromaDBClient>());
    await seedFiles.SeedAsync(fileDb);
    // Uncomment the line below to seed the dev admin user. For development purposes only.
    // await SeedDevAdmin.SeedAsync(services);
}

app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.Run();
