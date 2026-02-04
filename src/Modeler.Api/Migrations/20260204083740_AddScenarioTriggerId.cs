using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioTriggerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggerId",
                table: "Scenarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriggerKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTriggerLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TriggerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTriggerLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTriggerLinks_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTriggerLinks_Triggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Triggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_TriggerId",
                table: "Scenarios",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventKey",
                table: "Events",
                column: "EventKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTriggerLinks_EventId_TriggerId",
                table: "EventTriggerLinks",
                columns: new[] { "EventId", "TriggerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTriggerLinks_TriggerId",
                table: "EventTriggerLinks",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_TriggerKey",
                table: "Triggers",
                column: "TriggerKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenarios_Triggers_TriggerId",
                table: "Scenarios",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenarios_Triggers_TriggerId",
                table: "Scenarios");

            migrationBuilder.DropTable(
                name: "EventTriggerLinks");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Scenarios_TriggerId",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "TriggerId",
                table: "Scenarios");
        }
    }
}
