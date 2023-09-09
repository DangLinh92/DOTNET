using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblDailyPlanDataLFem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DAILY_PLAN_DATA_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    MesItemId = table.Column<string>(maxLength: 50, nullable: true),
                    Dam_WIP = table.Column<double>(nullable: false),
                    Mold_WIP = table.Column<double>(nullable: false),
                    Grinding_WIP = table.Column<double>(nullable: false),
                    Marking_WIP = table.Column<double>(nullable: false),
                    Dicing_WIP = table.Column<double>(nullable: false),
                    Test_WIP = table.Column<double>(nullable: false),
                    VisualInspection_WIP = table.Column<double>(nullable: false),
                    OQC_WIP = table.Column<double>(nullable: false),
                    Dam_KHSX = table.Column<double>(nullable: false),
                    Mold_KHSX = table.Column<double>(nullable: false),
                    Grinding_KHSX = table.Column<double>(nullable: false),
                    Marking_KHSX = table.Column<double>(nullable: false),
                    Dicing_KHSX = table.Column<double>(nullable: false),
                    Test_KHSX = table.Column<double>(nullable: false),
                    VisualInspection_KHSX = table.Column<double>(nullable: false),
                    OQC_KHSX = table.Column<double>(nullable: false),
                    Dam_PROD = table.Column<double>(nullable: false),
                    Mold_PROD = table.Column<double>(nullable: false),
                    Grinding_PROD = table.Column<double>(nullable: false),
                    Marking_PROD = table.Column<double>(nullable: false),
                    Dicing_PROD = table.Column<double>(nullable: false),
                    Test_PROD = table.Column<double>(nullable: false),
                    VisualInspection_PROD = table.Column<double>(nullable: false),
                    OQC_PROD = table.Column<double>(nullable: false),
                    DateActual = table.Column<string>(maxLength: 50, nullable: true),
                    WeekActual = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAILY_PLAN_DATA_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DAILY_PLAN_DATA_LFEM");
        }
    }
}
