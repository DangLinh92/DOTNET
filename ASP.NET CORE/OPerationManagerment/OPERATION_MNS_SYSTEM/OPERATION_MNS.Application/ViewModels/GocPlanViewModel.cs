using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class GocPlanViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string Division { get; set; }

        public float StandardQtyForMonth { get; set; }

        public string MonthPlan { get; set; }

        public string DatePlan { get; set; }

        public float QuantityPlan { get; set; }

        public float QuantityActual { get; set; }

        public float QuantityGap { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // chip , wafe

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
