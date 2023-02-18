using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateNgayKetThuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualFinish",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_PCCC");

            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM");

            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD");

            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA");

            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC");

            migrationBuilder.DropColumn(
                name: "ActualFinish",
                table: "EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK");
        }
    }
}
