using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addGocLfem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DATE_OFF_LINE_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemValue = table.Column<string>(maxLength: 50, nullable: true),
                    NoiDung = table.Column<string>(maxLength: 250, nullable: true),
                    DanhMuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DATE_OFF_LINE_LFEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GOC_PLAN_LFEM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesItemId = table.Column<string>(maxLength: 50, nullable: true),
                    MasterialName = table.Column<string>(maxLength: 50, nullable: true),
                    Erp_Item_Id = table.Column<string>(maxLength: 50, nullable: true),
                    Group1 = table.Column<string>(maxLength: 50, nullable: true),
                    Group2 = table.Column<string>(maxLength: 50, nullable: true),
                    Group3 = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    OperationId = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    Sales_approval = table.Column<string>(maxLength: 50, nullable: true),
                    Module_model = table.Column<string>(maxLength: 50, nullable: true),
                    MonthPlan = table.Column<string>(maxLength: 50, nullable: true),
                    DatePlan = table.Column<string>(maxLength: 50, nullable: true),
                    QuantityPlan = table.Column<double>(nullable: false),
                    QuantityActual = table.Column<double>(nullable: false),
                    QuantityGap = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DanhMuc = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOC_PLAN_LFEM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DATE_OFF_LINE_LFEM");

            migrationBuilder.DropTable(
                name: "GOC_PLAN_LFEM");
        }
    }
}
