using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class WaittimeViewModel
    {
        public string Id { get; set; }
        public string CassetteID { get; set; }
        public string Material { get; set; }
        public string OperationName { get; set; }
        public string OperationID { get; set; }
        public string Status { get; set; }
        public decimal StayDay { get; set; }
        public string LotId { get; set; }
        public int SoTam { get; set; }
    }

    public class OperationWaitimeSheet
    {
        public OperationWaitimeSheet()
        {
            lstWaittimeViewModel = new List<WaittimeViewModel>();
        }

        public string GridName { get; set; }
        public List<WaittimeViewModel> lstWaittimeViewModel;
    }
}
