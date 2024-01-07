using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblPayOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_THANHTOAN_NGHIVIEC",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    IsPay = table.Column<bool>(nullable: false),
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    IsPayed = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_THANHTOAN_NGHIVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_THANHTOAN_NGHIVIEC_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_THANHTOAN_NGHIVIEC_MaNV",
                table: "HR_THANHTOAN_NGHIVIEC",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_THANHTOAN_NGHIVIEC");
        }
    }
}
