using System.Text.Json;
using GetJobAI.Optimisation.OptimisationService.Contexts;

namespace GetJobAI.PromptSandbox;

public static class TestDataLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static OptimisationContext LoadContext(string filename)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "TestData", "contexts", filename);
    
        return Load<OptimisationContext>(fullPath);
    }

    public static T LoadEntry<T>(string filename)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "TestData", "entries", filename);
    
        return Load<T>(fullPath);
    }

    private static T Load<T>(string relativePath)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException(
                $"Test data file not found: {fullPath}. " +
                $"Make sure it is set to 'Copy to Output Directory' in the project file.");

        var json = File.ReadAllText(fullPath);
        
        return JsonSerializer.Deserialize<T>(json, Options)
               ?? throw new InvalidOperationException($"Failed to deserialize {relativePath}");
    }
}