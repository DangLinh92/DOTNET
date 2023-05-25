using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("PHUCAP_DOC_HAI")]
    public class PHUCAP_DOC_HAI : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string BoPhan { get;set; }

        public double PhuCap { get; set; }

        [StringLength(150)]
        public string Note { get; set; }

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
