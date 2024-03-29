﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addChamOTTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANGKY_CHAMCONG_OT_DACBIET",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    MaChamCong_ChiTiet = table.Column<int>(nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    HourInDay = table.Column<double>(nullable: false),
                    NoiDung = table.Column<string>(maxLength: 300, nullable: true),
                    Approve = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveLV2 = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveLV3 = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_CHAMCONG_OT_DACBIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_OT_DACBIET_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                        column: x => x.MaChamCong_ChiTiet,
                        principalTable: "DANGKY_CHAMCONG_CHITIET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_OT_DACBIET_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_OT_DACBIET_MaChamCong_ChiTiet",
                table: "DANGKY_CHAMCONG_OT_DACBIET",
                column: "MaChamCong_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_OT_DACBIET_MaNV",
                table: "DANGKY_CHAMCONG_OT_DACBIET",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DANGKY_CHAMCONG_OT_DACBIET");
        }
    }
}
