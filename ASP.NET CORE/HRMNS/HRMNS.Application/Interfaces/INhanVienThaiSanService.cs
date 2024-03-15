using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
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

        List<NhanVienThaiSanViewModel> Search(string maNV, string fromDate, string toDate,string chedo);

        void Save();

        List<HoTroSinhLyViewModel> GetHoTroSinhLyImport(string month,string boPhan);
        List<HoTroSinhLyViewModel> GetHoTroSinhLy(string month,string boPhan);
        HoTroSinhLyViewModel AddHotrosinhly(HoTroSinhLyViewModel model);
        HoTroSinhLyViewModel EditHotrosinhly(HoTroSinhLyViewModel model);
        void DeleteHotrosinhly(int id);
        ResultDB ImportExcel(string filePath, string param);
        ResultDB ImportThaiSanExcel(string filePath, string param);
    }
}
