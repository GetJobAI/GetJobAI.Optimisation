namespace GetJobAI.Optimisation.Data.Entities;

public class OptimisationActivitySuggestion
{
    public Guid Id { get; private set; }
    
    public Guid OptimisationId { get; private set; }
    
    public Guid EntryId { get; private set; }
    
    public bool Include { get; set; }
    
    public string? Reason { get; set; }
    
    public List<string> HighlightsRewritten { get; set; } = [];
    
    public bool? Accepted { get; set; }
    
    public string? RejectionHint { get; set; }
    
    public int RewriteCount { get; set; }

    public Optimisation Optimisation { get; private set; } = null!;

    private OptimisationActivitySuggestion() { }

    public static OptimisationActivitySuggestion Create(
        Guid optimisationId,
        Guid entryId,
        bool include,
        string? reason,
        List<string> highlightsRewritten) => new()
    {
        Id = Guid.NewGuid(),
        OptimisationId = optimisationId,
        EntryId = entryId,
        Include = include,
        Reason = reason,
        HighlightsRewritten = highlightsRewritten
    };
}
