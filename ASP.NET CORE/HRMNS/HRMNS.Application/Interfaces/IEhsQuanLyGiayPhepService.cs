using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsQuanLyGiayPhepService
    {
        List<EhsQuanLyGiayPhepViewModel> GetList();
        EhsQuanLyGiayPhepViewModel Update(EhsQuanLyGiayPhepViewModel model);
        EhsQuanLyGiayPhepViewModel Add(EhsQuanLyGiayPhepViewModel model);
        void Delete(int Id);
        EhsQuanLyGiayPhepViewModel GetById(int Id);
        void Save();
    }
}
