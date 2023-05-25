using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IChucDanhService : IDisposable
    {
        List<ChucDanhViewModel> GetAll(string filter);
        ChucDanhViewModel Add(ChucDanhViewModel model);
        ChucDanhViewModel Update(HR_CHUCDANH model);
        ChucDanhViewModel GetById(string id);
        void Save();
        void Delete(string id);
    }
}
