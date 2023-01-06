using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IMaterialToSapCodeService : IDisposable
    {
        List<MaterialToSapViewModel> GetAllData();
        MaterialToSapViewModel Update(MaterialToSapViewModel model);
        MaterialToSapViewModel Add(MaterialToSapViewModel model);

        MaterialToSapViewModel GetById(int id);

        void Delete(int Id);
        void Save();
    }
}
