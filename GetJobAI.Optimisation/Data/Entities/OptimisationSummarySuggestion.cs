namespace GetJobAI.Optimisation.Data.Entities;

public class OptimisationSummarySuggestion
{
    public Guid OptimisationId { get; private set; }
    
    public string Original { get; private set; } = string.Empty;
    
    public string Rewritten { get; set; } = string.Empty;
    
    public List<string> KeywordsIncorporated { get; set; } = [];
    
    public bool? Accepted { get; set; }
    
    public string? RejectionHint { get; set; }
    
    public int RewriteCount { get; set; }

    public Optimisation Optimisation { get; private set; } = null!;

    private OptimisationSummarySuggestion() { }

    public static OptimisationSummarySuggestion Create(
        Guid optimisationId,
        string original,
        string rewritten,
        List<string> keywordsIncorporated) => new()
    {
        OptimisationId = optimisationId,
        Original = original,
        Rewritten = rewritten,
        KeywordsIncorporated = keywordsIncorporated
    };
}
