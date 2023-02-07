using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblAntoanbxa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_KEHOACH_ANTOAN_BUCXA",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaDMKeHoach = table.Column<Guid>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    HangMuc = table.Column<string>(maxLength: 500, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 1000, nullable: true),
                    ChuKyThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 10, nullable: true),
                    ThoiGianDaoTao = table.Column<string>(maxLength: 250, nullable: true),
                    MaHieu = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiPhuTrach = table.Column<string>(maxLength: 50, nullable: true),
                    NhaThau = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGianCapL1 = table.Column<string>(maxLength: 250, nullable: true),
                    ThoiGianCapLai_L1 = table.Column<string>(maxLength: 250, nullable: true),
                    ThoiGianCapLai_L2 = table.Column<string>(maxLength: 250, nullable: true),
                    ThoiGianCapLai_L3 = table.Column<string>(maxLength: 250, nullable: true),
                    YeuCau = table.Column<string>(maxLength: 250, nullable: true),
                    QuyDinhVBPL = table.Column<string>(maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_EHS_KEHOACH_ANTOAN_BUCXA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_KEHOACH_ANTOAN_BUCXA_EHS_DM_KEHOACH_MaDMKeHoach",
                        column: x => x.MaDMKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaEvent = table.Column<Guid>(nullable: false),
                    MaKH_ATBX = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA_EVENT_SHEDULE_PARENT_MaEvent",
                        column: x => x.MaEvent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA_EHS_KEHOACH_ANTOAN_BUCXA_MaKH_ATBX",
                        column: x => x.MaKH_ATBX,
                        principalTable: "EHS_KEHOACH_ANTOAN_BUCXA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_KEHOACH_ANTOAN_BUCXA_MaDMKeHoach",
                table: "EHS_KEHOACH_ANTOAN_BUCXA",
                column: "MaDMKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA_MaEvent",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                column: "MaEvent");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA_MaKH_ATBX",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                column: "MaKH_ATBX");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropTable(
                name: "EHS_KEHOACH_ANTOAN_BUCXA");
        }
    }
}
