using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class GocStandarViewModel
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

        [StringLength(50)]
        public string MonthBegin { get; set; }

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
