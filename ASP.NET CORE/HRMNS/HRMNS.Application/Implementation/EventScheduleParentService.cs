using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EventScheduleParentService : BaseService, IEventScheduleParentService
    {
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _eventParentRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventScheduleParentService(IRespository<EVENT_SHEDULE_PARENT, Guid> eventParentRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _eventParentRepository = eventParentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
           return _mapper.Map<List<EventScheduleParentViewModel>>(_eventParentRepository.FindAll());
        }

        public EventScheduleParentViewModel GetEventById(Guid Id)
        {
            return _mapper.Map<EventScheduleParentViewModel>(_eventParentRepository.FindById(Id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
