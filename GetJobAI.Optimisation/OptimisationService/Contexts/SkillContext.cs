namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class SkillContext
{
    public string SkillName { get; set; } = string.Empty;
    
    public string SkillNameRaw { get; set; } = string.Empty;
    
    public string? Proficiency { get; set; }
    
    public string? Category { get; set; }
}