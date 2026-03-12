using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class AtsFormatSection
{
    [JsonPropertyName("earned")]
    public int Earned { get; init; }

    [JsonPropertyName("max")]
    public int Max { get; init; }

    [JsonPropertyName("parsing_flags")]
    public AtsParsingFlags ParsingFlags { get; init; } = new();
}