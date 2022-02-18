using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface INhanVienService: IDisposable
    {
        NhanVienViewModel Add(NhanVienViewModel nhanVienVm);

        void Update(NhanVienViewModel nhanVienVm);

        void UpdateSingle(NhanVienViewModel nhanVienVm);

        void Delete(string id);

        List<NhanVienViewModel> GetAll();

        List<NhanVienViewModel> GetAll(string keyword);

        //NhanVienViewModel GetById(string id);

        NhanVienViewModel GetById(string id, params Expression<Func<HR_NHANVIEN, object>>[] includeProperties);

        List<NhanVienViewModel> Search(string id, string name, string dept);

        void ImportExcel(string filePath,string param);

        void Save();
    }
}
