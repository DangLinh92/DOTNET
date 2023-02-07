using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachQuanTracService
    {
        List<EhsKeHoachQuanTracViewModel> GetList(string year);
        EhsKeHoachQuanTracViewModel Update(EhsKeHoachQuanTracViewModel model);
        EhsKeHoachQuanTracViewModel Add(EhsKeHoachQuanTracViewModel model);
        void Delete(int Id);

        EhsKeHoachQuanTracViewModel GetById(int Id);

        EhsNgayThucHienChiTietQuanTrac UpdateNgayQuanTrac(EhsNgayThucHienChiTietQuanTrac model);
        EhsNgayThucHienChiTietQuanTrac AddNgayQuanTrac(EhsNgayThucHienChiTietQuanTrac model);
        void DeleteNgayQuanTrac(int Id);
        EhsNgayThucHienChiTietQuanTrac GetNgayQuanTracById(int Id);

        void ImportExcel(string filePath);

        void Save();
    }
}
