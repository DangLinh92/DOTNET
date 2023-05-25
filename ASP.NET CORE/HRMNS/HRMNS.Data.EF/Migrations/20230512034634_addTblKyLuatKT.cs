using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblKyLuatKT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_KY_LUAT_KHENTHUONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    LoiViPham = table.Column<string>(maxLength: 500, nullable: true),
                    HinhThucKyLuat = table.Column<string>(maxLength: 500, nullable: true),
                    PhuongThucXuLy = table.Column<string>(maxLength: 500, nullable: true),
                    PhanLoai = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_KY_LUAT_KHENTHUONG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_KY_LUAT_KHENTHUONG_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_KY_LUAT_KHENTHUONG_MaNV",
                table: "HR_KY_LUAT_KHENTHUONG",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_KY_LUAT_KHENTHUONG");
        }
    }
}
