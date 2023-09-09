using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addLeadTimeLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LEAD_TIME_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkDate = table.Column<string>(maxLength: 50, nullable: true),
                    WorkWeek = table.Column<double>(nullable: false),
                    WorkMonth = table.Column<string>(maxLength: 50, nullable: true),
                    WorkYear = table.Column<string>(maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    Operation_short_name = table.Column<string>(maxLength: 50, nullable: true),
                    WaitTime = table.Column<double>(nullable: false),
                    RunTime = table.Column<double>(nullable: false),
                    LeadTime = table.Column<double>(nullable: false),
                    MaterialID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAD_TIME_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LEAD_TIME_LFEM");
        }
    }
}
