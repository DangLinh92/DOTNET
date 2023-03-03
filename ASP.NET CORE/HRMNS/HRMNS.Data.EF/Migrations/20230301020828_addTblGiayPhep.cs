using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblGiayPhep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_QUANLY_GIAY_PHEP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Demuc = table.Column<string>(maxLength: 250, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 1000, nullable: true),
                    LuatDinhLienQuan = table.Column<string>(maxLength: 1000, nullable: true),
                    LyDoThucHien = table.Column<string>(maxLength: 1000, nullable: true),
                    TienDo = table.Column<string>(maxLength: 250, nullable: true),
                    ThoiGianThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    SoNgayBaoTruoc = table.Column<int>(nullable: false),
                    KetQua = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiThucHien = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_QUANLY_GIAY_PHEP", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_QUANLY_GIAY_PHEP");
        }
    }
}
