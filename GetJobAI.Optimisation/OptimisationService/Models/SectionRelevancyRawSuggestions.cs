using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Models;

public class SectionRelevancyRawSuggestions
{
    [JsonPropertyName("publications")]
    public List<SectionRelevancySuggestion> Publications { get; set; } = [];

    [JsonPropertyName("activities")]
    public List<ActivitySuggestion> Activities { get; set; } = [];

    [JsonPropertyName("additional_sections")]
    public List<SectionRelevancySuggestion> AdditionalSections { get; set; } = [];
}