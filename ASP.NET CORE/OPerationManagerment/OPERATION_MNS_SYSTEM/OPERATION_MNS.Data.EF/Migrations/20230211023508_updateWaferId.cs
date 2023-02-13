using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateWaferId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WaferId_Mes",
                table: "POST_OPERATION_SHIPPING",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaferId_Mes",
                table: "POST_OPERATION_SHIPPING");
        }
    }
}
