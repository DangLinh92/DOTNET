using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateSalary19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY_HISTORY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY_HISTORY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
