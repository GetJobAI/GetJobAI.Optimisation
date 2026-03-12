using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class KeywordMatchDetails
{
    [JsonPropertyName("match")]
    public List<string> Match { get; init; } = [];

    [JsonPropertyName("partial")]
    public List<string> Partial { get; init; } = [];

    [JsonPropertyName("missing")]
    public List<string> Missing { get; init; } = [];
}