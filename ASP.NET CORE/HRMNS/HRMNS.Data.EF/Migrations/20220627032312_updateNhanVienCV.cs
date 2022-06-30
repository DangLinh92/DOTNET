using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateNhanVienCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChucVu2",
                table: "HR_NHANVIEN",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrucTiepSX",
                table: "HR_NHANVIEN",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChucVu2",
                table: "HR_NHANVIEN");

            migrationBuilder.DropColumn(
                name: "TrucTiepSX",
                table: "HR_NHANVIEN");
        }
    }
}
