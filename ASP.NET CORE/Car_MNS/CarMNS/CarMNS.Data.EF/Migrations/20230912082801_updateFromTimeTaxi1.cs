using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateFromTimeTaxi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTimePlan1",
                table: "DANG_KY_XE_TAXI",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToTimePlan1",
                table: "DANG_KY_XE_TAXI",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTimePlan1",
                table: "DANG_KY_XE_TAXI");

            migrationBuilder.DropColumn(
                name: "ToTimePlan1",
                table: "DANG_KY_XE_TAXI");
        }
    }
}
