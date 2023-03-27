using Microsoft.AspNetCore.SignalR;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
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
    public class liveUpdateSignalRHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        private readonly IInventoryTicker _inventoryTicker;

        public liveUpdateSignalRHub(IInventoryTicker inventoryTicker)
        {
            _inventoryTicker = inventoryTicker;
        }

        public IEnumerable<InventoryActualModel> GetAllStocks()
        {
            InventoryTicker.WLP1_Stock = true;
            InventoryTicker.WLP2_Stock = false;
            InventoryTicker.WLP1_DailyPlan = false;
            InventoryTicker.WLP2_DailyPlan = false;

            return _inventoryTicker.GetAllStocks();
        }

        public IEnumerable<DailyPlanViewModel> GetDailyPlans()
        {
            InventoryTicker.WLP1_Stock = false;
            InventoryTicker.WLP2_Stock = false;
            InventoryTicker.WLP1_DailyPlan = true;
            InventoryTicker.WLP2_DailyPlan = false;

            return _inventoryTicker.GetDailyPlan();
        }

        public IEnumerable<StockHoldPositionViewModel> GetWlp2StockHold()
        {
            InventoryTicker.WLP1_Stock = false;
            InventoryTicker.WLP2_Stock = true;
            InventoryTicker.WLP1_DailyPlan = false;
            InventoryTicker.WLP2_DailyPlan = false;

            return _inventoryTicker.GetWlp2StockHold();
        }


        public IEnumerable<DailyPlanWlp2ViewModel> GetWlp2DailyPlans()
        {
            InventoryTicker.WLP1_Stock = false;
            InventoryTicker.WLP2_Stock = false;
            InventoryTicker.WLP1_DailyPlan = false;
            InventoryTicker.WLP2_DailyPlan = true;

            return _inventoryTicker.GetWlp2DailyPlans();
        }
    }
}
