using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateApprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApproveL1",
                table: "PERMISSION",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApproveL2",
                table: "PERMISSION",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApproveL3",
                table: "PERMISSION",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "APP_USER",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveL1",
                table: "PERMISSION");

            migrationBuilder.DropColumn(
                name: "ApproveL2",
                table: "PERMISSION");

            migrationBuilder.DropColumn(
                name: "ApproveL3",
                table: "PERMISSION");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "APP_USER");
        }
    }
}
