namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record JobSkillContext(
    string SkillName,
    double ImportanceScore,
    string? Category,
    bool IsRequired);
