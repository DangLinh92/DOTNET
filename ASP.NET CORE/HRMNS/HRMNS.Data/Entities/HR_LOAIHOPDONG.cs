using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_LOAIHOPDONG")]
    public class HR_LOAIHOPDONG : DomainEntity<int>
    {
        public HR_LOAIHOPDONG()
        {
            HR_HOPDONG = new HashSet<HR_HOPDONG>();
        }

        [StringLength(500)]
        public string TenLoaiHD { get; set; }

        [StringLength(50)]
        public string ShortName { get; set; }

        public virtual ICollection<HR_HOPDONG> HR_HOPDONG { get; set; }
    }
}
