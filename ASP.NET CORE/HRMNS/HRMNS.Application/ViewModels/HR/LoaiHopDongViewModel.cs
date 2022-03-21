using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class LoaiHopDongViewModel
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string TenLoaiHD { get; set; }

        [StringLength(50)]
        public string ShortName { get; set; }

        public ICollection<HopDongViewModel> HR_HOPDONG { get; set; }
    }
}
