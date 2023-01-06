using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IYieldOfModelService : IDisposable
    {
        List<YieldOfModelViewModel> GetAllYeildOfModel();
        YieldOfModelViewModel UpdateYeildOfModel(YieldOfModelViewModel model);
        YieldOfModelViewModel AddYeildOfModel(YieldOfModelViewModel model);

        YieldOfModelViewModel GetYeildOfModelById(int id);

        List<string> GetAllModel();

        void Delete(int Id);
        void Save();
    }
}
