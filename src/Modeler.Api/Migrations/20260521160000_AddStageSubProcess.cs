using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    [Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(Modeler.Api.Persistence.ModelerDbContext))]
    [Migration("20260521160000_AddStageSubProcess")]
    public partial class AddStageSubProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubProcessId",
                table: "Stages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_SubProcessId",
                table: "Stages",
                column: "SubProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_SubProcesses_SubProcessId",
                table: "Stages",
                column: "SubProcessId",
                principalTable: "SubProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_SubProcesses_SubProcessId",
                table: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_Stages_SubProcessId",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "SubProcessId",
                table: "Stages");
        }
    }
}
