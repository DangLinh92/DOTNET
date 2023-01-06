using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class YieldOfModelViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public decimal YieldPlan { get; set; }
        public decimal YieldActual { get; set; }
        public decimal YieldGap { get; set; }
        public string Month { get; set; }

        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
