using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EventScheduleParentService : BaseService, IEventScheduleParentService
    {
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _eventParentRepository;
        private IRespository<EVENT_SHEDULE, Guid> _eventRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventScheduleParentService(IRespository<EVENT_SHEDULE_PARENT, Guid> eventParentRepository, IRespository<EVENT_SHEDULE, Guid> eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _eventParentRepository = eventParentRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public EventScheduleParentViewModel AddEventParent(EventScheduleParentViewModel even)
        {
            var evt = _mapper.Map<EVENT_SHEDULE_PARENT>(even);
            evt.UserCreated = GetUserId();
            _eventParentRepository.Add(evt);
            return even;
        }

        public void DeleteEvent(Guid Id)
        {
            var even = _eventParentRepository.FindById(Id);
            if(even != null)
            {
                _eventParentRepository.Remove(even);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EventScheduleParentViewModel EditEvent(EventScheduleParentViewModel even)
        {
            var oldEntity = _eventParentRepository.FindById(even.Id);
            oldEntity.CopyPropertiesFrom(even, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified" });
            oldEntity.UserModified = GetUserId();
            _eventParentRepository.Update(oldEntity);
            return even;
        }

        public List<EventScheduleParentViewModel> GetAllEvent()
        {
           return _mapper.Map<List<EventScheduleParentViewModel>>(_eventParentRepository.FindAll(x => x.EVENT_SHEDULE));
        }

        public EventScheduleParentViewModel GetEventById(Guid Id)
        {
            return _mapper.Map<EventScheduleParentViewModel>(_eventParentRepository.FindById(Id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<string> AddEvents(Guid maEvent, List<string> dates)
        {
            _eventRepository.RemoveMultiple(_eventRepository.FindAll(x => x.MaEventParent.Equals(maEvent)).ToList());

            EVENT_SHEDULE evnt = null;
            foreach (var item in dates)
            {
                evnt = new EVENT_SHEDULE(maEvent, item);
                _eventRepository.Add(evnt);
            }

            return dates;
        }
    }
}
