using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IDMucCalamviecService : IDisposable
    {
        DMCalamviecViewModel Add(DMCalamviecViewModel nhanVienLVVm);

        void Update(DMCalamviecViewModel nhanVienLVVm);

        void UpdateSingle(DMCalamviecViewModel nhanVienLVVm);

        void Delete(string id);

        List<DMCalamviecViewModel> GetAll();

        List<DMCalamviecViewModel> GetAll(string keyword);

        DMCalamviecViewModel GetById(string id, params Expression<Func<DM_CA_LVIEC, object>>[] includeProperties);

        List<DMCalamviecViewModel> Search(string id, string name, string dept);

        ResultDB ImportExcel(string filePath, string param);

        void Save();
    }
}
