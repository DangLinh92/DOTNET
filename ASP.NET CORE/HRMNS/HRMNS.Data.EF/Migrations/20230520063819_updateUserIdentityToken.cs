using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateUserIdentityToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APP_USER_TOKEN",
                table: "APP_USER_TOKEN");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "APP_USER_TOKEN",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "APP_USER_TOKEN",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_APP_USER_TOKEN",
                table: "APP_USER_TOKEN",
                columns: new[] { "UserId", "LoginProvider", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APP_USER_TOKEN",
                table: "APP_USER_TOKEN");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "APP_USER_TOKEN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "APP_USER_TOKEN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_APP_USER_TOKEN",
                table: "APP_USER_TOKEN",
                column: "UserId");
        }
    }
}
