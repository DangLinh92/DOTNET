using Microsoft.AspNetCore.SignalR;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.SMT;
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
    public class LiveUpdateSignalR_SMT_Hub : Hub
    {
        private readonly ISMTTicker _smtTicker;

        public LiveUpdateSignalR_SMT_Hub(ISMTTicker inventoryTicker)
        {
            _smtTicker = inventoryTicker;
        }

        public IEnumerable<DailyPlanSMTViewModel> GetDailyPlanSMT()
        {
            return _smtTicker.GetDailyPlan();
        }
    }
}
