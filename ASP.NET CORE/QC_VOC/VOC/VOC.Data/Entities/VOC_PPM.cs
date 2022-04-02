using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;
using VOC.Infrastructure.SharedKernel;

namespace VOC.Data.Entities
{
    [Table("VOC_PPM")]
    public class VOC_PPM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string Module { get; set; } // CSP,LFEM

        [StringLength(50)]
        public string Customer { get; set; } // SEV,SEVT

        [StringLength(50)]
        public string Type { get; set; } // Input - Defect

        public int Year { get; set; }
        public int Month { get; set; }

        public double Value { get; set; }

        public double TargetValue { get; set; } // Target PPM

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
