namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record AdditionalSectionContext(
    Guid EntryId,
    string SectionType,
    string Title,
    string ContentJson);