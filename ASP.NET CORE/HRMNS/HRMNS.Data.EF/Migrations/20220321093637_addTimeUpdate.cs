using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTimeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstIn_Time_Update",
                table: "CHAM_CONG_LOG",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Last_Out_Time_Update",
                table: "CHAM_CONG_LOG",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstIn_Time_Update",
                table: "CHAM_CONG_LOG");

            migrationBuilder.DropColumn(
                name: "Last_Out_Time_Update",
                table: "CHAM_CONG_LOG");
        }
    }
}
