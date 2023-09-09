using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DAILY_TIME_WORKING")]
    public class DAILY_TIME_WORKING : DomainEntity<int>, IDateTracking
    {
        public DAILY_TIME_WORKING() { }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayLViec { get; set; }
        public double Time_OT { get; set; }
        public double Time_Working { get; set; }

        [StringLength(50)]
        public string FromTime { get; set; }

        [StringLength(50)]
        public string ToTime { get; set; }

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
