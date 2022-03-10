using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class NgayDacBietModel
    {
        public string Id { get; set; }

        [StringLength(150)]
        public string TenNgayDacBiet { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public KyHieuChamCongViewModel KY_HIEU_CHAM_CONG { get; set; }
    }
}
