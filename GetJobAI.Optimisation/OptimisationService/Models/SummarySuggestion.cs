using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Models;

public class SummarySuggestion
{
    [JsonPropertyName("original")]
    public string Original { get; set; } = string.Empty;

    [JsonPropertyName("rewritten")]
    public string Rewritten { get; set; } = string.Empty;

    [JsonPropertyName("keywords_incorporated")]
    public List<string> KeywordsIncorporated { get; set; } = [];

    [JsonPropertyName("accepted")]
    public bool? Accepted { get; set; }

    [JsonPropertyName("rejection_hint")]
    public string? RejectionHint { get; set; }

    [JsonPropertyName("rewrite_count")]
    public int RewriteCount { get; set; } = 0;
}
