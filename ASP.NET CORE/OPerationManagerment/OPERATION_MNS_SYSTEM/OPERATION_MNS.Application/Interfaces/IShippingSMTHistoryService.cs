using OPERATION_MNS.Application.ViewModels.Wlp2;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IShippingSMTHistoryService : IDisposable
    {
        List<ShippingSMTHistoryViewModel> GetHistoryByTime(string fromDate, string toDate);
    }
}
