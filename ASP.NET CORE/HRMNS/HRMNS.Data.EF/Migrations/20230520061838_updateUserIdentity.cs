using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateUserIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "APP_USER_TOKEN",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_APP_USER_TOKEN_APP_USER_UserId",
                table: "APP_USER_TOKEN",
                column: "UserId",
                principalTable: "APP_USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APP_USER_TOKEN_APP_USER_UserId",
                table: "APP_USER_TOKEN");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "APP_USER_TOKEN");
        }
    }
}
