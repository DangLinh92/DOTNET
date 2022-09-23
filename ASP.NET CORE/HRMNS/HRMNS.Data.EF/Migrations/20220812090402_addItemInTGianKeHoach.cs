using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addItemInTGianKeHoach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SoLuong",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ViTri",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "ViTri",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH");
        }
    }
}
