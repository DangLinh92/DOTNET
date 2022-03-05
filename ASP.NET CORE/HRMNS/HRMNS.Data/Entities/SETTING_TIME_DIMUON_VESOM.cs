using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("SETTING_TIME_DIMUON_VESOM")]
    public class SETTING_TIME_DIMUON_VESOM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string Time_LateCome { get; set; }

        [StringLength(50)]
        public string Time_EarlyLeave { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("Danhmuc_CaLviec")]
        public virtual DM_CA_LVIEC DM_CA_LVIEC { get; set; }
    }
}
