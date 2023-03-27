using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addViewWipPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VIEW_WIP_POST_WLP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material_SAP_CODE = table.Column<string>(maxLength: 50, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    Series = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    Material = table.Column<string>(maxLength: 50, nullable: true),
                    Total = table.Column<int>(nullable: false),
                    CassetteInputStock = table.Column<int>(nullable: false),
                    Wait = table.Column<int>(nullable: false),
                    OQC_WaferInspection = table.Column<int>(nullable: false),
                    WaferCarrierPacking = table.Column<int>(nullable: false),
                    InputCheck = table.Column<int>(nullable: false),
                    WaferShipping = table.Column<int>(nullable: false),
                    Marking = table.Column<int>(nullable: false),
                    NgoaiQuanSauMarking = table.Column<int>(nullable: false),
                    TapeLamination = table.Column<int>(nullable: false),
                    NgoaiQuanSauLamination = table.Column<int>(nullable: false),
                    BackGrinding = table.Column<int>(nullable: false),
                    NgoaiQuanSauBackGrinding = table.Column<int>(nullable: false),
                    TapeDelamination = table.Column<int>(nullable: false),
                    NgoaiQuanSauMDS = table.Column<int>(nullable: false),
                    WaferOven = table.Column<int>(nullable: false),
                    WaferDicing = table.Column<int>(nullable: false),
                    UVInspection = table.Column<int>(nullable: false),
                    DeTaping = table.Column<int>(nullable: false),
                    ChipVisualInspection = table.Column<int>(nullable: false),
                    ReelPacking = table.Column<int>(nullable: false),
                    ReelOperationInput = table.Column<int>(nullable: false),
                    ReelVisualInspection = table.Column<int>(nullable: false),
                    ReelCounter = table.Column<int>(nullable: false),
                    ReelOven = table.Column<int>(nullable: false),
                    OneStPackingLabel = table.Column<int>(nullable: false),
                    OQC = table.Column<int>(nullable: false),
                    OneSTPacking = table.Column<int>(nullable: false),
                    Shipping = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIEW_WIP_POST_WLP", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VIEW_WIP_POST_WLP");
        }
    }
}
