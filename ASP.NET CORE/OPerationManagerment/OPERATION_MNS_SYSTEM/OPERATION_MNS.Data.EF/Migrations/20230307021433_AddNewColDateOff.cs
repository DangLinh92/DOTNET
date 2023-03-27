using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddNewColDateOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DanhMuc",
                table: "DATE_OFF_LINE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER",
                table: "DATE_OFF_LINE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DanhMuc",
                table: "DATE_OFF_LINE");

            migrationBuilder.DropColumn(
                name: "OWNER",
                table: "DATE_OFF_LINE");
        }
    }
}
