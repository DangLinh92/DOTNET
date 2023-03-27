using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class UpdateInventoryActual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Shipping",
                table: "INVENTORY_ACTUAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "St_Packing_Label",
                table: "INVENTORY_ACTUAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Wait",
                table: "INVENTORY_ACTUAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "GOC_PLAN_WLP2",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "INVENTORY_ACTUAL");

            migrationBuilder.DropColumn(
                name: "St_Packing_Label",
                table: "INVENTORY_ACTUAL");

            migrationBuilder.DropColumn(
                name: "Wait",
                table: "INVENTORY_ACTUAL");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "GOC_PLAN_WLP2");
        }
    }
}
