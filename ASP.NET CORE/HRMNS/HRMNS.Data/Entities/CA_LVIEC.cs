using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("CA_LVIEC")]
    public class CA_LVIEC : DomainEntity<int>, IDateTracking
    {
        public CA_LVIEC()
        {
            ATTENDANCE_OVERTIME = new HashSet<ATTENDANCE_OVERTIME>();
        }

        [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string DM_NgayLViec { get; set; }

        [StringLength(100)]
        public string TenCa { get; set; }

        [StringLength(50)]
        public string Time_BatDau { get; set; }

        [StringLength(50)]
        public string Time_BatDau2 { get; set; }

        [StringLength(50)]
        public string Time_KetThuc { get; set; }

        [StringLength(50)]
        public string Time_KetThuc2 { get; set; }

        public float HeSo_OT { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("Danhmuc_CaLviec")]
        public virtual DM_CA_LVIEC DM_CA_LVIEC { get; set; }

        [ForeignKey("DM_NgayLViec")]
        public virtual DM_NGAY_LAMVIEC DM_NGAY_LAMVIEC { get; set; }

        public virtual ICollection<ATTENDANCE_OVERTIME> ATTENDANCE_OVERTIME { get; set; }
    }
}
