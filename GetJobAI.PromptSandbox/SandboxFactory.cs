using GetJobAI.Optimisation.OptimisationService;
using GetJobAI.Optimisation.Prompts;
using Google.GenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GetJobAI.PromptSandbox;

public static class SandboxFactory
{
    private static PromptRunner BuildPromptRunner()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

        var apiKey = config["Gemini:ApiKey"]
                     ?? throw new InvalidOperationException("Gemini:ApiKey not set in appsettings.json");

        var client = new Client(apiKey: apiKey);

        var geminiOptions = Options.Create(
            config.GetSection("Gemini").Get<GeminiOptions>()
            ?? throw new InvalidOperationException("Gemini section missing from appsettings.json"));

        var registryLogger = loggerFactory.CreateLogger<PromptRegistry>();
        var registry = new PromptRegistry(registryLogger);

        return new PromptRunner(client, geminiOptions, registry);
    }

    public static OptimisationOrchestrator BuildOrchestrator()
    {
        return new OptimisationOrchestrator(BuildPromptRunner());
    }
}