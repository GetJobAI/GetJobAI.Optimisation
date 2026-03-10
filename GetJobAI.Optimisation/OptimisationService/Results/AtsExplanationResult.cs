using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class AtsExplanationResult
{
    [JsonPropertyName("headline_message")]
    public string HeadlineMessage { get; set; } = string.Empty;

    [JsonPropertyName("score_label")]
    public string ScoreLabel { get; set; } = string.Empty;

    [JsonPropertyName("what_is_working")]
    public string WhatIsWorking { get; set; } = string.Empty;

    [JsonPropertyName("biggest_opportunity")]
    public string BiggestOpportunity { get; set; } = string.Empty;

    [JsonPropertyName("top_quick_wins")]
    public List<string> TopQuickWins { get; set; } = [];

    [JsonPropertyName("encouragement")]
    public string Encouragement { get; set; } = string.Empty;
}