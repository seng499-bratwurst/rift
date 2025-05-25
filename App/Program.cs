using Microsoft.EntityFrameworkCore;
using NSwag;
using Rift;
using Rift.LLM;
using DotNetEnv;




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

builder.Services.AddControllers();

builder.Services.AddScoped<UserService>();

var llmProviderName = builder.Configuration["LLmSettings:Provider"];

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

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.Run();