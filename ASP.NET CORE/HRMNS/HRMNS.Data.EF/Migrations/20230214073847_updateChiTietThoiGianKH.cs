using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateChiTietThoiGianKH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShowBoard",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "IsShowBoard",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");
        }
    }
}
