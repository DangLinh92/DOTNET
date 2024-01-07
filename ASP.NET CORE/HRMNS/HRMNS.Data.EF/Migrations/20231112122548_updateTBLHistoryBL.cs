using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateTBLHistoryBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThanhToanLuong",
                table: "BANGLUONGCHITIET_HISTORY",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThanhToanLuong",
                table: "BANGLUONGCHITIET_HISTORY");
        }
    }
}
