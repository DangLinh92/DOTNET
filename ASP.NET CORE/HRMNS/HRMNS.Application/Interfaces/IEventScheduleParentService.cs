using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEventScheduleParentService : IDisposable
    {
        List<EventScheduleParentViewModel> GetAllEvent();
        EventScheduleParentViewModel AddEventParent(EventScheduleParentViewModel ev);
        EventScheduleParentViewModel EditEvent(EventScheduleParentViewModel ev);
        void DeleteEvent(Guid Id);
        EventScheduleParentViewModel GetEventById(Guid Id);

        List<string> AddEvents(Guid maEvent,List<string> dates);

        void Save();
    }
}
