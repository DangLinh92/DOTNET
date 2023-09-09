using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblRuntime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WARNING_LOT_RUN_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(nullable: true),
                    STBMin = table.Column<double>(nullable: false),
                    STBHour = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WARNING_LOT_RUN_LFEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WARNING_LOT_RUNTIME_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialCategory = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    LotID = table.Column<string>(maxLength: 50, nullable: true),
                    FA_ID = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    ChipQTy = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    StartFlg = table.Column<string>(maxLength: 50, nullable: true),
                    Comment = table.Column<string>(maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    STBMin = table.Column<double>(nullable: false),
                    TimeStart = table.Column<string>(maxLength: 50, nullable: true),
                    DateNow = table.Column<string>(maxLength: 50, nullable: true),
                    TimeNow = table.Column<string>(maxLength: 50, nullable: true),
                    RunTime_hmm = table.Column<string>(maxLength: 50, nullable: true),
                    RunTime_m = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WARNING_LOT_RUNTIME_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WARNING_LOT_RUN_LFEM");

            migrationBuilder.DropTable(
                name: "WARNING_LOT_RUNTIME_LFEM");
        }
    }
}
