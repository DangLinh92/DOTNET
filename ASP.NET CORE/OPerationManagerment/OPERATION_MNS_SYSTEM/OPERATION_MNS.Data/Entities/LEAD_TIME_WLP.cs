using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("LEAD_TIME_WLP")]
    public class LEAD_TIME_WLP : DomainEntity<int>
    {
        public LEAD_TIME_WLP()
        {

        }
        [StringLength(50)]
        public string WorkDate { get; set; }
        [StringLength(50)]
        public string WorkWeek { get; set; }
        [StringLength(50)]
        public string WorkMonth { get; set; }
        [StringLength(50)]
        public string WorkYear { get; set; }

        public double HoldTime { get; set; }
        public double WaitTime { get; set; }
        public double RunTime { get; set; }
        public double LeadTime { get; set; }
        public double LeadTimeMax { get; set; }

        [StringLength(50)]
        public string WLP { get; set; }

        [StringLength(50)]
        public string Ox { get; set; }
    }
}
