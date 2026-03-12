namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class ExperienceGapContext
{
    public string JobResponsibility { get; set; } = string.Empty;
    
    public string? ClosestMatch { get; set; }
    
    public double VectorSimilarityScore { get; set; }
    
    public string? Flag { get; set; }
}