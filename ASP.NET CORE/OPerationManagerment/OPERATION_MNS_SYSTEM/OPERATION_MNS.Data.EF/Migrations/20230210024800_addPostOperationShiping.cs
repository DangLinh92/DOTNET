using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addPostOperationShiping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POST_OPERATION_SHIPPING",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoveOutTime = table.Column<string>(maxLength: 50, nullable: true),
                    LotID = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    CassetteID = table.Column<string>(maxLength: 50, nullable: true),
                    Module = table.Column<string>(maxLength: 100, nullable: true),
                    WaferId = table.Column<string>(maxLength: 50, nullable: true),
                    DefaultChipQty = table.Column<double>(nullable: true),
                    OutputQty = table.Column<double>(nullable: true),
                    ChipMesQty = table.Column<double>(nullable: true),
                    DiffMapMes = table.Column<double>(nullable: true),
                    Rate = table.Column<double>(nullable: true),
                    VanDeDacBiet = table.Column<string>(maxLength: 250, nullable: true),
                    WaferQty = table.Column<double>(nullable: true),
                    ChipQty = table.Column<double>(nullable: true),
                    NguoiXuat = table.Column<string>(maxLength: 50, nullable: true),
                    NguoiKiemTraFA = table.Column<string>(maxLength: 50, nullable: true),
                    NguoiNhan = table.Column<string>(maxLength: 50, nullable: true),
                    NguoiKiemTra = table.Column<string>(nullable: true),
                    GhiChu_XH2 = table.Column<string>(maxLength: 250, nullable: true),
                    GhiChu_XH3 = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_OPERATION_SHIPPING", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POST_OPERATION_SHIPPING");
        }
    }
}
