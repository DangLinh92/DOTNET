using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class ChungChiViewModel
    {
        public string Id { get; set; }

        [StringLength(250)]
        public string TenChungChi { get; set; }
        public int? LoaiChungChi { get; set; }

        public LoaiChungChiViewModel LOAICHUNGCHI1 { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public ICollection<ChungChiNhanVienViewModel> HR_CHUNGCHI_NHANVIEN { get; set; }
    }
}
