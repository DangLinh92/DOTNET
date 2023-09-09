using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class UpdateTBLDailyPlanLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Test_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Test_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Test_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "OQC_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "OQC_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "OQC_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Mold_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Mold_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Mold_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Marking_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Marking_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Marking_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dam_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dam_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dam_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VisualInspection_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Test_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Test_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Test_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OQC_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OQC_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OQC_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mold_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mold_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mold_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Marking_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Marking_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Marking_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Grinding_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dicing_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dam_WIP",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dam_PROD",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dam_KHSX",
                table: "DAILY_PLAN_DATA_LFEM",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
