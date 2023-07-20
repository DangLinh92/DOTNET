using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDCChamCong07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DC100",
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
                name: "NgayDieuChinh2",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "DC_CHAM_CONG");

            migrationBuilder.AlterColumn<string>(
                name: "NoiDungDC",
                table: "DC_CHAM_CONG",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NgayDieuChinh",
                table: "DC_CHAM_CONG",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NoiDungDC",
                table: "DC_CHAM_CONG",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayDieuChinh",
                table: "DC_CHAM_CONG",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC100",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC150",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC190",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC200",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC210",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC270",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC300",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC390",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC85",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DSNS",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ELLC",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT100",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT150",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT200",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT390",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HT50",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NSBH",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NgayCong",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgayDieuChinh2",
                table: "DC_CHAM_CONG",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Other",
                table: "DC_CHAM_CONG",
                type: "real",
                nullable: true);
        }
    }
}
