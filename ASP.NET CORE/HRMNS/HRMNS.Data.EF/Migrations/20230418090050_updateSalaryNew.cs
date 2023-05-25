using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateSalaryNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thuong",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth1",
                table: "HR_SALARY_HISTORY");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth2",
                table: "HR_SALARY_HISTORY");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth1",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth2",
                table: "HR_SALARY");

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY_PHATSINH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY_PHATSINH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDung",
                table: "HR_SALARY_PHATSINH",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PI_SixMonth1",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PI_SixMonth2",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Thuong_Khac",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Thuong_Tet",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CI_SixMonth1",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth2",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "NoiDung",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "PI_SixMonth1",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "PI_SixMonth2",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "Thuong_Khac",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "Thuong_Tet",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.AddColumn<decimal>(
                name: "Thuong",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY_HISTORY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY_HISTORY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
