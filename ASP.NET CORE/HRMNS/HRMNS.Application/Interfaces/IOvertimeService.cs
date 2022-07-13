using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IOvertimeService : IDisposable
    {
        DangKyOTNhanVienViewModel Add(DangKyOTNhanVienViewModel overtimeVm);

        void Update(DangKyOTNhanVienViewModel nhanVienLVVm);

        void Delete(int id);

        List<DangKyOTNhanVienViewModel> GetAll(string keyword, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        DangKyOTNhanVienViewModel GetById(int id, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        List<DangKyOTNhanVienViewModel> Search(string role,string dept,string status,string timeFrom,string timeTo, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        ResultDB ImportExcel(string filePath, string param);

        void Save();

        void UpdateRange(List<DangKyOTNhanVienViewModel> OTVms);

        DangKyOTNhanVienViewModel CheckExist(int id, string maNV, string date,string hso);
    }
}
