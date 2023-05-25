using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_BO_PHAN_DETAIL")]
    public class HR_BO_PHAN_DETAIL : DomainEntity<int>, IDateTracking
    {
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

        public virtual ICollection<HR_NHANVIEN> HR_NHANVIEN { get; set; }
    }
}
