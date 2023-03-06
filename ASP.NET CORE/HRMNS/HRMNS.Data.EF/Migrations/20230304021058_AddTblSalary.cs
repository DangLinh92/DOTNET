using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class AddTblSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_SALARY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivingAllowance = table.Column<decimal>(nullable: false),
                    PositionAllowance = table.Column<decimal>(nullable: false),
                    AbilityAllowance = table.Column<decimal>(nullable: false),
                    FullAttendanceSupport = table.Column<decimal>(nullable: false),
                    SeniorityAllowance = table.Column<decimal>(nullable: false),
                    HarmfulAllowance = table.Column<decimal>(nullable: false),
                    IncentiveBase = table.Column<decimal>(nullable: false),
                    IncentiveLanguage = table.Column<decimal>(nullable: false),
                    IncentiveTechnical = table.Column<decimal>(nullable: false),
                    IncentiveOther = table.Column<decimal>(nullable: false),
                    Year = table.Column<string>(nullable: true),
                    IncentiveSixMonth1 = table.Column<decimal>(nullable: false),
                    IncentiveSixMonth2 = table.Column<decimal>(nullable: false),
                    CI_SixMonth1 = table.Column<decimal>(nullable: false),
                    CI_SixMonth2 = table.Column<decimal>(nullable: false),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    EventDate = table.Column<string>(maxLength: 50, nullable: true),
                    MaEventParent = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_SALARY_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SALARY_MaNV",
                table: "HR_SALARY",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_SALARY");
        }
    }
}
