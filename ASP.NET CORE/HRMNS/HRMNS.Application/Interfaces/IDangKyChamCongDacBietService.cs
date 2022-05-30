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
    public interface IDangKyChamCongDacBietService : IDisposable
    {
        List<DangKyChamCongDacBietViewModel> GetAll(string keyword);

        DangKyChamCongDacBietViewModel Add(DangKyChamCongDacBietViewModel hopDongVm);

        void Update(DangKyChamCongDacBietViewModel hopDongVm);
        void UpdateRange(List<DangKyChamCongDacBietViewModel> hopDongVm);

        void Delete(int id);

        List<DangKyChamCongDacBietViewModel> GetAll(params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties);

        DangKyChamCongDacBietViewModel GetById(int id);
        DangKyChamCongDacBietViewModel GetSingle(Expression<Func<DANGKY_CHAMCONG_DACBIET, bool>> predicate);

        List<DangKyChamCongDacBietViewModel> Search(string dept, string fromDate, string toDate, params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties);

        void Save();
    }
}
