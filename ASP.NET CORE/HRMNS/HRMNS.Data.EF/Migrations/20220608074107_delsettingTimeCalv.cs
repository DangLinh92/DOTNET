using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class delsettingTimeCalv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SETTING_TIME_CA_LVIEC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SETTING_TIME_CA_LVIEC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaLamViec = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayBatDauDangKy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayKetThucDangKy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SETTING_TIME_CA_LVIEC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SETTING_TIME_CA_LVIEC_DM_CA_LVIEC_CaLamViec",
                        column: x => x.CaLamViec,
                        principalTable: "DM_CA_LVIEC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SETTING_TIME_CA_LVIEC_CaLamViec",
                table: "SETTING_TIME_CA_LVIEC",
                column: "CaLamViec");
        }
    }
}
