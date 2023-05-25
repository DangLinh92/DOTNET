using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblNhanVienEX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NHANVIEN_INFOR_EX",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    HieuLucCapBac = table.Column<string>(maxLength: 50, nullable: true),
                    MaBoPhanEx = table.Column<string>(maxLength: 50, nullable: true),
                    Grade = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIEN_INFOR_EX", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_INFOR_EX_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_INFOR_EX_MaNV",
                table: "NHANVIEN_INFOR_EX",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NHANVIEN_INFOR_EX");
        }
    }
}
