using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsNoiDungViewModel
    {
        public EhsNoiDungViewModel()
        {
            EHS_NOIDUNG_KEHOACH = new HashSet<EhsNoiDungKeHoachViewModel>();
        }
        public Guid Id { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        public Guid MaKeHoach { get; set; }

        public Guid MaDeMucKH { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsDMKeHoachViewModel EHS_DM_KEHOACH { get; set; }

        public EhsDeMucKeHoachViewModel EHS_DEMUC_KEHOACH { get; set; }

        public ICollection<EhsNoiDungKeHoachViewModel> EHS_NOIDUNG_KEHOACH { get; set; }
    }
}
