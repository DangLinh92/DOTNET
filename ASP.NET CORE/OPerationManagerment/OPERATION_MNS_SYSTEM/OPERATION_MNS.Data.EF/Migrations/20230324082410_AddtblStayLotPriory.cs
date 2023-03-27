using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddtblStayLotPriory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_PRIORY_WLP2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SapCode = table.Column<string>(maxLength: 50, nullable: true),
                    Priory = table.Column<bool>(nullable: false),
                    Number_Priory = table.Column<int>(nullable: false),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    CassetteID = table.Column<string>(maxLength: 50, nullable: true),
                    LotID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    OperationId = table.Column<string>(maxLength: 50, nullable: true),
                    ERPProductionOrder = table.Column<float>(nullable: false),
                    ChipQty = table.Column<float>(nullable: false),
                    StayDay = table.Column<float>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_PRIORY_WLP2", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_PRIORY_WLP2");
        }
    }
}
