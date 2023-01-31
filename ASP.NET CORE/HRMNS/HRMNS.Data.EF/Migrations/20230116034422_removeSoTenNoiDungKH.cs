using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class removeSoTenNoiDungKH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoTien",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "EHS_CHIPHI_BY_MONTH");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SoTien",
                table: "EHS_NOIDUNG_KEHOACH",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "EHS_CHIPHI_BY_MONTH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
