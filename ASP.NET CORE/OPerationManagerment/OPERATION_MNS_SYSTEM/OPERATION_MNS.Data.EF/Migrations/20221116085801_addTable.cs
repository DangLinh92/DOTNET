using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTUAL_DAILY_VIEW",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model_GOC = table.Column<string>(maxLength: 50, nullable: true),
                    Material_MES = table.Column<string>(maxLength: 50, nullable: true),
                    Department = table.Column<string>(maxLength: 50, nullable: true),
                    DateActual = table.Column<string>(maxLength: 50, nullable: true),
                    Qty_ShippingWait = table.Column<float>(nullable: false),
                    Qty_PostOperationShipping = table.Column<float>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACTUAL_DAILY_VIEW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GOC_PLAN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    StandardQtyForMonth = table.Column<float>(nullable: false),
                    MonthPlan = table.Column<string>(nullable: true),
                    DatePlan = table.Column<string>(nullable: true),
                    QuantityPlan = table.Column<float>(nullable: false),
                    QuantityActual = table.Column<float>(nullable: false),
                    QuantityGap = table.Column<float>(nullable: false),
                    Department = table.Column<string>(maxLength: 50, nullable: true),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOC_PLAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "INVENTORY_ACTUAL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    Series = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Total = table.Column<float>(nullable: false),
                    CassetteInputStock_Pre = table.Column<float>(nullable: false),
                    PreOperationWaiting = table.Column<float>(nullable: false),
                    WaitMarkingIDCHK = table.Column<float>(nullable: false),
                    Input_wafer_inspection = table.Column<float>(nullable: false),
                    Wall_PR_Wafer_inspection = table.Column<float>(nullable: false),
                    Wall_PR = table.Column<float>(nullable: false),
                    EBR = table.Column<float>(nullable: false),
                    Wall_Mask_Cleaning = table.Column<float>(nullable: false),
                    Wall_Photo = table.Column<float>(nullable: false),
                    Wall_Develop = table.Column<float>(nullable: false),
                    Wall_Oven = table.Column<float>(nullable: false),
                    Wall_PR_Measure = table.Column<float>(nullable: false),
                    Wall_Ashing_Waiting = table.Column<float>(nullable: false),
                    Wall_Inspection = table.Column<float>(nullable: false),
                    Wall_Ashing = table.Column<float>(nullable: false),
                    Before_Roof_Lami_Wafer_Inspection = table.Column<float>(nullable: false),
                    Roof_Laminating = table.Column<float>(nullable: false),
                    After_Roof_Lami_Visual_Inspection = table.Column<float>(nullable: false),
                    Roof_Hardening = table.Column<float>(nullable: false),
                    Roof_Mask_Cleaning = table.Column<float>(nullable: false),
                    Roof_Photo = table.Column<float>(nullable: false),
                    Roof_Remover = table.Column<float>(nullable: false),
                    Roof_Oven_PEB = table.Column<float>(nullable: false),
                    Roof_Develop = table.Column<float>(nullable: false),
                    After_Roof_Develop_Visual_Inspection = table.Column<float>(nullable: false),
                    Roof_Ashing = table.Column<float>(nullable: false),
                    Roof_QDR = table.Column<float>(nullable: false),
                    Roof_Oven = table.Column<float>(nullable: false),
                    Wafer_Sorting = table.Column<float>(nullable: false),
                    Roof_Measure = table.Column<float>(nullable: false),
                    Roof_BST = table.Column<float>(nullable: false),
                    Roof_Inspection = table.Column<float>(nullable: false),
                    Seed_Deposition = table.Column<float>(nullable: false),
                    Before_Plate_PR_Wafer_Inspection = table.Column<float>(nullable: false),
                    Plate_Patterning_PR = table.Column<float>(nullable: false),
                    Plate_Patterning_Mask_Cleaning = table.Column<float>(nullable: false),
                    Plate_Patterning_Photo = table.Column<float>(nullable: false),
                    Plate_Patterning_Develop = table.Column<float>(nullable: false),
                    After_Plate_Develop_Visual_Inspection = table.Column<float>(nullable: false),
                    Plate_Patterning_Measure = table.Column<float>(nullable: false),
                    Plate_Patterning_PR_Ashing = table.Column<float>(nullable: false),
                    Plate_Patterning_Inspection = table.Column<float>(nullable: false),
                    Plating_Input_Waiting = table.Column<float>(nullable: false),
                    Cu_Sn_Plating = table.Column<float>(nullable: false),
                    St_Plate_Visual_Inspection = table.Column<float>(nullable: false),
                    SN_Plate_Measure = table.Column<float>(nullable: false),
                    PR_Strip_Cu_Etching = table.Column<float>(nullable: false),
                    Nd_Plate_Visual_Inspection = table.Column<float>(nullable: false),
                    Ti_ething = table.Column<float>(nullable: false),
                    Plate_Measure = table.Column<float>(nullable: false),
                    Plate_BST = table.Column<float>(nullable: false),
                    Plate_Inspection_Wait = table.Column<float>(nullable: false),
                    Plate_Inspection = table.Column<float>(nullable: false),
                    Probe_Waite = table.Column<float>(nullable: false),
                    Wafer_Probe_RF = table.Column<float>(nullable: false),
                    Wafer_Probe_IR = table.Column<float>(nullable: false),
                    Shipping_Wait = table.Column<float>(nullable: false),
                    Post_Operation_Shipping = table.Column<float>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVENTORY_ACTUAL", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTUAL_DAILY_VIEW");

            migrationBuilder.DropTable(
                name: "GOC_PLAN");

            migrationBuilder.DropTable(
                name: "INVENTORY_ACTUAL");
        }
    }
}
