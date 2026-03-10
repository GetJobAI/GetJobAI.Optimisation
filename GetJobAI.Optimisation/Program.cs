using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.OptimisationService;
using GetJobAI.Optimisation.Prompts;
using Google.GenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection(GeminiOptions.SectionName));

var geminiOptions = builder.Configuration
    .GetSection(GeminiOptions.SectionName)
    .Get<GeminiOptions>();

if (geminiOptions?.ApiKey is not null)
{
    builder.Services.AddSingleton(new Client(apiKey: geminiOptions.ApiKey));
}

builder.Services.AddSingleton<IPromptRegistry, PromptRegistry>();
builder.Services.AddScoped<IPromptRunner, PromptRunner>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();

