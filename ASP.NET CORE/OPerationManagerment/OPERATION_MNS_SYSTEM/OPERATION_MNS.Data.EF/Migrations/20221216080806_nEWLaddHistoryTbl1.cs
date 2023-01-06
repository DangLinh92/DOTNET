using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class nEWLaddHistoryTbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_HISTORY",
                columns: table => new
                {
                    LotId = table.Column<string>(maxLength: 50, nullable: false),
                    History_seq = table.Column<double>(nullable: false),
                    CassetteId = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 100, nullable: true),
                    StayDay = table.Column<decimal>(nullable: false),
                    ChipQty = table.Column<decimal>(nullable: false),
                    ERPProductOrder = table.Column<string>(maxLength: 50, nullable: true),
                    FABLotID = table.Column<string>(maxLength: 50, nullable: true),
                    HoldTime = table.Column<string>(maxLength: 50, nullable: true),
                    HoldCode = table.Column<string>(maxLength: 50, nullable: true),
                    HoldUserName = table.Column<string>(maxLength: 50, nullable: true),
                    HoldComment = table.Column<string>(maxLength: 50, nullable: true),
                    TenLoi = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiXuLy = table.Column<string>(maxLength: 50, nullable: true),
                    PhuongAnXuLy = table.Column<string>(maxLength: 1000, nullable: true),
                    ReleaseTime = table.Column<string>(maxLength: 50, nullable: true),
                    ReleaseFlag = table.Column<string>(maxLength: 1, nullable: true),
                    ReleaseCode = table.Column<string>(maxLength: 50, nullable: true),
                    ReleaseName = table.Column<string>(maxLength: 100, nullable: true),
                    ReleaseUser = table.Column<string>(maxLength: 50, nullable: true),
                    ReleaseComment = table.Column<string>(maxLength: 2000, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_HISTORY", x => new { x.LotId, x.History_seq });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_HISTORY");
        }
    }
}
