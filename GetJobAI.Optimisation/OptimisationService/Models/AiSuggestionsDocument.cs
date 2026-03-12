using System.Text.Json.Serialization;
using GetJobAI.Optimisation.OptimisationService.Results;

namespace GetJobAI.Optimisation.OptimisationService.Models;

public class AiSuggestionsDocument
{
    [JsonPropertyName("summary")]
    public SummarySuggestion? Summary { get; set; }

    [JsonPropertyName("work_experience")]
    public List<WorkExperienceSuggestion> WorkExperience { get; set; } = [];

    [JsonPropertyName("publications")]
    public List<SectionRelevancySuggestion> Publications { get; set; } = [];

    [JsonPropertyName("activities")]
    public List<ActivitySuggestion> Activities { get; set; } = [];

    [JsonPropertyName("additional_sections")]
    public List<SectionRelevancySuggestion> AdditionalSections { get; set; } = [];

    [JsonPropertyName("ats_explanation")]
    public AtsExplanationResult? AtsExplanation { get; set; }

    [JsonPropertyName("skills_gap")]
    public SkillsGapResult SkillsGap { get; set; }
}