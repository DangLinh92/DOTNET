using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateDangkyxe_dchi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaDiemDi",
                table: "DANG_KY_XE");

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDen_SoNha",
                table: "DANG_KY_XE",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaDiemDen_SoNha",
                table: "DANG_KY_XE");

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDi",
                table: "DANG_KY_XE",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
