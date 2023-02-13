using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updatePostOperationShiping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NguoiKiemTra",
                table: "POST_OPERATION_SHIPPING",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NguoiKiemTra",
                table: "POST_OPERATION_SHIPPING",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
