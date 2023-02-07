using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addKHQuanTrac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_KEHOACH_QUANTRAC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STT = table.Column<int>(nullable: false),
                    MaDMKeHoach = table.Column<Guid>(nullable: false),
                    Demuc = table.Column<string>(maxLength: 500, nullable: true),
                    LuatDinhLienQuan = table.Column<string>(maxLength: 500, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 1000, nullable: true),
                    ChuKyThucHien = table.Column<string>(maxLength: 100, nullable: true),
                    Year = table.Column<string>(maxLength: 4, nullable: true),
                    Month_1 = table.Column<bool>(nullable: false),
                    Month_2 = table.Column<bool>(nullable: false),
                    Month_3 = table.Column<bool>(nullable: false),
                    Month_4 = table.Column<bool>(nullable: false),
                    Month_5 = table.Column<bool>(nullable: false),
                    Month_6 = table.Column<bool>(nullable: false),
                    Month_7 = table.Column<bool>(nullable: false),
                    Month_8 = table.Column<bool>(nullable: false),
                    Month_9 = table.Column<bool>(nullable: false),
                    Month_10 = table.Column<bool>(nullable: false),
                    Month_11 = table.Column<bool>(nullable: false),
                    Month_12 = table.Column<bool>(nullable: false),
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
                    KhuVucLayMau = table.Column<string>(maxLength: 1000, nullable: true),
                    NhaThau = table.Column<string>(maxLength: 100, nullable: true),
                    NguoiPhuTrach = table.Column<string>(maxLength: 50, nullable: true),
                    TienDoHoanThanh = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_KEHOACH_QUANTRAC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_KEHOACH_QUANTRAC_EHS_DM_KEHOACH_MaDMKeHoach",
                        column: x => x.MaDMKeHoach,
                        principalTable: "EHS_DM_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_KEHOACH_QUANTRAC_MaDMKeHoach",
                table: "EHS_KEHOACH_QUANTRAC",
                column: "MaDMKeHoach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_KEHOACH_QUANTRAC");
        }
    }
}
