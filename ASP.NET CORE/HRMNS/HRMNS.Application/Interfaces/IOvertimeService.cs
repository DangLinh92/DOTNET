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

        void UpdateSingle(DangKyOTNhanVienViewModel nhanVienLVVm);

        void Delete(int id);

        List<DangKyOTNhanVienViewModel> GetAll(string keyword, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        DangKyOTNhanVienViewModel GetById(int id, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        List<DangKyOTNhanVienViewModel> Search(string dept,string status,string timeFrom,string timeTo, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties);

        ResultDB ImportExcel(string filePath, string param);

        void Save();

        void Approve(string dept, string status,bool isApprove);
        void ApproveSingle(int Id, bool isApprove);

        DangKyOTNhanVienViewModel CheckExist(int id, string maNV, string date);
    }
}
