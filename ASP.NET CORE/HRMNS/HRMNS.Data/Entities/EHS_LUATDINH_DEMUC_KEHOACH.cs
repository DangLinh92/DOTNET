using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    //[Table("EHS_LUATDINH_DEMUC_KEHOACH")]
    //public class EHS_LUATDINH_DEMUC_KEHOACH : DomainEntity<int>, IDateTracking
    //{
    //    public EHS_LUATDINH_DEMUC_KEHOACH()
    //    {

    //    }

    //    public EHS_LUATDINH_DEMUC_KEHOACH(int id,string luatDinh,Guid maDemuc)
    //    {
    //        Id = id;
    //        LuatDinhLienQuan = luatDinh;
    //        MaDeMuc = maDemuc;
    //    }

    //    [StringLength(1000)]
    //    public string LuatDinhLienQuan { get; set; }

    //    public Guid MaDeMuc { get; set; }

    //    [StringLength(50)]
    //    public string DateCreated { get; set; }

    //    [StringLength(50)]
    //    public string DateModified { get; set; }

    //    [StringLength(50)]
    //    public string UserCreated { get; set; }

    //    [StringLength(50)]
    //    public string UserModified { get; set; }

    //    [ForeignKey("MaDeMuc")]
    //    public virtual EHS_DEMUC_KEHOACH EHS_DEMUC_KEHOACH { get; set; }
    //}
}
