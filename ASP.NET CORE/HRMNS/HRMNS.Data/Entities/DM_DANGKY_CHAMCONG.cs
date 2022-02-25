using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DM_DANGKY_CHAMCONG")]
    public class DM_DANGKY_CHAMCONG: DomainEntity<int>, IDateTracking
    {
        public DM_DANGKY_CHAMCONG()
        {
            DANGKY_CHAMCONG_CHITIET = new HashSet<DANGKY_CHAMCONG_CHITIET>();
        }

        [StringLength(250)]
        public string TieuDe { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<DANGKY_CHAMCONG_CHITIET> DANGKY_CHAMCONG_CHITIET { get; set; }
    }
}
