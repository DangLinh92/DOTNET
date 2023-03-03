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

        TaskStatistics GetStatistics(string maKeHoach);

        List<EhsLuatDinhKeHoachViewModel> GetLuatDinhKeHoach(Guid? maKeHoach);
        EhsLuatDinhKeHoachViewModel UpdateLuatDinhKeHoach(EhsLuatDinhKeHoachViewModel model);
        EhsLuatDinhKeHoachViewModel DeleteLuatDinhKeHoach(int id);

        List<TotalAllItemByYear> TongHopKeHoachByYear(string year);

        List<EhsFileKetQuaViewModel> GetFileByNoiDung(string makehoach);

        List<EhsKeHoachItemModel> DanhSachKeHoachByTime(string fromTime, string ToTime);

        List<KanbanViewModel> GetKanBanBoard();
        KanbanViewModel GetEvenById(string id);

        List<Ehs_ThoiGianThucHien> GetThoiGianThucHien(string maKeHoach);

        string GetFolderKetQua(string maNgayChitiet);

        int UpdateEvent(string id,string status,string priority,string progress,string actualFinish,string begindate,string action);

        void Save();
    }
}
