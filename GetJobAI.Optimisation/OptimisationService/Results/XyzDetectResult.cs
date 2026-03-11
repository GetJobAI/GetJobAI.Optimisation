using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class XyzDetectResult
{
    [JsonPropertyName("needs_xyz")]
    public bool NeedsXyz { get; set; }
    
    [JsonPropertyName("missing_component")]
    public string? MissingComponent { get; set; }

    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("reasoning")]
    public string Reasoning { get; set; } = string.Empty;
}