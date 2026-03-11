namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class PublicationContext
{
    public Guid EntryId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Publisher { get; set; }
    
    public string? PublicationDate { get; set; }
    
    public string? Description { get; set; }
}