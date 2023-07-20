using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CarMNS.Data.Enums
{
    public enum ChamCongStatus
    {
        [Description("Normal")]
        Normal,

        [Description("Absence")]
        Absence,

        [Description("Miss In")]
        Miss_In,

        [Description("Miss Out")]
        Miss_Out,

        [Description("Non working")]
        NonWorking,

        [Description("Late In")]
        LateIn,

        [Description("Early Out")]
        Early_Out,

        [Description("In Working")]
        InWorking,
    }
}
