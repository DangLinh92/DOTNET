using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.LotTracking
{
    public class LotTrackingVIEW
    {
        public LotTrackingVIEW()
        {
            WaferInfoViewModels = new List<WaferInfo>();
        }
        public List<WaferInfo> WaferInfoViewModels { get; set; }
    }
    public class LotTrackingViewModel
    {
        public LotTrackingViewModel()
        {
            WaferInfos = new List<WaferInfo>();
            RelationLots = new List<string>();
        }

        public string LotModule { get; set; }
        public string Wlp2_Reel_Number { get; set; } // start with LA,LB,LC
        public string CassetId { get; set; } // WLP1_Wafer_Number
        public string Model { get; set; }
        public List<WaferInfo> WaferInfos { get; set; }
        public List<string> RelationLots { get; set; }
    }

    public class WaferInfo
    {
        public WaferInfo()
        {
            LotHistories = new List<LotHistory>();
        }

        public string LotModule { get; set; }
        public string Wlp2_Reel_Number { get; set; } // start with LA,LB,LC
        public string CassetId { get; set; } // WLP1_Wafer_Number
        public string Model { get; set; }
        public string LotID { get; set; }
        public string WaferID { get; set; }
        public string LotFAB { get; set; }
        public LotIDView LotIDView { get; set; }
        public string Operation { get; set; }
        public List<LotHistory> LotHistories { get; set; }

        public string EquiptmentName { get; set; }
        public string OnlineOffLine { get; set; }
        public string TranTime { get; set; }
    }

    public class LotHistory
    {
        public string CassetId { get; set; }
        public string LotID { get; set; }
        public string Operation { get; set; }
        public string EquiptmentName { get; set; }
        public string OnlineOffLine { get; set; }
        public string TranTime { get; set; }
        public string Wlp2_Reel_Number { get; set; }
    }

    public class LotIDView
    {
        public string LotID { get; set; }
        public string IsRelationLot { get; set; }
    }

}
