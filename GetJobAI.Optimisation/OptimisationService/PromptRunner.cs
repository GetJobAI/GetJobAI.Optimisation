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

    public async Task<WorkExperienceSuggestion> RewriteExperienceAsync(
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
                   .Select(s => new { keyword = s.SkillName, importance = s.ImportanceScore })
                   .OrderByDescending(s => s.importance))}
               </missing_keywords>

               {(hint is not null ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

               Rewrite the bullets. Return only valid JSON matching the schema in your instructions.
               """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-01").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<WorkExperienceSuggestion>("PR-01", userMessage, tempOverride, ct);

        parsed.EntryId = entry.EntryId;
        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SummarySuggestion> RewriteSummaryAsync(
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

               {(hint is not null ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

               Return only valid JSON matching the schema in your instructions.
               """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-02").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<SummarySuggestion>("PR-02", userMessage, tempOverride, ct);

        parsed.Original = ctx.ExistingSummary ?? string.Empty;
        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SkillsSuggestion> OptimiseSkillsAsync(
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
                skill       = s.SkillNameRaw,
                normalised  = s.SkillName,
                proficiency = s.Proficiency,
                category    = s.Category
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

            {(hint is not null ? $"The user rejected the previous suggestion. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

            Return only valid JSON matching the schema in your instructions.
            """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-03").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<SkillsSuggestion>("PR-03", userMessage, tempOverride, ct);

        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<SectionRelevancyRawSuggestions> AssessSectionRelevancyAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        if (ctx.Publications.Count == 0 && ctx.Activities.Count == 0 && ctx.AdditionalSections.Count == 0)
        {
            return new SectionRelevancyRawSuggestions();
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
                entry_id        = p.EntryId,
                title           = p.Title,
                publisher       = p.Publisher,
                publication_date = p.PublicationDate,
                description     = p.Description
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

        var parsed = await CallModelAsync<SectionRelevancyRawSuggestions>("PR-04", userMessage, null, ct);

        MapEntryIds(parsed.Publications, ctx.Publications.Select(p => p.EntryId).ToList());
        MapEntryIds(parsed.AdditionalSections, ctx.AdditionalSections.Select(a => a.EntryId).ToList());

        for (var i = 0; i < parsed.Activities.Count && i < ctx.Activities.Count; i++)
        {
            parsed.Activities[i].EntryId = ctx.Activities[i].EntryId;
        }

        return new SectionRelevancyRawSuggestions
        {
            Publications = parsed.Publications,
            Activities = parsed.Activities,
            AdditionalSections = parsed.AdditionalSections,
        };
    }
    
    public async Task<AtsExplanationResult> ExplainAtsScoreAsync(
        OptimisationContext ctx,
        CancellationToken ct)
    {
        var userMessage = $"""
            Explain the following ATS analysis result to the candidate in plain language.

            Target role: {ctx.JobTitle} at {ctx.CompanyName}

            Scores:
            <scores>
              Overall ATS score:     {ctx.OverallScore} / 100
              Keyword match:         {ctx.ScoreKeyword} / 100
              Skill alignment:       {ctx.ScoreSkill} / 100
              Resume parseability:   {ctx.ScoreFormat} / 100
              Experience relevance:  {ctx.ScoreExperience} / 100
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

        return await CallModelAsync<AtsExplanationResult>("PR-05", userMessage, null, ct);
    }
    
    public async Task<ActivitySuggestion> RewriteActivityAsync(
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

            {(hint is not null ? $"The user rejected the previous rewrite. Their feedback:\n<rejection_hint>{hint}</rejection_hint>" : "")}

            Assess relevance and rewrite highlights if included.
            Return only valid JSON matching the schema in your instructions.
            """;

        float? tempOverride = hint is not null
            ? promptRegistry.Get("PR-06").Temperature + 0.2f
            : null;

        var parsed = await CallModelAsync<ActivitySuggestion>("PR-06", userMessage, tempOverride, ct);

        parsed.EntryId      = activity.EntryId;
        parsed.RewriteCount = hint is not null ? 1 : 0;

        return parsed;
    }

    public async Task<XyzDetectResult> DetectXyzOpportunityAsync(
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

        return await CallModelAsync<XyzDetectResult>("PR-07", userMessage, null, ct);
    }

    public async Task<XyzRewriteResult> RewriteWithXyzAsync(
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
            return new XyzRewriteResult
            {
                RewrittenBullet = originalBullet,
                UsedOriginal    = true
            };
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

        return await CallModelAsync<XyzRewriteResult>("PR-08", userMessage, null, ct);
    }
    
    public async Task<CoverLetterResult> GenerateCoverLetterAsync(
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
        
        // todo: on each regeneration keep T between 0.65 and 0.85
        var result = await CallModelAsync<CoverLetterResult>("PR-09", userMessage, null, ct);

        result.RewriteCount = ctx.RewriteCount;

        return result;
    }

    private async Task<T> CallModelAsync<T>(
        string promptId,
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
            Temperature      = temperatureOverride ?? prompt.Temperature,
            MaxOutputTokens  = prompt.MaxTokens,
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
            var result = JsonSerializer.Deserialize<T>(json, JsonOptions);
            
            return result ?? throw new InvalidOperationException($"{promptId}: response deserialized to null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"{promptId}: failed to deserialize response. Raw: {json[..Math.Min(300, json.Length)]}", ex);
        }
    }
    
    private static void MapEntryIds(
        List<SectionRelevancySuggestion> suggestions,
        List<Guid> entryIds)
    {
        for (var i = 0; i < suggestions.Count && i < entryIds.Count; i++)
        {
            suggestions[i].EntryId = entryIds[i];
        }
    }
}