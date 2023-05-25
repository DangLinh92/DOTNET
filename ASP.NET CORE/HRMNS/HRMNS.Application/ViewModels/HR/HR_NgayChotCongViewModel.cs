using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class HR_NgayChotCongViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string NgayChotCong { get; set; }

        [StringLength(50)]
        public string ChotCongChoThang { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
