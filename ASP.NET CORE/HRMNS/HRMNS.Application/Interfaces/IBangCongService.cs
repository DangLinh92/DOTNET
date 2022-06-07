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
        AttendanceRecordViewModel Add(AttendanceRecordViewModel viewModel);

        void Update(AttendanceRecordViewModel viewModel);

        List<AttendanceRecordViewModel> GetAll(params Expression<Func<ATTENDANCE_RECORD, object>>[] includeProperties);

        AttendanceOvertimeViewModel AddOverTime(AttendanceOvertimeViewModel attendance);

        void UpdateOverTime(AttendanceOvertimeViewModel attendance);

        List<AttendanceOvertimeViewModel> GetAllAttendanceOT(params Expression<Func<ATTENDANCE_OVERTIME, object>>[] includeProperties);

        List<ChamCongDataViewModel> GetDataReport(string time,string dept);
    }
}
