using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APP_ROLE",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_ROLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APP_ROLE_CLAIM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_ROLE_CLAIM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APP_USER",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(maxLength: 250, nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ShowPass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APP_USER_CLAIM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER_CLAIM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APP_USER_LOGIN",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER_LOGIN", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "APP_USER_ROLE",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER_ROLE", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "APP_USER_TOKEN",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER_TOKEN", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BOPHAN",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenBoPhan = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOPHAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CHAM_CONG_LOG",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ngay_ChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    ID_NV = table.Column<string>(maxLength: 50, nullable: true),
                    Ten_NV = table.Column<string>(maxLength: 50, nullable: true),
                    FirstOut_Time = table.Column<string>(maxLength: 20, nullable: true),
                    Last_Out_Time = table.Column<string>(maxLength: 20, nullable: true),
                    FirstIn = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    LastOut = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHAM_CONG_LOG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_DANGKY_CHAMCONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_DANGKY_CHAMCONG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NGAY_LAMVIEC",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Ten_NgayLV = table.Column<string>(maxLength: 100, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_NGAY_LAMVIEC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FUNCTION",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    URL = table.Column<string>(maxLength: 250, nullable: false),
                    ParentId = table.Column<string>(maxLength: 128, nullable: true),
                    IconCss = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCTION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_BO_PHAN_DETAIL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBoPhanChiTiet = table.Column<string>(maxLength: 500, nullable: true),
                    MaBoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_BO_PHAN_DETAIL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_CHEDOBH",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenCheDo = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CHEDOBH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_CHUCDANH",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenChucDanh = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CHUCDANH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_LOAIHOPDONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiHD = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LOAIHOPDONG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KY_HIEU_CHAM_CONG",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 20, nullable: false),
                    GiaiThich = table.Column<string>(maxLength: 300, nullable: true),
                    Heso = table.Column<double>(nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KY_HIEU_CHAM_CONG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LANGUAGE",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Resources = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LANGUAGE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LOAICHUNGCHI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiChungChi = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAICHUNGCHI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRU_SO_LVIEC",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenTruSo = table.Column<string>(maxLength: 250, nullable: true),
                    DiaChi = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRU_SO_LVIEC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSION",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    FunctionId = table.Column<string>(maxLength: 128, nullable: false),
                    CanCreate = table.Column<bool>(nullable: false),
                    CanRead = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PERMISSION_FUNCTION_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "FUNCTION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERMISSION_APP_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalTable: "APP_ROLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_NHANVIEN",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenNV = table.Column<string>(maxLength: 250, nullable: true),
                    MaChucDanh = table.Column<string>(maxLength: 50, nullable: true),
                    MaBoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    MaBoPhanChiTiet = table.Column<int>(nullable: true),
                    GioiTinh = table.Column<string>(maxLength: 20, nullable: true),
                    NgaySinh = table.Column<string>(maxLength: 50, nullable: true),
                    NoiSinh = table.Column<string>(maxLength: 250, nullable: true),
                    TinhTrangHonNhan = table.Column<string>(maxLength: 50, nullable: true),
                    DanToc = table.Column<string>(maxLength: 50, nullable: true),
                    TonGiao = table.Column<string>(maxLength: 50, nullable: true),
                    DiaChiThuongTru = table.Column<string>(maxLength: 250, nullable: true),
                    SoDienThoai = table.Column<string>(maxLength: 50, nullable: true),
                    SoDienThoaiNguoiThan = table.Column<string>(maxLength: 50, nullable: true),
                    QuanHeNguoiThan = table.Column<string>(maxLength: 100, nullable: true),
                    CMTND = table.Column<string>(maxLength: 50, nullable: true),
                    NgayCapCMTND = table.Column<string>(maxLength: 50, nullable: true),
                    NoiCapCMTND = table.Column<string>(maxLength: 250, nullable: true),
                    SoTaiKhoanNH = table.Column<string>(maxLength: 50, nullable: true),
                    TenNganHang = table.Column<string>(maxLength: 50, nullable: true),
                    TruongDaoTao = table.Column<string>(maxLength: 500, nullable: true),
                    NgayVao = table.Column<string>(maxLength: 50, nullable: true),
                    NguyenQuan = table.Column<string>(maxLength: 250, nullable: true),
                    DChiHienTai = table.Column<string>(maxLength: 250, nullable: true),
                    KyLuatLD = table.Column<string>(maxLength: 500, nullable: true),
                    MaBHXH = table.Column<string>(maxLength: 50, nullable: true),
                    MaSoThue = table.Column<string>(maxLength: 50, nullable: true),
                    SoNguoiGiamTru = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    NgayNghiViec = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    Image = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true),
                    IsDelete = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_NHANVIEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_NHANVIEN_BOPHAN_MaBoPhan",
                        column: x => x.MaBoPhan,
                        principalTable: "BOPHAN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_NHANVIEN_HR_BO_PHAN_DETAIL_MaBoPhanChiTiet",
                        column: x => x.MaBoPhanChiTiet,
                        principalTable: "HR_BO_PHAN_DETAIL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_NHANVIEN_HR_CHUCDANH_MaChucDanh",
                        column: x => x.MaChucDanh,
                        principalTable: "HR_CHUCDANH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DANGKY_CHAMCONG_CHITIET",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChiTiet = table.Column<string>(maxLength: 150, nullable: true),
                    PhanLoaiDM = table.Column<int>(nullable: true),
                    KyHieuChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_CHAMCONG_CHITIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_CHITIET_KY_HIEU_CHAM_CONG_KyHieuChamCong",
                        column: x => x.KyHieuChamCong,
                        principalTable: "KY_HIEU_CHAM_CONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_CHITIET_DM_DANGKY_CHAMCONG_PhanLoaiDM",
                        column: x => x.PhanLoaiDM,
                        principalTable: "DM_DANGKY_CHAMCONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NGAY_LE_NAM",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 10, nullable: false),
                    TenNgayLe = table.Column<string>(maxLength: 150, nullable: true),
                    KyHieuChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGAY_LE_NAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NGAY_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong",
                        column: x => x.KyHieuChamCong,
                        principalTable: "KY_HIEU_CHAM_CONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NGAY_NGHI_BU_LE_NAM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungNghi = table.Column<string>(maxLength: 250, nullable: true),
                    KyHieuChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGAY_NGHI_BU_LE_NAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NGAY_NGHI_BU_LE_NAM_KY_HIEU_CHAM_CONG_KyHieuChamCong",
                        column: x => x.KyHieuChamCong,
                        principalTable: "KY_HIEU_CHAM_CONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CHUNG_CHI",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenChungChi = table.Column<string>(maxLength: 250, nullable: true),
                    LoaiChungChi = table.Column<int>(nullable: true),
                    DateCreated = table.Column<string>(nullable: true),
                    DateModified = table.Column<string>(nullable: true),
                    UserCreated = table.Column<string>(nullable: true),
                    UserModified = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHUNG_CHI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CHUNG_CHI_LOAICHUNGCHI_LoaiChungChi",
                        column: x => x.LoaiChungChi,
                        principalTable: "LOAICHUNGCHI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DM_CA_LVIEC",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenCaLamViec = table.Column<string>(maxLength: 100, nullable: true),
                    MaTruSo = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_CA_LVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_CA_LVIEC_TRU_SO_LVIEC_MaTruSo",
                        column: x => x.MaTruSo,
                        principalTable: "TRU_SO_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_RECORD",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    Time_Check = table.Column<string>(maxLength: 50, nullable: true),
                    Working_Status = table.Column<string>(maxLength: 20, nullable: true),
                    EL_LC = table.Column<double>(nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_RECORD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ATTENDANCE_RECORD_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ATTENDANCE_RECORD_KY_HIEU_CHAM_CONG_Working_Status",
                        column: x => x.Working_Status,
                        principalTable: "KY_HIEU_CHAM_CONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DANGKY_OT_NHANVIEN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayOT = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    DM_NgayLViec = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_OT_NHANVIEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_OT_NHANVIEN_DM_NGAY_LAMVIEC_DM_NgayLViec",
                        column: x => x.DM_NgayLViec,
                        principalTable: "DM_NGAY_LAMVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DANGKY_OT_NHANVIEN_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_BHXH",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    NgayThamGia = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_BHXH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_BHXH_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_HOPDONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHD = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    TenHD = table.Column<string>(maxLength: 500, nullable: true),
                    LoaiHD = table.Column<int>(nullable: true),
                    NgayTao = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKy = table.Column<string>(maxLength: 50, nullable: true),
                    NgayHieuLuc = table.Column<string>(maxLength: 50, nullable: true),
                    NgayHetHieuLuc = table.Column<string>(maxLength: 50, nullable: true),
                    ChucDanh = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true),
                    IsDelete = table.Column<string>(maxLength: 10, nullable: true),
                    DayNumberNoti = table.Column<int>(nullable: false, defaultValue: 30)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_HOPDONG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_HOPDONG_HR_LOAIHOPDONG_LoaiHD",
                        column: x => x.LoaiHD,
                        principalTable: "HR_LOAIHOPDONG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_HOPDONG_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_KEKHAIBAOHIEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    CheDoBH = table.Column<string>(maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    NgayThanhToan = table.Column<string>(maxLength: 50, nullable: true),
                    SoTienThanhToan = table.Column<double>(nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_KEKHAIBAOHIEM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_KEKHAIBAOHIEM_HR_CHEDOBH_CheDoBH",
                        column: x => x.CheDoBH,
                        principalTable: "HR_CHEDOBH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_KEKHAIBAOHIEM_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_PHEP_NAM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<string>(maxLength: 50, nullable: true),
                    SoPhepNam = table.Column<float>(nullable: false),
                    SoPhepConLai = table.Column<float>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_PHEP_NAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_PHEP_NAM_HR_NHANVIEN_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_QUATRINHLAMVIEC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: false),
                    TieuDe = table.Column<string>(maxLength: 500, nullable: true),
                    Note = table.Column<string>(nullable: true),
                    ThơiGianBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    ThoiGianKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_QUATRINHLAMVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_QUATRINHLAMVIEC_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_TINHTRANGHOSO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    SoYeuLyLich = table.Column<bool>(nullable: false),
                    CMTND = table.Column<bool>(nullable: false),
                    SoHoKhau = table.Column<bool>(nullable: false),
                    GiayKhaiSinh = table.Column<bool>(nullable: false),
                    BangTotNghiep = table.Column<bool>(nullable: false),
                    XacNhanDanSu = table.Column<bool>(nullable: false),
                    AnhThe = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_TINHTRANGHOSO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_TINHTRANGHOSO_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DANGKY_CHAMCONG_DACBIET",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    MaChamCong_ChiTiet = table.Column<int>(nullable: true),
                    NgayBatDau = table.Column<string>(maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 300, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_CHAMCONG_DACBIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_DACBIET_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                        column: x => x.MaChamCong_ChiTiet,
                        principalTable: "DANGKY_CHAMCONG_CHITIET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_DACBIET_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DC_CHAM_CONG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChamCong_ChiTiet = table.Column<int>(nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    NgayCanDieuChinh_From = table.Column<string>(maxLength: 50, nullable: true),
                    NgayCanDieuChinh_To = table.Column<string>(maxLength: 50, nullable: true),
                    NoiDungDC = table.Column<string>(maxLength: 300, nullable: true),
                    GiaTriSauDC = table.Column<double>(nullable: true),
                    GiaTriTruocDC = table.Column<double>(nullable: true),
                    KyHieuChamCong = table.Column<string>(maxLength: 20, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DC_CHAM_CONG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DC_CHAM_CONG_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                        column: x => x.MaChamCong_ChiTiet,
                        principalTable: "DANGKY_CHAMCONG_CHITIET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DC_CHAM_CONG_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_CHUNGCHI_NHANVIEN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    MaChungChi = table.Column<string>(maxLength: 50, nullable: true),
                    NoiCap = table.Column<string>(nullable: true),
                    NgayCap = table.Column<string>(maxLength: 50, nullable: true),
                    NgayHetHan = table.Column<string>(maxLength: 50, nullable: true),
                    ChuyenMon = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CHUNGCHI_NHANVIEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_CHUNGCHI_NHANVIEN_CHUNG_CHI_MaChungChi",
                        column: x => x.MaChungChi,
                        principalTable: "CHUNG_CHI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_CHUNGCHI_NHANVIEN_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CA_LVIEC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Danhmuc_CaLviec = table.Column<string>(maxLength: 50, nullable: true),
                    DM_NgayLViec = table.Column<string>(maxLength: 50, nullable: true),
                    TenCa = table.Column<string>(maxLength: 100, nullable: true),
                    Time_BatDau = table.Column<string>(maxLength: 50, nullable: true),
                    Time_BatDau2 = table.Column<string>(maxLength: 50, nullable: true),
                    Time_KetThuc = table.Column<string>(maxLength: 50, nullable: true),
                    Time_KetThuc2 = table.Column<string>(maxLength: 50, nullable: true),
                    HeSo_OT = table.Column<float>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CA_LVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CA_LVIEC_DM_NGAY_LAMVIEC_DM_NgayLViec",
                        column: x => x.DM_NgayLViec,
                        principalTable: "DM_NGAY_LAMVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CA_LVIEC_DM_CA_LVIEC_Danhmuc_CaLviec",
                        column: x => x.Danhmuc_CaLviec,
                        principalTable: "DM_CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN_CALAMVIEC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    Danhmuc_CaLviec = table.Column<string>(maxLength: 50, nullable: true),
                    BatDau_TheoCa = table.Column<string>(maxLength: 50, nullable: true),
                    KetThuc_TheoCa = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIEN_CALAMVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_CALAMVIEC_DM_CA_LVIEC_Danhmuc_CaLviec",
                        column: x => x.Danhmuc_CaLviec,
                        principalTable: "DM_CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_CALAMVIEC_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SETTING_TIME_DIMUON_VESOM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Danhmuc_CaLviec = table.Column<string>(maxLength: 50, nullable: true),
                    Time_LateCome = table.Column<string>(maxLength: 50, nullable: true),
                    Time_EarlyLeave = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SETTING_TIME_DIMUON_VESOM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SETTING_TIME_DIMUON_VESOM_DM_CA_LVIEC_Danhmuc_CaLviec",
                        column: x => x.Danhmuc_CaLviec,
                        principalTable: "DM_CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_OVERTIME",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaLviec = table.Column<int>(nullable: false),
                    MaAttendence = table.Column<long>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_OVERTIME", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ATTENDANCE_OVERTIME_CA_LVIEC_CaLviec",
                        column: x => x.CaLviec,
                        principalTable: "CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ATTENDANCE_OVERTIME_ATTENDANCE_RECORD_MaAttendence",
                        column: x => x.MaAttendence,
                        principalTable: "ATTENDANCE_RECORD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_OVERTIME_CaLviec",
                table: "ATTENDANCE_OVERTIME",
                column: "CaLviec");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_OVERTIME_MaAttendence",
                table: "ATTENDANCE_OVERTIME",
                column: "MaAttendence");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_RECORD_MaNV",
                table: "ATTENDANCE_RECORD",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_RECORD_Working_Status",
                table: "ATTENDANCE_RECORD",
                column: "Working_Status");

            migrationBuilder.CreateIndex(
                name: "IX_CA_LVIEC_DM_NgayLViec",
                table: "CA_LVIEC",
                column: "DM_NgayLViec");

            migrationBuilder.CreateIndex(
                name: "IX_CA_LVIEC_Danhmuc_CaLviec",
                table: "CA_LVIEC",
                column: "Danhmuc_CaLviec");

            migrationBuilder.CreateIndex(
                name: "IX_CHUNG_CHI_LoaiChungChi",
                table: "CHUNG_CHI",
                column: "LoaiChungChi");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_CHITIET_KyHieuChamCong",
                table: "DANGKY_CHAMCONG_CHITIET",
                column: "KyHieuChamCong");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_CHITIET_PhanLoaiDM",
                table: "DANGKY_CHAMCONG_CHITIET",
                column: "PhanLoaiDM");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_DACBIET_MaChamCong_ChiTiet",
                table: "DANGKY_CHAMCONG_DACBIET",
                column: "MaChamCong_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_DACBIET_MaNV",
                table: "DANGKY_CHAMCONG_DACBIET",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_OT_NHANVIEN_DM_NgayLViec",
                table: "DANGKY_OT_NHANVIEN",
                column: "DM_NgayLViec");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_OT_NHANVIEN_MaNV",
                table: "DANGKY_OT_NHANVIEN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_MaChamCong_ChiTiet",
                table: "DC_CHAM_CONG",
                column: "MaChamCong_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_DC_CHAM_CONG_MaNV",
                table: "DC_CHAM_CONG",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_DM_CA_LVIEC_MaTruSo",
                table: "DM_CA_LVIEC",
                column: "MaTruSo");

            migrationBuilder.CreateIndex(
                name: "IX_HR_BHXH_MaNV",
                table: "HR_BHXH",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_CHUNGCHI_NHANVIEN_MaChungChi",
                table: "HR_CHUNGCHI_NHANVIEN",
                column: "MaChungChi");

            migrationBuilder.CreateIndex(
                name: "IX_HR_CHUNGCHI_NHANVIEN_MaNV",
                table: "HR_CHUNGCHI_NHANVIEN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_HOPDONG_LoaiHD",
                table: "HR_HOPDONG",
                column: "LoaiHD");

            migrationBuilder.CreateIndex(
                name: "IX_HR_HOPDONG_MaNV",
                table: "HR_HOPDONG",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_KEKHAIBAOHIEM_CheDoBH",
                table: "HR_KEKHAIBAOHIEM",
                column: "CheDoBH");

            migrationBuilder.CreateIndex(
                name: "IX_HR_KEKHAIBAOHIEM_MaNV",
                table: "HR_KEKHAIBAOHIEM",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_NHANVIEN_MaBoPhan",
                table: "HR_NHANVIEN",
                column: "MaBoPhan");

            migrationBuilder.CreateIndex(
                name: "IX_HR_NHANVIEN_MaBoPhanChiTiet",
                table: "HR_NHANVIEN",
                column: "MaBoPhanChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_HR_NHANVIEN_MaChucDanh",
                table: "HR_NHANVIEN",
                column: "MaChucDanh");

            migrationBuilder.CreateIndex(
                name: "IX_HR_PHEP_NAM_MaNhanVien",
                table: "HR_PHEP_NAM",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_HR_QUATRINHLAMVIEC_MaNV",
                table: "HR_QUATRINHLAMVIEC",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_TINHTRANGHOSO_MaNV",
                table: "HR_TINHTRANGHOSO",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_NGAY_LE_NAM_KyHieuChamCong",
                table: "NGAY_LE_NAM",
                column: "KyHieuChamCong");

            migrationBuilder.CreateIndex(
                name: "IX_NGAY_NGHI_BU_LE_NAM_KyHieuChamCong",
                table: "NGAY_NGHI_BU_LE_NAM",
                column: "KyHieuChamCong");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_CALAMVIEC_Danhmuc_CaLviec",
                table: "NHANVIEN_CALAMVIEC",
                column: "Danhmuc_CaLviec");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_CALAMVIEC_MaNV",
                table: "NHANVIEN_CALAMVIEC",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSION_FunctionId",
                table: "PERMISSION",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSION_RoleId",
                table: "PERMISSION",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SETTING_TIME_DIMUON_VESOM_Danhmuc_CaLviec",
                table: "SETTING_TIME_DIMUON_VESOM",
                column: "Danhmuc_CaLviec");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APP_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "APP_USER");

            migrationBuilder.DropTable(
                name: "APP_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "APP_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "APP_USER_ROLE");

            migrationBuilder.DropTable(
                name: "APP_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_OVERTIME");

            migrationBuilder.DropTable(
                name: "CHAM_CONG_LOG");

            migrationBuilder.DropTable(
                name: "DANGKY_CHAMCONG_DACBIET");

            migrationBuilder.DropTable(
                name: "DANGKY_OT_NHANVIEN");

            migrationBuilder.DropTable(
                name: "DC_CHAM_CONG");

            migrationBuilder.DropTable(
                name: "HR_BHXH");

            migrationBuilder.DropTable(
                name: "HR_CHUNGCHI_NHANVIEN");

            migrationBuilder.DropTable(
                name: "HR_HOPDONG");

            migrationBuilder.DropTable(
                name: "HR_KEKHAIBAOHIEM");

            migrationBuilder.DropTable(
                name: "HR_PHEP_NAM");

            migrationBuilder.DropTable(
                name: "HR_QUATRINHLAMVIEC");

            migrationBuilder.DropTable(
                name: "HR_TINHTRANGHOSO");

            migrationBuilder.DropTable(
                name: "LANGUAGE");

            migrationBuilder.DropTable(
                name: "NGAY_LE_NAM");

            migrationBuilder.DropTable(
                name: "NGAY_NGHI_BU_LE_NAM");

            migrationBuilder.DropTable(
                name: "NHANVIEN_CALAMVIEC");

            migrationBuilder.DropTable(
                name: "PERMISSION");

            migrationBuilder.DropTable(
                name: "SETTING_TIME_DIMUON_VESOM");

            migrationBuilder.DropTable(
                name: "CA_LVIEC");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_RECORD");

            migrationBuilder.DropTable(
                name: "DANGKY_CHAMCONG_CHITIET");

            migrationBuilder.DropTable(
                name: "CHUNG_CHI");

            migrationBuilder.DropTable(
                name: "HR_LOAIHOPDONG");

            migrationBuilder.DropTable(
                name: "HR_CHEDOBH");

            migrationBuilder.DropTable(
                name: "FUNCTION");

            migrationBuilder.DropTable(
                name: "APP_ROLE");

            migrationBuilder.DropTable(
                name: "DM_NGAY_LAMVIEC");

            migrationBuilder.DropTable(
                name: "DM_CA_LVIEC");

            migrationBuilder.DropTable(
                name: "HR_NHANVIEN");

            migrationBuilder.DropTable(
                name: "KY_HIEU_CHAM_CONG");

            migrationBuilder.DropTable(
                name: "DM_DANGKY_CHAMCONG");

            migrationBuilder.DropTable(
                name: "LOAICHUNGCHI");

            migrationBuilder.DropTable(
                name: "TRU_SO_LVIEC");

            migrationBuilder.DropTable(
                name: "BOPHAN");

            migrationBuilder.DropTable(
                name: "HR_BO_PHAN_DETAIL");

            migrationBuilder.DropTable(
                name: "HR_CHUCDANH");
        }
    }
}
