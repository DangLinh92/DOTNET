using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatetblAntoanbucxa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThoiGianCapLai_L4",
                table: "EHS_KEHOACH_ANTOAN_BUCXA",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiGianCapLai_L4",
                table: "EHS_KEHOACH_ANTOAN_BUCXA");
        }
    }
}
