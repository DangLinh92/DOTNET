using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class initDB : Migration
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
                    ShowPass = table.Column<string>(nullable: true),
                    Department = table.Column<string>(maxLength: 50, nullable: true),
                    MaNhanVien = table.Column<string>(maxLength: 50, nullable: true)
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
                name: "BOPHAN",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TenBoPhan = table.Column<string>(maxLength: 150, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOPHAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CAR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BienSoXe = table.Column<string>(maxLength: 50, nullable: true),
                    LoaiXe = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DANG_KY_XE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgaySuDung = table.Column<DateTime>(nullable: true),
                    TenNguoiSuDung = table.Column<string>(maxLength: 50, nullable: true),
                    BoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    FromTimePlan = table.Column<string>(maxLength: 50, nullable: true),
                    ToTimePlan = table.Column<string>(maxLength: 50, nullable: true),
                    FromTimePlanActual = table.Column<string>(maxLength: 50, nullable: true),
                    ToTimePlanActual = table.Column<string>(maxLength: 50, nullable: true),
                    DiaDiemDi = table.Column<string>(maxLength: 250, nullable: true),
                    DiaDiemDen = table.Column<string>(maxLength: 250, nullable: true),
                    MucDichSuDung = table.Column<string>(maxLength: 250, nullable: true),
                    UrlDraf = table.Column<string>(maxLength: 500, nullable: true),
                    XacNhanLV1 = table.Column<bool>(nullable: false),
                    Nguoi_XacNhanLV1 = table.Column<string>(maxLength: 50, nullable: true),
                    XacNhanLV2 = table.Column<bool>(nullable: false),
                    Nguoi_XacNhanLV2 = table.Column<string>(maxLength: 50, nullable: true),
                    XacNhanLV3 = table.Column<bool>(nullable: false),
                    Nguoi_XacNhanLV3 = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANG_KY_XE", x => x.Id);
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
                    Area = table.Column<string>(maxLength: 150, nullable: true),
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
                name: "LAI_XE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(maxLength: 150, nullable: true),
                    CMTND = table.Column<string>(maxLength: 250, nullable: true),
                    SoDienThoai = table.Column<string>(maxLength: 20, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAI_XE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APP_USER_TOKEN",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USER_TOKEN", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_APP_USER_TOKEN_APP_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "APP_USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CanDelete = table.Column<bool>(nullable: false),
                    ApproveL1 = table.Column<bool>(nullable: false),
                    ApproveL2 = table.Column<bool>(nullable: false),
                    ApproveL3 = table.Column<bool>(nullable: false)
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
                name: "DIEUXE_DANGKY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDangKyXe = table.Column<int>(nullable: false),
                    MaXe = table.Column<int>(nullable: false),
                    MaLaiXe = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIEUXE_DANGKY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DIEUXE_DANGKY_DANG_KY_XE_MaDangKyXe",
                        column: x => x.MaDangKyXe,
                        principalTable: "DANG_KY_XE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DIEUXE_DANGKY_LAI_XE_MaLaiXe",
                        column: x => x.MaLaiXe,
                        principalTable: "LAI_XE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DIEUXE_DANGKY_CAR_MaXe",
                        column: x => x.MaXe,
                        principalTable: "CAR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LAI_XE_CAR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(nullable: false),
                    LaxeId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAI_XE_CAR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LAI_XE_CAR_CAR_CarId",
                        column: x => x.CarId,
                        principalTable: "CAR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LAI_XE_CAR_LAI_XE_LaxeId",
                        column: x => x.LaxeId,
                        principalTable: "LAI_XE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DIEUXE_DANGKY_MaDangKyXe",
                table: "DIEUXE_DANGKY",
                column: "MaDangKyXe");

            migrationBuilder.CreateIndex(
                name: "IX_DIEUXE_DANGKY_MaLaiXe",
                table: "DIEUXE_DANGKY",
                column: "MaLaiXe");

            migrationBuilder.CreateIndex(
                name: "IX_DIEUXE_DANGKY_MaXe",
                table: "DIEUXE_DANGKY",
                column: "MaXe");

            migrationBuilder.CreateIndex(
                name: "IX_LAI_XE_CAR_CarId",
                table: "LAI_XE_CAR",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_LAI_XE_CAR_LaxeId",
                table: "LAI_XE_CAR",
                column: "LaxeId");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSION_FunctionId",
                table: "PERMISSION",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSION_RoleId",
                table: "PERMISSION",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APP_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "APP_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "APP_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "APP_USER_ROLE");

            migrationBuilder.DropTable(
                name: "APP_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "BOPHAN");

            migrationBuilder.DropTable(
                name: "DIEUXE_DANGKY");

            migrationBuilder.DropTable(
                name: "LAI_XE_CAR");

            migrationBuilder.DropTable(
                name: "PERMISSION");

            migrationBuilder.DropTable(
                name: "APP_USER");

            migrationBuilder.DropTable(
                name: "DANG_KY_XE");

            migrationBuilder.DropTable(
                name: "CAR");

            migrationBuilder.DropTable(
                name: "LAI_XE");

            migrationBuilder.DropTable(
                name: "FUNCTION");

            migrationBuilder.DropTable(
                name: "APP_ROLE");
        }
    }
}
