using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class CheDoBaoHiemViewModel
    {
        public string Id { get; set; }

        [StringLength(250)]
        public string TenCheDo { get; set; }

        public ICollection<KeKhaiBaoHiemViewModel> HR_KEKHAIBAOHIEM { get; set; }
    }
}
