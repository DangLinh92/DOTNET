using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDCCong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoiTuongPhuCapDocHai",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiTuongPhuCapDocHai",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiTraVaoLuongThang2",
                table: "DC_CHAM_CONG",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiTuongPhuCapDocHai",
                table: "BANGLUONGCHITIET_HISTORY",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoiTuongPhuCapDocHai",
                table: "HR_SALARY_HISTORY");

            migrationBuilder.DropColumn(
                name: "DoiTuongPhuCapDocHai",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "ChiTraVaoLuongThang2",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DoiTuongPhuCapDocHai",
                table: "BANGLUONGCHITIET_HISTORY");
        }
    }
}
