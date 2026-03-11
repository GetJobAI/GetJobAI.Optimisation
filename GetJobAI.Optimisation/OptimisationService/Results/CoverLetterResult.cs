using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class CoverLetterResult
{
    [JsonPropertyName("cover_letter")]
    public string CoverLetter { get; set; } = string.Empty;

    [JsonPropertyName("word_count")]
    public int WordCount { get; set; }

    [JsonPropertyName("salutation_used")]
    public string SalutationUsed { get; set; } = string.Empty;

    [JsonPropertyName("accepted")]
    public bool? Accepted { get; set; }

    [JsonPropertyName("rewrite_count")]
    public int RewriteCount { get; set; } = 0;
}