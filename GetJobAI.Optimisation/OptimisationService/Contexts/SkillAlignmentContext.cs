namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class SkillAlignmentContext
{
    public string RequiredSkill { get; set; } = string.Empty;
    
    public string? ClosestMatch { get; set; }
    
    public double VectorSimilarityScore { get; set; }
    
    public string? Flag { get; set; }
}
