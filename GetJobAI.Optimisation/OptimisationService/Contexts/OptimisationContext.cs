namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public class OptimisationContext
{
    public Guid OptimisationId { get; set; }

    public Guid ResumeId { get; set; }

    public Guid UserId { get; set; }
    
    public string JobTitle { get; set; } = null!;
    
    public string CompanyName { get; set; } = null!;
    
    public string? CandidateName { get; set; }

    public string? ExistingSummary { get; set; }

    public string? DetectedLanguage { get; set; }
    
    public short OverallScore { get; set; }

    public short ScoreKeyword { get; set; }

    public short ScoreSkill { get; set; }

    public short ScoreFormat { get; set; }

    public short ScoreExperience { get; set; }

    public List<WorkExperienceContext> WorkExperiences { get; set; } = [];
    
    public List<SkillContext> Skills { get; set; } = [];

    public List<JobSkillContext> JobRequiredSkills { get; set; } = [];

    public List<JobSkillContext> JobPreferredSkills { get; set; } = [];

    public List<PublicationContext> Publications { get; set; } = [];

    public List<ActivityContext> Activities { get; set; } = [];

    public List<AdditionalSectionContext> AdditionalSections { get; set; } = [];
}