using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IInventoryService : IDisposable
    {
        List<InventoryActualViewModel> GetCurrentInventory(string unit);

        public LotMonitoringLfemViewModel GetMonitoringLfemData(string operation,string model,decimal stayday,string sheetNumber);
      
    }
}
