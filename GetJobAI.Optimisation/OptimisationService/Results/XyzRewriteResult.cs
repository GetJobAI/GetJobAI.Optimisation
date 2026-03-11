using System.Text.Json.Serialization;

namespace GetJobAI.Optimisation.OptimisationService.Results;

public class XyzRewriteResult
{
    [JsonPropertyName("rewritten_bullet")]
    public string RewrittenBullet { get; set; } = string.Empty;

    [JsonPropertyName("xyz_breakdown")]
    public XyzBreakdown XyzBreakdown { get; set; } = new();

    [JsonPropertyName("user_data_used")]
    public string? UserDataUsed { get; set; }

    [JsonPropertyName("used_original")]
    public bool UsedOriginal { get; set; }
}

public class XyzBreakdown
{
    [JsonPropertyName("x")]
    public string X { get; set; } = string.Empty;

    [JsonPropertyName("y")]
    public string Y { get; set; } = string.Empty;

    [JsonPropertyName("z")]
    public string Z { get; set; } = string.Empty;
}