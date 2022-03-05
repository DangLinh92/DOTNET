using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class AttendanceRecordViewModel
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string Time_Check { get; set; }

        [StringLength(20)]
        public string Working_Status { get; set; }

        public double? EL_LC { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<AttendanceOvertimeViewModel> ATTENDANCE_OVERTIME { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }

        public KyHieuChamCongViewModel KY_HIEU_CHAM_CONG { get; set; }
    }
}
