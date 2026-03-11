namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class JobSkillContext
{
    public string SkillName { get; set; } = string.Empty;
    
    public double ImportanceScore { get; set; }
    
    public string? Category { get; set; }
    
    public bool IsRequired { get; set; }
}