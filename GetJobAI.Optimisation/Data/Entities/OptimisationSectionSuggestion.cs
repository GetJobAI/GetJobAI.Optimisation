namespace GetJobAI.Optimisation.Data.Entities;

public class OptimisationSectionSuggestion
{
    public Guid Id { get; private set; }
    
    public Guid OptimisationId { get; private set; }
    
    public OptimisationSectionCategory Category { get; private set; }
    
    public Guid EntryId { get; private set; }
    
    public string? SectionType { get; private set; }
    
    public bool Include { get; set; }
    
    public string? Reason { get; set; }
    
    public bool? Accepted { get; set; }

    public Optimisation Optimisation { get; private set; } = null!;

    private OptimisationSectionSuggestion() { }

    public static OptimisationSectionSuggestion Create(
        Guid optimisationId,
        OptimisationSectionCategory category,
        Guid entryId,
        string? sectionType,
        bool include,
        string? reason) => new()
    {
        Id = Guid.NewGuid(),
        OptimisationId = optimisationId,
        Category = category,
        EntryId = entryId,
        SectionType = sectionType,
        Include = include,
        Reason = reason
    };
}
