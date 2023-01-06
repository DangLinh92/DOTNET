using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddItemInHoldLot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateCreated",
                table: "STAY_LOT_LIST",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CassetteId",
                table: "STAY_LOT_LIST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NguoiXuLy",
                table: "STAY_LOT_LIST",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CassetteId",
                table: "STAY_LOT_LIST");

            migrationBuilder.DropColumn(
                name: "NguoiXuLy",
                table: "STAY_LOT_LIST");

            migrationBuilder.AlterColumn<string>(
                name: "DateCreated",
                table: "STAY_LOT_LIST",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
