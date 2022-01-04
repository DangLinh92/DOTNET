using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("BOPHAN")]
    public class BOPHAN : DomainEntity<string>
    {
        public BOPHAN()
        {
            HR_NHANVIEN = new HashSet<HR_NHANVIEN>();
        }

        [StringLength(500)]
        public string TenBoPhan { get; set; }

        public virtual ICollection<HR_NHANVIEN> HR_NHANVIEN { get; set; }
    }
}
