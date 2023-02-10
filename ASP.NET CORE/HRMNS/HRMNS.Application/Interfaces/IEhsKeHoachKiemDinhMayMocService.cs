using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachKiemDinhMayMocService
    {
        List<EhsKeHoachKiemDinhMayMocViewModel> GetList(string year);
        EhsKeHoachKiemDinhMayMocViewModel Update(EhsKeHoachKiemDinhMayMocViewModel model);
        EhsKeHoachKiemDinhMayMocViewModel Add(EhsKeHoachKiemDinhMayMocViewModel model);
        void Delete(Guid Id);
        EhsKeHoachKiemDinhMayMocViewModel GetById(Guid Id);

        EhsThoiGianKiemDinhMayMocViewModel UpdateThoiGianKiemDinhMayMoc(EhsThoiGianKiemDinhMayMocViewModel model);
        EhsThoiGianKiemDinhMayMocViewModel AddThoiGianKiemDinhMayMoc(EhsThoiGianKiemDinhMayMocViewModel model);
        void DeleteThoiKiemDinhMayMoc(int Id);
        EhsThoiGianKiemDinhMayMocViewModel GetThoiGianKiemDinhMayMocById(int Id);

        string ImportExcel(string filePath);

        void Save();
    }
}
