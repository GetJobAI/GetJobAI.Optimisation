using System.Text.Json;
using GetJobAI.Optimisation.Contracts;
using GetJobAI.Optimisation.OptimisationService.Contexts;
using GetJobAI.Optimisation.OptimisationService.Models;
using GetJobAI.Optimisation.OptimisationService.Results;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;

namespace GetJobAI.Optimisation.OptimisationService;

public class PromptRunner(
    Client client,
    IOptions<GeminiOptions> options,
    IPromptRegistry promptRegistry) : IPromptRunner
{
    private readonly GeminiOptions _options = options.Value;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PromptResult<WorkExperienceSuggestion>> RewriteExperienceAsync(
        WorkExperienceContext entry,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
            I am optimising a resume for the following role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Here is one work experience entry to rewrite:
            <experience_entry>
              Entry Id:  {entry.EntryId}
              Job Title: {entry.JobTitle}
              Company:   {entry.CompanyName}
              Period:    {entry.StartDate} – {entry.EndDate}
              Bullets:
              {string.Join("\n  ", entry.Bullets.Select((b, i) => $"{i + 1}. {b}"))}
            </experience_entry>

            Missing ATS keywords to naturally incorporate (ordered by importance):
            <missing_keywords>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Select(s => new { keyword = s.SkillName, importance = s.ImportanceScore }))}
            </missing_keywords>

            {HintBlock(hint)}

            Rewrite the bullets. Return only valid JSON matching the schema in your instructions.
            """;

        var result = await CallModelAsync<WorkExperienceSuggestion>(
            "PR-01", ctx.OptimisationId, userMessage, null, ct);

        result.Content.EntryId      = entry.EntryId;
        result.Content.RewriteCount = hint is not null ? 1 : 0;

        return result;
    }

    public async Task<PromptResult<SummarySuggestion>> RewriteSummaryAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
            Rewrite the professional summary for a candidate applying for the following role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Candidate's current summary:
            <existing_summary>
            {ctx.ExistingSummary ?? "(no existing summary — write from scratch based on the context below)"}
            </existing_summary>

            Candidate's skills for context:
            <candidate_skills>
            {JsonSerializer.Serialize(ctx.Skills.Select(s => new { skill = s.SkillNameRaw, proficiency = s.Proficiency }))}
            </candidate_skills>

            Missing ATS keywords to incorporate naturally:
            <missing_keywords>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Take(10)
                .Select(s => s.SkillName))}
            </missing_keywords>

            {HintBlock(hint)}

            Return only valid JSON matching the schema in your instructions.
            """;

        var result = await CallModelAsync<SummarySuggestion>(
            "PR-02", ctx.OptimisationId, userMessage, null, ct);

        result.Content.Original     = ctx.ExistingSummary ?? string.Empty;
        result.Content.RewriteCount = hint is not null ? 1 : 0;

        return result;
    }
    
    public async Task<PromptResult<SkillsSuggestion>> OptimiseSkillsAsync(
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
            Analyse the candidate's skills against the target role and produce a skills optimisation plan.

            Target role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Candidate's current skills:
            <candidate_skills>
            {JsonSerializer.Serialize(ctx.Skills.Select(s => new
            {
                skill      = s.SkillNameRaw,
                normalised = s.SkillName,
                proficiency = s.Proficiency,
                category   = s.Category
            }))}
            </candidate_skills>

            Required skills from the job description (ordered by importance):
            <required_skills>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Select(s => new { skill = s.SkillName, importance = s.ImportanceScore, category = s.Category }))}
            </required_skills>

            Preferred skills from the job description:
            <preferred_skills>
            {JsonSerializer.Serialize(ctx.JobPreferredSkills
                .Select(s => new { skill = s.SkillName, category = s.Category }))}
            </preferred_skills>

            ATS skill alignment score: {ctx.ScoreSkill}/100

            {HintBlock(hint)}

            Return only valid JSON matching the schema in your instructions.
            """;

        var result = await CallModelAsync<SkillsSuggestion>(
            "PR-03", ctx.OptimisationId, userMessage, null, ct);

        result.Content.RewriteCount = hint is not null ? 1 : 0;

        return result;
    }

    public async Task<PromptResult<SectionRelevancyRawSuggestions>> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        if (ctx.Publications.Count == 0
            && ctx.Activities.Count == 0
            && ctx.AdditionalSections.Count == 0)
        {
            return PromptResult<SectionRelevancyRawSuggestions>.Empty(
                new SectionRelevancyRawSuggestions());
        }

        var userMessage = $"""
            Assess which of the following resume sections are relevant to include
            for a candidate applying for the role below.

            Target role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Publications:
            <publications>
            {JsonSerializer.Serialize(ctx.Publications.Select(p => new
            {
                entry_id         = p.EntryId,
                title            = p.Title,
                publisher        = p.Publisher,
                publication_date = p.PublicationDate,
                description      = p.Description
            }))}
            </publications>

            Activities:
            <activities>
            {JsonSerializer.Serialize(ctx.Activities.Select(a => new
            {
                entry_id     = a.EntryId,
                activity     = a.ActivityName,
                organisation = a.Organization,
                role         = a.Role,
                highlights   = a.Highlights
            }))}
            </activities>

            Additional sections:
            <additional_sections>
            {JsonSerializer.Serialize(ctx.AdditionalSections.Select(a => new
            {
                entry_id     = a.EntryId,
                section_type = a.SectionType,
                title        = a.Title,
                content      = a.ContentJson
            }))}
            </additional_sections>

            Return only valid JSON matching the schema in your instructions.
            """;

        var result = await CallModelAsync<SectionRelevancyRawSuggestions>(
            "PR-04", ctx.OptimisationId, userMessage, null, ct);

        MapEntryIds(result.Content.Publications, ctx.Publications.Select(p => p.EntryId).ToList());
        MapEntryIds(result.Content.AdditionalSections, ctx.AdditionalSections.Select(a => a.EntryId).ToList());

        for (var i = 0; i < result.Content.Activities.Count && i < ctx.Activities.Count; i++)
        {
            result.Content.Activities[i].EntryId = ctx.Activities[i].EntryId;
        }

        return result;
    }

    public async Task<PromptResult<AtsExplanationResult>> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        var userMessage = $"""
            Explain the following ATS analysis result to the candidate in plain language.

            Target role: {ctx.JobTitle} at {ctx.CompanyName}

            Scores:
            <scores>
              Overall ATS score:    {ctx.OverallScore} / 100
              Keyword match:        {ctx.ScoreKeyword} / 100
              Skill alignment:      {ctx.ScoreSkill} / 100
              Resume parseability:  {ctx.ScoreFormat} / 100
              Experience relevance: {ctx.ScoreExperience} / 100
            </scores>

            Top missing required keywords:
            <missing_keywords>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Take(5)
                .Select(s => s.SkillName))}
            </missing_keywords>

            Return only valid JSON matching the schema in your instructions.
            """;

        return await CallModelAsync<AtsExplanationResult>(
            "PR-05", ctx.OptimisationId, userMessage, null, ct);
    }

    public async Task<PromptResult<ActivitySuggestion>> RewriteActivityAsync(
        ActivityContext activity,
        OptimisationContext ctx,
        CancellationToken ct,
        string? hint = null)
    {
        var userMessage = $"""
            I am optimising a resume for the following role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            Here is one activity / volunteering entry to assess and rewrite:
            <activity_entry>
              Activity:     {activity.ActivityName}
              Organisation: {activity.Organization ?? "N/A"}
              Role:         {activity.Role ?? "N/A"}
              Highlights:
              {string.Join("\n  ", activity.Highlights.Select((h, i) => $"{i + 1}. {h}"))}
            </activity_entry>

            Missing ATS keywords to naturally incorporate if relevant:
            <missing_keywords>
            {JsonSerializer.Serialize(ctx.JobRequiredSkills
                .OrderByDescending(s => s.ImportanceScore)
                .Take(8)
                .Select(s => s.SkillName))}
            </missing_keywords>

            {HintBlock(hint)}

            Assess relevance and rewrite highlights if included.
            Return only valid JSON matching the schema in your instructions.
            """;

        var result = await CallModelAsync<ActivitySuggestion>(
            "PR-06", ctx.OptimisationId, userMessage, null, ct);

        result.Content.EntryId = activity.EntryId;
        result.Content.RewriteCount = hint is not null ? 1 : 0;

        return result;
    }

    public async Task<PromptResult<XyzDetectResult>> DetectXyzOpportunityAsync(
        string bulletText,
        string jobTitle,
        string companyName,
        CancellationToken ct)
    {
        var userMessage = $"""
            Assess whether the following resume bullet would benefit from XYZ reformatting.

            Context — this bullet is from a candidate applying for:
            <target_role>
              Job Title: {jobTitle}
              Company:   {companyName}
            </target_role>

            Bullet to assess:
            <bullet>{bulletText}</bullet>

            Return only valid JSON matching the schema in your instructions.
            """;

        return await CallModelAsync<XyzDetectResult>("PR-07", null, userMessage, null, ct);
    }

    public async Task<PromptResult<XyzRewriteResult>> RewriteWithXyzAsync(
        string originalBullet,
        string missingComponent,
        string coachQuestion,
        string userAnswer,
        string jobTitle,
        CancellationToken ct)
    {
        var emptyAnswers = new[] { "", "n/a", "na", "no", "none", "i don't know", "idk" };

        if (emptyAnswers.Contains(userAnswer.Trim().ToLowerInvariant()))
        {
            return PromptResult<XyzRewriteResult>.Empty(new XyzRewriteResult
            {
                RewrittenBullet = originalBullet,
                UsedOriginal    = true
            });
        }

        var userMessage = $"""
            Rewrite the following resume bullet into XYZ format using the candidate's answer.

            Target role: {jobTitle}

            Original bullet:
            <original_bullet>{originalBullet}</original_bullet>

            The missing XYZ component was: {missingComponent}

            The coaching question asked:
            <question>{coachQuestion}</question>

            The candidate answered:
            <candidate_answer>{userAnswer}</candidate_answer>

            Return only valid JSON matching the schema in your instructions.
            """;

        return await CallModelAsync<XyzRewriteResult>("PR-08", null, userMessage, null, ct);
    }
    
    public async Task<PromptResult<CoverLetterResult>> GenerateCoverLetterAsync(
        CoverLetterContext ctx,
        CancellationToken ct)
    {
        var userMessage = $"""
            Write a professional cover letter for the following candidate and role.

            Candidate: {ctx.CandidateName ?? "the candidate"}
            Target role:
            <target_role>
              Job Title: {ctx.JobTitle}
              Company:   {ctx.CompanyName}
            </target_role>

            About the company:
            <company_description>{ctx.CompanyDescription}</company_description>

            Candidate's accepted professional summary:
            <summary>{ctx.AcceptedSummary}</summary>

            Top achievements to draw on (pick the most relevant 2–3):
            <achievements>
            {JsonSerializer.Serialize(ctx.TopAchievements)}
            </achievements>

            Accepted skills:
            <skills>
            {JsonSerializer.Serialize(ctx.AcceptedSkills)}
            </skills>

            Keywords still missing from the resume to weave in naturally:
            <missing_keywords>
            {JsonSerializer.Serialize(ctx.MissingKeywords)}
            </missing_keywords>

            Language/locale: {ctx.Language}

            {(ctx.CustomNote is not null ? $"Additional instruction from the candidate:\n<custom_note>{ctx.CustomNote}</custom_note>" : "")}

            Return only valid JSON matching the schema in your instructions.
            """;

        // Each regeneration nudges temperature up: 0.65 → 0.75 → 0.85, capped at 0.85
        var baseTemp    = promptRegistry.Get("PR-09").Temperature;
        var tempOverride = ctx.RewriteCount > 0
            ? Math.Min(baseTemp + 0.1f * ctx.RewriteCount, 0.85f)
            : (float?)null;

        var result = await CallModelAsync<CoverLetterResult>(
            "PR-09", null, userMessage, tempOverride, ct);

        result.Content.RewriteCount = ctx.RewriteCount;

        return result;
    }
    
    private async Task<PromptResult<T>> CallModelAsync<T>(
        string promptId,
        Guid? optimisationId,
        string userMessage,
        float? temperatureOverride,
        CancellationToken ct)
    {
        var prompt = promptRegistry.Get(promptId);

        var config = new GenerateContentConfig
        {
            SystemInstruction = new Content
            {
                Parts = [new Part { Text = prompt.SystemMessage }]
            },
            Temperature = temperatureOverride ?? prompt.Temperature,
            MaxOutputTokens = prompt.MaxTokens,
            ResponseMimeType = "application/json"
        };

        var response = await client.Models.GenerateContentAsync(
            model: _options.PrimaryModel,
            contents: new Content
            {
                Role  = "user",
                Parts = [new Part { Text = userMessage }]
            },
            config: config,
            ct
        );

        if (response.Candidates is null || response.Candidates.Count == 0)
        {
            throw new InvalidOperationException($"{promptId}: model returned no candidates.");
        }

        var json = response.Candidates[0].Content?.Parts?[0].Text;

        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException($"{promptId}: model response was empty.");
        }

        try
        {
            var content = JsonSerializer.Deserialize<T>(json, JsonOptions)
                ?? throw new InvalidOperationException($"{promptId}: response deserialized to null.");

            return MapToPromptResult(response, content, prompt.Version, optimisationId);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"{promptId}: failed to deserialize response. Raw: {json[..Math.Min(300, json.Length)]}", ex);
        }
    }

    private PromptResult<T> MapToPromptResult<T>(
        GenerateContentResponse response,
        T content,
        string version,
        Guid? optimisationId = null)
    {
        var usage     = response.UsageMetadata;
        var candidate = response.Candidates.FirstOrDefault();

        return new PromptResult<T>
        {
            Content = content,
            PromptVersion = version,
            Model = response.ModelVersion ?? _options.PrimaryModel,
            Success = candidate?.FinishReason == FinishReason.Stop,
            InputTokens = usage?.PromptTokenCount,
            OutputTokens = usage?.CandidatesTokenCount,
            CachedTokens = usage?.CachedContentTokenCount,
            TotalTokens = usage?.TotalTokenCount,
            FinishReason = candidate?.FinishReason.ToString(),
            OptimisationId = optimisationId
        };
    }

    private static string HintBlock(string? hint)
    {
        return hint is not null
            ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>"
            : string.Empty;
    }

    private static void MapEntryIds(List<SectionRelevancySuggestion> suggestions, List<Guid> entryIds)
    {
        for (var i = 0; i < suggestions.Count && i < entryIds.Count; i++)
        {
            suggestions[i].EntryId = entryIds[i];
        }
    }
}