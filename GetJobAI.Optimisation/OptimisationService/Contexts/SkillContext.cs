namespace GetJobAI.Optimisation.OptimisationService.Contexts;

public record SkillContext(
    string SkillName,
    string SkillNameRaw,
    string? Proficiency,
    string? Category);