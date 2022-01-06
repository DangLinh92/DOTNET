using HRMNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class BoPhanViewModel
    {
        public string Id { get; set; }
        public string TenBoPhan { get; set; }

        public ICollection<NhanVienViewModel> HR_NHANVIEN { get; set; }
    }
}
