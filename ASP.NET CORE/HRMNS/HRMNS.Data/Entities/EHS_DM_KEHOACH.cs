using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_DM_KEHOACH")]
    public class EHS_DM_KEHOACH : DomainEntity<Guid>, IDateTracking
    {
        public EHS_DM_KEHOACH()
        {
            EHS_LUATDINH_KEHOACH = new HashSet<EHS_LUATDINH_KEHOACH>();
            EHS_KEHOACH_QUANTRAC = new HashSet<EHS_KEHOACH_QUANTRAC>();
            EHS_KE_HOACH_KHAM_SK = new HashSet<EHS_KE_HOACH_KHAM_SK>();
            EHS_KEHOACH_DAOTAO_ANTOAN_VSLD = new HashSet<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD>();
            EHS_KEHOACH_PCCC = new HashSet<EHS_KEHOACH_PCCC>();
        }

        public EHS_DM_KEHOACH(Guid id,string tenKeHoachVn,string tenKeHoachKR,int order)
        {
            Id = id;
            TenKeHoach_VN = tenKeHoachVn;
            TenKeHoach_KR = tenKeHoachKR;
            OrderDM = order;
        }

        [StringLength(1000)]
        public string TenKeHoach_VN { get; set; }

        [StringLength(1000)]
        public string TenKeHoach_KR { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public int OrderDM { get; set; } 

        public virtual ICollection<EHS_LUATDINH_KEHOACH> EHS_LUATDINH_KEHOACH { get; set; }
        public virtual ICollection<EHS_KEHOACH_QUANTRAC> EHS_KEHOACH_QUANTRAC { get; set; }
        public virtual ICollection<EHS_KE_HOACH_KHAM_SK> EHS_KE_HOACH_KHAM_SK { get; set; }
        public virtual ICollection<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD> EHS_KEHOACH_DAOTAO_ANTOAN_VSLD { get; set; }
        public virtual ICollection<EHS_KEHOACH_PCCC> EHS_KEHOACH_PCCC { get; set; }
    }
}
