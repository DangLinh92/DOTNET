using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblNG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoiSachCaiTien",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQua",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EHS_HANGMUC_NG",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNgayChiTiet = table.Column<string>(maxLength: 50, nullable: true),
                    HangMucNG = table.Column<string>(maxLength: 500, nullable: true),
                    NoiDungVanDeNG = table.Column<string>(maxLength: 1000, nullable: true),
                    NguyenNhan = table.Column<string>(maxLength: 500, nullable: true),
                    DoiSachCaiTien = table.Column<string>(maxLength: 1000, nullable: true),
                    TinhHinhCaiTien = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_HANGMUC_NG", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_HANGMUC_NG");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "DoiSachCaiTien",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "KetQua",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");
        }
    }
}
