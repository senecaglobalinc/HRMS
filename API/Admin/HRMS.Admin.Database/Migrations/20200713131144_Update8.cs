using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HRMS.Admin.Database.Migrations
{
    public partial class Update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetencySkills",
                columns: table => new
                {
                    CompetencySkillId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleMasterId = table.Column<int>(nullable: true),
                    CompetencyAreaId = table.Column<int>(nullable: true),
                    SkillId = table.Column<int>(nullable: true),
                    SkillGroupId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SystemInfo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true),
                    ProficiencyLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetencySkills", x => x.CompetencySkillId);
                    table.ForeignKey(
                        name: "FK_CompetencySkills_CompetencyArea_CompetencyAreaId",
                        column: x => x.CompetencyAreaId,
                        principalTable: "CompetencyArea",
                        principalColumn: "CompetencyAreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetencySkills_ProficiencyLevel_ProficiencyLevelId",
                        column: x => x.ProficiencyLevelId,
                        principalTable: "ProficiencyLevel",
                        principalColumn: "ProficiencyLevelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetencySkills_RoleMaster_RoleMasterId",
                        column: x => x.RoleMasterId,
                        principalTable: "RoleMaster",
                        principalColumn: "RoleMasterID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetencySkills_SkillGroup_SkillGroupId",
                        column: x => x.SkillGroupId,
                        principalTable: "SkillGroup",
                        principalColumn: "SkillGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetencySkills_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetencySkills_CompetencyAreaId",
                table: "CompetencySkills",
                column: "CompetencyAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetencySkills_ProficiencyLevelId",
                table: "CompetencySkills",
                column: "ProficiencyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetencySkills_RoleMasterId",
                table: "CompetencySkills",
                column: "RoleMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetencySkills_SkillGroupId",
                table: "CompetencySkills",
                column: "SkillGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetencySkills_SkillId",
                table: "CompetencySkills",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetencySkills");
        }
    }
}
