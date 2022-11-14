using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateEHSNoiDungDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaHieuMayKiemTra",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SoTien",
                table: "EHS_NOIDUNG_KEHOACH",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TienDoHoanThanh",
                table: "EHS_NOIDUNG_KEHOACH",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "MaHieuMayKiemTra",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "SoTien",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "TienDoHoanThanh",
                table: "EHS_NOIDUNG_KEHOACH");
        }
    }
}
