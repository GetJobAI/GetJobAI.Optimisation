namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class WorkExperienceContext
{
    public Guid EntryId { get; set; }
    
    public string JobTitle { get; set; } = string.Empty;
    
    public string CompanyName { get; set; } = string.Empty;
    
    public string StartDate { get; set; } = string.Empty;
    
    public string EndDate { get; set; } = string.Empty;
    
    public List<string> Bullets { get; set; } = [];
}
