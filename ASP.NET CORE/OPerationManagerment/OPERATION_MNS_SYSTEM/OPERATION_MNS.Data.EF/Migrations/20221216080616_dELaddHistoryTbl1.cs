using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class dELaddHistoryTbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_HISTORY");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_HISTORY",
                columns: table => new
                {
                    CassetteId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    History_seq = table.Column<double>(type: "float", nullable: false),
                    ChipQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPProductOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FABLotID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldComment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NguoiXuLy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhuongAnXuLy = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReleaseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReleaseComment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReleaseFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReleaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReleaseTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReleaseUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StayDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenLoi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_HISTORY", x => new { x.CassetteId, x.History_seq });
                });
        }
    }
}
