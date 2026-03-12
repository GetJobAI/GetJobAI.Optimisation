namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class OptimisationContext
{
    public Guid OptimisationId { get; set; }
    
    public Guid ResumeId { get; set; }
    
    public string JobTitle { get; set; } = null!;
    
    public string CompanyName { get; set; } = null!;
    
    public string? CandidateName { get; set; }
    
    public string? ExistingSummary { get; set; }
    
    public string? DetectedLanguage { get; set; }

    // Scores
    public int OverallScore { get; set; }
    
    public short ScoreKeywordEarned { get; set; }
    
    public short ScoreKeywordMax { get; set; }
    
    public short ScoreSkillEarned { get; set; }
    
    public short ScoreSkillMax { get; set; }
    
    public short ScoreFormatEarned { get; set; }
    
    public short ScoreFormatMax { get; set; }
    
    public short ScoreExperienceEarned { get; set; }
    
    public short ScoreExperienceMax { get; set; }

    // ATS breakdown details
    public List<string> MatchedKeywords { get; set; } = [];
    
    public List<string> PartialKeywords { get; set; } = [];
    
    public List<string> MissingKeywords { get; set; } = [];
    
    public List<SkillAlignmentContext> SkillAlignmentDetails { get; set; } = [];
    
    public List<ExperienceGapContext> ExperienceGapDetails { get; set; } = [];
    
    public AtsParsingFlagsContext? ParsingFlags { get; set; }

    // Resume content 
    public List<WorkExperienceContext> WorkExperiences { get; set; } = [];
    
    public List<SkillContext> Skills { get; set; } = [];
    
    public List<PublicationContext> Publications { get; set; } = [];
    
    public List<ActivityContext> Activities { get; set; } = [];
    
    public List<AdditionalSectionContext> AdditionalSections { get; set; } = [];

    // Job requirements
    public List<JobSkillContext> JobRequiredSkills { get; set; } = [];
    
    public List<JobSkillContext> JobPreferredSkills { get; set; } = [];
}
