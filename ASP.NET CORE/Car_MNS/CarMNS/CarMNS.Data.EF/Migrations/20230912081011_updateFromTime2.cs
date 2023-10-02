using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateFromTime2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTimePlan2",
                table: "DANG_KY_XE");

            migrationBuilder.AddColumn<string>(
                name: "ToTimePlan2",
                table: "DANG_KY_XE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToTimePlan2",
                table: "DANG_KY_XE");

            migrationBuilder.AddColumn<string>(
                name: "FromTimePlan2",
                table: "DANG_KY_XE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
