using AutoMapper;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class DateOffLineService : IDateOffLineService
    {
        private IRespository<DATE_OFF_LINE, int> _dateOffLineRepository;
        private IRespository<SETTING_ITEMS, string> _settingItemRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DateOffLineService(IRespository<DATE_OFF_LINE, int> dateOffLineRepository, IRespository<SETTING_ITEMS, string> settingItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _dateOffLineRepository = dateOffLineRepository;
            _settingItemRepository = settingItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DateOffLineViewModel> GetDateOffLine()
        {
          return _mapper.Map<List<DateOffLineViewModel>>(_dateOffLineRepository.FindAll(x=>x.ON_OFF == "OFF"));
        }

        public int GetLeadTime()
        {
           return int.Parse(_settingItemRepository.FindById("LEAD_TIME_PLAN").ItemValue);
        }
    }
}
