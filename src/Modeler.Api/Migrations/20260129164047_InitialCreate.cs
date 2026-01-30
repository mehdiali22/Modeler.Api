using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtifactKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsChildOfCase = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConditionKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Expression = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    FailMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TargetArtifactId = table.Column<int>(type: "int", nullable: true),
                    ExecutorKind = table.Column<int>(type: "int", nullable: true),
                    ExecutorActorId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DefaultParamsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionCatalog_Actors_ExecutorActorId",
                        column: x => x.ExecutorActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionCatalog_Artifacts_TargetArtifactId",
                        column: x => x.TargetArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtifactId = table.Column<int>(type: "int", nullable: false),
                    FactKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    StageKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    SubProcessKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubProcesses_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConditionFactUsed",
                columns: table => new
                {
                    ConditionId = table.Column<int>(type: "int", nullable: false),
                    FactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionFactUsed", x => new { x.ConditionId, x.FactId });
                    table.ForeignKey(
                        name: "FK_ConditionFactUsed_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConditionFactUsed_Facts_FactId",
                        column: x => x.FactId,
                        principalTable: "Facts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactEnumValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactId = table.Column<int>(type: "int", nullable: false),
                    EnumKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactEnumValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactEnumValues_Facts_FactId",
                        column: x => x.FactId,
                        principalTable: "Facts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    OwnerSubdomain = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenarios_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioDecisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    DecisionKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UiActionKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioDecisions_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioFactChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    FactId = table.Column<int>(type: "int", nullable: false),
                    Op = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioFactChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioFactChanges_Facts_FactId",
                        column: x => x.FactId,
                        principalTable: "Facts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioFactChanges_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioInputArtifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    ArtifactId = table.Column<int>(type: "int", nullable: false),
                    RoleKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioInputArtifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioInputArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioInputArtifacts_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioPreconditions",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    ConditionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioPreconditions", x => new { x.ScenarioId, x.ConditionId });
                    table.ForeignKey(
                        name: "FK_ScenarioPreconditions_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioPreconditions_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioDecisionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioDecisionId = table.Column<int>(type: "int", nullable: false),
                    OptionKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ConditionIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProducedEventIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioDecisionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioDecisionOptions_ScenarioDecisions_ScenarioDecisionId",
                        column: x => x.ScenarioDecisionId,
                        principalTable: "ScenarioDecisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DecisionOptionFactChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioDecisionOptionId = table.Column<int>(type: "int", nullable: false),
                    FactId = table.Column<int>(type: "int", nullable: false),
                    Op = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionOptionFactChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionOptionFactChanges_Facts_FactId",
                        column: x => x.FactId,
                        principalTable: "Facts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DecisionOptionFactChanges_ScenarioDecisionOptions_ScenarioDecisionOptionId",
                        column: x => x.ScenarioDecisionOptionId,
                        principalTable: "ScenarioDecisionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionCatalog_ActionKey",
                table: "ActionCatalog",
                column: "ActionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionCatalog_ExecutorActorId",
                table: "ActionCatalog",
                column: "ExecutorActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionCatalog_TargetArtifactId",
                table: "ActionCatalog",
                column: "TargetArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_ActorKey",
                table: "Actors",
                column: "ActorKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_ArtifactKey",
                table: "Artifacts",
                column: "ArtifactKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConditionFactUsed_FactId",
                table: "ConditionFactUsed",
                column: "FactId");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_ConditionKey",
                table: "Conditions",
                column: "ConditionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecisionOptionFactChanges_FactId",
                table: "DecisionOptionFactChanges",
                column: "FactId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionOptionFactChanges_ScenarioDecisionOptionId",
                table: "DecisionOptionFactChanges",
                column: "ScenarioDecisionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryTerms_TermKey",
                table: "DictionaryTerms",
                column: "TermKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactEnumValues_FactId_EnumKey",
                table: "FactEnumValues",
                columns: new[] { "FactId", "EnumKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facts_ArtifactId",
                table: "Facts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_Facts_FactKey",
                table: "Facts",
                column: "FactKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ProcessKey",
                table: "Processes",
                column: "ProcessKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioDecisionOptions_ScenarioDecisionId_OptionKey",
                table: "ScenarioDecisionOptions",
                columns: new[] { "ScenarioDecisionId", "OptionKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioDecisions_ScenarioId_DecisionKey",
                table: "ScenarioDecisions",
                columns: new[] { "ScenarioId", "DecisionKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioFactChanges_FactId",
                table: "ScenarioFactChanges",
                column: "FactId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioFactChanges_ScenarioId",
                table: "ScenarioFactChanges",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioInputArtifacts_ArtifactId",
                table: "ScenarioInputArtifacts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioInputArtifacts_ScenarioId",
                table: "ScenarioInputArtifacts",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioPreconditions_ConditionId",
                table: "ScenarioPreconditions",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_ScenarioKey",
                table: "Scenarios",
                column: "ScenarioKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_StageId",
                table: "Scenarios",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_ProcessId_StageKey",
                table: "Stages",
                columns: new[] { "ProcessId", "StageKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubProcesses_ProcessId_SubProcessKey",
                table: "SubProcesses",
                columns: new[] { "ProcessId", "SubProcessKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionCatalog");

            migrationBuilder.DropTable(
                name: "ConditionFactUsed");

            migrationBuilder.DropTable(
                name: "DecisionOptionFactChanges");

            migrationBuilder.DropTable(
                name: "DictionaryTerms");

            migrationBuilder.DropTable(
                name: "FactEnumValues");

            migrationBuilder.DropTable(
                name: "ScenarioFactChanges");

            migrationBuilder.DropTable(
                name: "ScenarioInputArtifacts");

            migrationBuilder.DropTable(
                name: "ScenarioPreconditions");

            migrationBuilder.DropTable(
                name: "SubProcesses");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "ScenarioDecisionOptions");

            migrationBuilder.DropTable(
                name: "Facts");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "ScenarioDecisions");

            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Processes");
        }
    }
}
