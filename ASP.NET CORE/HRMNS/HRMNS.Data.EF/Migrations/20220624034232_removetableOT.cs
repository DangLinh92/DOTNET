using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class removetableOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DANGKY_CHAMCONG_OT_DACBIET");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANGKY_CHAMCONG_OT_DACBIET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approve = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApproveLV2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApproveLV3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HourInDay = table.Column<double>(type: "float", nullable: false),
                    MaChamCong_ChiTiet = table.Column<int>(type: "int", nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayBatDau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayKetThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKY_CHAMCONG_OT_DACBIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_OT_DACBIET_DANGKY_CHAMCONG_CHITIET_MaChamCong_ChiTiet",
                        column: x => x.MaChamCong_ChiTiet,
                        principalTable: "DANGKY_CHAMCONG_CHITIET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DANGKY_CHAMCONG_OT_DACBIET_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_OT_DACBIET_MaChamCong_ChiTiet",
                table: "DANGKY_CHAMCONG_OT_DACBIET",
                column: "MaChamCong_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_DANGKY_CHAMCONG_OT_DACBIET_MaNV",
                table: "DANGKY_CHAMCONG_OT_DACBIET",
                column: "MaNV");
        }
    }
}
