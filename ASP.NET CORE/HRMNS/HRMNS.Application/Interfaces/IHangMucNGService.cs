using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IHangMucNGService
    {
        List<EhsHangMucNGViewModel> GetAll();
        List<EhsHangMucNGViewModel> GetByKey(string key);
        List<EhsHangMucNGViewModel> GetByYear(string year);
        EhsHangMucNGViewModel GetById(int id);
        EhsHangMucNGViewModel Add(EhsHangMucNGViewModel item);

        EhsHangMucNGViewModel Update(EhsHangMucNGViewModel item);

        void Delete(int id);

        void Save();
    }
}
