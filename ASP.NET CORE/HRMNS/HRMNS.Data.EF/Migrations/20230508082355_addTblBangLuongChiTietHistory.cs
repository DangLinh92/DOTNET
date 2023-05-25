using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblBangLuongChiTietHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BANGLUONGCHITIET_HISTORY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    HieuLucCapBac = table.Column<string>(maxLength: 50, nullable: true),
                    MaBoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    Grade = table.Column<string>(maxLength: 50, nullable: true),
                    TenNV = table.Column<string>(maxLength: 50, nullable: true),
                    BoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    ChucVu = table.Column<string>(maxLength: 50, nullable: true),
                    NgayVao = table.Column<string>(maxLength: 50, nullable: true),
                    BasicSalary = table.Column<double>(nullable: false),
                    LivingAllowance = table.Column<double>(nullable: false),
                    PositionAllowance = table.Column<double>(nullable: false),
                    AbilityAllowance = table.Column<double>(nullable: false),
                    SeniorityAllowance = table.Column<double>(nullable: false),
                    HarmfulAllowance = table.Column<double>(nullable: false),
                    TongNgayCongThucTe = table.Column<double>(nullable: false),
                    NgayCongThuViecBanNgay = table.Column<double>(nullable: false),
                    NgayCongThuViecBanDem = table.Column<double>(nullable: false),
                    NgayCongChinhThucBanNgay = table.Column<double>(nullable: false),
                    NgayCongChinhThucBanDem = table.Column<double>(nullable: false),
                    NghiViecCoLuong = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_150 = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_200 = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_210 = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_270 = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_300 = table.Column<double>(nullable: false),
                    GioLamThemTrongTV_390 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_150 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_200 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_210 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_270 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_300 = table.Column<double>(nullable: false),
                    GioLamThemTrongCT_390 = table.Column<double>(nullable: false),
                    SoNgayLamCaDemTruocLe_TV = table.Column<double>(nullable: false),
                    SoNgayLamCaDemTruocLe_CT = table.Column<double>(nullable: false),
                    SoNgayLamCaDem_TV = table.Column<double>(nullable: false),
                    SoNgayLamCaDem_CT = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_150 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_200_NT = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_200_CN = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_270 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_300 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecTV_390 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_150 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_200_NT = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_200_CN = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_270 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_300 = table.Column<double>(nullable: false),
                    HoTroThoiGianLamViecCT_390 = table.Column<double>(nullable: false),
                    HoTroNgayThanhLapCty_CaNgayTV = table.Column<double>(nullable: false),
                    HoTroNgayThanhLapCty_CaNgayCT = table.Column<double>(nullable: false),
                    HoTroNgayThanhLapCty_CaDemTV_TruocLe = table.Column<double>(nullable: false),
                    HoTroNgayThanhLapCty_CaDemCT_TruocLe = table.Column<double>(nullable: false),
                    NghiKhamThai = table.Column<double>(nullable: false),
                    NghiViecKhongThongBao = table.Column<double>(nullable: false),
                    SoNgayNghiBu_AL30 = table.Column<double>(nullable: false),
                    SoNgayNghiBu_NB = table.Column<double>(nullable: false),
                    HoTroPCCC_CoSo = table.Column<double>(nullable: false),
                    HoTroAT_SinhVien = table.Column<double>(nullable: false),
                    TV_NghiKhongLuong = table.Column<double>(nullable: false),
                    NghiKhongLuong = table.Column<double>(nullable: false),
                    Probation_Late_Come_Early_Leave_Time = table.Column<double>(nullable: false),
                    Official_Late_Come_Early_Leave_Time = table.Column<double>(nullable: false),
                    ThuocDoiTuong_BHXH = table.Column<string>(maxLength: 50, nullable: true),
                    TruQuyPhongChongThienTai = table.Column<double>(nullable: false),
                    Thuong = table.Column<double>(nullable: false),
                    SoNguoiPhuThuoc = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 50, nullable: true),
                    InsentiveStandard = table.Column<decimal>(nullable: false),
                    DanhGia = table.Column<string>(maxLength: 50, nullable: true),
                    HoTroCongDoan = table.Column<decimal>(nullable: false),
                    SoTK = table.Column<string>(maxLength: 50, nullable: true),
                    DoiTuongThamGiaCD = table.Column<string>(maxLength: 50, nullable: true),
                    DoiTuongTruyThuBHYT = table.Column<string>(maxLength: 50, nullable: true),
                    SoConNho = table.Column<int>(nullable: false),
                    SoNgayNghi70 = table.Column<double>(nullable: false),
                    NgayNghiViec = table.Column<string>(maxLength: 50, nullable: true),
                    DieuChinhCong_Total = table.Column<double>(nullable: false),
                    TraTienPhepNam_Total = table.Column<double>(nullable: false),
                    TT_Tien_GioiThieu = table.Column<double>(nullable: false),
                    ThangNam = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANGLUONGCHITIET_HISTORY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANGLUONGCHITIET_HISTORY");
        }
    }
}
