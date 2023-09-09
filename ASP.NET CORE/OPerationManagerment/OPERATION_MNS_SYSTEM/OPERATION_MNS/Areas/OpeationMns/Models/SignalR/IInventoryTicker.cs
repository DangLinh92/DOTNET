using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.SMT;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.Entities;
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

    public interface ISMTTicker : IDisposable
    {
        IEnumerable<DailyPlanSMTViewModel> GetDailyPlan();  
    }

    public interface ILFEMTicker : IDisposable
    {
        IEnumerable<DailyPlanLfemViewModel> DailyPlanLfem();
        IEnumerable<WARNING_LOT_RUNTIME_LFEM> RunTimeLfem();
    }

    public interface IWLP2Ticker : IDisposable
    {
        IEnumerable<StockHoldPositionViewModel> GetWlp2StockHold();
        IEnumerable<DailyPlanWlp2ViewModel> GetWlp2DailyPlans();
    }
}
