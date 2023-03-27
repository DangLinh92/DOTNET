using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDCChamCongNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropIndex(
                name: "IX_DC_CHAM_CONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "GiaTriBoXung",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NgayCanDieuChinh_From",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NgayCanDieuChinh_To",
                table: "DC_CHAM_CONG");

            migrationBuilder.AddColumn<float>(
                name: "DC150",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC190",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC200",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC210",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC270",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC300",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC390",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC85",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DSNS",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ELLC",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT100",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT150",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT200",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT390",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT50",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NSBH",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NgayCong",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDieuChinh",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayDieuChinh2",
                table: "DC_CHAM_CONG",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Other",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TongSoTien",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                column: "DM_DIEUCHINH_CHAMCONGId");

            migrationBuilder.AddForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                column: "DM_DIEUCHINH_CHAMCONGId",
                principalTable: "DM_DIEUCHINH_CHAMCONG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropIndex(
                name: "IX_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC150",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC190",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC200",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC210",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC270",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC300",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC390",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DC85",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DSNS",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "ELLC",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "HT100",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "HT150",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "HT200",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "HT390",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "HT50",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NSBH",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NgayCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NgayDieuChinh",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "NgayDieuChinh2",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "TongSoTien",
                table: "DC_CHAM_CONG");

            migrationBuilder.AddColumn<int>(
                name: "DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GiaTriBoXung",
                table: "DC_CHAM_CONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayCanDieuChinh_From",
                table: "DC_CHAM_CONG",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayCanDieuChinh_To",
                table: "DC_CHAM_CONG",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                column: "DM_DieuChinhCong");

            migrationBuilder.AddForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                column: "DM_DieuChinhCong",
                principalTable: "DM_DIEUCHINH_CHAMCONG",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
