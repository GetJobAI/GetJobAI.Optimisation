using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.Messaging.Events.ResumeScored;

public class AtsParsingFlags
{
    [JsonPropertyName("has_complex_layout")]
    public bool HasComplexLayout { get; init; }

    [JsonPropertyName("has_graphics")]
    public bool HasGraphics { get; init; }

    [JsonPropertyName("has_headers_footers")]
    public bool HasHeadersFooters { get; init; }

    [JsonPropertyName("has_non_standard_fonts")]
    public bool HasNonStandardFonts { get; init; }
}
