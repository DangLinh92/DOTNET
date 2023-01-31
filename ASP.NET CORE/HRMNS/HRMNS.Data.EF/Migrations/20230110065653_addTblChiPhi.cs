using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblChiPhi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EHS_CHIPHI_BY_MONTH",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    MaNoiDungKeHoach = table.Column<Guid>(nullable: false),
                    ChiPhi1 = table.Column<double>(nullable: false),
                    ChiPhi2 = table.Column<double>(nullable: false),
                    ChiPhi3 = table.Column<double>(nullable: false),
                    ChiPhi4 = table.Column<double>(nullable: false),
                    ChiPhi5 = table.Column<double>(nullable: false),
                    ChiPhi6 = table.Column<double>(nullable: false),
                    ChiPhi7 = table.Column<double>(nullable: false),
                    ChiPhi8 = table.Column<double>(nullable: false),
                    ChiPhi9 = table.Column<double>(nullable: false),
                    ChiPhi10 = table.Column<double>(nullable: false),
                    ChiPhi11 = table.Column<double>(nullable: false),
                    ChiPhi12 = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EHS_CHIPHI_BY_MONTH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EHS_CHIPHI_BY_MONTH_EHS_NOIDUNG_KEHOACH_MaNoiDungKeHoach",
                        column: x => x.MaNoiDungKeHoach,
                        principalTable: "EHS_NOIDUNG_KEHOACH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EHS_CHIPHI_BY_MONTH_MaNoiDungKeHoach",
                table: "EHS_CHIPHI_BY_MONTH",
                column: "MaNoiDungKeHoach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EHS_CHIPHI_BY_MONTH");
        }
    }
}
