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

        public string DM_DieuChinhCong { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_From { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_To { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? GiaTriBoXung { get; set; }

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
