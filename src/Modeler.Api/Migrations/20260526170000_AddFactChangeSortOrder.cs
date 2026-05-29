using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    [Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(Modeler.Api.Persistence.ModelerDbContext))]
    [Migration("20260526170000_AddFactChangeSortOrder")]
    public partial class AddFactChangeSortOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ScenarioFactChanges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "DecisionOptionFactChanges",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ScenarioFactChanges");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "DecisionOptionFactChanges");
        }
    }
}
