using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DMDangKyChamCongViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string TieuDe { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<DangKyChamCongChiTietViewModel> DANGKY_CHAMCONG_CHITIET { get; set; }
    }
}
