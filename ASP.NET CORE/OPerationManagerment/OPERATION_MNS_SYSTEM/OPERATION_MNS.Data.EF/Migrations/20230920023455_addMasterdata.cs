using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addMasterdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MATERIALS_PLAN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    ItemID = table.Column<string>(maxLength: 50, nullable: true),
                    ItemName = table.Column<string>(maxLength: 50, nullable: true),
                    ItemGR = table.Column<string>(maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 50, nullable: true),
                    ModuleModel = table.Column<string>(maxLength: 500, nullable: true),
                    OnHand = table.Column<double>(nullable: true),
                    PreBuld = table.Column<double>(nullable: true),
                    AsumeBuld = table.Column<double>(nullable: false),
                    CategoryCD = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    Week = table.Column<string>(maxLength: 50, nullable: true),
                    Day = table.Column<string>(maxLength: 50, nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIALS_PLAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OPERATION_FLOW",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    MaterialId = table.Column<string>(maxLength: 50, nullable: true),
                    Route = table.Column<string>(maxLength: 50, nullable: true),
                    OperationSEQ = table.Column<int>(nullable: false),
                    Operation = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    ActualYield = table.Column<double>(nullable: true),
                    PreBufferTime = table.Column<double>(nullable: true),
                    RuntTime = table.Column<double>(nullable: true),
                    PostBufferTime = table.Column<double>(nullable: true),
                    TimeUOM = table.Column<string>(maxLength: 50, nullable: true),
                    LeadTimeDay = table.Column<double>(nullable: false),
                    IsStock = table.Column<bool>(nullable: true),
                    Desc = table.Column<string>(maxLength: 250, nullable: true),
                    Valid = table.Column<bool>(nullable: false),
                    ATTB1 = table.Column<string>(maxLength: 50, nullable: true),
                    ATTB2 = table.Column<string>(maxLength: 50, nullable: true),
                    ATTB3 = table.Column<string>(maxLength: 50, nullable: true),
                    ATTB4 = table.Column<string>(maxLength: 50, nullable: true),
                    ATTB5 = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPERATION_FLOW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OPERATION_STANDARD_INFO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationID = table.Column<string>(nullable: true),
                    OperationName = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPERATION_STANDARD_INFO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_MIX_CAPA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    TyleValue = table.Column<string>(maxLength: 50, nullable: true),
                    PMixID = table.Column<string>(maxLength: 150, nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(maxLength: 50, nullable: true),
                    Bucket = table.Column<string>(maxLength: 50, nullable: true),
                    Start = table.Column<string>(maxLength: 50, nullable: true),
                    End = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_MIX_CAPA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_PLAN",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerGR = table.Column<string>(maxLength: 50, nullable: true),
                    Customer = table.Column<string>(maxLength: 50, nullable: true),
                    MesItemID = table.Column<string>(maxLength: 50, nullable: true),
                    ERPItemID = table.Column<string>(maxLength: 50, nullable: true),
                    Group1 = table.Column<string>(maxLength: 50, nullable: true),
                    Group2 = table.Column<string>(maxLength: 50, nullable: true),
                    Group3 = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    OperationID = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    SalesApproval = table.Column<string>(maxLength: 50, nullable: true),
                    ModuleModel = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    Month = table.Column<string>(maxLength: 50, nullable: true),
                    Week = table.Column<string>(maxLength: 50, nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PLAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_APPROVE_MANUFATURE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<string>(maxLength: 50, nullable: true),
                    SalesApprovalCode = table.Column<string>(maxLength: 50, nullable: true),
                    ERPCode = table.Column<string>(maxLength: 50, nullable: true),
                    MesCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsManufacture = table.Column<bool>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 50, nullable: true),
                    SMTPoint = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_APPROVE_MANUFATURE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCP_PLAN_BOM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    Version = table.Column<string>(maxLength: 150, nullable: true),
                    SalesApprovalCode = table.Column<string>(maxLength: 50, nullable: true),
                    MesMaterialID = table.Column<string>(maxLength: 50, nullable: true),
                    ProduceItem = table.Column<string>(maxLength: 50, nullable: true),
                    ItemName = table.Column<string>(maxLength: 50, nullable: true),
                    ConsumeItem = table.Column<string>(maxLength: 50, nullable: true),
                    ConsumeName = table.Column<string>(maxLength: 150, nullable: true),
                    ConsumeRate = table.Column<double>(nullable: false),
                    Route = table.Column<string>(maxLength: 50, nullable: true),
                    Operation = table.Column<string>(maxLength: 50, nullable: true),
                    OperationName = table.Column<string>(maxLength: 50, nullable: true),
                    Valid = table.Column<string>(maxLength: 5, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCP_PLAN_BOM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SITE_CALENDAR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<string>(maxLength: 50, nullable: true),
                    MasterID = table.Column<string>(maxLength: 50, nullable: true),
                    SizeID = table.Column<string>(maxLength: 50, nullable: true),
                    WorkDate = table.Column<string>(maxLength: 50, nullable: true),
                    OnTime = table.Column<double>(nullable: true),
                    TimeOUM = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITE_CALENDAR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MATERIALS_PLAN");

            migrationBuilder.DropTable(
                name: "OPERATION_FLOW");

            migrationBuilder.DropTable(
                name: "OPERATION_STANDARD_INFO");

            migrationBuilder.DropTable(
                name: "PRODUCT_MIX_CAPA");

            migrationBuilder.DropTable(
                name: "PRODUCTION_PLAN");

            migrationBuilder.DropTable(
                name: "SALES_APPROVE_MANUFATURE");

            migrationBuilder.DropTable(
                name: "SCP_PLAN_BOM");

            migrationBuilder.DropTable(
                name: "SITE_CALENDAR");
        }
    }
}
