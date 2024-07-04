using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Data.Entities.Payroll
{
    public class HR_PRINT_SALARY_DATA : DomainEntity<long>, IDateTracking
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string BP { get; set; }
        public string Ngayvao { get; set; }
        public string LCB { get; set; }
        public string PCDS { get; set; }
        public string PCCV { get; set; }
        public string PC_NL { get; set; }
        public string PCTN { get; set; }
        public string PCDH { get; set; }
        public string Luong_D { get; set; }
        public string Luong_H { get; set; }
        public string TV_Ngay { get; set; }
        public string TTien_TV_Ngay { get; set; }
        public string TV_Dem { get; set; }
        public string TTien_TV_Dem { get; set; }
        public string CT_ngay { get; set; }
        public string TTien_CT_Ngay { get; set; }
        public string CT_Dem { get; set; }
        public string TTien_CT_Dem { get; set; }
        public string Nghi_Co_Luong { get; set; }
        public string TTien_Nghi_Co_Luong { get; set; }
        public string Nghi_KL { get; set; }
        public string TT_Lviec { get; set; }
        public string Cong_Ngay { get; set; }
        public string Cong_Dem { get; set; }
        public string Luong_Theo_Ngay_Cong { get; set; }
        public string SoNgayNghi_70 { get; set; }
        public string ThanhTienNghi_70 { get; set; }
        public string SoNgayLuuTruCongTy { get; set; }
        public string ThanhTien_LuuTruCongTy { get; set; }
        public string HoTroDienThoai { get; set; }
        public string Ca_Ngay_TV { get; set; }
        public double Ca_ngay_CT { get; set; }
        public double CaDem_TV_KN_TruocLe { get; set; }
        public double CaDem_CT_KN_TruocLe { get; set; }
        public double ThanhTien { get; set; }
        public double AM_15 { get; set; }
        public double AN_TV15 { get; set; }
        public double AO_20 { get; set; }
        public double AP_TV20 { get; set; }
        public double AQ_21 { get; set; }
        public double AR_TV21 { get; set; }
        public double AS_27 { get; set; }
        public double AT_TV27 { get; set; }
        public double AU_30 { get; set; }
        public double AV_TV30 { get; set; }
        public double AW_39 { get; set; }
        public double AX_TV39 { get; set; }
        public double AY_26_DemTruocLe { get; set; }
        public double AZ_TV26_DemTruocLe { get; set; }
        public double BA_C15 { get; set; }
        public double BB_CT15 { get; set; }
        public double BC_C20 { get; set; }
        public double BD_CT20 { get; set; }
        public double BE_C21 { get; set; }
        public double BF_CT21 { get; set; }
        public double BG_C27 { get; set; }
        public double BH_CT27 { get; set; }
        public double BI_C30 { get; set; }
        public double BJ_CT30 { get; set; }
        public double BK_C39 { get; set; }
        public double BL_CT39 { get; set; }
        public double BM_C26_DemTruocLe { get; set; }
        public double BN_CT26_DemTruocLe { get; set; }
        public double BO_OT_150 { get; set; }
        public double BP_OT_200 { get; set; }
        public double BQ_OT_210 { get; set; }
        public double BR_OT_270 { get; set; }
        public double BS_OT_300 { get; set; }
        public double BT_OT_390 { get; set; }
        public double BU_OT_260 { get; set; }
        public double BV_Cong15 { get; set; }
        public double BW_Cong20 { get; set; }
        public double BX_Cong21 { get; set; }
        public double BY_Cong27 { get; set; }
        public double BZ_Cong30 { get; set; }
        public double CA_Cong39 { get; set; }
        public double CB_Cong26 { get; set; }
        public double CC_TongOT { get; set; }
        public double CD_TongOT { get; set; }




        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
