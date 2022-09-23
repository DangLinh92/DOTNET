using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateNoiDungKeHoach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_THOIGIAN_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "DonViChuKy",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.AlterColumn<string>(
                name: "ChuKy",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "NgayKhaiBaoThietBi",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayThucHien",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SoLuong",
                table: "EHS_NOIDUNG_KEHOACH",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ThoiGianThongBao",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThoiGian_ThucHien",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViTri",
                table: "EHS_NOIDUNG_KEHOACH",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayKhaiBaoThietBi",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "NgayThucHien",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "ThoiGianThongBao",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "ThoiGian_ThucHien",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropColumn(
                name: "ViTri",
                table: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.AlterColumn<double>(
                name: "ChuKy",
                table: "EHS_NOIDUNG_KEHOACH",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonViChuKy",
                table: "EHS_NOIDUNG_KEHOACH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayBatDau",
                table: "EHS_NOIDUNG_KEHOACH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaNoiDungKeHoach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayThucHien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoLuong = table.Column<double>(type: "float", nullable: false),
                    ThoiGian_ThucHien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Year = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_THOIGIAN_NOIDUNG_KEHOACH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_NOIDUNG_KEHOACH_EHS_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                        column: x => x.MaNoiDungKeHoach,
                        principalTable: "EHS_NOIDUNG_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                column: "MaNoiDungKeHoach");
        }
    }
}
