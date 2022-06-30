using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IBangCongService : IDisposable
    {
        List<ChamCongDataViewModel> GetDataReport(string time,string dept);
        List<TongHopNhanSuDailyViewModel> TongHopNhanSuReport(string time, string dept);
    }
}
