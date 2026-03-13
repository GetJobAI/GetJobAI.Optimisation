using Google.GenAI.Types;
using GeminiType = Google.GenAI.Types.Type;

namespace GetJobAI.Optimisation.OptimisationService;

public static class ResponseSchemas
{
    // PR-01
    public static readonly Schema WorkExperienceResponseSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["entry_id", "include", "bullets"],
        Properties = new Dictionary<string, Schema>
        {
            ["entry_id"] = new() { Type = GeminiType.String },
            ["include"]  = new() { Type = GeminiType.Boolean },
            ["reason"]   = new() { Type = GeminiType.String, Nullable = true },
            ["bullets"]  = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["original", "rewritten", "keywords_added", "xyz_applied"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["original"]       = new() { Type = GeminiType.String },
                        ["rewritten"]      = new() { Type = GeminiType.String },
                        ["keywords_added"] = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } },
                        ["xyz_applied"]    = new() { Type = GeminiType.Boolean }
                    }
                }
            }
        }
    };

    // PR-02
    public static readonly Schema SummarySuggestionSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["rewritten", "keywords_incorporated"],
        Properties = new Dictionary<string, Schema>
        {
            ["original"]              = new() { Type = GeminiType.String },
            ["rewritten"]             = new() { Type = GeminiType.String },
            ["keywords_incorporated"] = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } }
        }
    };

    // PR-03
    public static readonly Schema SkillsGapSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["skills_analysis", "hidden_strengths", "blocker_count", "summary_sentence"],
        Properties = new Dictionary<string, Schema>
        {
            ["skills_analysis"] = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["skill", "status"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["skill"]                = new() { Type = GeminiType.String },
                        ["status"]               = new() { Type = GeminiType.String },
                        ["candidate_equivalent"] = new() { Type = GeminiType.String, Nullable = true },
                        ["suggestion"]           = new() { Type = GeminiType.String, Nullable = true }
                    }
                }
            },
            ["hidden_strengths"] = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["skill", "relevance_note"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["skill"]          = new() { Type = GeminiType.String },
                        ["relevance_note"] = new() { Type = GeminiType.String }
                    }
                }
            },
            ["blocker_count"]    = new() { Type = GeminiType.Integer },
            ["summary_sentence"] = new() { Type = GeminiType.String }
        }
    };

    // PR-04
    public static readonly Schema SectionRelevancySchema = new()
    {
        Type = GeminiType.Object,
        Required = ["publications", "activities", "additional_sections"],
        Properties = new Dictionary<string, Schema>
        {
            ["publications"] = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["entry_id", "include"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["entry_id"] = new() { Type = GeminiType.String },
                        ["include"]  = new() { Type = GeminiType.Boolean },
                        ["reason"]   = new() { Type = GeminiType.String, Nullable = true }
                    }
                }
            },
            ["activities"] = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["entry_id", "include", "highlights_rewritten"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["entry_id"]             = new() { Type = GeminiType.String },
                        ["include"]              = new() { Type = GeminiType.Boolean },
                        ["reason"]               = new() { Type = GeminiType.String, Nullable = true },
                        ["highlights_rewritten"] = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } }
                    }
                }
            },
            ["additional_sections"] = new()
            {
                Type  = GeminiType.Array,
                Items = new Schema
                {
                    Type     = GeminiType.Object,
                    Required = ["entry_id", "include"],
                    Properties = new Dictionary<string, Schema>
                    {
                        ["entry_id"]     = new() { Type = GeminiType.String },
                        ["section_type"] = new() { Type = GeminiType.String, Nullable = true },
                        ["include"]      = new() { Type = GeminiType.Boolean },
                        ["reason"]       = new() { Type = GeminiType.String, Nullable = true }
                    }
                }
            }
        }
    };

    // PR-05
    public static readonly Schema AtsExplanationSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["headline_message", "score_label", "what_is_working", "biggest_opportunity", "top_quick_wins", "encouragement"],
        Properties = new Dictionary<string, Schema>
        {
            ["headline_message"]    = new() { Type = GeminiType.String },
            ["score_label"]         = new() { Type = GeminiType.String },
            ["what_is_working"]     = new() { Type = GeminiType.String },
            ["biggest_opportunity"] = new() { Type = GeminiType.String },
            ["top_quick_wins"]      = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } },
            ["encouragement"]       = new() { Type = GeminiType.String }
        }
    };

    // PR-06
    public static readonly Schema ActivitySuggestionSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["include", "highlights_rewritten"],
        Properties = new Dictionary<string, Schema>
        {
            ["include"]              = new() { Type = GeminiType.Boolean },
            ["reason"]               = new() { Type = GeminiType.String, Nullable = true },
            ["highlights_rewritten"] = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } }
        }
    };

    // PR-07
    public static readonly Schema XyzDetectSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["needs_xyz", "reasoning"],
        Properties = new Dictionary<string, Schema>
        {
            ["needs_xyz"]         = new() { Type = GeminiType.Boolean },
            ["missing_component"] = new() { Type = GeminiType.String, Nullable = true },
            ["question"]          = new() { Type = GeminiType.String, Nullable = true },
            ["reasoning"]         = new() { Type = GeminiType.String }
        }
    };

    // PR-08
    public static readonly Schema XyzRewriteSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["rewritten_bullet", "xyz_breakdown", "used_original"],
        Properties = new Dictionary<string, Schema>
        {
            ["rewritten_bullet"] = new() { Type = GeminiType.String },
            ["xyz_breakdown"] = new()
            {
                Type     = GeminiType.Object,
                Required = ["x", "y", "z"],
                Properties = new Dictionary<string, Schema>
                {
                    ["x"] = new() { Type = GeminiType.String },
                    ["y"] = new() { Type = GeminiType.String },
                    ["z"] = new() { Type = GeminiType.String }
                }
            },
            ["user_data_used"] = new() { Type = GeminiType.String, Nullable = true },
            ["used_original"]  = new() { Type = GeminiType.Boolean }
        }
    };

    // PR-09
    public static readonly Schema CoverLetterSchema = new()
    {
        Type = GeminiType.Object,
        Required = ["cover_letter", "word_count", "salutation_used", "key_points_made"],
        Properties = new Dictionary<string, Schema>
        {
            ["cover_letter"]    = new() { Type = GeminiType.String },
            ["word_count"]      = new() { Type = GeminiType.Integer },
            ["salutation_used"] = new() { Type = GeminiType.String },
            ["key_points_made"] = new() { Type = GeminiType.Array, Items = new Schema { Type = GeminiType.String } }
        }
    };
}