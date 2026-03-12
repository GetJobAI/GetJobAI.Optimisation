using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class AtsBreakdown
{
    [JsonPropertyName("keyword_match_rate")]
    public AtsScoreSection<KeywordMatchDetails> KeywordMatchRate { get; init; } = new();

    [JsonPropertyName("skill_alignment")]
    public AtsScoreSection<List<SkillAlignmentDetail>> SkillAlignment { get; init; } = new();

    [JsonPropertyName("experience_relevance")]
    public AtsScoreSection<List<ExperienceRelevanceDetail>> ExperienceRelevance { get; init; } = new();

    [JsonPropertyName("format_and_parseability")]
    public AtsFormatSection FormatAndParseability { get; init; } = new();
}