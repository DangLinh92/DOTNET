using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addGocProductionPlanLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GOC_PRODUCTION_PLAN_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesItemId = table.Column<string>(maxLength: 50, nullable: true),
                    MonthPlan = table.Column<string>(maxLength: 50, nullable: true),
                    DatePlan = table.Column<string>(maxLength: 50, nullable: true),
                    WeekPlan = table.Column<string>(maxLength: 50, nullable: true),
                    QuantityPlan_KHSX = table.Column<double>(nullable: false),
                    QuantityActual_KHSX = table.Column<double>(nullable: false),
                    QuantityGap_KHSX = table.Column<double>(nullable: false),
                    QuantityPlan_DEMAND = table.Column<double>(nullable: false),
                    QuantityActual_DEMAND = table.Column<double>(nullable: false),
                    QuantityGap_DEMAND = table.Column<double>(nullable: false),
                    QuantityPlan_STOCK = table.Column<double>(nullable: false),
                    QuantityActual_STOCK = table.Column<double>(nullable: false),
                    QuantityGap_STOCK = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOC_PRODUCTION_PLAN_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GOC_PRODUCTION_PLAN_LFEM");
        }
    }
}
