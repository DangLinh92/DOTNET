using CarMNS.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
