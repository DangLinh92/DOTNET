﻿using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IWaitimeService : IDisposable
    {
        List<OperationWaitimeSheet> GetWaitTime_WLP1();
    }
}
