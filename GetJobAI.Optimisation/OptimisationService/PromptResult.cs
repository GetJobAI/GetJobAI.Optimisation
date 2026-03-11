namespace GetJobAI.Optimisation.OptimisationService;

public class PromptResult<T>
{
    public T Content { get; init; }
    
    public string? PromptVersion { get; init; }
    
    public string? Model { get; init; }
    
    public int? InputTokens { get; init; }
    
    public int? OutputTokens { get; init; }
    
    public int? CachedTokens { get; init; }
    
    public int? TotalTokens { get; init; }
    
    public bool Success { get; init; }
    
    public string? FinishReason { get; init; }
    
    public Guid? OptimisationId { get; init; }
    
    public int CalculatedTotal => (InputTokens ?? 0) + (OutputTokens ?? 0) + (CachedTokens ?? 0);
    
    public static PromptResult<T> Empty(T content) => new()
    {
        Content = content,
        Success = true,
        Model = null,
        PromptVersion = null,
        InputTokens = null,
        OutputTokens = null,
        CachedTokens = null,
        TotalTokens = null,
        FinishReason = "skipped",
        OptimisationId = null
    };
}