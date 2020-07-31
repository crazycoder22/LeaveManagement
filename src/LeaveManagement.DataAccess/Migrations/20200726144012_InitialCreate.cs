using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LeaveManagement.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestType = table.Column<int>(nullable: false),
                    RequesterId = table.Column<string>(nullable: true),
                    ApproverId = table.Column<string>(nullable: true),
                    RequesterComment = table.Column<string>(nullable: true),
                    ApproverComment = table.Column<string>(nullable: true),
                    RequestDetails = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true),
                    DateOfJoining = table.Column<DateTime>(nullable: false),
                    ManagerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false),
                    NoOfDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaveBalance",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(nullable: false),
                    LeaveType = table.Column<int>(nullable: false),
                    NoOfDays = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLeaveBalance", x => new { x.EmployeeId, x.LeaveType });
                    table.ForeignKey(
                        name: "FK_EmployeeLeaveBalance_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId",
                table: "Employee",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRequest");

            migrationBuilder.DropTable(
                name: "EmployeeLeaveBalance");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
