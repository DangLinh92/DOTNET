using VOC.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
