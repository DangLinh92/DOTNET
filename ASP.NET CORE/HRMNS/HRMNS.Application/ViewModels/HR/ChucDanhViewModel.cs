using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class ChucDanhViewModel
    {
        public string Id { get; set; }

        [StringLength(50)]
        public string TenChucDanh { get; set; }

        public double PhuCap { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<NhanVienViewModel> HR_NHANVIEN { get; set; }
    }
}
