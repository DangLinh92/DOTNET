using HRMNS.Application.ViewModels.Time_Attendance;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.System
{
    public class ChamCongDataViewModel
    {
        public ChamCongDataViewModel()
        {
            
        }
        public string MaNV { get; set; }

        public string TenNV { get; set; }

        public string NgayVao { get; set; }

        public string BoPhan { get; set; }

        public string Time_Check { get; set; }

        public string DayOfWeek { get; set; }

        public string Working_Status_Tile { get; set; }

        public string OT_Tile { get; set; }

        public string EL_LC_Tile { get; set; }

        public string Working_Status { get; set; }

        public double? EL_LC { get; set; }

       
    }

    public class OvertimeAttendance
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
