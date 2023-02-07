using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class removetableUnuse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_CHIPHI_BY_MONTH");

            migrationBuilder.DropTable(
                name: "EHS_LUATDINH_DEMUC_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropTable(
                name: "EVENT_SHEDULE");

            migrationBuilder.DropTable(
                name: "EHS_NOIDUNG");

            migrationBuilder.DropTable(
                name: "EHS_DEMUC_KEHOACH");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_DEMUC_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenKeDeMuc_KR = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TenKeDeMuc_VN = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_DEMUC_KEHOACH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EVENT_SHEDULE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaEventParent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_SHEDULE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EVENT_SHEDULE_EVENT_SHEDULE_PARENT_MaEventParent",
                        column: x => x.MaEventParent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_LUATDINH_DEMUC_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LuatDinhLienQuan = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MaDeMuc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_LUATDINH_DEMUC_KEHOACH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_LUATDINH_DEMUC_KEHOACH_EHS_DEMUC_KEHOACH_MaDeMuc",
                        column: x => x.MaDeMuc,
                        principalTable: "EHS_DEMUC_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_NOIDUNG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaDeMucKH = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaKeHoach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_NOIDUNG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_NOIDUNG_EHS_DEMUC_KEHOACH_MaDeMucKH",
                        column: x => x.MaDeMucKH,
                        principalTable: "EHS_DEMUC_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_NOIDUNG_EHS_DM_KEHOACH_MaKeHoach",
                        column: x => x.MaKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_CHIPHI_BY_MONTH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChiPhi1 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi10 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi11 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi12 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi2 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi3 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi4 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi5 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi6 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi7 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi8 = table.Column<double>(type: "float", nullable: false),
                    ChiPhi9 = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaNoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Year = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_CHIPHI_BY_MONTH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_MaNoiDung",
                        column: x => x.MaNoiDung,
                        principalTable: "EHS_NOIDUNG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_NOIDUNG_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChuKy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KetQua = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MaHieuMayKiemTra = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MaNoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayKhaiBaoThietBi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayThucHien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NguoiPhucTrach = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NhaThau = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoLuong = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThoiGianThongBao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ThoiGian_ThucHien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TienDoHoanThanh = table.Column<double>(type: "float", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeuCau = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_NOIDUNG_KEHOACH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_NOIDUNG_KEHOACH_EHS_NOIDUNG_MaNoiDung",
                        column: x => x.MaNoiDung,
                        principalTable: "EHS_NOIDUNG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDung",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_LUATDINH_DEMUC_KEHOACH_MaDeMuc",
                table: "EHS_LUATDINH_DEMUC_KEHOACH",
                column: "MaDeMuc");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NOIDUNG_MaDeMucKH",
                table: "EHS_NOIDUNG",
                column: "MaDeMucKH");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NOIDUNG_MaKeHoach",
                table: "EHS_NOIDUNG",
                column: "MaKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NOIDUNG_KEHOACH_MaNoiDung",
                table: "EHS_NOIDUNG_KEHOACH",
                column: "MaNoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_SHEDULE_MaEventParent",
                table: "EVENT_SHEDULE",
                column: "MaEventParent");
        }
    }
}
