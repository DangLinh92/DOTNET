using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class AddELTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANGKY_DIMUON_VSOM_NHANVIEN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDangKy = table.Column<string>(maxLength: 50, nullable: true),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    SoGioDangKy = table.Column<double>(nullable: false),
                    NoiDung = table.Column<string>(maxLength: 250, nullable: true),
                    Approve = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveLV2 = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveLV3 = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_DIMUON_VSOM_NHANVIEN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_DIMUON_VSOM_NHANVIEN_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_DIMUON_VSOM_NHANVIEN_MaNV",
                table: "DANGKY_DIMUON_VSOM_NHANVIEN",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DANGKY_DIMUON_VSOM_NHANVIEN");
        }
    }
}
