using System.Diagnostics;
using System.Text.Json;

namespace GetJobAI.PromptSandbox;

public static class FullFlowScenario
{
    private static readonly string[] ContextFiles =
    [
        "backend_engineer_weak.json",
        "career_changer.json",
        "senior_with_publications.json"
    ];

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static async Task RunAsync()
    {
        Console.WriteLine("Which context?");
        
        for (var i = 0; i < ContextFiles.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {ContextFiles[i]}");
        }
        
        Console.WriteLine($"  {ContextFiles.Length + 1}. All");

        var input = Console.ReadLine()?.Trim();
        var files = input == (ContextFiles.Length + 1).ToString()
            ? ContextFiles
            : int.TryParse(input, out var idx) && idx >= 1 && idx <= ContextFiles.Length
                ? [ContextFiles[idx - 1]]
                : throw new InvalidOperationException($"Invalid choice: {input}");

        var orchestrator = SandboxFactory.BuildOrchestrator();

        foreach (var contextFile in files)
        {
            Console.WriteLine($"\n--- Running full flow: {contextFile} ---");
            var ctx = TestDataLoader.LoadContext(contextFile);

            var sw = Stopwatch.StartNew();
            var suggestions = await orchestrator.RunAsync(ctx, CancellationToken.None);
            sw.Stop();

            var resultName = Path.GetFileNameWithoutExtension(contextFile);
            var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "results", $"full_flow_{resultName}.json");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(suggestions, JsonOptions));

            Console.WriteLine($"  Completed in {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"  Summary:               {(suggestions.Summary      is not null ? "yes" : "no")}");
            Console.WriteLine($"  Work experiences:      {suggestions.WorkExperience.Count}");
            Console.WriteLine($"  Skills:                {(suggestions.Skills        is not null ? "yes" : "no")}");
            Console.WriteLine($"  ATS explanation:       {(suggestions.AtsExplanation is not null ? "yes" : "no")}");
            Console.WriteLine($"  Publications assessed: {suggestions.Publications.Count}");
            Console.WriteLine($"  Activities assessed:   {suggestions.Activities.Count}");
            Console.WriteLine($"  Additional sections:   {suggestions.AdditionalSections.Count}");
            Console.WriteLine($"  Result saved → {filePath}");
        }
    }
}
