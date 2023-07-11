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

        [StringLength(150)]
        public string NgayDieuChinh { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? TongSoTien { get; set; }

        [StringLength(20)]
        public string TrangThaiChiTra { get; set; }

        public DateTime? ChiTraVaoLuongThang { get; set; }

        public string ChiTraVaoLuongThang2 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }

        public int STT { get; set; }
    }
}
