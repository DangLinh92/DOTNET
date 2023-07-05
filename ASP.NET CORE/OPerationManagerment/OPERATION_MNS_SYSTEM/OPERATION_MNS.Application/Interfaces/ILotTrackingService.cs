using OPERATION_MNS.Application.ViewModels.LotTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ILotTrackingService : IDisposable
    {
        List<LotTrackingViewModel> GetPackingInfo(string LotNo);
        List<WaferInfo> GetAffectlotInfo(string LotNo);
        List<WaferInfo> GetTrackingVIEWs(string LotNo);
    }
}
