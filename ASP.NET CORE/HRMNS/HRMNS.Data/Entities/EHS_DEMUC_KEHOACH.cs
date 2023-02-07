using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    //[Table("EHS_DEMUC_KEHOACH")]
    //public class EHS_DEMUC_KEHOACH : DomainEntity<Guid>, IDateTracking
    //{
    //    public EHS_DEMUC_KEHOACH()
    //    {
    //        EHS_LUATDINH_DEMUC_KEHOACH = new HashSet<EHS_LUATDINH_DEMUC_KEHOACH>();
    //        EHS_NOIDUNG = new HashSet<EHS_NOIDUNG>();
    //    }

    //    public EHS_DEMUC_KEHOACH(Guid id,string tenDemucVN,string tenDemucKR)
    //    {
    //        Id = id;
    //        TenKeDeMuc_VN = tenDemucVN;
    //        TenKeDeMuc_KR = tenDemucKR;
    //    }

    //    [StringLength(1000)]
    //    public string TenKeDeMuc_VN { get; set; }

    //    [StringLength(1000)]
    //    public string TenKeDeMuc_KR { get; set; }

    //    [StringLength(50)]
    //    public string DateCreated { get; set; }

    //    [StringLength(50)]
    //    public string DateModified { get; set; }

    //    [StringLength(50)]
    //    public string UserCreated { get; set; }

    //    [StringLength(50)]
    //    public string UserModified { get; set; }

    //    public virtual ICollection<EHS_LUATDINH_DEMUC_KEHOACH> EHS_LUATDINH_DEMUC_KEHOACH { get; set; }
    //    public virtual ICollection<EHS_NOIDUNG> EHS_NOIDUNG { get; set; }
    //}
}
