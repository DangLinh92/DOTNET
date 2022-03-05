using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class KyHieuChamCongViewModel
    {
        public string Id { get; set; }

        [StringLength(300)]
        public string GiaiThich { get; set; }

        public double? Heso { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<DangKyChamCongChiTietViewModel> DANGKY_CHAMCONG_CHITIET { get; set; }

        public ICollection<NgayLeNamViewModel> NGAY_LE_NAM { get; set; }

        public ICollection<NgayNghiBuLeNamViewModel> NGAY_NGHI_BU_LE_NAM { get; set; }
        public ICollection<AttendanceRecordViewModel> ATTENDANCE_RECORD { get; set; }
    }
}
