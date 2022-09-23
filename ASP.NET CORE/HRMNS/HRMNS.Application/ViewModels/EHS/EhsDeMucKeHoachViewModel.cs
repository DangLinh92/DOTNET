using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsDeMucKeHoachViewModel
    {
        public Guid Id { get; set; }

        [StringLength(1000)]
        public string TenKeDeMuc_VN { get; set; }

        [StringLength(1000)]
        public string TenKeDeMuc_KR { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<EhsLuatDinhDeMucKeHoachViewModel> EHS_LUATDINH_DEMUC_KEHOACH { get; set; }
        public virtual ICollection<EhsNoiDungViewModel> EHS_NOIDUNG { get; set; }
    }
}
