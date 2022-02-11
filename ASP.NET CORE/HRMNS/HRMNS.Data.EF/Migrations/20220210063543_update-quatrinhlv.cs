using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatequatrinhlv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChuyenChucVu",
                table: "HR_QUATRINHLAMVIEC");

            migrationBuilder.DropColumn(
                name: "ChuyenPhongBan",
                table: "HR_QUATRINHLAMVIEC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChuyenChucVu",
                table: "HR_QUATRINHLAMVIEC",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChuyenPhongBan",
                table: "HR_QUATRINHLAMVIEC",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
