using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DMucNgayLamViecViewModel
    {
        public string Id { get; set; }

        [StringLength(100)]
        public string Ten_NgayLV { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<CaLamViecViewModel> CA_LVIEC { get; set; }

        public ICollection<DangKyOTNhanVienViewModel> DANGKY_OT_NHANVIEN { get; set; }
    }
}
