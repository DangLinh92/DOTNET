using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatePhepNam07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThoiGianChiTra",
                table: "HR_PHEP_NAM",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DailySalary",
                table: "BANGLUONGCHITIET_HISTORY",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailySalary",
                table: "BANGLUONGCHITIET_HISTORY");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianChiTra",
                table: "HR_PHEP_NAM",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
