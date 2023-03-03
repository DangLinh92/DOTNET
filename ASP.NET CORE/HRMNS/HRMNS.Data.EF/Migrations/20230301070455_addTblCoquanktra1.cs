using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblCoquanktra1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_COQUAN_KIEMTRA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Demuc = table.Column<string>(maxLength: 500, nullable: true),
                    CoQuanKiemTra = table.Column<string>(maxLength: 200, nullable: true),
                    NgayKiemTra = table.Column<string>(maxLength: 50, nullable: true),
                    NoiDungKiemTra = table.Column<string>(maxLength: 500, nullable: true),
                    KetQua = table.Column<string>(maxLength: 50, nullable: true),
                    NoiDungNG = table.Column<string>(maxLength: 1000, nullable: true),
                    NguyenNhan = table.Column<string>(maxLength: 1000, nullable: true),
                    DoiSachCaiTien = table.Column<string>(maxLength: 1000, nullable: true),
                    TienDoCaiTien = table.Column<string>(maxLength: 200, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_COQUAN_KIEMTRA", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_COQUAN_KIEMTRA");
        }
    }
}
