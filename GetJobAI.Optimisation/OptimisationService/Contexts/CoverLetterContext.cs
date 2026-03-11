namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class CoverLetterContext
{
    public string JobTitle { get; set; } = string.Empty;
    
    public string CompanyName { get; set; } = string.Empty;
    
    public string CompanyDescription { get; set; } = string.Empty;
    
    public string? CandidateName { get; set; }
    
    public string AcceptedSummary { get; set; } = string.Empty;
    
    public List<string> TopAchievements { get; set; } = [];
    
    public List<string> AcceptedSkills { get; set; } = [];
    
    public List<string> MissingKeywords { get; set; } = [];
    
    public string Language { get; set; } = "en-GB";
    
    public string? CustomNote { get; set; }
    
    public int RewriteCount { get; set; } = 0;
}