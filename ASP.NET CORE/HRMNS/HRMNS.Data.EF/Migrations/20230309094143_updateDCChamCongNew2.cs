using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateDCChamCongNew2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropIndex(
                name: "IX_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                column: "DM_DIEUCHINH_CHAMCONGId");

            migrationBuilder.AddForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DIEUCHINH_CHAMCONGId",
                table: "DC_CHAM_CONG",
                column: "DM_DIEUCHINH_CHAMCONGId",
                principalTable: "DM_DIEUCHINH_CHAMCONG",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
