using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("BANG_CONG_EXTENTION")]
    public class BANG_CONG_EXTENTION : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string ThangNam { get; set; }

        public double AM_38_PH { get; set; }
        public double AN_39_PD { get; set; }
        public double AO_40_PN { get; set; }
        public double AP_41_BH { get; set; }
        public double AQ_42_DS { get; set; }
        public double AR_43_NS { get; set; }
        public double AS_44_AL { get; set; }
        public double AT_45_TotalALPaid { get; set; }
        public double AU_46_TotalUnPaid { get; set; }
        public double AV_47_LamCD_TV { get; set; }
        public double AW_48_LamCD_CT { get; set; }
        public double AX_49_AL { get; set; }
        public double AY_50_AL30 { get; set; }
        public double AZ_51_SL { get; set; }
        public double BA_52_NH { get; set; }
        public double BB_53_HL { get; set; }
        public double BC_54_UL { get; set; }
        public double BD_55_NB { get; set; }
        public double BE_56_NL { get; set; }
        public double BF_57_IL { get; set; }
        public double BG_58_KT { get; set; }
        public double BH_59_L70 { get; set; }
        public double BI_60_MD { get; set; }
        public double BJ_61_PMD { get; set; }
        public double BK_62_PM { get; set; }
        public double BL_63_BM { get; set; }
        public double BM_64_15 { get; set; }
        public double BN_65_20 { get; set; }
        public double BO_66_21 { get; set; }
        public double BP_67_27 { get; set; }
        public double BQ_68_30 { get; set; }
        public double BR_69_39 { get; set; }
        public double BS_70_15 { get; set; }
        public double BT_71_20 { get; set; }
        public double BU_72_21 { get; set; }
        public double BV_73_27 { get; set; }
        public double BW_74_30 { get; set; }
        public double BX_75_39 { get; set; }
        public double BY_76_ELLC { get; set; }
        public double BZ_77_OCT { get; set; }
        public double CA_78_OT { get; set; }
        public double CB_79_150 { get; set; }
        public double CC_80_200 { get; set; }
        public double CD_81_200 { get; set; }
        public double CE_82_270 { get; set; }
        public double CF_83_300 { get; set; }
        public double CG_84_390 { get; set; }
        public double CH_85_150 { get; set; }
        public double CI_86_200 { get; set; }
        public double CJ_87_200 { get; set; }
        public double CK_88_270 { get; set; }
        public double CL_89_300 { get; set; }
        public double CM_90_390 { get; set; }
        public string CO_92_VPSX { get; set; }
        public double CP_93 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
