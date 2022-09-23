using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTableEHS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_DEMUC_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenKeDeMuc_VN = table.Column<string>(maxLength: 1000, nullable: true),
                    TenKeDeMuc_KR = table.Column<string>(maxLength: 1000, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_DEMUC_KEHOACH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EHS_DM_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenKeHoach_VN = table.Column<string>(maxLength: 1000, nullable: true),
                    TenKeHoach_KR = table.Column<string>(maxLength: 1000, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_DM_KEHOACH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EHS_LUATDINH_DEMUC_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LuatDinhLienQuan = table.Column<string>(maxLength: 1000, nullable: true),
                    MaDeMuc = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
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
                name: "EHS_LUATDINH_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungLuatDinh = table.Column<string>(maxLength: 1000, nullable: true),
                    MaKeHoach = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_LUATDINH_KEHOACH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_LUATDINH_KEHOACH_EHS_DM_KEHOACH_MaKeHoach",
                        column: x => x.MaKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EHS_NOIDUNG",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NoiDung = table.Column<string>(maxLength: 1000, nullable: true),
                    MaKeHoach = table.Column<Guid>(nullable: false),
                    MaDeMucKH = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
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
                name: "EHS_NOIDUNG_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Year = table.Column<string>(nullable: true),
                    MaNoiDung = table.Column<Guid>(nullable: false),
                    NhaThau = table.Column<string>(maxLength: 250, nullable: true),
                    ChuKy = table.Column<double>(nullable: false),
                    DonViChuKy = table.Column<string>(maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    YeuCau = table.Column<string>(maxLength: 500, nullable: true),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNoiDungKeHoach = table.Column<Guid>(nullable: false),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    NgayThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGian_ThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_THOIGIAN_NOIDUNG_KEHOACH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_THOIGIAN_NOIDUNG_KEHOACH_EHS_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                        column: x => x.MaNoiDungKeHoach,
                        principalTable: "EHS_NOIDUNG_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_LUATDINH_DEMUC_KEHOACH_MaDeMuc",
                table: "EHS_LUATDINH_DEMUC_KEHOACH",
                column: "MaDeMuc");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_LUATDINH_KEHOACH_MaKeHoach",
                table: "EHS_LUATDINH_KEHOACH",
                column: "MaKeHoach");

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
                name: "IX_EHS_THOIGIAN_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                table: "EHS_THOIGIAN_NOIDUNG_KEHOACH",
                column: "MaNoiDungKeHoach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_LUATDINH_DEMUC_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_LUATDINH_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_THOIGIAN_NOIDUNG_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_NOIDUNG_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_NOIDUNG");

            migrationBuilder.DropTable(
                name: "EHS_DEMUC_KEHOACH");

            migrationBuilder.DropTable(
                name: "EHS_DM_KEHOACH");
        }
    }
}
