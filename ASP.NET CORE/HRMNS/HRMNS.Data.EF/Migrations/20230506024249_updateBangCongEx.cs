using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMNS.Data.EF.Migrations
{
    public partial class updateBangCongEx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AY_49_AL30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "AZ_50_SL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BA_51_NH",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BB_52_HL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BC_53_UL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BD_54_NB",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BE_55_NL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BF_56_IL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BG_57_KT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BH_58_L70",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BI_59_MD",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BJ_60_PMD",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BK_61_PM",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BL_62_BM",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BM_63_15",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BN_64_20",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BO_65_21",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BP_66_27",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BQ_67_30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BR_68_39",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BS_69_15",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BT_70_20",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BU_71_21",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BV_72_27",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BW_73_30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BX_74_39",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BY_75_ELLC",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BZ_76_OCT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CA_77_OT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CB_78_150",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CC_79_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CD_80_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CE_81_270",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CF_82_300",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CG_83_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CH_84_150",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CI_85_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CJ_86_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CK_87_270",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CL_88_300",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CM_89_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CO_91_VPSX",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CP_92",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.AddColumn<double>(
                name: "AY_50_AL30",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AZ_51_SL",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BA_52_NH",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BB_53_HL",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BC_54_UL",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BD_55_NB",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BE_56_NL",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BF_57_IL",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BG_58_KT",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BH_59_L70",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BI_60_MD",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BJ_61_PMD",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BK_62_PM",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BL_63_BM",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BM_64_15",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BN_65_20",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BO_66_21",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BP_67_27",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BQ_68_30",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BR_69_39",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BS_70_15",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BT_71_20",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BU_72_21",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BV_73_27",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BW_74_30",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BX_75_39",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BY_76_ELLC",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BZ_77_OCT",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CA_78_OT",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CB_79_150",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CC_80_200",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CD_81_200",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CE_82_270",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CF_83_300",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CG_84_390",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CH_85_150",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CI_86_200",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CJ_87_200",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CK_88_270",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CL_89_300",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CM_91_390",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CO_92_VPSX",
                table: "BANG_CONG_EXTENTION",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CP_93",
                table: "BANG_CONG_EXTENTION",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AY_50_AL30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "AZ_51_SL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BA_52_NH",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BB_53_HL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BC_54_UL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BD_55_NB",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BE_56_NL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BF_57_IL",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BG_58_KT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BH_59_L70",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BI_60_MD",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BJ_61_PMD",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BK_62_PM",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BL_63_BM",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BM_64_15",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BN_65_20",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BO_66_21",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BP_67_27",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BQ_68_30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BR_69_39",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BS_70_15",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BT_71_20",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BU_72_21",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BV_73_27",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BW_74_30",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BX_75_39",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BY_76_ELLC",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "BZ_77_OCT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CA_78_OT",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CB_79_150",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CC_80_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CD_81_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CE_82_270",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CF_83_300",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CG_84_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CH_85_150",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CI_86_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CJ_87_200",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CK_88_270",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CL_89_300",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CM_91_390",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CO_92_VPSX",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.DropColumn(
                name: "CP_93",
                table: "BANG_CONG_EXTENTION");

            migrationBuilder.AddColumn<double>(
                name: "AY_49_AL30",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AZ_50_SL",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BA_51_NH",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BB_52_HL",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BC_53_UL",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BD_54_NB",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BE_55_NL",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BF_56_IL",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BG_57_KT",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BH_58_L70",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BI_59_MD",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BJ_60_PMD",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BK_61_PM",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BL_62_BM",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BM_63_15",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BN_64_20",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BO_65_21",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BP_66_27",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BQ_67_30",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BR_68_39",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BS_69_15",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BT_70_20",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BU_71_21",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BV_72_27",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BW_73_30",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BX_74_39",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BY_75_ELLC",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BZ_76_OCT",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CA_77_OT",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CB_78_150",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CC_79_200",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CD_80_200",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CE_81_270",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CF_82_300",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CG_83_390",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CH_84_150",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CI_85_200",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CJ_86_200",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CK_87_270",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CL_88_300",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CM_89_390",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CO_91_VPSX",
                table: "BANG_CONG_EXTENTION",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CP_92",
                table: "BANG_CONG_EXTENTION",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
