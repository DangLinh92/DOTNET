using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IDetailSalaryService : IDisposable
    {
        List<BangLuongChiTietViewModel> GetBangLuongChiTiet(string thangNam,string chedo);
        List<BangLuongChiTietViewModel> GetHistoryBangLuongChiTiet(string thangNam,string chedo);
        void ChotBangLuong(string time, List<BangLuongChiTietViewModel> data);
        ResultDB ImportExcel(string filePath, out List<HR_SALARY> lstUpdate);

        void XacNhanChiTra(List<string> lstMaNV);
    }
}
