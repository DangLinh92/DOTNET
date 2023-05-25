using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IDCChamCongService : IDisposable
    {
        List<DCChamCongViewModel> Search(string status,string dept,string timeFrom,string timeTo, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties);

        DCChamCongViewModel Add(DCChamCongViewModel nhanVienLVVm);

        void Update(DCChamCongViewModel nhanVienLVVm);

        void Delete(int id);

        List<DCChamCongViewModel> GetAll(string keyword, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties);

        DCChamCongViewModel GetById(int id, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties);
        ResultDB ImportExcel(string path);
        void Save();
    }
}
