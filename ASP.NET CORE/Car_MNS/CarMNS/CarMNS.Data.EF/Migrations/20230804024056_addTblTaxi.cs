using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarMNS.Data.EF.Migrations
{
    public partial class addTblTaxi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANG_KY_XE_TAXI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgaySuDung = table.Column<DateTime>(nullable: true),
                    TenNguoiSuDung = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: false),
                    BoPhan = table.Column<string>(maxLength: 50, nullable: true),
                    FromTimePlan = table.Column<DateTime>(nullable: true),
                    ToTimePlan = table.Column<DateTime>(nullable: true),
                    DiaDiemDen_SoNha = table.Column<string>(maxLength: 250, nullable: true),
                    DiaDiemDen_Xa = table.Column<string>(maxLength: 250, nullable: true),
                    DiaDiemDen_Huyen = table.Column<string>(maxLength: 250, nullable: true),
                    DiaDiemDen_Tinh = table.Column<string>(maxLength: 250, nullable: true),
                    MucDichSuDung = table.Column<string>(maxLength: 250, nullable: true),
                    Lxe_BienSo = table.Column<string>(maxLength: 500, nullable: true),
                    SoTien = table.Column<double>(nullable: true),
                    NguoiDangKy = table.Column<string>(maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_DANG_KY_XE_TAXI", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DANG_KY_XE_TAXI");
        }
    }
}
