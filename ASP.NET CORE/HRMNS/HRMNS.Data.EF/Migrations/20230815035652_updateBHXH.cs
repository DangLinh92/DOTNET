using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateBHXH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayThamGia",
                table: "HR_BHXH");

            migrationBuilder.AddColumn<string>(
                name: "NgayBatDau",
                table: "HR_BHXH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhanLoai",
                table: "HR_BHXH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThangThamGia",
                table: "HR_BHXH",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "HR_BHXH");

            migrationBuilder.DropColumn(
                name: "PhanLoai",
                table: "HR_BHXH");

            migrationBuilder.DropColumn(
                name: "ThangThamGia",
                table: "HR_BHXH");

            migrationBuilder.AddColumn<string>(
                name: "NgayThamGia",
                table: "HR_BHXH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
