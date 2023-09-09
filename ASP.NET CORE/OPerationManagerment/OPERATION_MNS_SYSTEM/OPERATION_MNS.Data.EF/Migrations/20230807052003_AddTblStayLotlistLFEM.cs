using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddTblStayLotlistLFEM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_PRIORY_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StayDay = table.Column<float>(nullable: false),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    MesItem = table.Column<string>(maxLength: 50, nullable: true),
                    LotID = table.Column<string>(maxLength: 50, nullable: true),
                    ProductOrder = table.Column<string>(maxLength: 50, nullable: true),
                    FAID = table.Column<string>(maxLength: 50, nullable: true),
                    AssyLotID = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    OperationId = table.Column<string>(maxLength: 50, nullable: true),
                    DateDiff = table.Column<float>(nullable: false),
                    ChipQty = table.Column<float>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    StartFlag = table.Column<string>(maxLength: 50, nullable: true),
                    EquipmentName = table.Column<string>(maxLength: 50, nullable: true),
                    Worker = table.Column<string>(maxLength: 50, nullable: true),
                    Priory = table.Column<bool>(nullable: false),
                    Number_Priory = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_PRIORY_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_PRIORY_LFEM");
        }
    }
}
