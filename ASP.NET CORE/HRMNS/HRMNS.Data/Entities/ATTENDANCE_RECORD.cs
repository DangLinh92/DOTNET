using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("ATTENDANCE_RECORD")]
    public class ATTENDANCE_RECORD : DomainEntity<long>, IDateTracking
    {
        public ATTENDANCE_RECORD()
        {
            ATTENDANCE_OVERTIME = new HashSet<ATTENDANCE_OVERTIME>();
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string Time_Check { get; set; }

        [StringLength(20)]
        public string Working_Status { get; set; }

        public double? EL_LC { get; set; }

        [StringLength(20)]
        public string IsLock { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<ATTENDANCE_OVERTIME> ATTENDANCE_OVERTIME { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }

        [ForeignKey("Working_Status")]
        public virtual KY_HIEU_CHAM_CONG KY_HIEU_CHAM_CONG { get; set; }

    }
}
