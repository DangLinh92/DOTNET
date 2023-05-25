using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblSalaryHis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncentiveBase",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "HR_SALARY");

            migrationBuilder.AddColumn<decimal>(
                name: "BasicSalary",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoTroATVS_SinhVien",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoTroCongDoan",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IncentiveStandard",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PCCC_CoSo",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SoConNho",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SoNguoiPhuThuoc",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ThamGiaCongDoan",
                table: "HR_SALARY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "HR_SALARY_HISTORY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasicSalary = table.Column<decimal>(nullable: false),
                    LivingAllowance = table.Column<decimal>(nullable: false),
                    PositionAllowance = table.Column<decimal>(nullable: false),
                    AbilityAllowance = table.Column<decimal>(nullable: false),
                    FullAttendanceSupport = table.Column<decimal>(nullable: false),
                    SeniorityAllowance = table.Column<decimal>(nullable: false),
                    HarmfulAllowance = table.Column<decimal>(nullable: false),
                    IncentiveStandard = table.Column<decimal>(nullable: false),
                    IncentiveLanguage = table.Column<decimal>(nullable: false),
                    IncentiveTechnical = table.Column<decimal>(nullable: false),
                    IncentiveOther = table.Column<decimal>(nullable: false),
                    HoTroCongDoan = table.Column<decimal>(nullable: false),
                    PCCC_CoSo = table.Column<decimal>(nullable: false),
                    HoTroATVS_SinhVien = table.Column<decimal>(nullable: false),
                    SoNguoiPhuThuoc = table.Column<decimal>(nullable: false),
                    ThuocDoiTuongBaoHiemXH = table.Column<decimal>(nullable: false),
                    DoiTuongTruyThuBHYT = table.Column<decimal>(nullable: false),
                    ThamGiaCongDoan = table.Column<string>(maxLength: 50, nullable: true),
                    IncentiveSixMonth1 = table.Column<decimal>(nullable: false),
                    IncentiveSixMonth2 = table.Column<decimal>(nullable: false),
                    CI_SixMonth1 = table.Column<decimal>(nullable: false),
                    CI_SixMonth2 = table.Column<decimal>(nullable: false),
                    SoConNho = table.Column<int>(nullable: false),
                    ThoiGian = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_HISTORY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_SALARY_HISTORY_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HR_SALARY_PHATSINH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruQuyPhongChongThienTai = table.Column<decimal>(nullable: false),
                    Thuong = table.Column<decimal>(nullable: false),
                    ThoiGianApDung = table.Column<DateTime>(nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_PHATSINH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_SALARY_PHATSINH_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SALARY_HISTORY_MaNV",
                table: "HR_SALARY_HISTORY",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HR_SALARY_PHATSINH_MaNV",
                table: "HR_SALARY_PHATSINH",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_SALARY_HISTORY");

            migrationBuilder.DropTable(
                name: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "DoiTuongTruyThuBHYT",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "HoTroATVS_SinhVien",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "HoTroCongDoan",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "IncentiveStandard",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "PCCC_CoSo",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "SoConNho",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "SoNguoiPhuThuoc",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "ThamGiaCongDoan",
                table: "HR_SALARY");

            migrationBuilder.DropColumn(
                name: "ThuocDoiTuongBaoHiemXH",
                table: "HR_SALARY");

            migrationBuilder.AddColumn<decimal>(
                name: "IncentiveBase",
                table: "HR_SALARY",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "HR_SALARY",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }
    }
}
