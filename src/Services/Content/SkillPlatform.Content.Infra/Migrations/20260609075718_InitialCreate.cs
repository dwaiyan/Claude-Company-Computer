using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillPlatform.Content.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tech_trees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tech_trees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tech_trees_tech_trees_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tech_trees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tech_nodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tech_nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tech_nodes_tech_nodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tech_nodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tech_nodes_tech_trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "tech_trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interview_questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AnswerTip = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interview_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_interview_questions_tech_nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "tech_nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resources_tech_nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "tech_nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_interview_questions_NodeId",
                table: "interview_questions",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_resources_NodeId",
                table: "resources",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_tech_nodes_ParentId",
                table: "tech_nodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_tech_nodes_TreeId",
                table: "tech_nodes",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_tech_trees_ParentId",
                table: "tech_trees",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interview_questions");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "tech_nodes");

            migrationBuilder.DropTable(
                name: "tech_trees");
        }
    }
}
