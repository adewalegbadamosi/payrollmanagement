using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalaryService.Migrations
{
    public partial class updatedmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxDeduction",
                table: "Salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "month",
                table: "Salaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "year",
                table: "Salaries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxDeduction",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "month",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "year",
                table: "Salaries");
        }
    }
}
