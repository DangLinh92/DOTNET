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

        List<EhsDeMucKeHoachViewModel> GetDeMucKeHoachByKeHoach(Guid? maKeHoach);
        Guid UpdateDeMucKeHoach(Guid maDemuc,string tenDemuc);
        Guid DeleteDeMucKeHoach(Guid Id);
        DeMucLuatDinh GetTenDeMucKeHoach(Guid maDemuc);

        List<EhsLuatDinhDeMucKeHoachViewModel> GetLuatDinhDeMucKeHoach();
        Guid UpdateLuatDinhDeMucKeHoach(Guid maDemuc,string luatDinh);
        EhsLuatDinhDeMucKeHoachViewModel DeleteLuatDinhDemucKeHoach(int id);

        List<EhsNoiDungViewModel> GetNoiDungByKeHoach(Guid maKeHoach,string year);
        Guid UpdateNoiDung(Guid maNoiDung,string noidung);
        Guid DeleteNoiDung(Guid maNoiDung);
        string GetNoiDungKeHoach(Guid maNoiDung);

        List<EhsNoiDungKeHoachViewModel> GetNoiDungKeHoachByMaNoiDung(string maNoiDung,string year);
        EhsNoiDungKeHoachViewModel GetNoiDungKeHoachById(string Id);
        EhsNoiDungKeHoachViewModel UpdateNoiDungKeHoach(EhsNoiDungKeHoachViewModel model);
        EhsNoiDungKeHoachViewModel UpdateNoiDungKeHoachSingle(EhsNoiDungKeHoachViewModel model);
        string DeleteNoiDungKeHoach(string Id);

        ResultDB ImportExcel(string filePath,string maKH);

        TongHopKeHoachALL TongHopKeHoachByYear(string year);

        EhsChiPhiByMonthViewModel GetChiPhiNoiDung(string noidungId,string year);
        EhsChiPhiByMonthViewModel GetChiPhiById(int Id);
        EhsChiPhiByMonthViewModel AddChiPhi(EhsChiPhiByMonthViewModel model);
        EhsChiPhiByMonthViewModel UpdateChiPhi(EhsChiPhiByMonthViewModel model);

        void Save();
    }
}
