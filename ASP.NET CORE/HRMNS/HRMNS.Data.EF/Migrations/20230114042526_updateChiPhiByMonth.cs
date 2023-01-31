using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateChiPhiByMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.DropIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.DropColumn(
                name: "MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.AddColumn<Guid>(
                name: "MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDung",
                principalTable: "EHS_NOIDUNG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.DropIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.DropColumn(
                name: "MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.AddColumn<Guid>(
                name: "MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDungKeHoach");

            migrationBuilder.AddForeignKey(
                name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDungKeHoach",
                principalTable: "EHS_NOIDUNG_KEHOACH",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
