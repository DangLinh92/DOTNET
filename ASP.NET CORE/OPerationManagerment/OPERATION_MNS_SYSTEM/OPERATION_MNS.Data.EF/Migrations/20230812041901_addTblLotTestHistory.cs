using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblLotTestHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOT_TEST_HISTOTY_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: true),
                    ChiLayKetQua = table.Column<string>(maxLength: 50, nullable: true),
                    FA_Module = table.Column<string>(maxLength: 50, nullable: true),
                    DTC_Module = table.Column<string>(maxLength: 50, nullable: true),
                    MucDich = table.Column<string>(maxLength: 50, nullable: true),
                    ModelName = table.Column<string>(maxLength: 50, nullable: true),
                    LotNo = table.Column<string>(maxLength: 50, nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    Operation = table.Column<string>(maxLength: 50, nullable: true),
                    KetQua = table.Column<string>(maxLength: 150, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOT_TEST_HISTOTY_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOT_TEST_HISTOTY_LFEM");
        }
    }
}
