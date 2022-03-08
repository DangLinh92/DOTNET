using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DM_CA_LVIEC")]
    public class DM_CA_LVIEC : DomainEntity<string>, IDateTracking
    {
        public DM_CA_LVIEC()
        {
            CA_LVIEC = new HashSet<CA_LVIEC>();
            NHANVIEN_CALAMVIEC = new HashSet<NHANVIEN_CALAMVIEC>();
            SETTING_TIME_DIMUON_VESOM = new HashSet<SETTING_TIME_DIMUON_VESOM>();
            SETTING_TIME_CA_LVIEC = new HashSet<SETTING_TIME_CA_LVIEC>();
        }

        [StringLength(100)]
        public string TenCaLamViec { get; set; }

        [StringLength(50)]
        public string MaTruSo { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<CA_LVIEC> CA_LVIEC { get; set; }

        [ForeignKey("MaTruSo")]
        public virtual TRU_SO_LVIEC TRU_SO_LVIEC { get; set; }

        public virtual ICollection<NHANVIEN_CALAMVIEC> NHANVIEN_CALAMVIEC { get; set; }

        public virtual ICollection<SETTING_TIME_DIMUON_VESOM> SETTING_TIME_DIMUON_VESOM { get; set; }

        public virtual ICollection<SETTING_TIME_CA_LVIEC> SETTING_TIME_CA_LVIEC { get; set; }
    }
}
