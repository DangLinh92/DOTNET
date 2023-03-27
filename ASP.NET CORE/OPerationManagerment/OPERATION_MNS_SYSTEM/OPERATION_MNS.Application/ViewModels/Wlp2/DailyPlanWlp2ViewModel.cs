using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class DailyPlanWlp2ViewModel
    {
        public string LastUpdate { get; set; }
        public string Module { get; set; }
        public string SapCode { get; set; } // Model
        public int ToTalWip { get; set;}
        public int ChoXuatHang { get; set; }
        public int HoldNVL { get; set; }
        public int NguyenLieuOKSX { get; set; }
        public float KHSXTheoNgay { get; set; }
        public float SoLuongConLaiSauKHSX { get; set; }
        public float QtyChipBase { get; set; }
        public string Type { get; set; }
        public float BackGrinding { get; set; }
        public float WaferOven { get; set; }
        public float Dicing { get; set; }
        public float ChipInspection { get; set; }
        public float Packing { get; set; }
        public float ReelInspection { get; set; }
        public float QC_Pass { get; set; }
        public double CumYield { get; set; } 

        public string PrioryInOperation { get; set; }
        public int NumberPriory { get; set; }

        public string IsLoadPage { get; set; }
    }
}
