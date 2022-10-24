using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EVENT_SHEDULE_PARENT")]
    public class EVENT_SHEDULE_PARENT : DomainEntity<Guid>, IDateTracking
    {
        public EVENT_SHEDULE_PARENT()
        {
        }

        public EVENT_SHEDULE_PARENT(Guid id, string title, string startevent, string endevent, string repeat, string content, string bophan,
            string timealert, Guid manoidungkh)
        {
            Id = id;
            Title = title;
            StartEvent = startevent;
            EndEvent = endevent;
            Repeat = repeat;
            Content = content;
            BoPhan = bophan;
            TimeAlert = timealert;
            MaNoiDungKH = manoidungkh;
        }

        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(50)]
        public string StartEvent { get; set; }
        [StringLength(50)]
        public string EndEvent { get; set; }
        [StringLength(50)]
        public string Repeat { get; set; } // chu ky thuc hien 1-M,1-Y
        [StringLength(1000)]
        public string Content { get; set; }
        [StringLength(50)]
        public string BoPhan { get; set; }
        [StringLength(50)]
        public string TimeAlert { get; set; }
        public Guid MaNoiDungKH { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<EVENT_SHEDULE> EVENT_SHEDULE { get; set; }
    }
}
