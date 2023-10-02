using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("PLAN_RANGE_TIME")]
    public class PLAN_RANGE_TIME : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MasterId { get; set; }

        [StringLength(50)]
        public string PlanId { get; set; }

        [StringLength(50)]
        public string SiteId { get; set; }

        [StringLength(50)]
        public string FromDate { get; set; }

        [StringLength(50)]
        public string EndDate { get; set; }

        public bool IsUse { get; set; }

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
