using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ILeadTimeService : IDisposable
    {
        List<LeadTimeViewModel> GetLeadTime(string year,string month,string week,string day,string ox);
        List<LeadTimeViewModel> GetLeadTimeLFEM(string year,string month,string week,string day,string ox,string model);

        double GetTargetWLP(string year);
    }
}
