using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IDeNghiXuatNVLService : IDisposable
    {
        List<BoPhanDeNghiXuatNVLViewModel> GetAllData(string ngay);
        BoPhanDeNghiXuatNVLViewModel Update(BoPhanDeNghiXuatNVLViewModel model);
        BoPhanDeNghiXuatNVLViewModel Add(BoPhanDeNghiXuatNVLViewModel model);

        BoPhanDeNghiXuatNVLViewModel GetById(int id);
        ResultDB ImportExcel(string filePath, string param);
        void Delete(int Id);
        void Save();
    }
}
