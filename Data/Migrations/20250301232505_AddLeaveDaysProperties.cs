using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeLeaveManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveDaysProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysRemaining",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeaveDays",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysRemaining",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LeaveDays",
                table: "Employees");
        }
    }
}
