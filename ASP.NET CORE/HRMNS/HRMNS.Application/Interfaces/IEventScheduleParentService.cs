﻿using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEventScheduleParentService : IDisposable
    {
        List<EventScheduleParentViewModel> GetAllEvent();
        List<EventScheduleParentViewModel> GetAllEventWithTime(DateTime? begin,DateTime? end);
        EventScheduleParentViewModel AddEventParent(EventScheduleParentViewModel ev);
        EventScheduleParentViewModel EditEvent(EventScheduleParentViewModel ev);
        void DeleteEvent(Guid Id);
        EventScheduleParentViewModel GetEventById(Guid Id);

        void Save();
    }
}
