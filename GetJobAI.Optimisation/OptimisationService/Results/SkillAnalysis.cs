using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class SkillAnalysis
{
    [JsonPropertyName("skill")]
    public string Skill { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("candidate_equivalent")]
    public string? CandidateEquivalent { get; set; }

    [JsonPropertyName("suggestion")]
    public string? Suggestion { get; set; }
}