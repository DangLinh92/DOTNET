using Microsoft.AspNetCore.SignalR;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Hubs
{
    /// <summary>
    /// Can you check if WebSockets Protocol is enabled in IIS.
    ///"Turn Windows features on or off" -> Internet Information Services ->World Wide Web Services -> Application Development Features -> WebSocket Protocol
    /// </summary>
    public class LiveUpdateSignalR_WLP2_Hub : Hub
    {
        private readonly IWLP2Ticker _inventoryTicker;

        public LiveUpdateSignalR_WLP2_Hub(IWLP2Ticker inventoryTicker)
        {
            _inventoryTicker = inventoryTicker;
        }

        public IEnumerable<StockHoldPositionViewModel> GetWlp2StockHold()
        {

            WLP2Ticker.WLP2_Stock = true;
            WLP2Ticker.WLP2_DailyPlan = false;
         

            return _inventoryTicker.GetWlp2StockHold();
        }

        public IEnumerable<DailyPlanWlp2ViewModel> GetWlp2DailyPlans()
        {
            WLP2Ticker.WLP2_Stock = false;
            WLP2Ticker.WLP2_DailyPlan = true;

            return _inventoryTicker.GetWlp2DailyPlans();
        }
    }
}
