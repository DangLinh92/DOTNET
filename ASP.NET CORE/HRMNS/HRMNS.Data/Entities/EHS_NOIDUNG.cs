using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    //[Table("EHS_NOIDUNG")]
    //public class EHS_NOIDUNG : DomainEntity<Guid>, IDateTracking
    //{
    //    public EHS_NOIDUNG()
    //    {
    //        EHS_NOIDUNG_KEHOACH = new HashSet<EHS_NOIDUNG_KEHOACH>();
    //        EHS_CHIPHI_BY_MONTH = new HashSet<EHS_CHIPHI_BY_MONTH>();
    //    }

    //    public EHS_NOIDUNG(Guid id,string noidung,Guid maKeHoach,Guid maDeMucKH)
    //    {
    //        Id = id;
    //        NoiDung = noidung;
    //        MaKeHoach = maKeHoach;
    //        MaDeMucKH = maDeMucKH;
    //    }

    //    [StringLength(1000)]
    //    public string NoiDung { get; set; }

    //    public Guid MaKeHoach { get; set; }

    //    public Guid MaDeMucKH { get; set; }

    //    [StringLength(50)]
    //    public string DateCreated { get; set; }

    //    [StringLength(50)]
    //    public string DateModified { get; set; }

    //    [StringLength(50)]
    //    public string UserCreated { get; set; }

    //    [StringLength(50)]
    //    public string UserModified { get; set; }


    //    [ForeignKey("MaKeHoach")]
    //    public virtual EHS_DM_KEHOACH EHS_DM_KEHOACH { get; set; }

    //    [ForeignKey("MaDeMucKH")]
    //    public virtual EHS_DEMUC_KEHOACH EHS_DEMUC_KEHOACH { get; set; }

    //    public virtual ICollection<EHS_NOIDUNG_KEHOACH> EHS_NOIDUNG_KEHOACH { get; set; }
    //    public virtual ICollection<EHS_CHIPHI_BY_MONTH> EHS_CHIPHI_BY_MONTH { get; set; }
    //}
}
