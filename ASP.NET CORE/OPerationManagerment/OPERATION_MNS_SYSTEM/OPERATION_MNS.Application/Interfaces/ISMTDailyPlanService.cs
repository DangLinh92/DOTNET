using OPERATION_MNS.Application.ViewModels.SMT;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ISMTDailyPlanService : IDisposable
    {
        List<DailyPlanSMTViewModel> GetDailyPlanSMT(string date);
        NextDay_SMT GetNexDaySMT(string date);
    }
}
