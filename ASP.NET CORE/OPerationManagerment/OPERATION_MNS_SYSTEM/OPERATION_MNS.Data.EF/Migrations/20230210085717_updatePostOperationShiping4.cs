using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updatePostOperationShiping4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WaferId",
                table: "POST_OPERATION_SHIPPING",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WaferId",
                table: "POST_OPERATION_SHIPPING",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
