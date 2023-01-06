using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Models.SignalR
{
    public class InventoryActualModel : InventoryActualViewModel
    {
        public string InventoryId { get; set; }
        public void Update(InventoryActualModel newModel)
        {
            this.CopyPropertiesFromWhithoutType(newModel, new List<string>() { });
        }
    }
}
