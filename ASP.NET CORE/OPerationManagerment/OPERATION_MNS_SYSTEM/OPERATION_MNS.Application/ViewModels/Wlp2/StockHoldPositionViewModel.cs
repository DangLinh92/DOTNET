using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class StockHoldPositionViewModel
    {
        public StockHoldPositionViewModel()
        {
            
        }

        public string Module { get; set; }
        public string SapCode { get; set; }
        //public string Material { get; set; }
        public int WaferNguyenLieu { get; set; }
        public int BanThanhPham { get; set; }
        public int ChoXuatHang { get; set; }
        public int Sum { get; set; }

        // hold
        public int HoldWafer { get; set; }
        public int HoldReel { get; set; }
        public int HoldOQC { get; set; }
        public int SmtReturn { get; set; }

        // ton vi tri

        // 대기공정
        public int AtmosphericProcess { get; set; }

        // 후공정 생산대기
        public int WatingProcess { get; set; }

        public int BG { get; set; }
        public int WaferOven { get; set; }
        public int Dicing { get; set; }
        public int ChipInspection { get; set; }
        public int ReelPacking { get; set; }
        public int ReelInspection { get; set; }
        public int ReelCounter { get; set; }
        public int ReelOven { get; set; }
        public int OQC { get; set; }
        public int WaitingforShipment { get; set; }

        public string LastUpdate { get; set; }
    }
}
