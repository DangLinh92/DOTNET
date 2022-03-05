using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DangKyChamCongChiTietViewModel
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string TenChiTiet { get; set; }

        public int? PhanLoaiDM { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        public DMDangKyChamCongViewModel DM_DANGKY_CHAMCONG { get; set; }

        public KyHieuChamCongViewModel KY_HIEU_CHAM_CONG { get; set; }

        public ICollection<DangKyChamCongDacBietViewModel> DANGKY_CHAMCONG_DACBIET { get; set; }

        public ICollection<DCChamCongViewModel> DC_CHAM_CONG { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
