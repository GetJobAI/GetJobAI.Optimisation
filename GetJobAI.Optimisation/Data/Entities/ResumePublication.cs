namespace GetJobAI.Optimisation.Data.Entities;

public class ResumePublication
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Publisher { get; set; }
    
    public string? PublicationDate { get; set; }
    
    public string? Description { get; set; }

    public Resume Resume { get; private set; } = null!;

    private ResumePublication() { }

    public static ResumePublication Create(
        Guid entryId,
        Guid resumeId,
        string title,
        string? publisher,
        string? publicationDate,
        string? description) => new()
    {
        Id = entryId,
        ResumeId = resumeId,
        Title = title,
        Publisher = publisher,
        PublicationDate = publicationDate,
        Description = description
    };
}
