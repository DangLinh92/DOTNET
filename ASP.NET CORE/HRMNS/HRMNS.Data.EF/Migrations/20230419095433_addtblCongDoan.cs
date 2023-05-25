using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addtblCongDoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONGDOAN_NOT_JOIN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<DateTime>(nullable: true),
                    NguyenNhan = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONGDOAN_NOT_JOIN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONGDOAN_NOT_JOIN_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONGDOAN_NOT_JOIN_MaNV",
                table: "CONGDOAN_NOT_JOIN",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONGDOAN_NOT_JOIN");
        }
    }
}
