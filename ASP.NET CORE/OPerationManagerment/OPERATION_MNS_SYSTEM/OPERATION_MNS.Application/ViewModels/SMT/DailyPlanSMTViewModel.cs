using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.SMT
{
    public class DailyPlanSMTViewModel
    {
        public string Model { get; set; }
        public string MesCode { get; set; }

        // tồn hiện tại
        public double Inventory { get; set; }

        public double GocPlanToday { get; set; }
        public double GocPlanToday_1 { get; set; }
        public double GocPlanToday_2 { get; set; }
        public double GocPlanToday_3 { get; set; }
        public double GocPlanToday_4 { get; set; }
        public double GocPlanToday_5 { get; set; }
        public double ActualToday { get; set; }
        public double PlanToday { get; set; }
        public double PlanToday_1 { get; set; }
        public double PlanToday_2 { get; set; }
        public double PlanToday_3 { get; set; }
        public double PlanToday_4 { get; set; }
        public double PlanToday_5 { get; set; }
        public string LastUpdate { get; set; }
    }

    public class NextDay_SMT
    {
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
    }
}
