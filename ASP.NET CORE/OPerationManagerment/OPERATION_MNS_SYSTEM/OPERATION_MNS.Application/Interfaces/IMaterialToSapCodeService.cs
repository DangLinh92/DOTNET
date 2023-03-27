using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IMaterialToSapCodeService : IDisposable
    {
        List<MaterialToSapViewModel> GetAllData(string dept);
        MaterialToSapViewModel Update(MaterialToSapViewModel model);
        MaterialToSapViewModel Add(MaterialToSapViewModel model);

        MaterialToSapViewModel GetById(int id);
        ResultDB ImportExcel(string filePath, string param);
        void Delete(int Id);
        void Save();
    }
}
