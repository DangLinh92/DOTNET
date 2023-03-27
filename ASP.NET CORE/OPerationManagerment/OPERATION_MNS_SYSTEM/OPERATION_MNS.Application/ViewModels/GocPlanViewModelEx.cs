using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class GocPlanViewModelEx
    {
        public GocPlanViewModelEx()
        {
            QuantityByDays = new List<QuantityByDay>();
        }


        // standar qty Id
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

        public string Type { get; set; }

        public string DanhMuc { get; set; }

        public float StandardQtyForMonth { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        public float Total_Plan { get; set; }
        public float Total_Actual { get; set; }
        public float Total_Gap { get; set; }

        public string Unit { get; set; } // chip , wafe

        [StringLength(50)]
        public string Material_In_MES { get; set; }

        public List<QuantityByDay> QuantityByDays { get; set; }
    }

    public class QuantityByDay
    {
        // Goc plan Id
        public int Id { get; set; }
        public string DatePlan { get; set; }
        public float QuantityPlan { get; set; }
        public float QuantityActual { get; set; }
        public float QuantityGap { get; set; }
        public float QtyPlan_Ytd { get; set; }
        public float QtyActual_Ytd { get; set; }
        public float QtyGap_Ytd { get; set; }

        public double Qty_Percen_Ytd { get; set; }
    }

    public class ProcActualPlanModel
    {
        public ProcActualPlanModel()
        {
            QuantityByDays = new List<QuantityByDay>();
        }

        public string Month { get; set; }
        public int DayOfMonth { get; set; }

        public string DanhMuc { get; set; }

        public string CFAB { get; set; }

       public List<QuantityByDay> QuantityByDays;
    }
}
