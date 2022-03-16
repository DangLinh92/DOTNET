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
    public interface IDM_DCChamCongService : IDisposable
    {
        List<DMDieuChinhChamCongViewModel> Search(string status,string dept,string timeFrom,string timeTo);

        DMDieuChinhChamCongViewModel Add(DMDieuChinhChamCongViewModel nhanVienLVVm);

        void Update(DMDieuChinhChamCongViewModel nhanVienLVVm);

        void UpdateSingle(DMDieuChinhChamCongViewModel nhanVienLVVm);

        void Delete(int id);

        List<DMDieuChinhChamCongViewModel> GetAll(string keyword, params Expression<Func<DM_DIEUCHINH_CHAMCONG, object>>[] includeProperties);

        DMDieuChinhChamCongViewModel GetById(int id, params Expression<Func<DM_DIEUCHINH_CHAMCONG, object>>[] includeProperties);

        void Save();
    }
}
