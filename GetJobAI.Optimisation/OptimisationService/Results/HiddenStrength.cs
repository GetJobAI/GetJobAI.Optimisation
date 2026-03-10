using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class HiddenStrength
{
    [JsonPropertyName("skill")]
    public string Skill { get; set; } = string.Empty;

    [JsonPropertyName("relevance_note")]
    public string RelevanceNote { get; set; } = string.Empty;
}