using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contents",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "EVENT_SHEDULE_PARENT",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "EVENT_SHEDULE_PARENT");

            migrationBuilder.AddColumn<string>(
                name: "Contents",
                table: "EVENT_SHEDULE_PARENT",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
