using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDetailSalaryHis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BauThaiSan",
                table: "BANGLUONGCHITIET_HISTORY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "BANGLUONGCHITIET_HISTORY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ThoiGianChuaNghi",
                table: "BANGLUONGCHITIET_HISTORY",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BauThaiSan",
                table: "BANGLUONGCHITIET_HISTORY");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "BANGLUONGCHITIET_HISTORY");

            migrationBuilder.DropColumn(
                name: "ThoiGianChuaNghi",
                table: "BANGLUONGCHITIET_HISTORY");
        }
    }
}
