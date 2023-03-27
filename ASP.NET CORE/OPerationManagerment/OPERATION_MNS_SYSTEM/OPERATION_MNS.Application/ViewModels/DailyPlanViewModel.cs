using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OPERATION_MNS.Application.ViewModels
{
    public class DailyPlanViewModel
    {
        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; }

        public string Model_Short { get; set; }

        public decimal StandardQty { get; set; }

        public decimal Wall_PR { get; set; }

        public decimal Wall_Oven { get; set; }
        public decimal Wall_Inspection { get; set; } //Wall_Ashing

        public decimal Roof_Laminating { get; set; }

        public decimal Roof_Ashing { get; set; }

        public decimal Roof_Measure { get; set; }
        public decimal Roof_Photo { get; set; }

        public decimal Roof_Inspection { get; set; }

        public decimal Seed_Deposition { get; set; }

        public decimal Plate_Patterning_PR { get; set; }

        public decimal Plate_Patterning_Inspection { get; set; }

        public decimal Cu_Sn_Plating { get; set; }

        public decimal PR_Strip_Cu_Etching { get; set; }

        public decimal Plate_BST { get; set; }

        public decimal Plate_Inspection { get; set; }

        public decimal Shipping_Wait { get; set; }

        public decimal GapActualPlan_SH { get; set; }

        public decimal GapActualPlan_EA { get; set; }

        public string LastUpdate { get; set; }

        public int STT { get; set; }

        public decimal YieldPlan { get; set; }
        public decimal YieldActual { get; set; }
        public decimal YieldGap { get; set; }
    }
}
