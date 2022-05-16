using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ISettingTimeCalamviecService : IDisposable
    {
        SettingTimeCalamviecViewModel Add(SettingTimeCalamviecViewModel settingTimeVm);

        void Update(SettingTimeCalamviecViewModel settingTimeVm);

        void UpdateSingle(SettingTimeCalamviecViewModel settingTimeVm);

        void Delete(int id);

        List<SettingTimeCalamviecViewModel> GetAll();

        List<SettingTimeCalamviecViewModel> GetAll(string keyword, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties);

        SettingTimeCalamviecViewModel GetById(int id, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties);

        SettingTimeCalamviecViewModel GetByStatus(string status, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties);

        SettingTimeCalamviecViewModel GetByCaLamViecAndStatus(string status,string caLamViec, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties);

        void Save();
    }
}
