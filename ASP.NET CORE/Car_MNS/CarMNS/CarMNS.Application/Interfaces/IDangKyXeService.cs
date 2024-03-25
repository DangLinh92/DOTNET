using CarMNS.Application.ViewModels;
using CarMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.Interfaces
{
    public interface IDangKyXeService : IDisposable
    {
        List<DANG_KY_XE> GetDangKyXeHistory();
        List<DANG_KY_XE> GetAllDangKyXe(string role, string bophan);
        DANG_KY_XE AddDangKyXe(DANG_KY_XE dangky, string role);
        DANG_KY_XE UpdateDangKyXe(DANG_KY_XE dangky);
        void DeleteDangKyXe(int id);
        DANG_KY_XE GetDangKyXeById(int id);
        List<BOPHAN> GetBoPhan();

        List<TAXI_CARD_INFO> GetBoCardInfo();
        List<MUCDICHSD_XE> GetMucDichSD();

        List<DIEUXE_DANGKY> GetXe(int maDangKy);
        DIEUXE_DANGKY AddXe(DIEUXE_DANGKY en);
        DIEUXE_DANGKY UpdateXe(DIEUXE_DANGKY en);
        DIEUXE_DANGKY GetDieuXeById(int id);
        void DeleteXe(int id);

        bool Approve(int maDangKy, string role);
        bool UnApprove(int maDangKy, string role);

        Dictionary<string, List<string>> GetUserSendMail(int maDangKy, bool isNew, bool isApprove, bool isUnApprove);
        Dictionary<string, List<string>> GetUserSendMailTaxi(int maDangKy, bool isNew, bool isApprove, bool isUnApprove);
        void Save();

        List<DANG_KY_XE_TAXI> GetAllDangKyXe_Taxi(string role, string bophan);
        List<DANG_KY_XE_TAXI> GetHistoryByUser(string user);
        DANG_KY_XE_TAXI AddDangKyXe_Taxi(DANG_KY_XE_TAXI dangky, string role);
        DANG_KY_XE_TAXI GetDangKyXeTaxiById(int id);
        DANG_KY_XE_TAXI UpdateDangKyXeTaxi(DANG_KY_XE_TAXI dangky);
        void DeleteDangKyXeTaxi(int id);
        bool ApproveTaxi(int maDangKy, string role);
        bool UnApproveTaxi(int maDangKy, string role);
        List<DANG_KY_XE_TAXI> GetDangKyXeTaxiHistory();

        List<NguoiDungTaxi> GetReportTaxi(string fromDate, string toDate, string boPhan);
        List<NguoiDungTaxi> GetReportTaxiInYear(string fromDate, string toDate);

        List<ItemValue> GetListTime();

        List<TaxiCostReportViewModel> TaxiCostReportData(string fromTime, string toTime);
    }
}
