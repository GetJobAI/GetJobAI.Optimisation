using GetJobAI.Optimisation.OptimisationService.Results;

namespace GetJobAI.Optimisation.Data.Entities;

public class Optimisation
{
    public Guid Id { get; private set; }
    
    public Guid ResumeId { get; private set; }
    
    public Guid JobAnalysisId { get; private set; }

    public string JobTitle { get; private set; } = string.Empty;

    public string CompanyName { get; private set; } = string.Empty;
    
    public OptimisationStatus Status { get; private set; }

    public int OverallScore { get; private set; }

    public short ScoreKeywordEarned { get; private set; }
    
    public short ScoreKeywordMax { get; private set; }

    public short ScoreSkillEarned { get; private set; }
    
    public short ScoreSkillMax { get; private set; }

    public short ScoreFormatEarned { get; private set; }
    
    public short ScoreFormatMax { get; private set; }

    public short ScoreExperienceEarned { get; private set; }
    
    public short ScoreExperienceMax { get; private set; }

    public string? AtsDetailsJson { get; private set; }

    public AtsExplanationResult? AtsExplanation { get; private set; }
    
    public SkillsGapResult? SkillsGap { get; private set; }
    
    public string? ErrorMessage { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? StartedAt { get; private set; }
    
    public DateTime? CompletedAt { get; private set; }

    public OptimisationSummarySuggestion? SummarySuggestion { get; private set; }

    public OptimisationCoverLetter? CoverLetter { get; private set; }
    
    public List<OptimisationWorkExperienceSuggestion> WorkExperienceSuggestions { get; private set; } = [];
    
    public List<OptimisationActivitySuggestion> ActivitySuggestions { get; private set; } = [];
    
    public List<OptimisationSectionSuggestion> SectionSuggestions { get; private set; } = [];

    private Optimisation() { }

    public static Optimisation Create(
        Guid resumeId,
        Guid jobAnalysisId,
        string jobTitle,
        string companyName,
        int overallScore,
        short scoreKeywordEarned, short scoreKeywordMax,
        short scoreSkillEarned, short scoreSkillMax,
        short scoreFormatEarned, short scoreFormatMax,
        short scoreExperienceEarned, short scoreExperienceMax,
        string? atsDetailsJson = null)
    {
        return new Optimisation
        {
            Id = Guid.NewGuid(),
            ResumeId = resumeId,
            JobAnalysisId = jobAnalysisId,
            JobTitle = jobTitle,
            CompanyName = companyName,
            Status = OptimisationStatus.Pending,
            OverallScore = overallScore,
            ScoreKeywordEarned = scoreKeywordEarned,
            ScoreKeywordMax = scoreKeywordMax,
            ScoreSkillEarned = scoreSkillEarned,
            ScoreSkillMax = scoreSkillMax,
            ScoreFormatEarned = scoreFormatEarned,
            ScoreFormatMax = scoreFormatMax,
            ScoreExperienceEarned = scoreExperienceEarned,
            ScoreExperienceMax = scoreExperienceMax,
            AtsDetailsJson = atsDetailsJson,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Start()
    {
        Status = OptimisationStatus.InProgress;
        StartedAt = DateTime.UtcNow;
    }

    public void Complete(AtsExplanationResult? atsExplanation, SkillsGapResult? skillsGap)
    {
        Status = OptimisationStatus.AwaitingReview;
        AtsExplanation = atsExplanation;
        SkillsGap = skillsGap;
        CompletedAt = DateTime.UtcNow;
    }

    public void Fail(string errorMessage)
    {
        Status = OptimisationStatus.Failed;
        ErrorMessage = errorMessage;
        CompletedAt = DateTime.UtcNow;
    }
}
