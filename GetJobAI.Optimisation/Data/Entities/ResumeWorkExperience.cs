namespace GetJobAI.Optimisation.Data.Entities;

public class ResumeWorkExperience
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public string JobTitle { get; set; } = string.Empty;
    
    public string CompanyName { get; set; } = string.Empty;
    
    public string StartDate { get; set; } = string.Empty;
    
    public string EndDate { get; set; } = string.Empty;
    
    public List<string> Bullets { get; set; } = [];

    public Resume Resume { get; private set; } = null!;

    private ResumeWorkExperience() { }

    public static ResumeWorkExperience Create(
        Guid entryId,
        Guid resumeId,
        string jobTitle,
        string companyName,
        string startDate,
        string endDate,
        List<string> bullets) => new()
    {
        Id = entryId,
        ResumeId = resumeId,
        JobTitle = jobTitle,
        CompanyName = companyName,
        StartDate = startDate,
        EndDate = endDate,
        Bullets = bullets
    };
}
