using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations.SqliteMigrations
{
    public partial class addtblsalaryphatsinhpr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_SALARY_PHATSINH_PR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DanhMucPhatSinh = table.Column<int>(nullable: false),
                    SoTien = table.Column<double>(nullable: false),
                    ThoiGianApDung_From = table.Column<DateTime>(nullable: true),
                    ThoiGianApDung_To = table.Column<DateTime>(nullable: true),
                    FromTime = table.Column<string>(maxLength: 50, nullable: true),
                    ToTime = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SALARY_PHATSINH_PR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_SALARY_PHATSINH_PR");
        }
    }
}
