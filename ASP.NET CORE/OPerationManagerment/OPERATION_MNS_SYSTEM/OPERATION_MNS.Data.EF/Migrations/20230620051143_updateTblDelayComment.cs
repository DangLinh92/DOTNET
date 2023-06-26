using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class updateTblDelayComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DELAY_COMMENT_SAMPLE_MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE",
                column: "MaTinhHinhSX");

            migrationBuilder.AddForeignKey(
                name: "FK_DELAY_COMMENT_SAMPLE_TINH_HINH_SAN_XUAT_SAMPLE_MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE",
                column: "MaTinhHinhSX",
                principalTable: "TINH_HINH_SAN_XUAT_SAMPLE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DELAY_COMMENT_SAMPLE_TINH_HINH_SAN_XUAT_SAMPLE_MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE");

            migrationBuilder.DropIndex(
                name: "IX_DELAY_COMMENT_SAMPLE_MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE");

            migrationBuilder.DropColumn(
                name: "MaTinhHinhSX",
                table: "DELAY_COMMENT_SAMPLE");
        }
    }
}
