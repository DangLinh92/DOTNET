using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("TRU_SO_LVIEC")]
    public class TRU_SO_LVIEC : DomainEntity<string>, IDateTracking
    {
        public TRU_SO_LVIEC()
        {
            DM_CA_LVIEC = new HashSet<DM_CA_LVIEC>();
        }

        [StringLength(250)]
        public string TenTruSo { get; set; }

        [StringLength(250)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<DM_CA_LVIEC> DM_CA_LVIEC { get; set; }
    }
}
