using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateTimeDailyWK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTime",
                table: "DAILY_TIME_WORKING",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToTime",
                table: "DAILY_TIME_WORKING",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTime",
                table: "DAILY_TIME_WORKING");

            migrationBuilder.DropColumn(
                name: "ToTime",
                table: "DAILY_TIME_WORKING");
        }
    }
}
