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

        public double PH { get; set; }
        public double PD { get; set; }
        public double PN { get; set; }
        public double BH { get; set; }
        public double DS { get; set; }
        public double NS { get; set; }
        public double NCL { get; set; }
        public double TP { get; set; }
        public double TUP { get; set; }
        public double CD_TV { get; set; }
        public double CD_CT { get; set; }
        public double AL { get; set; }
        public double AL30 { get; set; }
        public double L160 { get; set; }
        public double SL { get; set; }
        public double NH { get; set; }
        public double HL { get; set; }
        public double UL { get; set; }
        public double NB { get; set; }
        public double NL { get; set; }
        public double IL { get; set; }
        public double KT { get; set; }
        public double L70 { get; set; }
        public double MD { get; set; }
        public double PMD { get; set; }
        public double PM { get; set; }
        public double BM { get; set; }
        public double OT_TV_15 { get; set; }
        public double OT_TV_20 { get; set; }
        public double OT_TV_21 { get; set; }
        public double OT_TV_27 { get; set; }
        public double OT_TV_30 { get; set; }
        public double OT_TV_39 { get; set; }
        public double OT_CT_15 { get; set; }
        public double OT_CT_20 { get; set; }
        public double OT_CT_21 { get; set; }
        public double OT_CT_27 { get; set; }
        public double OT_CT_30 { get; set; }
        public double OT_CT_39 { get; set; }
        public double P_TV { get; set; }
        public double O_CT { get; set; }
        public double OT { get; set; }
        public double TV_150 { get; set; }
        public double TV_D_NT_200 { get; set; }
        public double TV_CN_200 { get; set; }
        public double TV_D_CN_270 { get; set; }
        public double TV_NL_300 { get; set; }
        public double TV_D_NL_390 { get; set; }
        public double CT_150 { get; set; }
        public double CT_D_NT_200 { get; set; }
        public double CT_CN_200 { get; set; }
        public double CT_D_CN_270 { get; set; }
        public double CT_NL_300 { get; set; }
        public double CT_D_NL_390 { get; set; }
        public string VP_SX { get; set; }
        public double SUM_OTHER { get; set; }

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
