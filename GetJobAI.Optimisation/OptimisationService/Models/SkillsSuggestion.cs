using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Models;

public class SkillsSuggestion
{
    [JsonPropertyName("keep")]
    public List<string> Keep { get; set; } = [];

    [JsonPropertyName("remove")]
    public List<string> Remove { get; set; } = [];

    [JsonPropertyName("add")]
    public List<string> Add { get; set; } = [];

    [JsonPropertyName("accepted")]
    public bool? Accepted { get; set; }

    [JsonPropertyName("rejection_hint")]
    public string? RejectionHint { get; set; }

    [JsonPropertyName("rewrite_count")]
    public int RewriteCount { get; set; } = 0;
}