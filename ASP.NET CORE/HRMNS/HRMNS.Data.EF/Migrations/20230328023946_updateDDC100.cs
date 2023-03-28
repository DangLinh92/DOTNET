using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDDC100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ChiTraVaoLuongThang",
                table: "DC_CHAM_CONG",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DC100",
                table: "DC_CHAM_CONG",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DC100",
                table: "DC_CHAM_CONG");

            migrationBuilder.AlterColumn<string>(
                name: "ChiTraVaoLuongThang",
                table: "DC_CHAM_CONG",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
