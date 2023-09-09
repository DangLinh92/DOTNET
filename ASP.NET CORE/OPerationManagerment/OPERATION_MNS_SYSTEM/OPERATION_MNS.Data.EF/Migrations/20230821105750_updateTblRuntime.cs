using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateTblRuntime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "STBMin",
                table: "WARNING_LOT_RUNTIME_LFEM",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChipQTy",
                table: "WARNING_LOT_RUNTIME_LFEM",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "STBMin",
                table: "WARNING_LOT_RUNTIME_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "ChipQTy",
                table: "WARNING_LOT_RUNTIME_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
