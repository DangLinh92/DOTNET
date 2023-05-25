using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations.SqliteMigrations
{
    public partial class initPayrollDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BANGLUONGCHITIET_HISTORY_PR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaNV = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HieuLucCapBac = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    MaBoPhan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Grade = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TenNV = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BoPhan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ChucVu = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NgayVao = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BasicSalary = table.Column<double>(type: "REAL", nullable: false),
                    LivingAllowance = table.Column<double>(type: "REAL", nullable: false),
                    PositionAllowance = table.Column<double>(type: "REAL", nullable: false),
                    AbilityAllowance = table.Column<double>(type: "REAL", nullable: false),
                    SeniorityAllowance = table.Column<double>(type: "REAL", nullable: false),
                    HarmfulAllowance = table.Column<double>(type: "REAL", nullable: false),
                    TongNgayCongThucTe = table.Column<double>(type: "REAL", nullable: false),
                    NgayCongThuViecBanNgay = table.Column<double>(type: "REAL", nullable: false),
                    NgayCongThuViecBanDem = table.Column<double>(type: "REAL", nullable: false),
                    NgayCongChinhThucBanNgay = table.Column<double>(type: "REAL", nullable: false),
                    NgayCongChinhThucBanDem = table.Column<double>(type: "REAL", nullable: false),
                    NghiViecCoLuong = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_150 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_200 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_210 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_270 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_300 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongTV_390 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_150 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_200 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_210 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_270 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_300 = table.Column<double>(type: "REAL", nullable: false),
                    GioLamThemTrongCT_390 = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayLamCaDemTruocLe_TV = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayLamCaDemTruocLe_CT = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayLamCaDem_TV = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayLamCaDem_CT = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_150 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_200_NT = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_200_CN = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_270 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_300 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecTV_390 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_150 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_200_NT = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_200_CN = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_270 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_300 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroThoiGianLamViecCT_390 = table.Column<double>(type: "REAL", nullable: false),
                    HoTroNgayThanhLapCty_CaNgayTV = table.Column<double>(type: "REAL", nullable: false),
                    HoTroNgayThanhLapCty_CaNgayCT = table.Column<double>(type: "REAL", nullable: false),
                    HoTroNgayThanhLapCty_CaDemTV_TruocLe = table.Column<double>(type: "REAL", nullable: false),
                    HoTroNgayThanhLapCty_CaDemCT_TruocLe = table.Column<double>(type: "REAL", nullable: false),
                    NghiKhamThai = table.Column<double>(type: "REAL", nullable: false),
                    NghiViecKhongThongBao = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayNghiBu_AL30 = table.Column<double>(type: "REAL", nullable: false),
                    SoNgayNghiBu_NB = table.Column<double>(type: "REAL", nullable: false),
                    HoTroPCCC_CoSo = table.Column<double>(type: "REAL", nullable: false),
                    HoTroAT_SinhVien = table.Column<double>(type: "REAL", nullable: false),
                    TV_NghiKhongLuong = table.Column<double>(type: "REAL", nullable: false),
                    NghiKhongLuong = table.Column<double>(type: "REAL", nullable: false),
                    Probation_Late_Come_Early_Leave_Time = table.Column<double>(type: "REAL", nullable: false),
                    Official_Late_Come_Early_Leave_Time = table.Column<double>(type: "REAL", nullable: false),
                    ThuocDoiTuong_BHXH = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TruQuyPhongChongThienTai = table.Column<double>(type: "REAL", nullable: false),
                    Thuong = table.Column<double>(type: "REAL", nullable: false),
                    SoNguoiPhuThuoc = table.Column<decimal>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    InsentiveStandard = table.Column<decimal>(type: "TEXT", nullable: false),
                    DanhGia = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HoTroCongDoan = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoTK = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DoiTuongThamGiaCD = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DoiTuongTruyThuBHYT = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    SoConNho = table.Column<int>(type: "INTEGER", nullable: false),
                    SoNgayNghi70 = table.Column<double>(type: "REAL", nullable: false),
                    NgayNghiViec = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DieuChinhCong_Total = table.Column<double>(type: "REAL", nullable: false),
                    TraTienPhepNam_Total = table.Column<double>(type: "REAL", nullable: false),
                    TT_Tien_GioiThieu = table.Column<double>(type: "REAL", nullable: false),
                    ThangNam = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DoiTuongPhuCapDocHai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANGLUONGCHITIET_HISTORY_PR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_SALARY_PR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BasicSalary = table.Column<decimal>(type: "TEXT", nullable: false),
                    LivingAllowance = table.Column<decimal>(type: "TEXT", nullable: false),
                    PositionAllowance = table.Column<decimal>(type: "TEXT", nullable: false),
                    AbilityAllowance = table.Column<decimal>(type: "TEXT", nullable: false),
                    FullAttendanceSupport = table.Column<decimal>(type: "TEXT", nullable: false),
                    SeniorityAllowance = table.Column<decimal>(type: "TEXT", nullable: false),
                    HarmfulAllowance = table.Column<decimal>(type: "TEXT", nullable: false),
                    IncentiveStandard = table.Column<decimal>(type: "TEXT", nullable: false),
                    IncentiveLanguage = table.Column<decimal>(type: "TEXT", nullable: false),
                    IncentiveTechnical = table.Column<decimal>(type: "TEXT", nullable: false),
                    IncentiveOther = table.Column<decimal>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    HoTroCongDoan = table.Column<decimal>(type: "TEXT", nullable: false),
                    PCCC_CoSo = table.Column<decimal>(type: "TEXT", nullable: false),
                    HoTroATVS_SinhVien = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoNguoiPhuThuoc = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThuocDoiTuongBaoHiemXH = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DoiTuongTruyThuBHYT = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DoiTuongPhuCapDocHai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ThamGiaCongDoan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IncentiveSixMonth1 = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IncentiveSixMonth2 = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    SoConNho = table.Column<int>(type: "INTEGER", nullable: false),
                    MaNV = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_PR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANGLUONGCHITIET_HISTORY_PR");

            migrationBuilder.DropTable(
                name: "HR_SALARY_PR");
        }
    }
}
