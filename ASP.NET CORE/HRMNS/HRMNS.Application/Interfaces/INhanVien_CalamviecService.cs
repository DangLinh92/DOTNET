using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface INhanVien_CalamviecService : IDisposable
    {
        NhanVien_CalamViecViewModel Add(NhanVien_CalamViecViewModel nhanVienLVVm);

        void Update(NhanVien_CalamViecViewModel nhanVienLVVm);

        void UpdateSingle(NhanVien_CalamViecViewModel nhanVienLVVm);

        void Delete(string id);

        List<NhanVien_CalamViecViewModel> GetAll();

        List<NhanVien_CalamViecViewModel> GetAll(string keyword, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties);

        NhanVien_CalamViecViewModel GetById(string id, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties);

        List<NhanVien_CalamViecViewModel> Search(string id, string name, string dept);

        ResultDB ImportExcel(string filePath, string param);
        List<DMCalamviecViewModel> GetDMCalamViec();
        void Save();
    }
}
