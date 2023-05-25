using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateSalaryInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IncentiveSixMonth2",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "IncentiveSixMonth1",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "HR_SALARY_HISTORY",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IncentiveSixMonth2",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "IncentiveSixMonth1",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "HR_SALARY",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "HR_SALARY_HISTORY");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "HR_SALARY");

            migrationBuilder.AlterColumn<decimal>(
                name: "IncentiveSixMonth2",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IncentiveSixMonth1",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CI_SixMonth2",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CI_SixMonth1",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IncentiveSixMonth2",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IncentiveSixMonth1",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CI_SixMonth2",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CI_SixMonth1",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
