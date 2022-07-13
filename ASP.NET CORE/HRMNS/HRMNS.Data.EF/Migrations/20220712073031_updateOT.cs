using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeSoOT",
                table: "DANGKY_OT_NHANVIEN",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDung",
                table: "DANGKY_OT_NHANVIEN",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SoGioOT",
                table: "DANGKY_OT_NHANVIEN",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeSoOT",
                table: "DANGKY_OT_NHANVIEN");

            migrationBuilder.DropColumn(
                name: "NoiDung",
                table: "DANGKY_OT_NHANVIEN");

            migrationBuilder.DropColumn(
                name: "SoGioOT",
                table: "DANGKY_OT_NHANVIEN");
        }
    }
}
