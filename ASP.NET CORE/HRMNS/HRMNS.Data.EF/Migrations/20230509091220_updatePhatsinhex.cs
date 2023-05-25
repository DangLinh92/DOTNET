using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatePhatsinhex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTime",
                table: "HR_SALARY_PHATSINH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToTime",
                table: "HR_SALARY_PHATSINH",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTime",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "ToTime",
                table: "HR_SALARY_PHATSINH");
        }
    }
}
