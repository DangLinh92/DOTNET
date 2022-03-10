using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class ngaydacbiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NgayNghiBu",
                table: "NGAY_NGHI_BU_LE_NAM",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NGAY_DAC_BIET",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 10, nullable: false),
                    TenNgayDacBiet = table.Column<string>(maxLength: 150, nullable: true),
                    KyHieuChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGAY_DAC_BIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NGAY_DAC_BIET_KY_HIEU_CHAM_CONG_KyHieuChamCong",
                        column: x => x.KyHieuChamCong,
                        principalTable: "KY_HIEU_CHAM_CONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NGAY_DAC_BIET_KyHieuChamCong",
                table: "NGAY_DAC_BIET",
                column: "KyHieuChamCong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NGAY_DAC_BIET");

            migrationBuilder.DropColumn(
                name: "NgayNghiBu",
                table: "NGAY_NGHI_BU_LE_NAM");
        }
    }
}
