using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class addFromTime2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTimePlan1",
                table: "DANG_KY_XE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromTimePlan2",
                table: "DANG_KY_XE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTimePlan1",
                table: "DANG_KY_XE");

            migrationBuilder.DropColumn(
                name: "FromTimePlan2",
                table: "DANG_KY_XE");
        }
    }
}
