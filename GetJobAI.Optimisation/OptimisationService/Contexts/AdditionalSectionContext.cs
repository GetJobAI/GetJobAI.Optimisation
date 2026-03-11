namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class AdditionalSectionContext
{
    public Guid EntryId { get; set; }
    
    public string SectionType { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string ContentJson { get; set; } = string.Empty;
}