using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class AddLeadTimeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LEAD_TIME_WLP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkDate = table.Column<string>(nullable: true),
                    WorkWeek = table.Column<string>(nullable: true),
                    WorkMonth = table.Column<string>(nullable: true),
                    WorkYear = table.Column<string>(nullable: true),
                    HoldTime = table.Column<double>(nullable: false),
                    WaitTime = table.Column<double>(nullable: false),
                    RunTime = table.Column<double>(nullable: false),
                    LeadTime = table.Column<double>(nullable: false),
                    LeadTimeMax = table.Column<double>(nullable: false),
                    WLP = table.Column<string>(nullable: true),
                    Ox = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAD_TIME_WLP", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LEAD_TIME_WLP");
        }
    }
}
