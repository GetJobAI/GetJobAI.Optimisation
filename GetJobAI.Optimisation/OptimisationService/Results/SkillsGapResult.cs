using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class SkillsGapResult
{
    [JsonPropertyName("skills_analysis")]
    public List<SkillAnalysis> SkillsAnalysis { get; set; } = [];

    [JsonPropertyName("hidden_strengths")]
    public List<HiddenStrength> HiddenStrengths { get; set; } = [];

    [JsonPropertyName("blocker_count")]
    public int BlockerCount { get; set; }

    [JsonPropertyName("summary_sentence")]
    public string SummarySentence { get; set; } = string.Empty;
}