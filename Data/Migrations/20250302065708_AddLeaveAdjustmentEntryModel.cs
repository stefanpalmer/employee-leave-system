using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeLeaveManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveAdjustmentEntryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveAdjustmentEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    NoOfDays = table.Column<int>(type: "int", nullable: false),
                    LeaveAdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdjustmentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveAdjustmentEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveAdjustmentEntries_DropdownOptions_AdjustmentTypeId",
                        column: x => x.AdjustmentTypeId,
                        principalTable: "DropdownOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveAdjustmentEntries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAdjustmentEntries_AdjustmentTypeId",
                table: "LeaveAdjustmentEntries",
                column: "AdjustmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAdjustmentEntries_EmployeeId",
                table: "LeaveAdjustmentEntries",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveAdjustmentEntries");
        }
    }
}
