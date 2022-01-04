using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("LOAICHUNGCHI")]
    public class LOAICHUNGCHI : DomainEntity<int>
    {
        public LOAICHUNGCHI()
        {
            CHUNG_CHI = new HashSet<CHUNG_CHI>();
        }

        [StringLength(250)]
        public string TenLoaiChungChi { get; set; }

        public virtual ICollection<CHUNG_CHI> CHUNG_CHI { get; set; }
    }
}
