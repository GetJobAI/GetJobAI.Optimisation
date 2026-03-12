namespace GetJobAI.Optimisation.Data.Entities;

public class Resume
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; set; }
    
    public string? CandidateName { get; set; }
    
    public string? ExistingSummary { get; set; }
    
    public string? DetectedLanguage { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public List<ResumeWorkExperience> WorkExperiences { get; private set; } = [];
    
    public List<ResumeSkill> Skills { get; private set; } = [];
    
    public List<ResumePublication> Publications { get; private set; } = [];
    
    public List<ResumeActivity> Activities { get; private set; } = [];
    
    public List<ResumeAdditionalSection> AdditionalSections { get; private set; } = [];

    private Resume() { }

    public static Resume Create(Guid resumeId, Guid userId) => new()
    {
        Id = resumeId,
        UserId = userId,
        UpdatedAt = DateTime.UtcNow
    };
}
