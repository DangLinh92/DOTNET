using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DangKyOTNhanVienViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string NgayOT { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string DM_NgayLViec { get; set; }

        public double SoGioOT { get; set; }

        [StringLength(50)]
        public string HeSoOT { get; set; }

        [StringLength(250)]
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

        [ForeignKey("DM_NgayLViec")]
        public DMucNgayLamViecViewModel DM_NGAY_LAMVIEC { get; set; }

        [ForeignKey("MaNV")]
        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
