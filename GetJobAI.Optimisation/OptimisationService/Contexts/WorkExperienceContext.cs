namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record WorkExperienceContext(
    Guid EntryId,
    string JobTitle,
    string CompanyName,
    string StartDate,
    string EndDate,
    List<string> Bullets);
