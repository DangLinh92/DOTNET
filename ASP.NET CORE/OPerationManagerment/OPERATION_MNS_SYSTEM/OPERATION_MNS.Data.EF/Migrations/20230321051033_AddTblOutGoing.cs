using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddTblOutGoing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOPHAN_DE_NGHI_XUAT_NLIEU",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoPhanDeNghi = table.Column<string>(maxLength: 50, nullable: true),
                    NgayDeNghi = table.Column<string>(maxLength: 50, nullable: true),
                    SanPham = table.Column<string>(maxLength: 50, nullable: true),
                    Module = table.Column<string>(maxLength: 50, nullable: true),
                    SapCode = table.Column<string>(maxLength: 50, nullable: true),
                    DinhMuc = table.Column<float>(nullable: false),
                    DonVi = table.Column<string>(nullable: true),
                    SoLuongYeuCau = table.Column<float>(nullable: false),
                    LuongThucTe = table.Column<float>(nullable: false),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOPHAN_DE_NGHI_XUAT_NLIEU", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHUNG_THOI_GIAN_XUAT_HANG_WLP2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGianKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHUNG_THOI_GIAN_XUAT_HANG_WLP2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OUTGOING_RECEIPT_WLP2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(maxLength: 150, nullable: true),
                    LotId = table.Column<string>(maxLength: 50, nullable: true),
                    SapCode = table.Column<string>(maxLength: 50, nullable: true),
                    NgayXuat = table.Column<string>(maxLength: 50, nullable: true),
                    SoLuongYeuCau = table.Column<float>(nullable: false),
                    LuongDuKien_1 = table.Column<float>(nullable: false),
                    ThoiGianDuKien_1 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongDuKien_2 = table.Column<float>(nullable: false),
                    ThoiGianDuKien_2 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongDuKien_3 = table.Column<float>(nullable: false),
                    ThoiGianDuKien_3 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongDuKien_4 = table.Column<float>(nullable: false),
                    ThoiGianDuKien_4 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongDuKien_5 = table.Column<float>(nullable: false),
                    ThoiGianDuKien_5 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongThucTe_1 = table.Column<float>(nullable: false),
                    ThoiGianThucTe_1 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongThucTe_2 = table.Column<float>(nullable: false),
                    ThoiGianThucTe_2 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongThucTe_3 = table.Column<float>(nullable: false),
                    ThoiGianThucTe_3 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongThucTe_4 = table.Column<float>(nullable: false),
                    ThoiGianThucTe_4 = table.Column<string>(maxLength: 250, nullable: true),
                    LuongThucTe_5 = table.Column<float>(nullable: false),
                    ThoiGianThucTe_5 = table.Column<string>(maxLength: 250, nullable: true),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiGiao = table.Column<string>(maxLength: 50, nullable: true),
                    NguoiNhan = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTGOING_RECEIPT_WLP2", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOPHAN_DE_NGHI_XUAT_NLIEU");

            migrationBuilder.DropTable(
                name: "KHUNG_THOI_GIAN_XUAT_HANG_WLP2");

            migrationBuilder.DropTable(
                name: "OUTGOING_RECEIPT_WLP2");
        }
    }
}
