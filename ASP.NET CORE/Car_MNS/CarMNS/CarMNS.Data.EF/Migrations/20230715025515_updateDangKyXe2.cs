using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateDangKyXe2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lxe_BienSo",
                table: "DANG_KY_XE",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lxe_BienSo",
                table: "DANG_KY_XE");
        }
    }
}
