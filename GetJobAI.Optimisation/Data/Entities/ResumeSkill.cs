namespace GetJobAI.Optimisation.Data.Entities;

public class ResumeSkill
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public string SkillName { get; set; } = string.Empty;
    
    public string SkillNameRaw { get; set; } = string.Empty;
    
    public string? Proficiency { get; set; }
    
    public string? Category { get; set; }

    public Resume Resume { get; private set; } = null!;

    private ResumeSkill() { }

    public static ResumeSkill Create(
        Guid resumeId,
        string skillName,
        string skillNameRaw,
        string? proficiency,
        string? category) => new()
    {
        Id = Guid.NewGuid(),
        ResumeId = resumeId,
        SkillName = skillName,
        SkillNameRaw = skillNameRaw,
        Proficiency = proficiency,
        Category = category
    };
}
