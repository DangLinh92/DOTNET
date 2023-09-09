using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblOuputLotLFEM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OUT_PUT_BY_LOT_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Work_date = table.Column<string>(maxLength: 50, nullable: true),
                    Lot_id = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialGroup = table.Column<string>(maxLength: 50, nullable: true),
                    QtyInput = table.Column<double>(nullable: false),
                    QtyOutput = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_PUT_BY_LOT_LFEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OUT_PUT_SHIPPING_LOT_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Work_date = table.Column<string>(maxLength: 50, nullable: true),
                    Lot_id = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Site = table.Column<string>(maxLength: 50, nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_PUT_SHIPPING_LOT_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUT_PUT_BY_LOT_LFEM");

            migrationBuilder.DropTable(
                name: "OUT_PUT_SHIPPING_LOT_LFEM");
        }
    }
}
