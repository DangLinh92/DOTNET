using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DCChamCongViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        public DateTime? NgayDieuChinh { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? TongSoTien { get; set; }

        [StringLength(20)]
        public string TrangThaiChiTra { get; set; }

        [StringLength(50)]
        public string ChiTraVaoLuongThang { get; set; }

        public string NgayDieuChinh2 { get; set; }

        public float? NgayCong { get; set; }
        public float? DSNS { get; set; } // 30%
        public float? NSBH { get; set; } // 260%

        public float? DC85 { get; set; }
        public float? DC150 { get; set; }
        public float? DC190 { get; set; }
        public float? DC200 { get; set; }
        public float? DC210 { get; set; }
        public float? DC270 { get; set; }
        public float? DC300 { get; set; }
        public float? DC390 { get; set; }
        public float? HT50 { get; set; }
        public float? HT100 { get; set; }
        public float? HT150 { get; set; }
        public float? HT200 { get; set; }
        public float? HT390 { get; set; }
        public float? ELLC { get; set; }
        public float? Other { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
