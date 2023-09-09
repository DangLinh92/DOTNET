using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("ACTUAL_PLAN_SAMPLE")]
    public class ACTUAL_PLAN_SAMPLE : DomainEntity<int>, IDateTracking
    {
        public double LotPlan { get; set; }
        public double LotActual { get; set; }
        public double CasseteActual { get; set; }
        public double CassetePlan { get; set; }

        [StringLength(50)]
        public string NgayThang { get; set; } // ngày tháng năm yyyy-MM-dd
        public int Day { get; set; }

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
