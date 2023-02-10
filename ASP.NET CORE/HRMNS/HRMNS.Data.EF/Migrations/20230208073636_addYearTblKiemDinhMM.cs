using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addYearTblKiemDinhMM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "EHS_KEHOACH_KIEMDINH_MAYMOC",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "EHS_KEHOACH_KIEMDINH_MAYMOC");
        }
    }
}
