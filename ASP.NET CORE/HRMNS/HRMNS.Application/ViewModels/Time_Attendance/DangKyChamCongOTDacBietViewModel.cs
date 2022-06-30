using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DangKyChamCongOTDacBietViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        public int? MaChamCong_ChiTiet { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        public double HourInDay { get; set; } // so giơ OT trong 1ngày

        [StringLength(300)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string Approve { get; set; }

        [StringLength(50)]
        public string ApproveLV2 { get; set; }

        [StringLength(50)]
        public string ApproveLV3 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public DANGKY_CHAMCONG_CHITIET DANGKY_CHAMCONG_CHITIET { get; set; }

        public HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
