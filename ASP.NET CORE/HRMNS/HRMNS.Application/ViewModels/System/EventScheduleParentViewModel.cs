using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.System
{
   public class EventScheduleParentViewModel
    {
        public EventScheduleParentViewModel()
        {

        }
        public EventScheduleParentViewModel(Guid id, string title, string startevent, string endevent, string repeat, string content, string bophan,
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

        public Guid Id { get; set; }

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

        public ICollection<EventSheduleViewModel> EVENT_SHEDULE { get; set; }
    }

    public class EventSheduleViewModel
    {
        public EventSheduleViewModel()
        {

        }
        public EventSheduleViewModel(Guid maEventparent, string date)
        {
            MaEventParent = maEventparent;
            EventDate = date;
        }

        [StringLength(50)]
        public string EventDate { get; set; }

        public Guid MaEventParent { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EventScheduleParentViewModel EVENT_SHEDULE_PARENT { get; set; }
    }
}
