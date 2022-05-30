using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateOTNVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApproveLV2",
                table: "DANGKY_OT_NHANVIEN",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproveLV3",
                table: "DANGKY_OT_NHANVIEN",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveLV2",
                table: "DANGKY_OT_NHANVIEN");

            migrationBuilder.DropColumn(
                name: "ApproveLV3",
                table: "DANGKY_OT_NHANVIEN");
        }
    }
}
