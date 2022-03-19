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
            WorkingStatuses = new List<WorkingStatus>();
            EL_LC_Statuses = new List<EL_LC_Status>();
            OvertimeValues = new List<OvertimeValue>();
        }

        public string MaNV { get; set; }

        public string TenNV { get; set; }

        public string NgayVao { get; set; }

        public string BoPhan { get; set; }

        public string BoPhanDetail { get; set; }

        public int LoaiHD { get; set; }
        public string TenHD { get; set; }
        public string NgayHieuLucHD { get; set; }
        public string NgayHetHLHD { get; set; }

        public string month_Check { get; set; }

        public List<string> lstDanhMucOT { get; set; } // 150%,200%,....

        public List<WorkingStatus> WorkingStatuses { get; set; }
        public List<EL_LC_Status> EL_LC_Statuses { get; set; }
        public List<OvertimeValue> OvertimeValues { get; set; }
    }

    public class WorkingStatus
    {
        public string DayCheck { get; set; }
        public string Value { get; set; } // ky hieu cham cong
    }

    public class EL_LC_Status
    {
        public string DayCheck_EL { get; set; }
        public string Value_EL { get; set; } // ky hieu cham cong
    }

    public class OvertimeValue
    {
        public string DMOvertime { get; set; }
        public string DayCheckOT { get; set; }
        public string ValueOT { get; set; }
    }
}
