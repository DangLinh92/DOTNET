using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateBangCongEx2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CM_91_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.AddColumn<string>(
                name: "KeyDanhMuc",
                table: "HR_SALARY_DANHMUC_PHATSINH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CM_90_390",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyDanhMuc",
                table: "HR_SALARY_DANHMUC_PHATSINH");

            migrationBuilder.DropColumn(
                name: "CM_90_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.AddColumn<double>(
                name: "CM_91_390",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
