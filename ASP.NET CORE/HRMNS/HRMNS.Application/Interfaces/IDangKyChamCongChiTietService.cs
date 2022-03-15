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
    public interface IDangKyChamCongChiTietService : IDisposable
    {
        List<DangKyChamCongChiTietViewModel> GetAll(string keyword);

        DangKyChamCongChiTietViewModel Add(DangKyChamCongChiTietViewModel hopDongVm);

        void Update(DangKyChamCongChiTietViewModel hopDongVm);

        void Delete(int id);

        List<DangKyChamCongChiTietViewModel> GetAll(params Expression<Func<DANGKY_CHAMCONG_CHITIET, object>>[] includeProperties);

        DangKyChamCongChiTietViewModel GetById(int id);

        void Save();
    }
}
