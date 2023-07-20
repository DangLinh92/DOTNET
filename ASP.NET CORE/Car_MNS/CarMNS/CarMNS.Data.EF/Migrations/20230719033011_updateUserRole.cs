using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class updateUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APP_USER_ROLE",
                table: "APP_USER_ROLE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APP_USER_ROLE",
                table: "APP_USER_ROLE",
                columns: new[] { "UserId", "RoleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APP_USER_ROLE",
                table: "APP_USER_ROLE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APP_USER_ROLE",
                table: "APP_USER_ROLE",
                columns: new[] { "RoleId", "UserId" });
        }
    }
}
