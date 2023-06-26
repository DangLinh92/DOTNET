using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("SETTING_TIME_CA_LVIEC")]
    public class SETTING_TIME_CA_LVIEC : DomainEntity<int>, IDateTracking
    {
        public SETTING_TIME_CA_LVIEC()
        {
        }

        [StringLength(50)]
        public string StartWorking { get; set; }

        [StringLength(50)]
        public string EndWorking { get; set; }

        [StringLength(50)]
        public string BeginTimeOT { get; set; }

        [StringLength(50)]
        public string CaLamViec { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string BoPhan { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("CaLamViec")]
        public virtual DM_CA_LVIEC DM_CA_LVIEC { get; set; }
    }
}
