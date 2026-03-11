namespace GetJobAI.Optimisation.OptimisationService.MetricsCollector;

public sealed class PromptCallMetrics
{
    public string PromptId { get; init; } = string.Empty;
    
    public string PromptVersion { get; set; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public int InputTokens { get; init; }

    public int OutputTokens { get; init; }
    
    public int CachedTokens { get; init; }

    public int TotalTokens { get; init; }
    
    public long ElapsedMs { get; init; }

    public bool Success { get; init; }
    
    public string? Reason { get; init; }
    
    public DateTime CreatedAt { get; set; }

    public Guid? OptimisationId { get; init; }
}