using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateThongTinEx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HieuLucCapBac",
                table: "NHANVIEN_INFOR_EX");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "NHANVIEN_INFOR_EX",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianViPham",
                table: "HR_KY_LUAT_KHENTHUONG",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "NHANVIEN_INFOR_EX");

            migrationBuilder.DropColumn(
                name: "ThoiGianViPham",
                table: "HR_KY_LUAT_KHENTHUONG");

            migrationBuilder.AddColumn<string>(
                name: "HieuLucCapBac",
                table: "NHANVIEN_INFOR_EX",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
