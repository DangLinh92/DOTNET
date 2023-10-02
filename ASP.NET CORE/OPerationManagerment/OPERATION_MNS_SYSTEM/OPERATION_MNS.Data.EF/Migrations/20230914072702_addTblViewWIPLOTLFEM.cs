using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblViewWIPLOTLFEM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VIEW_WIP_LOT_LIST_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialCategory = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialName = table.Column<string>(maxLength: 50, nullable: true),
                    LotID = table.Column<string>(maxLength: 50, nullable: true),
                    ProductOrder = table.Column<string>(maxLength: 50, nullable: true),
                    FAID = table.Column<string>(maxLength: 50, nullable: true),
                    AssyLotID = table.Column<string>(maxLength: 50, nullable: true),
                    MatVendor = table.Column<string>(maxLength: 50, nullable: true),
                    LotCategory = table.Column<string>(maxLength: 50, nullable: true),
                    LotType = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    Operation = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    StayDay = table.Column<int>(nullable: false),
                    ChipQty = table.Column<decimal>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    Equipment = table.Column<string>(maxLength: 50, nullable: true),
                    EquipmentName = table.Column<string>(maxLength: 50, nullable: true),
                    Worker = table.Column<string>(maxLength: 50, nullable: true),
                    Hold = table.Column<string>(maxLength: 50, nullable: true),
                    Rework = table.Column<string>(maxLength: 50, nullable: true),
                    ReworkCode = table.Column<string>(maxLength: 50, nullable: true),
                    Site = table.Column<string>(maxLength: 50, nullable: true),
                    Route = table.Column<string>(maxLength: 50, nullable: true),
                    RouteName = table.Column<string>(maxLength: 50, nullable: true),
                    MarkingNo = table.Column<string>(maxLength: 50, nullable: true),
                    TotalInspection = table.Column<string>(maxLength: 50, nullable: true),
                    IN_EX = table.Column<string>(maxLength: 50, nullable: true),
                    VIEnd = table.Column<string>(maxLength: 50, nullable: true),
                    ShipVendor = table.Column<string>(maxLength: 50, nullable: true),
                    Comment = table.Column<string>(maxLength: 50, nullable: true),
                    _Day = table.Column<string>(maxLength: 50, nullable: true),
                    Time = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIEW_WIP_LOT_LIST_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VIEW_WIP_LOT_LIST_LFEM");
        }
    }
}
