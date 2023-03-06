using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatePhepNamAndDCC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SoTienChiTra",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ThoiGianChiTra",
                table: "HR_PHEP_NAM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiTraVaoLuongThang",
                table: "DC_CHAM_CONG",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoTienChiTra",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "ThoiGianChiTra",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "ChiTraVaoLuongThang",
                table: "DC_CHAM_CONG");
        }
    }
}
