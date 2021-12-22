using HRMNS.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
