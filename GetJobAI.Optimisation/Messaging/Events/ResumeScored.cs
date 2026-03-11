namespace GetJobAI.Optimisation.Messaging.Events;

public record ResumeScored
{
    public Guid OptimisationId { get; init; }
    
    public Guid ResumeId { get; init; }
    
    public Guid UserId { get; init; }
    
    public Guid JobAnalysisId { get; init; }

    public string JobTitle { get; init; } = string.Empty;
    
    public string CompanyName { get; init; } = string.Empty;

    public string? CandidateName { get; init; }
    
    public string? ExistingSummary { get; init; }
    
    public string? DetectedLanguage { get; init; }

    public short OverallScore { get; init; }
    
    public short ScoreKeyword { get; init; }
    
    public short ScoreSkill { get; init; }
    
    public short ScoreFormat { get; init; }
    
    public short ScoreExperience { get; init; }

    public List<WorkExperienceItem> WorkExperiences { get; init; } = [];
    
    public List<SkillItem> Skills { get; init; } = [];
    
    public List<JobSkillItem> JobRequiredSkills { get; init; } = [];
    
    public List<JobSkillItem> JobPreferredSkills { get; init; } = [];
    
    public List<PublicationItem> Publications { get; init; } = [];
    
    public List<ActivityItem> Activities { get; init; } = [];
    
    public List<AdditionalSectionItem> AdditionalSections { get; init; } = [];
}

public record WorkExperienceItem
{
    public Guid EntryId { get; init; }
    
    public string JobTitle { get; init; } = string.Empty;
    
    public string CompanyName { get; init; } = string.Empty;
    
    public string StartDate { get; init; } = string.Empty;
    
    public string EndDate { get; init; } = string.Empty;
    
    public List<string> Bullets { get; init; } = [];
}

public record SkillItem
{
    public string SkillName { get; init; } = string.Empty;
    
    public string SkillNameRaw { get; init; } = string.Empty;
    
    public string? Proficiency { get; init; }
    
    public string? Category { get; init; }
}

public record JobSkillItem
{
    public string SkillName { get; init; } = string.Empty;
    
    public double ImportanceScore { get; init; }
    
    public string? Category { get; init; }
    
    public bool IsRequired { get; init; }
}

public record PublicationItem
{
    public Guid EntryId { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string? Publisher { get; init; }
    
    public string? PublicationDate { get; init; }
    
    public string? Description { get; init; }
}

public record ActivityItem
{
    public Guid EntryId { get; init; }
    
    public string ActivityName { get; init; } = string.Empty;
    
    public string? Organization { get; init; }
    
    public string? Role { get; init; }
    
    public List<string> Highlights { get; init; } = [];
}

public record AdditionalSectionItem
{
    public Guid EntryId { get; init; }
    
    public string? SectionType { get; init; }
    
    public string? Title { get; init; }
    
    public string? ContentJson { get; init; }
}
