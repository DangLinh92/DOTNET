using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class addTblBangLuongChiTiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DanhMuc",
                table: "HR_SALARY_DANHMUC_PHATSINH",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BANG_CONG_EXTENTION",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNV = table.Column<string>(maxLength: 50, nullable: true),
                    ThangNam = table.Column<string>(maxLength: 50, nullable: true),
                    AM_38_PH = table.Column<double>(nullable: false),
                    AN_39_PD = table.Column<double>(nullable: false),
                    AO_40_PN = table.Column<double>(nullable: false),
                    AP_41_BH = table.Column<double>(nullable: false),
                    AQ_42_DS = table.Column<double>(nullable: false),
                    AR_43_NS = table.Column<double>(nullable: false),
                    AS_44_AL = table.Column<double>(nullable: false),
                    AT_45_TotalALPaid = table.Column<double>(nullable: false),
                    AU_46_TotalUnPaid = table.Column<double>(nullable: false),
                    AV_47_LamCD_TV = table.Column<double>(nullable: false),
                    AW_48_LamCD_CT = table.Column<double>(nullable: false),
                    AX_49_AL = table.Column<double>(nullable: false),
                    AY_49_AL30 = table.Column<double>(nullable: false),
                    AZ_50_SL = table.Column<double>(nullable: false),
                    BA_51_NH = table.Column<double>(nullable: false),
                    BB_52_HL = table.Column<double>(nullable: false),
                    BC_53_UL = table.Column<double>(nullable: false),
                    BD_54_NB = table.Column<double>(nullable: false),
                    BE_55_NL = table.Column<double>(nullable: false),
                    BF_56_IL = table.Column<double>(nullable: false),
                    BG_57_KT = table.Column<double>(nullable: false),
                    BH_58_L70 = table.Column<double>(nullable: false),
                    BI_59_MD = table.Column<double>(nullable: false),
                    BJ_60_PMD = table.Column<double>(nullable: false),
                    BK_61_PM = table.Column<double>(nullable: false),
                    BL_62_BM = table.Column<double>(nullable: false),
                    BM_63_15 = table.Column<double>(nullable: false),
                    BN_64_20 = table.Column<double>(nullable: false),
                    BO_65_21 = table.Column<double>(nullable: false),
                    BP_66_27 = table.Column<double>(nullable: false),
                    BQ_67_30 = table.Column<double>(nullable: false),
                    BR_68_39 = table.Column<double>(nullable: false),
                    BS_69_15 = table.Column<double>(nullable: false),
                    BT_70_20 = table.Column<double>(nullable: false),
                    BU_71_21 = table.Column<double>(nullable: false),
                    BV_72_27 = table.Column<double>(nullable: false),
                    BW_73_30 = table.Column<double>(nullable: false),
                    BX_74_39 = table.Column<double>(nullable: false),
                    BY_75_ELLC = table.Column<double>(nullable: false),
                    BZ_76_OCT = table.Column<double>(nullable: false),
                    CA_77_OT = table.Column<double>(nullable: false),
                    CB_78_150 = table.Column<double>(nullable: false),
                    CC_79_200 = table.Column<double>(nullable: false),
                    CD_80_200 = table.Column<double>(nullable: false),
                    CE_81_270 = table.Column<double>(nullable: false),
                    CF_82_300 = table.Column<double>(nullable: false),
                    CG_83_390 = table.Column<double>(nullable: false),
                    CH_84_150 = table.Column<double>(nullable: false),
                    CI_85_200 = table.Column<double>(nullable: false),
                    CJ_86_200 = table.Column<double>(nullable: false),
                    CK_87_270 = table.Column<double>(nullable: false),
                    CL_88_300 = table.Column<double>(nullable: false),
                    CM_89_390 = table.Column<double>(nullable: false),
                    CO_91_VPSX = table.Column<string>(nullable: true),
                    CP_92 = table.Column<double>(nullable: false),
                    DateCreated = table.Column<string>(maxLength: 50, nullable: true),
                    DateModified = table.Column<string>(maxLength: 50, nullable: true),
                    UserCreated = table.Column<string>(maxLength: 50, nullable: true),
                    UserModified = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANG_CONG_EXTENTION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BANG_CONG_EXTENTION_HR_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "HR_NHANVIEN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BANG_CONG_EXTENTION_MaNV",
                table: "BANG_CONG_EXTENTION",
                column: "MaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANG_CONG_EXTENTION");

            migrationBuilder.AlterColumn<string>(
                name: "DanhMuc",
                table: "HR_SALARY_DANHMUC_PHATSINH",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
