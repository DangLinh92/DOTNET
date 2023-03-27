using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Models.SignalR
{
    public interface IInventoryTicker : IDisposable
    {
        IEnumerable<InventoryActualModel> GetAllStocks();
        IEnumerable<DailyPlanViewModel> GetDailyPlan();
        IEnumerable<StockHoldPositionViewModel> GetWlp2StockHold();
        IEnumerable<DailyPlanWlp2ViewModel> GetWlp2DailyPlans();
    }
}
