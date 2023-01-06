using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Models.SignalR
{
    public class GOC_PlanModel
    {
        public GOC_PlanModel()
        {
            GocPlanViewModels = new List<GocPlanViewModel>();
            DateOffLineViewModels = new List<DateOffLineViewModel>();
            SettingItemsViewModels = new List<SettingItemsViewModel>();
            InventoryActualModels = new List<InventoryActualModel>();
            GocPlanWaferViewModels = new List<GocPlanViewModel>();
        }

        public List<GocPlanViewModel> GocPlanViewModels { get; set; }
        public List<GocPlanViewModel> GocPlanWaferViewModels { get; set; }
        public List<DateOffLineViewModel> DateOffLineViewModels { get; set; }
        public List<SettingItemsViewModel> SettingItemsViewModels { get; set; }

        public List<InventoryActualModel> InventoryActualModels { get; set; }
    }
}
