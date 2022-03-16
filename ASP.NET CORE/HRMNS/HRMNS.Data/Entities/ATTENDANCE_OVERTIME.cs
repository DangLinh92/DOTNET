using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("ATTENDANCE_OVERTIME")]
    public class ATTENDANCE_OVERTIME : DomainEntity<int>, IDateTracking
    {
        public int CaLviec { get; set; }

        public long MaAttendence { get; set; }

        public double? Value { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaAttendence")]
        public virtual ATTENDANCE_RECORD ATTENDANCE_RECORD { get; set; }

        [ForeignKey("CaLviec")]
        public virtual CA_LVIEC CA_LVIEC { get; set; }
    }
}
