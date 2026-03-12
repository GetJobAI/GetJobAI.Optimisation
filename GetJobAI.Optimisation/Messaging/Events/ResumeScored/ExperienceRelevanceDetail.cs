using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class ExperienceRelevanceDetail
{
    [JsonPropertyName("job_responsibility")]
    public string JobResponsibility { get; init; } = string.Empty;

    [JsonPropertyName("closest_match")]
    public string? ClosestMatch { get; init; }

    [JsonPropertyName("vector_similarity_score")]
    public double VectorSimilarityScore { get; init; }

    [JsonPropertyName("flag")]
    public string? Flag { get; init; }
}