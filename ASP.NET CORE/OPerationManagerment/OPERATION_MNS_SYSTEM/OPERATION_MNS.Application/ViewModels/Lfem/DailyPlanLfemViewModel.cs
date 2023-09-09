using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Lfem
{
    public class DailyPlanLfemViewModel
    {
        public DailyPlanLfemViewModel()
        {
            Dam = new QtyByOperation();
            Mold = new QtyByOperation();
            Grinding = new QtyByOperation();
            Marking = new QtyByOperation();
            Dicing = new QtyByOperation();
            Test = new QtyByOperation();
            VisualInspection = new QtyByOperation();
            OQC = new QtyByOperation();
        }

        public string Model { get; set; }
        public string MesCode { get; set; }
        public QtyByOperation Dam { get; set; }
        public QtyByOperation Mold { get; set; }
        public QtyByOperation Grinding { get; set; }
        public QtyByOperation Marking { get; set; }
        public QtyByOperation Dicing { get; set; }
        public QtyByOperation Test { get; set; }
        public QtyByOperation VisualInspection { get; set; }
        public QtyByOperation OQC { get; set; }
        public string LastUpdate { get; set; }
        public int STT { get; set; }
        public string KHSX { get; set; }

        public string PrioryInOperation { get; set; }
        public int NumberPriory { get; set; }
        public string IsLoadPage { get; set; }
    }

    public class QtyByOperation
    {
        public QtyByOperation()
        {

        }
        public double Wip { get; set; }
        public double Plan { get; set; }
        public double Actual { get; set; }
        public string TotalStr { get; set; }

        public double QtyT0 { get; set; }
        public double QtyT1 { get; set; }
        public double QtyT2 { get; set; }
    }
}
