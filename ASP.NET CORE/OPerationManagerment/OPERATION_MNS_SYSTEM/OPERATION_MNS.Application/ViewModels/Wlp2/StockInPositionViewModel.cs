using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class StockInPositionViewModel
    {
        public string SapCode { get; set; }

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
    }
}
