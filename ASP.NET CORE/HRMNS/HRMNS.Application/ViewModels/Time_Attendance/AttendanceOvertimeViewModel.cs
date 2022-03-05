using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class AttendanceOvertimeViewModel
    {
        public int Id { get; set; }
        public int CaLviec { get; set; }

        public long MaAttendence { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public AttendanceRecordViewModel ATTENDANCE_RECORD { get; set; }

        public  CaLamViecViewModel CA_LVIEC { get; set; }
    }
}
