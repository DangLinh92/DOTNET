using HRMNS.Application.ViewModels.EHS;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IDanhMucKeHoachService : IDisposable
    {
        EhsDanhMucKeHoachPageViewModel GetDataDanhMucKeHoachPage(Guid? maKeHoach);
        List<EhsDMKeHoachViewModel> GetDMKehoach();
        EhsDMKeHoachViewModel UpdateDMKeHoach(EhsDMKeHoachViewModel kehoach);
        Guid DeleteDMKeHoach(Guid id);

        List<EhsLuatDinhKeHoachViewModel> GetLuatDinhKeHoach(Guid? maKeHoach);
        EhsLuatDinhKeHoachViewModel UpdateLuatDinhKeHoach(EhsLuatDinhKeHoachViewModel model);
        EhsLuatDinhKeHoachViewModel DeleteLuatDinhKeHoach(int id);

        //TongHopKeHoachALL TongHopKeHoachByYear(string year);

        void Save();
    }
}
