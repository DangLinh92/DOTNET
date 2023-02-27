using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateNameFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "FileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "UrlFileNameResult",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");
        }
    }
}
