namespace GetJobAI.Optimisation.Data.Entities;

public class ResumeAdditionalSection
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public string? SectionType { get; set; }
    
    public string? Title { get; set; }
    
    public string? ContentJson { get; set; }

    public Resume Resume { get; private set; } = null!;

    private ResumeAdditionalSection() { }

    public static ResumeAdditionalSection Create(
        Guid entryId,
        Guid resumeId,
        string? sectionType,
        string? title,
        string? contentJson) => new()
    {
        Id = entryId,
        ResumeId = resumeId,
        SectionType = sectionType,
        Title = title,
        ContentJson = contentJson
    };
}
