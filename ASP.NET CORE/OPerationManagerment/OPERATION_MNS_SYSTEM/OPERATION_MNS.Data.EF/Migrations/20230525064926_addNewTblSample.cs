using Microsoft.EntityFrameworkCore.Migrations;

namespace OPERATION_MNS.Data.EF.Migrations
{
    public partial class addNewTblSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PHAN_LOAI_HANG_SAMPLE",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHAN_LOAI_HANG_SAMPLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TCARD_SAMPLE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    LotNo = table.Column<string>(maxLength: 50, nullable: true),
                    WaferID = table.Column<string>(maxLength: 50, nullable: true),
                    OutPutDatePlan = table.Column<string>(maxLength: 50, nullable: true),
                    InputDatePlan = table.Column<string>(maxLength: 50, nullable: true),
                    NguoiChiuTrachNhiem = table.Column<string>(maxLength: 50, nullable: true),
                    MucDichInput = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCARD_SAMPLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TINH_HINH_SAN_XUAT_SAMPLE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    MucDoKhanCap = table.Column<string>(maxLength: 50, nullable: true),
                    Model = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    PhanLoai = table.Column<string>(maxLength: 50, nullable: true),
                    ModelDonLinhKien = table.Column<string>(maxLength: 50, nullable: true),
                    LotNo = table.Column<string>(maxLength: 50, nullable: true),
                    QtyInput = table.Column<int>(nullable: false),
                    QtyNG = table.Column<int>(nullable: false),
                    OperationNow = table.Column<string>(maxLength: 50, nullable: true),
                    MucDichNhap = table.Column<string>(maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(maxLength: 250, nullable: true),
                    NguoiChiuTrachNhiem = table.Column<string>(maxLength: 50, nullable: true),
                    InputDate = table.Column<string>(maxLength: 50, nullable: true),
                    OutputDate = table.Column<string>(maxLength: 50, nullable: true),
                    PlanInputDate = table.Column<string>(maxLength: 50, nullable: true),
                    PlanOutputDate = table.Column<string>(maxLength: 50, nullable: true),
                    Wall_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Wall_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Roof_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Roof_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Seed_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Seed_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PlatePR_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PlatePR_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Plate_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Plate_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PreProbe_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PreProbe_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PreDicing_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    PreDicing_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    AllProbe_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    AllProbe_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    BG_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    BG_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Dicing_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Dicing_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    ChipIns_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    ChipIns_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Packing_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Packing_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    OQC_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    OQC_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Shipping_Plan_Date = table.Column<string>(maxLength: 50, nullable: true),
                    Shipping_Actual_Date = table.Column<string>(maxLength: 50, nullable: true),
                    LeadTime = table.Column<int>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TINH_HINH_SAN_XUAT_SAMPLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PHAN_LOAI_MODEL_SAMPLE",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    PhanLoai = table.Column<string>(maxLength: 50, nullable: true),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHAN_LOAI_MODEL_SAMPLE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PHAN_LOAI_MODEL_SAMPLE_PHAN_LOAI_HANG_SAMPLE_PhanLoai",
                        column: x => x.PhanLoai,
                        principalTable: "PHAN_LOAI_HANG_SAMPLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PHAN_LOAI_MODEL_SAMPLE_PhanLoai",
                table: "PHAN_LOAI_MODEL_SAMPLE",
                column: "PhanLoai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PHAN_LOAI_MODEL_SAMPLE");

            migrationBuilder.DropTable(
                name: "TCARD_SAMPLE");

            migrationBuilder.DropTable(
                name: "TINH_HINH_SAN_XUAT_SAMPLE");

            migrationBuilder.DropTable(
                name: "PHAN_LOAI_HANG_SAMPLE");
        }
    }
}
