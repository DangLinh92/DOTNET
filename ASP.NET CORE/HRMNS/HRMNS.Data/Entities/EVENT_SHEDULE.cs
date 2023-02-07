using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    //[Table("EVENT_SHEDULE")]
    //public class EVENT_SHEDULE : DomainEntity<int>, IDateTracking
    //{
    //    public EVENT_SHEDULE()
    //    {
    //    }

    //    public EVENT_SHEDULE(Guid maEventparent,string date)
    //    {
    //        MaEventParent = maEventparent;
    //        EventDate = date;
    //    }

    //    [StringLength(50)]
    //    public string EventDate { get; set; }

    //    public Guid MaEventParent { get; set; }

    //    [StringLength(50)]
    //    public string DateCreated { get; set; }

    //    [StringLength(50)]
    //    public string DateModified { get; set; }

    //    [StringLength(50)]
    //    public string UserCreated { get; set; }

    //    [StringLength(50)]
    //    public string UserModified { get; set; }

    //    [ForeignKey("MaEventParent")]
    //    public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    //}
}
