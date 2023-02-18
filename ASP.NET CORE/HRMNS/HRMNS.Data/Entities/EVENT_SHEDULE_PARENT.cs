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
            string timealert, Guid manoidungkh, DateTime? startTime, DateTime? endTime, 
            bool? isAllDay,string recurrenceRule,string startTimezone,string endTimezone,
            int? recurrenceID, string recurrenceException, int? conferenceId,string location,int roomId)
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
            RoomId = roomId;
        }

        [StringLength(250)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string StartEvent { get; set; }

        [StringLength(50)]
        public string EndEvent { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? StartTime { get; set; }

        [Column(TypeName = "datetime")]
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

        public int RoomId { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
