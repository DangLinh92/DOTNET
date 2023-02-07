using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachKhamSKService
    {
        List<EhsKeHoachKhamSKViewModel> GetList(string year);
        EhsKeHoachKhamSKViewModel Update(EhsKeHoachKhamSKViewModel model);
        EhsKeHoachKhamSKViewModel Add(EhsKeHoachKhamSKViewModel model);
        void Delete(Guid Id);
        EhsKeHoachKhamSKViewModel GetById(Guid Id);

        EhsNgayThucHienChiTietKhamSKViewModel UpdateNgayKhamSK(EhsNgayThucHienChiTietKhamSKViewModel model);
        EhsNgayThucHienChiTietKhamSKViewModel AddNgayKhamSK(EhsNgayThucHienChiTietKhamSKViewModel model);
        void DeleteNgayKhamSK(int Id);
        EhsNgayThucHienChiTietKhamSKViewModel GetNgayKhamSKById(int Id);

        EhsNhanVienKhamSucKhoe UpdateNhanVienKhamSK(EhsNhanVienKhamSucKhoe model);
        EhsNhanVienKhamSucKhoe AddNhanVienKhamSK(EhsNhanVienKhamSucKhoe model);
        void DeleteNhanVienKhamSK(int Id);
        List<EhsNhanVienKhamSucKhoe> GetNhanVienKhamSKByKeHoach(Guid maKHKhamSK);
        EhsNhanVienKhamSucKhoe GetNhanVienKhamSKById(int Id);

        void ImportExcel(string filePath,string Id);

        void Save();
    }
}
