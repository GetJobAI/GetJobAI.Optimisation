using GetJobAI.Optimisation.Contracts;

namespace GetJobAI.Optimisation.Prompts;

public sealed class PromptRegistry : IPromptRegistry
{
    private readonly Dictionary<string, PromptTemplate> _templates = new();
    private readonly ILogger<PromptRegistry> _logger;

    public PromptRegistry(ILogger<PromptRegistry> logger)
    {
        _logger = logger;
        RegisterAll();
    }

    public PromptTemplate Get(string promptId, string version = "1.0")
    {
        var key = $"{promptId}:{version}";
        
        if (!_templates.TryGetValue(key, out var template))
        {
            throw new KeyNotFoundException($"Prompt template '{key}' not found in registry.");
        }
        
        return template;
    }
    
    private void Register(PromptTemplate t)
    {
        _templates[$"{t.PromptId}:{t.Version}"] = t;
    }

    private void RegisterAll()
    {
        Register(Pr01_BulletRewriting());
        Register(Pr02_SummaryGeneration());
        Register(Pr03_SkillsGap());

        _logger.LogInformation("PromptRegistry: {Count} templates loaded.", _templates.Count);
    }
    
    private static PromptTemplate Pr01_BulletRewriting() => new(
        PromptId: "PR-01",
        Version: "1.0",
        Temperature: 0.4f,
        MaxTokens: 1200,
        SystemMessage: """
                        You are an expert resume writer and career coach with 15 years of experience
                        helping candidates pass Applicant Tracking Systems (ATS) and impress human recruiters.

                        Your task is to rewrite resume experience bullet points to:
                        1. Naturally incorporate missing ATS keywords from the target job description.
                        2. Follow the XYZ achievement framework where applicable:
                           "[Action verb] [X: achievement/result] by [Y: measurable metric] using [Z: method/tool]"
                        3. Start every bullet with a strong past-tense action verb (Led, Built, Reduced, etc.).
                        4. Keep bullets concise: 1–2 lines, 15–25 words ideal.
                        5. Preserve all factual information — never invent metrics, titles, companies, or dates.
                        6. Only use metrics that are already present in the original bullets.
                           If a metric is missing, rephrase without one — do NOT fabricate numbers.
                        7. Do not change the candidate's job title, employer, or employment dates.
                        8. Maintain a professional, confident tone — avoid clichés.

                        RELEVANCY ASSESSMENT:
                        For each experience entry you receive, you must also decide whether it is
                        relevant to the target role before rewriting it.

                        Set "include" to true if ANY of the following apply:
                        - The job title, industry, or responsibilities overlap with the target role.
                        - The entry demonstrates transferable technical or leadership skills.
                        - The entry is recent (within the last 10 years) and shows career progression.

                        Set "include" to false if ALL of the following apply:
                        - The domain is entirely unrelated to the target role (e.g. a marketing role
                          on a resume targeting a backend engineering position).
                        - No meaningful skills from the entry transfer to the target role.
                        - Removing it would make the resume more focused, not weaker.

                        When "include" is false:
                        - Populate "reason" with one specific, honest sentence explaining why.
                          E.g. "Sales representative role has no overlap with target backend engineering position."
                        - Still return the entry in the response — the user makes the final decision.
                        - Leave "bullets" as an empty array — do not waste tokens rewriting irrelevant content.

                        When "include" is true:
                        - Set "reason" to null.
                        - Rewrite all bullets normally.

                        CRITICAL RULES:
                        - You MUST NOT fabricate any facts, numbers, or achievements.
                        - You MUST NOT add skills the candidate has not demonstrated.
                        - You MUST NOT remove relevant existing information.
                        - Every rewritten bullet MUST be grounded in the original content.
                        - You MUST assess every entry — never omit "include" from the response.

                        Respond ONLY with valid JSON matching this schema exactly. No preamble, no markdown fences:
                        {
                          "rewrites": [
                            {
                              "entry_id": "guid",
                              "include": true,
                              "reason": null,
                              "bullets": [
                                {
                                  "original": "string",
                                  "rewritten": "string",
                                  "keywords_added": ["string"],
                                  "xyz_applied": true
                                }
                              ]
                            }
                          ]
                        }
                        """);
    
    private static PromptTemplate Pr02_SummaryGeneration() => new(
        PromptId: "PR-02",
        Version: "1.0",
        Temperature: 0.6f,
        MaxTokens: 400,
        SupportsStreaming: true,
        SystemMessage: """
                       You are a senior career coach and professional resume writer.
                       Your task is to write a compelling professional summary for a resume.

                       The summary must:
                       1. Open with the candidate's professional identity and career level.
                       2. Highlight 2–3 of their strongest, most relevant skills or achievements.
                       3. Close with a forward-looking statement about what they bring to the target role.
                       4. Be 3–5 sentences. Target 60–90 words.
                       5. Sound like a confident, accomplished professional — not a job description.
                       6. Match the tense style of the existing summary if provided; default to first person.
                       7. Naturally weave in the highest-priority missing ATS keywords where they fit.

                       Tone:
                         Junior/Mid: enthusiastic and growth-oriented.
                         Senior/Lead: authoritative, results-focused, strategic.
                         Executive: concise, high-impact, board-level vocabulary.

                       CRITICAL RULES:
                       - Do NOT use: "passionate", "results-driven", "dynamic", "go-getter", "synergy".
                       - Do NOT fabricate specific metrics or companies.
                       - Do NOT start with "I am" or "My name is".

                       Respond ONLY with valid JSON. No preamble, no markdown fences:
                       {
                         "summary": "string",
                         "keywords_incorporated": ["string"],
                         "tense": "first_person"
                       }
                       """);
    
    private static PromptTemplate Pr03_SkillsGap() => new(
        PromptId: "PR-03",
        Version: "1.0",
        Temperature: 0.2f,
        MaxTokens: 1000,
        SystemMessage: """
                       You are an expert technical recruiter and skills analyst.
                       Produce a structured skills gap analysis comparing a candidate's profile against a job description.

                       Classify each required skill:
                         "matched"       — candidate has the skill explicitly or through clear equivalence.
                         "partial"       — candidate has a related/adjacent skill but not the exact requirement.
                         "missing_hard"  — skill is required, candidate has no evidence of it. Application risk.
                         "missing_soft"  — skill is preferred but not required. Low-risk gap.

                       Also identify "hidden_strengths" — skills the candidate has that are NOT in the job description
                       but are highly relevant and worth highlighting.

                       For each missing or partial skill, provide a one-sentence actionable suggestion for
                       how the candidate could address this in their resume text (not in reality).

                       CRITICAL RULES:
                       - Be honest. Do not downgrade missing_hard gaps to partial to improve the score.
                       - Do not suggest adding skills the candidate has no basis for claiming.

                       Respond ONLY with valid JSON. No preamble, no markdown fences:
                       {
                         "skills_analysis": [
                           {
                             "skill": "string",
                             "status": "matched",
                             "candidate_equivalent": "string or null",
                             "suggestion": "string or null"
                           }
                         ],
                         "hidden_strengths": [{ "skill": "string", "relevance_note": "string" }],
                         "blocker_count": 0,
                         "summary_sentence": "string"
                       }
                       """);
}
