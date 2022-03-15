using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addDMDCChamCong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DC_CHAM_CONG_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropIndex(
                name: "IX_DC_CHAM_CONG_MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "GiaTriSauDC",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "GiaTriTruocDC",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "KyHieuChamCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG");

            migrationBuilder.AddColumn<int>(
                name: "DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GiaTriBoXung",
                table: "DC_CHAM_CONG",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DM_DIEUCHINH_CHAMCONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_DIEUCHINH_CHAMCONG", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                column: "DM_DieuChinhCong");

            migrationBuilder.AddForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG",
                column: "DM_DieuChinhCong",
                principalTable: "DM_DIEUCHINH_CHAMCONG",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DC_CHAM_CONG_DM_DIEUCHINH_CHAMCONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropTable(
                name: "DM_DIEUCHINH_CHAMCONG");

            migrationBuilder.DropIndex(
                name: "IX_DC_CHAM_CONG_DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "DM_DieuChinhCong",
                table: "DC_CHAM_CONG");

            migrationBuilder.DropColumn(
                name: "GiaTriBoXung",
                table: "DC_CHAM_CONG");

            migrationBuilder.AddColumn<double>(
                name: "GiaTriSauDC",
                table: "DC_CHAM_CONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GiaTriTruocDC",
                table: "DC_CHAM_CONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieuChamCong",
                table: "DC_CHAM_CONG",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG",
                column: "MaChamCong_ChiTiet");

            migrationBuilder.AddForeignKey(
                name: "FK_DC_CHAM_CONG_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG",
                column: "MaChamCong_ChiTiet",
                principalTable: "DANGKY_CHAMCONG_CHITIET",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
