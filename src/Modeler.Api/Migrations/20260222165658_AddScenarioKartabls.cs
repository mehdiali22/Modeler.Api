using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioKartabls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kartabls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KartablKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OwnerSubdomain = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartabls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KartablRoutingRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OwnerSubdomain = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    FromKartablId = table.Column<int>(type: "int", nullable: true),
                    TargetKartablId = table.Column<int>(type: "int", nullable: false),
                    ConditionIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleFa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KartablRoutingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KartablRoutingRules_Kartabls_FromKartablId",
                        column: x => x.FromKartablId,
                        principalTable: "Kartabls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KartablRoutingRules_Kartabls_TargetKartablId",
                        column: x => x.TargetKartablId,
                        principalTable: "Kartabls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioKartabls",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    KartablId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioKartabls", x => new { x.ScenarioId, x.KartablId });
                    table.ForeignKey(
                        name: "FK_ScenarioKartabls_Kartabls_KartablId",
                        column: x => x.KartablId,
                        principalTable: "Kartabls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioKartabls_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KartablRoutingRules_FromKartablId",
                table: "KartablRoutingRules",
                column: "FromKartablId");

            migrationBuilder.CreateIndex(
                name: "IX_KartablRoutingRules_RuleKey",
                table: "KartablRoutingRules",
                column: "RuleKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KartablRoutingRules_TargetKartablId",
                table: "KartablRoutingRules",
                column: "TargetKartablId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartabls_KartablKey",
                table: "Kartabls",
                column: "KartablKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioKartabls_KartablId",
                table: "ScenarioKartabls",
                column: "KartablId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KartablRoutingRules");

            migrationBuilder.DropTable(
                name: "ScenarioKartabls");

            migrationBuilder.DropTable(
                name: "Kartabls");
        }
    }
}
