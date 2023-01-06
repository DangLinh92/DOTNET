using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateYield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "YIELD_OF_MODEL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateModified",
                table: "YIELD_OF_MODEL",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "YIELD_OF_MODEL",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserModified",
                table: "YIELD_OF_MODEL",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "YIELD_OF_MODEL");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "YIELD_OF_MODEL");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "YIELD_OF_MODEL");

            migrationBuilder.DropColumn(
                name: "UserModified",
                table: "YIELD_OF_MODEL");
        }
    }
}
