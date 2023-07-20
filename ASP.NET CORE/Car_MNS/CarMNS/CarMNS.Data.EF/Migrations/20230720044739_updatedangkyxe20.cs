using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updatedangkyxe20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaDiemDen",
                table: "DANG_KY_XE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToTimePlan",
                table: "DANG_KY_XE",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromTimePlan",
                table: "DANG_KY_XE",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDen_Huyen",
                table: "DANG_KY_XE",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDen_Tinh",
                table: "DANG_KY_XE",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDen_Xa",
                table: "DANG_KY_XE",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NguoiDangKy",
                table: "DANG_KY_XE",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaDiemDen_Huyen",
                table: "DANG_KY_XE");

            migrationBuilder.DropColumn(
                name: "DiaDiemDen_Tinh",
                table: "DANG_KY_XE");

            migrationBuilder.DropColumn(
                name: "DiaDiemDen_Xa",
                table: "DANG_KY_XE");

            migrationBuilder.DropColumn(
                name: "NguoiDangKy",
                table: "DANG_KY_XE");

            migrationBuilder.AlterColumn<string>(
                name: "ToTimePlan",
                table: "DANG_KY_XE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromTimePlan",
                table: "DANG_KY_XE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDen",
                table: "DANG_KY_XE",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
