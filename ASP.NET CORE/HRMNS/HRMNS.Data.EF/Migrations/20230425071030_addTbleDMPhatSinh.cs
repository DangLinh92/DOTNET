using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTbleDMPhatSinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CI_SixMonth1",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "CI_SixMonth2",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "NoiDung",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "PI_SixMonth1",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "PI_SixMonth2",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "ThoiGianApDung",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "Thuong_Khac",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "Thuong_Tet",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "TruQuyPhongChongThienTai",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.AddColumn<int>(
                name: "DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SoTien",
                table: "HR_SALARY_PHATSINH",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianApDung_From",
                table: "HR_SALARY_PHATSINH",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianApDung_To",
                table: "HR_SALARY_PHATSINH",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HR_SALARY_DANHMUC_PHATSINH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DanhMuc = table.Column<string>(nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_DANHMUC_PHATSINH", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SALARY_PHATSINH_DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH",
                column: "DanhMucPhatSinh");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_SALARY_PHATSINH_HR_SALARY_DANHMUC_PHATSINH_DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH",
                column: "DanhMucPhatSinh",
                principalTable: "HR_SALARY_DANHMUC_PHATSINH",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_SALARY_PHATSINH_HR_SALARY_DANHMUC_PHATSINH_DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropTable(
                name: "HR_SALARY_DANHMUC_PHATSINH");

            migrationBuilder.DropIndex(
                name: "IX_HR_SALARY_PHATSINH_DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "DanhMucPhatSinh",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "SoTien",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "ThoiGianApDung_From",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.DropColumn(
                name: "ThoiGianApDung_To",
                table: "HR_SALARY_PHATSINH");

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth1",
                table: "HR_SALARY_PHATSINH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CI_SixMonth2",
                table: "HR_SALARY_PHATSINH",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDung",
                table: "HR_SALARY_PHATSINH",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PI_SixMonth1",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PI_SixMonth2",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianApDung",
                table: "HR_SALARY_PHATSINH",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Thuong_Khac",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Thuong_Tet",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TruQuyPhongChongThienTai",
                table: "HR_SALARY_PHATSINH",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
