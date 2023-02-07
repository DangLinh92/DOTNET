using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addKhamSK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_KE_HOACH_KHAM_SK",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaDMKeHoach = table.Column<Guid>(nullable: false),
                    LuatDinhLienQuan = table.Column<string>(maxLength: 500, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 500, nullable: true),
                    ChuKyThucHien = table.Column<string>(maxLength: 100, nullable: true),
                    Year = table.Column<string>(maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_EHS_KE_HOACH_KHAM_SK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_KE_HOACH_KHAM_SK_EHS_DM_KEHOACH_MaDMKeHoach",
                        column: x => x.MaDMKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaEvent = table.Column<Guid>(nullable: false),
                    MaKHKhamSK = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK_EVENT_SHEDULE_PARENT_MaEvent",
                        column: x => x.MaEvent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK_EHS_KE_HOACH_KHAM_SK_MaKHKhamSK",
                        column: x => x.MaKHKhamSK,
                        principalTable: "EHS_KE_HOACH_KHAM_SK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_NHANVIEN_KHAM_SK",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKHKhamSK = table.Column<Guid>(nullable: false),
                    ThoiGianKhamSK = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    TenNV = table.Column<string>(maxLength: 150, nullable: true),
                    Section = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_NHANVIEN_KHAM_SK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_NHANVIEN_KHAM_SK_EHS_KE_HOACH_KHAM_SK_MaKHKhamSK",
                        column: x => x.MaKHKhamSK,
                        principalTable: "EHS_KE_HOACH_KHAM_SK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_KE_HOACH_KHAM_SK_MaDMKeHoach",
                table: "EHS_KE_HOACH_KHAM_SK",
                column: "MaDMKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK_MaEvent",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                column: "MaEvent");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK_MaKHKhamSK",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                column: "MaKHKhamSK");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NHANVIEN_KHAM_SK_MaKHKhamSK",
                table: "EHS_NHANVIEN_KHAM_SK",
                column: "MaKHKhamSK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropTable(
                name: "EHS_NHANVIEN_KHAM_SK");

            migrationBuilder.DropTable(
                name: "EHS_KE_HOACH_KHAM_SK");
        }
    }
}
