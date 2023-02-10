using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachAnToanBucXaService
    {
        List<EhsKeHoachAnToanBucXaViewModel> GetList(string year);
        EhsKeHoachAnToanBucXaViewModel Update(EhsKeHoachAnToanBucXaViewModel model);
        EhsKeHoachAnToanBucXaViewModel Add(EhsKeHoachAnToanBucXaViewModel model);
        void Delete(Guid Id);
        EhsKeHoachAnToanBucXaViewModel GetById(Guid Id);

        EhsThoiGianThucHienAnToanBucXaViewModel UpdateThoiGianBucXa(EhsThoiGianThucHienAnToanBucXaViewModel model);
        EhsThoiGianThucHienAnToanBucXaViewModel AddThoiGianBucXa(EhsThoiGianThucHienAnToanBucXaViewModel model);
        void DeleteThoiGianBucXa(int Id);
        EhsThoiGianThucHienAnToanBucXaViewModel GetThoiGianBucXaById(int Id);

        string ImportExcel(string filePath);

        void Save();
    }
}
