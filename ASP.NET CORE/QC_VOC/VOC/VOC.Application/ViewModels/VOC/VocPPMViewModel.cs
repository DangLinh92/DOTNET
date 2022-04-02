using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocPPMViewModel
    {
        public int Id { get; set; }

        public string Module { get; set; } // CSP,LFEM

        public string Customer { get; set; } // SEV,SEVT

        public string Type { get; set; } // Input - Defect

        public int Year { get; set; }

        public int Month { get; set; }

        public double Value { get; set; }

        public double TargetValue { get; set; }

        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
