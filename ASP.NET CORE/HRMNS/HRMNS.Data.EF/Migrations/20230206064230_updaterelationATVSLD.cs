using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updaterelationATVSLD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD_MaDMKeHoach",
                table: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD",
                column: "MaDMKeHoach");

            migrationBuilder.AddForeignKey(
                name: "FK_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD_EHS_DM_KEHOACH_MaDMKeHoach",
                table: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD",
                column: "MaDMKeHoach",
                principalTable: "EHS_DM_KEHOACH",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD_EHS_DM_KEHOACH_MaDMKeHoach",
                table: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD");

            migrationBuilder.DropIndex(
                name: "IX_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD_MaDMKeHoach",
                table: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD");
        }
    }
}
