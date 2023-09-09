using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("LEAD_TIME_LFEM")]
    public class LEAD_TIME_LFEM : DomainEntity<int>
    {
        public LEAD_TIME_LFEM()
        {

        }
        [StringLength(50)]
        public string WorkDate { get; set; }

        public double WorkWeek { get; set; }

        [StringLength(50)]
        public string WorkMonth { get; set; }

        [StringLength(50)]
        public string WorkYear { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string Operation_short_name { get; set; }

        public double WaitTime { get; set; }
        public double RunTime { get; set; }
        public double LeadTime { get; set; }
        public double HoldTime { get; set; }
        public double LeadTimeStartEnd { get; set; }
        public double LeadTimeInOut { get; set; }

        [StringLength(50)]
        public string MaterialID { get; set; }

        [StringLength(50)]
        public string LotID { get; set; }

        public double DisplayOrder { get; set; }

    }
}
