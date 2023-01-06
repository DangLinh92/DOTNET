using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddTblCTQ1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VIEW_CONTROL_CHART_MODEL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHART_X = table.Column<string>(nullable: true),
                    DATE = table.Column<string>(nullable: true),
                    MATERIAL_ID = table.Column<string>(nullable: true),
                    LOT_ID = table.Column<string>(nullable: true),
                    CASSETTE_ID = table.Column<string>(nullable: true),
                    MAIN_OPERATION = table.Column<string>(nullable: true),
                    MAIN_EQUIPMENT_ID = table.Column<string>(nullable: true),
                    MAIN_EQUIPMENT_NAME = table.Column<string>(nullable: true),
                    MAIN_CHARACTER = table.Column<string>(nullable: true),
                    MAIN_UNIT = table.Column<string>(nullable: true),
                    MAIN_TARGET_USL = table.Column<double>(nullable: false),
                    MAIN_FIXED_UCL = table.Column<double>(nullable: false),
                    MAIN_TARGET = table.Column<double>(nullable: false),
                    MAIN_FIXED_LCL = table.Column<double>(nullable: false),
                    MAIN_TARGET_LSL = table.Column<double>(nullable: false),
                    MAIN_TARGET_UCL = table.Column<double>(nullable: false),
                    MAIN_TARGET_LCL = table.Column<double>(nullable: false),
                    MAIN_VALUE_COUNT = table.Column<double>(nullable: false),
                    MAIN_VALUE1 = table.Column<double>(nullable: false),
                    MAIN_VALUE2 = table.Column<double>(nullable: false),
                    MAIN_VALUE3 = table.Column<double>(nullable: false),
                    MAIN_VALUE4 = table.Column<double>(nullable: false),
                    MAIN_VALUE5 = table.Column<double>(nullable: false),
                    MAIN_VALUE6 = table.Column<double>(nullable: false),
                    MAIN_VALUE7 = table.Column<double>(nullable: false),
                    MAIN_VALUE8 = table.Column<double>(nullable: false),
                    MAIN_VALUE9 = table.Column<double>(nullable: false),
                    MAIN_VALUE10 = table.Column<double>(nullable: false),
                    MAIN_VALUE11 = table.Column<double>(nullable: false),
                    MAIN_VALUE12 = table.Column<double>(nullable: false),
                    MAIN_VALUE13 = table.Column<double>(nullable: false),
                    MAIN_VALUE14 = table.Column<double>(nullable: false),
                    MAIN_VALUE15 = table.Column<double>(nullable: false),
                    MAIN_VALUE16 = table.Column<double>(nullable: false),
                    MAIN_VALUE17 = table.Column<double>(nullable: false),
                    MAIN_VALUE18 = table.Column<double>(nullable: false),
                    MAIN_VALUE19 = table.Column<double>(nullable: false),
                    MAIN_VALUE20 = table.Column<double>(nullable: false),
                    MAIN_VALUE21 = table.Column<double>(nullable: false),
                    MAIN_VALUE22 = table.Column<double>(nullable: false),
                    MAIN_VALUE23 = table.Column<double>(nullable: false),
                    MAIN_VALUE24 = table.Column<double>(nullable: false),
                    MAIN_VALUE25 = table.Column<double>(nullable: false),
                    MAIN_VALUE26 = table.Column<double>(nullable: false),
                    MAIN_VALUE27 = table.Column<double>(nullable: false),
                    MAIN_VALUE28 = table.Column<double>(nullable: false),
                    MAIN_VALUE29 = table.Column<double>(nullable: false),
                    MAIN_VALUE30 = table.Column<double>(nullable: false),
                    MAIN_MAX_VALUE = table.Column<double>(nullable: false),
                    MAIN_MIN_VALUE = table.Column<double>(nullable: false),
                    MAIN_AVG_VALUE = table.Column<double>(nullable: false),
                    MAIN_RANGE = table.Column<double>(nullable: false),
                    MAIN_JUDGE_FLAG = table.Column<string>(maxLength: 50, nullable: true),
                    IsSendTeams = table.Column<string>(maxLength: 10, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIEW_CONTROL_CHART_MODEL", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VIEW_CONTROL_CHART_MODEL");
        }
    }
}
