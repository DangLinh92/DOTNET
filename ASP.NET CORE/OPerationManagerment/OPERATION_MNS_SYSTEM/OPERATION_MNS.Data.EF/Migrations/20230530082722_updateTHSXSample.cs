using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateTHSXSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MucDoKhanCap",
                table: "TINH_HINH_SAN_XUAT_SAMPLE",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteFlg",
                table: "TINH_HINH_SAN_XUAT_SAMPLE",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteFlg",
                table: "TINH_HINH_SAN_XUAT_SAMPLE");

            migrationBuilder.AlterColumn<string>(
                name: "MucDoKhanCap",
                table: "TINH_HINH_SAN_XUAT_SAMPLE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
