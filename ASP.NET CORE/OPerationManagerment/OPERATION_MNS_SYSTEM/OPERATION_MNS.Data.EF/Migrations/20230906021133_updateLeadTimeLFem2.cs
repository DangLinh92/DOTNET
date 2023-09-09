using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateLeadTimeLFem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DisplayOrder",
                table: "LEAD_TIME_LFEM",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HoldTime",
                table: "LEAD_TIME_LFEM",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LeadTimeInOut",
                table: "LEAD_TIME_LFEM",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LeadTimeStartEnd",
                table: "LEAD_TIME_LFEM",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "LEAD_TIME_LFEM");

            migrationBuilder.DropColumn(
                name: "HoldTime",
                table: "LEAD_TIME_LFEM");

            migrationBuilder.DropColumn(
                name: "LeadTimeInOut",
                table: "LEAD_TIME_LFEM");

            migrationBuilder.DropColumn(
                name: "LeadTimeStartEnd",
                table: "LEAD_TIME_LFEM");
        }
    }
}
