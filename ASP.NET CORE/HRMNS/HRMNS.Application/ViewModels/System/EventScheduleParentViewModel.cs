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
             string timealert, Guid manoidungkh, DateTime? startTime, DateTime? endTime,
             bool? isAllDay, string recurrenceRule, string startTimezone, string endTimezone,
             int? recurrenceID, string recurrenceException, int? conferenceId,string location)
        {
            Id = id;
            Subject = title;
            StartEvent = startevent;
            EndEvent = endevent;
            Repeat = repeat;
            Description = content;
            BoPhan = bophan;
            TimeAlert = timealert;
            MaNoiDungKH = manoidungkh;
            StartTime = startTime;
            EndTime = endTime;
            IsAllDay = isAllDay;
            RecurrenceRule = recurrenceRule;
            StartTimezone = startTimezone;
            EndTimezone = endTimezone;
            RecurrenceID = recurrenceID;
            RecurrenceException = recurrenceException;
            ConferenceId = conferenceId;
            Location = location;
        }

        public Guid Id { get; set; }

        [StringLength(250)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string StartEvent { get; set; }

        [StringLength(50)]
        public string EndEvent { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string Repeat { get; set; } // chu ky thuc hien 1-M,1-Y

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string BoPhan { get; set; }

        [StringLength(50)]
        public string TimeAlert { get; set; }

        public Guid MaNoiDungKH { get; set; }

        public bool? IsAllDay { get; set; }

        [StringLength(250)]
        public string RecurrenceRule { get; set; }

        [StringLength(250)]
        public string StartTimezone { get; set; }

        [StringLength(250)]
        public string EndTimezone { get; set; }

        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public int? ConferenceId { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

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
