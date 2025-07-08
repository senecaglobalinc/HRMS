using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Employee.Database.Migrations
{
    public partial class update_associateLongLeaveAndResignation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociateLongLeave_Employee_EmployeeId",
                table: "AssociateLongLeave");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonId",
                table: "AssociateResignation",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociateLongLeave_Employee_EmployeeId",
                table: "AssociateLongLeave",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociateResignation_Employee_EmployeeId",
                table: "AssociateResignation",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociateLongLeave_Employee_EmployeeId",
                table: "AssociateLongLeave");

            migrationBuilder.DropForeignKey(
                name: "FK_AssociateResignation_Employee_EmployeeId",
                table: "AssociateResignation");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonId",
                table: "AssociateResignation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociateLongLeave_Employee_EmployeeId",
                table: "AssociateLongLeave",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            
        }
    }
}
