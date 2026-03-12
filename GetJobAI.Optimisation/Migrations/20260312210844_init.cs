using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetJobAI.Optimisation.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "optimisations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_analysis_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    company_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    overall_score = table.Column<int>(type: "integer", nullable: false),
                    score_keyword_earned = table.Column<short>(type: "smallint", nullable: false),
                    score_keyword_max = table.Column<short>(type: "smallint", nullable: false),
                    score_skill_earned = table.Column<short>(type: "smallint", nullable: false),
                    score_skill_max = table.Column<short>(type: "smallint", nullable: false),
                    score_format_earned = table.Column<short>(type: "smallint", nullable: false),
                    score_format_max = table.Column<short>(type: "smallint", nullable: false),
                    score_experience_earned = table.Column<short>(type: "smallint", nullable: false),
                    score_experience_max = table.Column<short>(type: "smallint", nullable: false),
                    ats_details = table.Column<string>(type: "jsonb", nullable: true),
                    ats_explanation = table.Column<string>(type: "jsonb", nullable: true),
                    skills_gap = table.Column<string>(type: "jsonb", nullable: true),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resumes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    candidate_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    existing_summary = table.Column<string>(type: "text", nullable: true),
                    detected_language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resumes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_activity_suggestions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    optimisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    include = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    highlights_rewritten = table.Column<List<string>>(type: "text[]", nullable: false),
                    accepted = table.Column<bool>(type: "boolean", nullable: true),
                    rejection_hint = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    rewrite_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_activity_suggestions", x => x.id);
                    table.ForeignKey(
                        name: "FK_optimisation_activity_suggestions_optimisations_optimisatio~",
                        column: x => x.optimisation_id,
                        principalTable: "optimisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_cover_letters",
                columns: table => new
                {
                    optimisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cover_letter = table.Column<string>(type: "text", nullable: false),
                    word_count = table.Column<int>(type: "integer", nullable: false),
                    salutation_used = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    key_points_made = table.Column<List<string>>(type: "text[]", nullable: false),
                    accepted = table.Column<bool>(type: "boolean", nullable: true),
                    rewrite_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    generated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_cover_letters", x => x.optimisation_id);
                    table.ForeignKey(
                        name: "FK_optimisation_cover_letters_optimisations_optimisation_id",
                        column: x => x.optimisation_id,
                        principalTable: "optimisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_section_suggestions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    optimisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    include = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    accepted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_section_suggestions", x => x.id);
                    table.ForeignKey(
                        name: "FK_optimisation_section_suggestions_optimisations_optimisation~",
                        column: x => x.optimisation_id,
                        principalTable: "optimisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_summary_suggestions",
                columns: table => new
                {
                    optimisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original = table.Column<string>(type: "text", nullable: false),
                    rewritten = table.Column<string>(type: "text", nullable: false),
                    keywords_incorporated = table.Column<List<string>>(type: "text[]", nullable: false),
                    accepted = table.Column<bool>(type: "boolean", nullable: true),
                    rejection_hint = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    rewrite_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_summary_suggestions", x => x.optimisation_id);
                    table.ForeignKey(
                        name: "FK_optimisation_summary_suggestions_optimisations_optimisation~",
                        column: x => x.optimisation_id,
                        principalTable: "optimisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_work_experience_suggestions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    optimisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    include = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    accepted = table.Column<bool>(type: "boolean", nullable: true),
                    rejection_hint = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    rewrite_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_work_experience_suggestions", x => x.id);
                    table.ForeignKey(
                        name: "FK_optimisation_work_experience_suggestions_optimisations_opti~",
                        column: x => x.optimisation_id,
                        principalTable: "optimisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resume_activities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    organization = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    role = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    highlights = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resume_activities", x => x.id);
                    table.ForeignKey(
                        name: "FK_resume_activities_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resume_additional_sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    content_json = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resume_additional_sections", x => x.id);
                    table.ForeignKey(
                        name: "FK_resume_additional_sections_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resume_publications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    publisher = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    publication_date = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resume_publications", x => x.id);
                    table.ForeignKey(
                        name: "FK_resume_publications_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resume_skills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    skill_name_raw = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    proficiency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resume_skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_resume_skills_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resume_work_experiences",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    company_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    start_date = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    end_date = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    bullets = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resume_work_experiences", x => x.id);
                    table.ForeignKey(
                        name: "FK_resume_work_experiences_resumes_resume_id",
                        column: x => x.resume_id,
                        principalTable: "resumes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optimisation_bullet_suggestions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_experience_suggestion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original = table.Column<string>(type: "text", nullable: false),
                    rewritten = table.Column<string>(type: "text", nullable: false),
                    keywords_added = table.Column<List<string>>(type: "text[]", nullable: false),
                    xyz_applied = table.Column<bool>(type: "boolean", nullable: false),
                    accepted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optimisation_bullet_suggestions", x => x.id);
                    table.ForeignKey(
                        name: "FK_optimisation_bullet_suggestions_optimisation_work_experienc~",
                        column: x => x.work_experience_suggestion_id,
                        principalTable: "optimisation_work_experience_suggestions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_optimisation_activity_suggestions_optimisation_entry",
                table: "optimisation_activity_suggestions",
                columns: new[] { "optimisation_id", "entry_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_optimisation_bullet_suggestions_work_experience_id",
                table: "optimisation_bullet_suggestions",
                column: "work_experience_suggestion_id");

            migrationBuilder.CreateIndex(
                name: "ix_optimisation_section_suggestions_optimisation_category_entry",
                table: "optimisation_section_suggestions",
                columns: new[] { "optimisation_id", "category", "entry_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_optimisation_work_experience_suggestions_optimisation_entry",
                table: "optimisation_work_experience_suggestions",
                columns: new[] { "optimisation_id", "entry_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_optimisations_created_at",
                table: "optimisations",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_optimisations_job_analysis_id",
                table: "optimisations",
                column: "job_analysis_id");

            migrationBuilder.CreateIndex(
                name: "ix_optimisations_resume_id",
                table: "optimisations",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_optimisations_status",
                table: "optimisations",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_resume_activities_resume_id",
                table: "resume_activities",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_resume_additional_sections_resume_id",
                table: "resume_additional_sections",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_resume_publications_resume_id",
                table: "resume_publications",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_resume_skills_resume_id",
                table: "resume_skills",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_resume_work_experiences_resume_id",
                table: "resume_work_experiences",
                column: "resume_id");

            migrationBuilder.CreateIndex(
                name: "ix_resumes_user_id",
                table: "resumes",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "optimisation_activity_suggestions");

            migrationBuilder.DropTable(
                name: "optimisation_bullet_suggestions");

            migrationBuilder.DropTable(
                name: "optimisation_cover_letters");

            migrationBuilder.DropTable(
                name: "optimisation_section_suggestions");

            migrationBuilder.DropTable(
                name: "optimisation_summary_suggestions");

            migrationBuilder.DropTable(
                name: "resume_activities");

            migrationBuilder.DropTable(
                name: "resume_additional_sections");

            migrationBuilder.DropTable(
                name: "resume_publications");

            migrationBuilder.DropTable(
                name: "resume_skills");

            migrationBuilder.DropTable(
                name: "resume_work_experiences");

            migrationBuilder.DropTable(
                name: "optimisation_work_experience_suggestions");

            migrationBuilder.DropTable(
                name: "resumes");

            migrationBuilder.DropTable(
                name: "optimisations");
        }
    }
}
