using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DM_NGAY_LAMVIEC")]
    public class DM_NGAY_LAMVIEC : DomainEntity<int>, IDateTracking
    {
        public DM_NGAY_LAMVIEC()
        {
            CA_LVIEC = new HashSet<CA_LVIEC>();
            DANGKY_OT_NHANVIEN = new HashSet<DANGKY_OT_NHANVIEN>();
            HE_SO_OVERTIME = new HashSet<HE_SO_OVERTIME>();
        }

        [StringLength(100)]
        public string Ten_NgayLV { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<CA_LVIEC> CA_LVIEC { get; set; }

        public virtual ICollection<DANGKY_OT_NHANVIEN> DANGKY_OT_NHANVIEN { get; set; }

        public virtual ICollection<HE_SO_OVERTIME> HE_SO_OVERTIME { get; set; }
    }
}
