using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatesalaryGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradeYear",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaBoPhanEx",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "GradeYear",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "MaBoPhanEx",
                table: "HR_SALARY");
        }
    }
}
