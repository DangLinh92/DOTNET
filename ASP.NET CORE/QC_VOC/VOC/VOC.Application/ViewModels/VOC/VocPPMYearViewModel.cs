using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocPPMYearViewModel
    {
        public int Id { get; set; }

        public int Year { get; set; }

        [StringLength(50)]
        public string Module { get; set; } // LFEM/CSP

        public double ValuePPM { get; set; }

        public double TargetPPM { get; set; }

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
