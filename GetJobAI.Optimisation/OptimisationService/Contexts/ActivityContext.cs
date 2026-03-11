namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class ActivityContext
{
    public Guid EntryId { get; set; }
    
    public string ActivityName { get; set; } = string.Empty;
    
    public string? Organization { get; set; }
    
    public string? Role { get; set; }
    
    public List<string> Highlights { get; set; } = [];
}