using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class SkillAlignmentDetail
{
    [JsonPropertyName("required_skill")]
    public string RequiredSkill { get; init; } = string.Empty;

    [JsonPropertyName("closest_match")]
    public string? ClosestMatch { get; init; }

    [JsonPropertyName("vector_similarity_score")]
    public double VectorSimilarityScore { get; init; }

    [JsonPropertyName("flag")]
    public string? Flag { get; init; }
}