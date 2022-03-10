using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IDMucNgaylamviecService : IDisposable
    {
        DMucNgayLamViecViewModel Add(DMucNgayLamViecViewModel nhanVienLVVm);

        void Delete(string id);

        List<DMucNgayLamViecViewModel> GetAll();

        DMucNgayLamViecViewModel GetById(string id, params Expression<Func<DM_NGAY_LAMVIEC, object>>[] includeProperties);

        void Save();
    }
}
