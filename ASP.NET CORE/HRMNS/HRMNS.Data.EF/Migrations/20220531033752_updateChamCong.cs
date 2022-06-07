using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateChamCong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passed",
                table: "DANGKY_OT_NHANVIEN");

            migrationBuilder.AddColumn<string>(
                name: "UserHandle",
                table: "CHAM_CONG_LOG",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserHandle",
                table: "CHAM_CONG_LOG");

            migrationBuilder.AddColumn<string>(
                name: "Passed",
                table: "DANGKY_OT_NHANVIEN",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }
    }
}
