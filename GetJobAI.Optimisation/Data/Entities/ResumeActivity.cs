namespace GetJobAI.Optimisation.Data.Entities;

public class ResumeActivity
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public string ActivityName { get; set; } = string.Empty;
    
    public string? Organization { get; set; }
    
    public string? Role { get; set; }
    
    public List<string> Highlights { get; set; } = [];

    public Resume Resume { get; private set; } = null!;

    private ResumeActivity() { }

    public static ResumeActivity Create(
        Guid entryId,
        Guid resumeId,
        string activityName,
        string? organization,
        string? role,
        List<string> highlights) => new()
    {
        Id = entryId,
        ResumeId = resumeId,
        ActivityName = activityName,
        Organization = organization,
        Role = role,
        Highlights = highlights
    };
}
