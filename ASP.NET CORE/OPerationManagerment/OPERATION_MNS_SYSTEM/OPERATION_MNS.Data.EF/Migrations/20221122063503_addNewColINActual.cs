using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addNewColINActual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material_In_MES",
                table: "GOC_PLAN");

            migrationBuilder.AddColumn<string>(
                name: "Material_SAP_CODE",
                table: "INVENTORY_ACTUAL",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material_SAP_CODE",
                table: "INVENTORY_ACTUAL");

            migrationBuilder.AddColumn<string>(
                name: "Material_In_MES",
                table: "GOC_PLAN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
