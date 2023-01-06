using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IDateOffLineService : IDisposable
    {
        List<DateOffLineViewModel> GetDateOffLine();
        int GetLeadTime();
    }
}
