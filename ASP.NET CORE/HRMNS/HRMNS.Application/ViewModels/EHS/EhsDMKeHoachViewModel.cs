using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsDMKeHoachViewModel
    {
        public EhsDMKeHoachViewModel()
        {
            EHS_LUATDINH_KEHOACH = new HashSet<EhsLuatDinhKeHoachViewModel>();
            EHS_NOIDUNG = new HashSet<EhsNoiDungViewModel>();
        }

        public Guid Id { get; set; }

        [StringLength(1000)]
        public string TenKeHoach_VN { get; set; }

        [StringLength(1000)]
        public string TenKeHoach_KR { get; set; }

        public int OrderDM { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<EhsLuatDinhKeHoachViewModel> EHS_LUATDINH_KEHOACH { get; set; }
        public ICollection<EhsNoiDungViewModel> EHS_NOIDUNG { get; set; }
    }
}
