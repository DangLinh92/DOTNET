using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addChiTietNgayQTrac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaEvent = table.Column<Guid>(nullable: false),
                    MaKHQuanTrac = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC_EVENT_SHEDULE_PARENT_MaEvent",
                        column: x => x.MaEvent,
                        principalTable: "EVENT_SHEDULE_PARENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC_EHS_KEHOACH_QUANTRAC_MaKHQuanTrac",
                        column: x => x.MaKHQuanTrac,
                        principalTable: "EHS_KEHOACH_QUANTRAC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC_MaEvent",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                column: "MaEvent");

            migrationBuilder.CreateIndex(
                name: "IX_EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC_MaKHQuanTrac",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                column: "MaKHQuanTrac");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");
        }
    }
}
