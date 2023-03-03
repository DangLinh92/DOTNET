using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsCoQuanKiemtraService
    {
        List<EhsCoQuanKiemTraViewModel> GetList();
        List<EhsCoQuanKiemTraViewModel> GetNG(string year);
        EhsCoQuanKiemTraViewModel Update(EhsCoQuanKiemTraViewModel model);
        EhsCoQuanKiemTraViewModel Add(EhsCoQuanKiemTraViewModel model);
        void Delete(int Id);
        EhsCoQuanKiemTraViewModel GetById(int Id);
        void Save();
    }
}
