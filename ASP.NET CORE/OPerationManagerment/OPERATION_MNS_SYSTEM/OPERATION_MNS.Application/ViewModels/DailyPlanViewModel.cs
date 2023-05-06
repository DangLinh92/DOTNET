using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OPERATION_MNS.Application.ViewModels
{
    public class DailyPlanViewModel
    {
        public DailyPlanViewModel()
        {
             Wall_PR = new PlanAndStock();
             Wall_Oven = new PlanAndStock();
             Wall_Inspection = new PlanAndStock();
             Roof_Laminating = new PlanAndStock();
             Roof_Ashing = new PlanAndStock();
             Roof_Measure = new PlanAndStock();
             Roof_Photo = new PlanAndStock();
             Roof_Inspection = new PlanAndStock();
             Seed_Deposition = new PlanAndStock();
             Plate_Patterning_PR = new PlanAndStock();
             Plate_Patterning_Inspection = new PlanAndStock();
             Cu_Sn_Plating = new PlanAndStock();
             PR_Strip_Cu_Etching = new PlanAndStock();
             Plate_BST = new PlanAndStock();
             Plate_Inspection = new PlanAndStock();
             Shipping_Wait = new PlanAndStock();
        }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; } // key

        public string Model_Short { get; set; }

        public decimal StandardQty { get; set; }

        public PlanAndStock Wall_PR { get; set; }
        public PlanAndStock Wall_Oven { get; set; }
        public PlanAndStock Wall_Inspection { get; set; } //Wall_Ashing
        public PlanAndStock Roof_Laminating { get; set; }
        public PlanAndStock Roof_Ashing { get; set; }
        public PlanAndStock Roof_Measure { get; set; }
        public PlanAndStock Roof_Photo { get; set; }
        public PlanAndStock Roof_Inspection { get; set; }
        public PlanAndStock Seed_Deposition { get; set; }
        public PlanAndStock Plate_Patterning_PR { get; set; }
        public PlanAndStock Plate_Patterning_Inspection { get; set; }
        public PlanAndStock Cu_Sn_Plating { get; set; }
        public PlanAndStock PR_Strip_Cu_Etching { get; set; }
        public PlanAndStock Plate_BST { get; set; }
        public PlanAndStock Plate_Inspection { get; set; }
        public PlanAndStock Shipping_Wait { get; set; }

        public decimal GapActualPlan_SH { get; set; }

        public decimal GapActualPlan_EA { get; set; }

        public string LastUpdate { get; set; }

        public int STT { get; set; }

        public decimal YieldPlan { get; set; }
        public decimal YieldActual { get; set; }
        public decimal YieldGap { get; set; }

        public string ChipWafer { get; set; }
    }

    public class PlanAndStock
    {
        public decimal Plan { get; set; }
        public decimal StockChip { get; set; }
        public decimal StockWafer { get; set; }
    }

}
