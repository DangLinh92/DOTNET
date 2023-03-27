using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateMTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hold_Flag",
                table: "VIEW_WIP_POST_WLP",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostOperationInputWait",
                table: "VIEW_WIP_POST_WLP",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "MATERIAL_TO_SAP",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hold_Flag",
                table: "VIEW_WIP_POST_WLP");

            migrationBuilder.DropColumn(
                name: "PostOperationInputWait",
                table: "VIEW_WIP_POST_WLP");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "MATERIAL_TO_SAP");
        }
    }
}
