using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMNS.Data.Entities;

namespace HRMNS.Application.ViewModels.HR
{
    public class BoPhanDetailViewModel
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string TenBoPhanChiTiet { get; set; }
        [StringLength(50)]
        public string MaBoPhan { get; set; }

        [StringLength(50)]
        public string MaBoPhan_TOP2 { get; set; }

        [StringLength(50)]
        public string MaBoPhan_TOP1 { get; set; }
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<HR_NHANVIEN> HR_NHANVIEN { get; set; }
    }
}
