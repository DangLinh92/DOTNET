using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("CHUNG_CHI")]
    public class CHUNG_CHI : DomainEntity<string>, IDateTracking
    {
        public CHUNG_CHI()
        {
            HR_CHUNGCHI_NHANVIEN = new HashSet<HR_CHUNGCHI_NHANVIEN>();
        }

        [StringLength(250)]
        public string TenChungChi { get; set; }
        public int LoaiChungChi { get; set; }

        [ForeignKey("LoaiChungChi")]
        public virtual LOAICHUNGCHI LOAICHUNGCHI1 { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public virtual ICollection<HR_CHUNGCHI_NHANVIEN> HR_CHUNGCHI_NHANVIEN { get; set; }
    }
}
