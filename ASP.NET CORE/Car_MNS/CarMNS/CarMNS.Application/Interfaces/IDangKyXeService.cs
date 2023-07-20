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

        List<DIEUXE_DANGKY> GetXe(int maDangKy);
        DIEUXE_DANGKY AddXe(DIEUXE_DANGKY en);
        DIEUXE_DANGKY UpdateXe(DIEUXE_DANGKY en);
        DIEUXE_DANGKY GetDieuXeById(int id);
        void DeleteXe(int id);

        bool Approve(int maDangKy, string role);
        bool UnApprove(int maDangKy, string role);

        Dictionary<string, List<string>> GetUserSendMail(int maDangKy, bool isNew, bool isApprove, bool isUnApprove);

        void Save();
    }
}
