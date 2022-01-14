using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface INhanVienService: IDisposable
    {
        NhanVienViewModel Add(NhanVienViewModel nhanVienVm);

        void Update(NhanVienViewModel nhanVienVm);

        void Delete(string id);

        List<NhanVienViewModel> GetAll();

        List<NhanVienViewModel> GetAll(string keyword);

        NhanVienViewModel GetById(string id);

        List<NhanVienViewModel> Search(string id, string name, string dept);

        void Save();
    }
}
