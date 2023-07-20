using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateLxe2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NgaySinh",
                table: "LAI_XE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "LAI_XE");
        }
    }
}
