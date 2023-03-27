using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DM_DIEUCHINH_CHAMCONG")]
    public class DM_DIEUCHINH_CHAMCONG : DomainEntity<int>, IDateTracking
    {
        public DM_DIEUCHINH_CHAMCONG(int id,string tieude)
        {
            Id = id;
            TieuDe = tieude;
        }

        public DM_DIEUCHINH_CHAMCONG()
        {
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
    }
}
