using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatename_chamcong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstOut_Time",
                table: "CHAM_CONG_LOG");

            migrationBuilder.AddColumn<string>(
                name: "FirstIn_Time",
                table: "CHAM_CONG_LOG",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstIn_Time",
                table: "CHAM_CONG_LOG");

            migrationBuilder.AddColumn<string>(
                name: "FirstOut_Time",
                table: "CHAM_CONG_LOG",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
