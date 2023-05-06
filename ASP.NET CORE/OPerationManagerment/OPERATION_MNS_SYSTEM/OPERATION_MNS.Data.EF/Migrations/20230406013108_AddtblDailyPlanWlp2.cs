using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddtblDailyPlanWlp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DAILY_PLAN_WLP2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    DatePlan = table.Column<string>(maxLength: 50, nullable: true),
                    BackGrinding = table.Column<float>(nullable: false),
                    WaferOven = table.Column<float>(nullable: false),
                    Dicing = table.Column<float>(nullable: false),
                    ChipInspection = table.Column<float>(nullable: false),
                    Packing = table.Column<float>(nullable: false),
                    ReelInspection = table.Column<float>(nullable: false),
                    QC_Pass = table.Column<float>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAILY_PLAN_WLP2", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DAILY_PLAN_WLP2");
        }
    }
}
