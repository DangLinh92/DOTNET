using OPERATION_MNS.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
