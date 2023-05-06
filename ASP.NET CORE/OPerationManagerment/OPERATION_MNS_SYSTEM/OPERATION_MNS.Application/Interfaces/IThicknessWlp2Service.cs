using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IThicknessWlp2Service : IDisposable
    {
        List<ThickNetModelWlp2ViewModel> GetAllData();
        ThickNetModelWlp2ViewModel Update(ThickNetModelWlp2ViewModel model);
        ThickNetModelWlp2ViewModel Add(ThickNetModelWlp2ViewModel model);

        ThickNetModelWlp2ViewModel GetById(int id);
        ResultDB ImportExcel(string filePath, string param);
        void Delete(int Id);
        void Save();
    }
}
