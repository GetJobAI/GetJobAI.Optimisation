using System.Text.Json.Serialization;
using MassTransit;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

[EntityName("resume.scored")]
public class ResumeScoredEvent
{
    [JsonPropertyName("resume_id")]
    public Guid ResumeId { get; init; }

    [JsonPropertyName("job_analysis_id")]
    public Guid JobAnalysisId { get; init; }

    [JsonPropertyName("job_title")]
    public string JobTitle { get; init; } = string.Empty;

    [JsonPropertyName("company_name")]
    public string CompanyName { get; init; } = string.Empty;

    [JsonPropertyName("score")]
    public int Score { get; init; }

    [JsonPropertyName("breakdown")]
    public AtsBreakdown Breakdown { get; init; } = new();
}