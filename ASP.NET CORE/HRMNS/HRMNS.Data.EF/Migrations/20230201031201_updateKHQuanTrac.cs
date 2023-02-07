using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateKHQuanTrac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KhuVucLayMau",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_1",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_10",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_11",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_12",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_2",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_3",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_4",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_5",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_6",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_7",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_8",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayMau_Month_9",
                table: "EHS_KEHOACH_QUANTRAC",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayMau_Month_1",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_10",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_11",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_12",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_2",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_3",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_4",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_5",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_6",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_7",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_8",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "LayMau_Month_9",
                table: "EHS_KEHOACH_QUANTRAC");

            migrationBuilder.AlterColumn<string>(
                name: "KhuVucLayMau",
                table: "EHS_KEHOACH_QUANTRAC",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
