using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateHoldHisLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HoldCode",
                table: "STAY_LOT_LIST_HISTORY_LFEM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldComment",
                table: "STAY_LOT_LIST_HISTORY_LFEM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldTime",
                table: "STAY_LOT_LIST_HISTORY_LFEM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldUserName",
                table: "STAY_LOT_LIST_HISTORY_LFEM",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoldCode",
                table: "STAY_LOT_LIST_HISTORY_LFEM");

            migrationBuilder.DropColumn(
                name: "HoldComment",
                table: "STAY_LOT_LIST_HISTORY_LFEM");

            migrationBuilder.DropColumn(
                name: "HoldTime",
                table: "STAY_LOT_LIST_HISTORY_LFEM");

            migrationBuilder.DropColumn(
                name: "HoldUserName",
                table: "STAY_LOT_LIST_HISTORY_LFEM");
        }
    }
}
