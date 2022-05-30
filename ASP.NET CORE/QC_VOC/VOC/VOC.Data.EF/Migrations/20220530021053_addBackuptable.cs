using Microsoft.EntityFrameworkCore.Migrations;

namespace VOC.Data.EF.Migrations
{
    public partial class addBackuptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VOC_MST_BACKUP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackUpDate = table.Column<string>(maxLength: 50, nullable: true),
                    Received_site = table.Column<string>(maxLength: 50, nullable: true),
                    PlaceOfOrigin = table.Column<string>(maxLength: 50, nullable: true),
                    ReceivedDept = table.Column<string>(maxLength: 50, nullable: true),
                    ReceivedDate = table.Column<string>(maxLength: 50, nullable: true),
                    SPLReceivedDate = table.Column<string>(maxLength: 50, nullable: true),
                    SPLReceivedDateWeek = table.Column<string>(maxLength: 50, nullable: true),
                    Customer = table.Column<string>(maxLength: 150, nullable: true),
                    SETModelCustomer = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessCustomer = table.Column<string>(maxLength: 250, nullable: true),
                    ModelFullname = table.Column<string>(maxLength: 50, nullable: true),
                    DefectNameCus = table.Column<string>(maxLength: 500, nullable: true),
                    DefectRate = table.Column<string>(maxLength: 50, nullable: true),
                    PBA_FAE_Result = table.Column<string>(maxLength: 50, nullable: true),
                    PartsClassification = table.Column<string>(maxLength: 50, nullable: true),
                    PartsClassification2 = table.Column<string>(maxLength: 50, nullable: true),
                    ProdutionDateMarking = table.Column<string>(maxLength: 500, nullable: true),
                    AnalysisResult = table.Column<string>(maxLength: 500, nullable: true),
                    VOCCount = table.Column<string>(maxLength: 50, nullable: true),
                    DefectCause = table.Column<string>(maxLength: 500, nullable: true),
                    DefectClassification = table.Column<string>(maxLength: 100, nullable: true),
                    CustomerResponse = table.Column<string>(maxLength: 500, nullable: true),
                    Report_FinalApprover = table.Column<string>(maxLength: 50, nullable: true),
                    Report_Sender = table.Column<string>(maxLength: 50, nullable: true),
                    Rport_sentDate = table.Column<string>(maxLength: 50, nullable: true),
                    VOCState = table.Column<string>(nullable: true),
                    VOCFinishingDate = table.Column<string>(maxLength: 50, nullable: true),
                    VOC_TAT = table.Column<string>(maxLength: 50, nullable: true),
                    LinkReport = table.Column<string>(maxLength: 1000, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOC_MST_BACKUP", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VOC_MST_BACKUP");
        }
    }
}
