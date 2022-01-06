using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class LoaiChungChiViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string TenLoaiChungChi { get; set; }

        public ICollection<ChungChiViewModel> CHUNG_CHI { get; set; }
    }
}
