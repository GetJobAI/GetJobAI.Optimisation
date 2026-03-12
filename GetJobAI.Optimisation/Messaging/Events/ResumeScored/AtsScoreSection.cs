using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class AtsScoreSection<TDetails>
{
    [JsonPropertyName("earned")]
    public int Earned { get; init; }

    [JsonPropertyName("max")]
    public int Max { get; init; }

    [JsonPropertyName("details")]
    public TDetails Details { get; init; } = default!;
}