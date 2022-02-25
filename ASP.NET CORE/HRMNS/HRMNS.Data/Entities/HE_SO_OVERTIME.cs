using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HE_SO_OVERTIME")]
    public class HE_SO_OVERTIME : DomainEntity<int>, IDateTracking
    {
        public double? Heso_OT { get; set; }

        public double? Tru_di_an { get; set; }

        public int? NgayLamViec { get; set; }

        [StringLength(50)]
        public string Time_Start { get; set; }

        [StringLength(50)]
        public string Time_End { get; set; }

        [StringLength(50)]
        public string ApDung_From { get; set; }

        [StringLength(50)]
        public string ApDung_To { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("NgayLamViec")]
        public virtual DM_NGAY_LAMVIEC DM_NGAY_LAMVIEC { get; set; }
    }
}
