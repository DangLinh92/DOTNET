using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updatePhepNamTemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SoPhepTonNamTmp",
                table: "HR_PHEP_NAM",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoPhepTonNamTmp",
                table: "HR_PHEP_NAM");
        }
    }
}
