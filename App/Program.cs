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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddDbContext<FileDbContext>(options =>
{
    var fileDbConnectionString = builder.Configuration.GetConnectionString("FileDbConnection");
    options.UseNpgsql(fileDbConnectionString);
});

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
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


builder.Services.AddScoped<IRAGService, RAGService>();
builder.Services.AddScoped<ReRanker>();
builder.Services.AddScoped<ResponseProcessor>();
builder.Services.AddScoped(provider =>
{
    var systemPrompt =
        "You are a helpful ocean network canada assistant that interprets " +
        "the data given and answers the user prompt with accuracy.";
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
});

// Add Authorization
builder.Services.AddAuthorization();


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
    // Uncomment the line below to seed the dev admin user. For development purposes only.
    // await SeedDevAdmin.SeedAsync(services);
}

app.UseRouting();

app.UseAuthentication();
app.UseCors("AllowAllOrigins");
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