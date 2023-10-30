using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateHotrosinhly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoPhan",
                table: "HOTRO_SINH_LY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ThoiGianChuaNghi",
                table: "HOTRO_SINH_LY",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoPhan",
                table: "HOTRO_SINH_LY");

            migrationBuilder.DropColumn(
                name: "ThoiGianChuaNghi",
                table: "HOTRO_SINH_LY");
        }
    }
}
