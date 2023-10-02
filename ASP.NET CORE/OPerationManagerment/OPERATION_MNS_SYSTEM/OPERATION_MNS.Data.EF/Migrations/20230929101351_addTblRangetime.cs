using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTblRangetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MasterID",
                table: "GOC_PRODUCTION_PLAN_LFEM_UPDATE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteId",
                table: "GOC_PRODUCTION_PLAN_LFEM_UPDATE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PLAN_RANGE_TIME",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<string>(maxLength: 50, nullable: true),
                    PlanId = table.Column<string>(maxLength: 50, nullable: true),
                    SiteId = table.Column<string>(maxLength: 50, nullable: true),
                    FromDate = table.Column<string>(maxLength: 50, nullable: true),
                    EndDate = table.Column<string>(maxLength: 50, nullable: true),
                    IsUse = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAN_RANGE_TIME", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLAN_RANGE_TIME");

            migrationBuilder.DropColumn(
                name: "MasterID",
                table: "GOC_PRODUCTION_PLAN_LFEM_UPDATE");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "GOC_PRODUCTION_PLAN_LFEM_UPDATE");
        }
    }
}
