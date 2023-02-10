using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblKiemDinhMM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_KEHOACH_KIEMDINH_MAYMOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaDMKeHoach = table.Column<Guid>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    TenMayMoc = table.Column<string>(maxLength: 500, nullable: true),
                    ChuKyKiemDinh = table.Column<string>(maxLength: 50, nullable: true),
                    SoLuongThietBi = table.Column<int>(nullable: false),
                    ViTri = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiPhuTrach = table.Column<string>(maxLength: 50, nullable: true),
                    NhaThau = table.Column<string>(maxLength: 50, nullable: true),
                    LanKiemDinhKeTiep = table.Column<string>(maxLength: 50, nullable: true),
                    LanKiemDinhKeTiep1 = table.Column<string>(maxLength: 50, nullable: true),
                    LanKiemDinhKeTiep2 = table.Column<string>(maxLength: 50, nullable: true),
                    LanKiemDinhKeTiep3 = table.Column<string>(maxLength: 50, nullable: true),
                    CostMonth_1 = table.Column<double>(nullable: false),
                    CostMonth_2 = table.Column<double>(nullable: false),
                    CostMonth_3 = table.Column<double>(nullable: false),
                    CostMonth_4 = table.Column<double>(nullable: false),
                    CostMonth_5 = table.Column<double>(nullable: false),
                    CostMonth_6 = table.Column<double>(nullable: false),
                    CostMonth_7 = table.Column<double>(nullable: false),
                    CostMonth_8 = table.Column<double>(nullable: false),
                    CostMonth_9 = table.Column<double>(nullable: false),
                    CostMonth_10 = table.Column<double>(nullable: false),
                    CostMonth_11 = table.Column<double>(nullable: false),
                    CostMonth_12 = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_KEHOACH_KIEMDINH_MAYMOC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_KEHOACH_KIEMDINH_MAYMOC_EHS_DM_KEHOACH_MaDMKeHoach",
                        column: x => x.MaDMKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaEvent = table.Column<Guid>(nullable: false),
                    MaKH_KDMM = table.Column<Guid>(nullable: false),
                    NoiDung = table.Column<string>(maxLength: 1000, nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM_EVENT_SHEDULE_PARENT_MaEvent",
                        column: x => x.MaEvent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM_EHS_KEHOACH_KIEMDINH_MAYMOC_MaKH_KDMM",
                        column: x => x.MaKH_KDMM,
                        principalTable: "EHS_KEHOACH_KIEMDINH_MAYMOC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_KEHOACH_KIEMDINH_MAYMOC_MaDMKeHoach",
                table: "EHS_KEHOACH_KIEMDINH_MAYMOC",
                column: "MaDMKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM_MaEvent",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                column: "MaEvent");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM_MaKH_KDMM",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                column: "MaKH_KDMM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropTable(
                name: "EHS_KEHOACH_KIEMDINH_MAYMOC");
        }
    }
}
