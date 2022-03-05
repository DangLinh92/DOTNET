using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class CaLamViecViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string DM_NgayLViec { get; set; }

        [StringLength(100)]
        public string TenCa { get; set; }

        [StringLength(50)]
        public string Time_BatDau { get; set; }

        [StringLength(50)]
        public string Time_BatDau2 { get; set; }

        [StringLength(50)]
        public string Time_KetThuc { get; set; }

        [StringLength(50)]
        public string Time_KetThuc2 { get; set; }

        public float HeSo_OT { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public DMCalamviecViewModel DM_CA_LVIEC { get; set; }

        public DMucNgayLamViecViewModel DM_NGAY_LAMVIEC { get; set; }

        public ICollection<AttendanceOvertimeViewModel> ATTENDANCE_OVERTIME { get; set; }
    }
}
