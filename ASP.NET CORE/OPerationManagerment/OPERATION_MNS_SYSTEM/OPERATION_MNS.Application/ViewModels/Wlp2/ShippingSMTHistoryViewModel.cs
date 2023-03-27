using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class ShippingSMTHistoryByLotIdViewModel
    {
        public string NgayXuat { get; set; }
        public string MoveOutTime { get; set; }
        public string LotID { get; set; }
        public string SapMaterial { get; set; }
        public float OutPutQty { get; set; }
        public string Note { get; set; }
        public string Material { get; set; }
    }

    public class ShippingSMTHistoryBySapCodeViewModel
    {
        public string SapMaterial { get; set; }
        public float OutPutQty { get; set; }
    }

    public class ShippingSMTHistoryViewModel
    {
        public ShippingSMTHistoryViewModel()
        {
            shippingSMTHistoryByLotIdViewModels = new List<ShippingSMTHistoryByLotIdViewModel>();
            shippingSMTHistoryBySapCodeViewModels = new List<ShippingSMTHistoryBySapCodeViewModel>();
        }

        public List<ShippingSMTHistoryByLotIdViewModel> shippingSMTHistoryByLotIdViewModels;
        public List<ShippingSMTHistoryBySapCodeViewModel> shippingSMTHistoryBySapCodeViewModels;
    }
}
