using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class addLinkReport2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkReport2",
                table: "VOC_MST",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkReport2",
                table: "VOC_MST");
        }
    }
}
