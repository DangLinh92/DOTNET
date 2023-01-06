using OPERATION_MNS.Application.ViewModels;
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
    }
}
