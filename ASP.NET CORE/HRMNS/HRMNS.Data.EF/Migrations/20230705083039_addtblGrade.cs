using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addtblGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_SALARY_GRADE",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BasicSalaryStandard = table.Column<double>(nullable: false),
                    IncentiveLanguage = table.Column<double>(nullable: false),
                    BasicSalary = table.Column<double>(nullable: false),
                    LivingAllowance = table.Column<double>(nullable: false),
                    IncentiveStandard = table.Column<double>(nullable: false),
                    AttendanceAllowance = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_GRADE", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_SALARY_GRADE");
        }
    }
}
