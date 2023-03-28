using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatePhepNam0328 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "MucThanhToan",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_1",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_10",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_11",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_12",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_2",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_3",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_4",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_5",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_6",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_7",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_8",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NghiThang_9",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepCongThem",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepDaUng",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepDocHai",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepDuocHuong",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepKhongDuocSuDung",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepThanhToanNghiViec",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepTonNam",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SoPhepTonThang",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThangBatDauDocHai",
                table: "HR_PHEP_NAM",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThangKetThucDocHai",
                table: "HR_PHEP_NAM",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TongNgayNghi",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MucThanhToan",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_1",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_10",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_11",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_12",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_2",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_3",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_4",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_5",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_6",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_7",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_8",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "NghiThang_9",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepCongThem",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepDaUng",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepDocHai",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepDuocHuong",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepKhongDuocSuDung",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepThanhToanNghiViec",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepTonNam",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "SoPhepTonThang",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "ThangBatDauDocHai",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "ThangKetThucDocHai",
                table: "HR_PHEP_NAM");

            migrationBuilder.DropColumn(
                name: "TongNgayNghi",
                table: "HR_PHEP_NAM");
        }
    }
}
