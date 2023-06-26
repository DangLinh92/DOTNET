using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTbleHoldSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_SAMPLE",
                columns: table => new
                {
                    LotId = table.Column<string>(maxLength: 50, nullable: false),
                    History_seq = table.Column<double>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    CassetteId = table.Column<string>(maxLength: 50, nullable: true),
                    PhuongAnXuLy = table.Column<string>(maxLength: 1000, nullable: true),
                    TenLoi = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiXuLy = table.Column<string>(maxLength: 50, nullable: true),
                    ReleaseFlag = table.Column<string>(maxLength: 1, nullable: true),
                    History_delete_flag = table.Column<string>(maxLength: 1, nullable: true),
                    PhanLoaiLoi = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_SAMPLE", x => new { x.LotId, x.History_seq });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_SAMPLE");
        }
    }
}
