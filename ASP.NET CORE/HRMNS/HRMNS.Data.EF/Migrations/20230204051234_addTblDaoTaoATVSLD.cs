using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblDaoTaoATVSLD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaDMKeHoach = table.Column<Guid>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    NoiDung = table.Column<string>(maxLength: 500, nullable: true),
                    NguoiThamGia = table.Column<string>(maxLength: 500, nullable: true),
                    ChuKyThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGianCapLanDau = table.Column<string>(maxLength: 500, nullable: true),
                    ThoiGianHuanLuyenLanDau = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGianHuanLuyenLai = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 10, nullable: true),
                    ThoiGianDaoTao = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiPhuTrach = table.Column<string>(maxLength: 50, nullable: true),
                    NhaThau = table.Column<string>(maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaEvent = table.Column<Guid>(nullable: false),
                    MaKHDaoTaoATLD = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD_EVENT_SHEDULE_PARENT_MaEvent",
                        column: x => x.MaEvent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD_EHS_KEHOACH_DAOTAO_ANTOAN_VSLD_MaKHDaoTaoATLD",
                        column: x => x.MaKHDaoTaoATLD,
                        principalTable: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD_MaEvent",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                column: "MaEvent");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD_MaKHDaoTaoATLD",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                column: "MaKHDaoTaoATLD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropTable(
                name: "EHS_KEHOACH_DAOTAO_ANTOAN_VSLD");
        }
    }
}
