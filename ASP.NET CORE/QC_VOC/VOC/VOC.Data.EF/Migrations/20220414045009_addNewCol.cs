using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class addNewCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "VOC_ONSITE",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "VOC_ONSITE");
        }
    }
}
