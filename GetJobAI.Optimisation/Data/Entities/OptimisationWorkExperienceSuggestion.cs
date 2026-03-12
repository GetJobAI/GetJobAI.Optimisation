namespace GetJobAI.Optimisation.Data.Entities;

public class OptimisationWorkExperienceSuggestion
{
    public Guid Id { get; private set; }
    
    public Guid OptimisationId { get; private set; }
    
    public Guid EntryId { get; private set; }
    
    public bool Include { get; set; }
    
    public string? Reason { get; set; }
    
    public bool? Accepted { get; set; }
    
    public string? RejectionHint { get; set; }
    
    public int RewriteCount { get; set; }

    public Optimisation Optimisation { get; private set; } = null!;
    
    public List<OptimisationBulletSuggestion> Bullets { get; private set; } = [];

    private OptimisationWorkExperienceSuggestion() { }

    public static OptimisationWorkExperienceSuggestion Create(
        Guid optimisationId,
        Guid entryId,
        bool include,
        string? reason) => new()
    {
        Id = Guid.NewGuid(),
        OptimisationId = optimisationId,
        EntryId = entryId,
        Include = include,
        Reason = reason
    };
}
