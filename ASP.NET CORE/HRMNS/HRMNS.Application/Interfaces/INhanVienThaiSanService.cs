using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface INhanVienThaiSanService : IDisposable
    {
        NhanVienThaiSanViewModel Add(NhanVienThaiSanViewModel nhanVienVm);

        void Update(NhanVienThaiSanViewModel nhanVienVm);

        void Delete(int id);

        List<NhanVienThaiSanViewModel> GetAll();

        NhanVienThaiSanViewModel GetById(int id, params Expression<Func<HR_THAISAN_CONNHO, object>>[] includeProperties);

        List<NhanVienThaiSanViewModel> Search(string maNV, string fromDate, string toDate);

        void Save();
    }
}
