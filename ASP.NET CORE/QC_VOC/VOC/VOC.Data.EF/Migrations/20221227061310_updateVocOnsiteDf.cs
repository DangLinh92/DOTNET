using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class updateVocOnsiteDf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PBA_FAE_Result",
                table: "VOC_MST_BACKUP");

            migrationBuilder.AddColumn<string>(
                name: "CustomerDefectName",
                table: "VOC_ONSITE",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerDefectName",
                table: "VOC_ONSITE");

            migrationBuilder.AddColumn<string>(
                name: "PBA_FAE_Result",
                table: "VOC_MST_BACKUP",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
