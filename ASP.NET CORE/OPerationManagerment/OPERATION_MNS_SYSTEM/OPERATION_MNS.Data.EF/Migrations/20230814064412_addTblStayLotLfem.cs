using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblStayLotLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAY_LOT_LIST_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialCategory = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    LotId = table.Column<string>(maxLength: 50, nullable: true),
                    FAID = table.Column<string>(maxLength: 50, nullable: true),
                    AssyLotID = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    DATE_DIFF = table.Column<int>(nullable: false),
                    ChipQty = table.Column<double>(nullable: false),
                    Worker = table.Column<string>(maxLength: 150, nullable: true),
                    Comment = table.Column<string>(maxLength: 250, nullable: true),
                    LoaiLoi = table.Column<string>(maxLength: 100, nullable: true),
                    LichTrinhXuLy = table.Column<string>(maxLength: 100, nullable: true),
                    ChiuTrachNhiem = table.Column<string>(maxLength: 100, nullable: true),
                    History_seq = table.Column<double>(nullable: false),
                    ReleaseFlag = table.Column<string>(maxLength: 1, nullable: true),
                    History_delete_flag = table.Column<string>(maxLength: 1, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAY_LOT_LIST_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAY_LOT_LIST_LFEM");
        }
    }
}
