using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class SettingTimeCalamviecService : BaseService, ISettingTimeCalamviecService
    {
        private IRespository<SETTING_TIME_CA_LVIEC, int> _settingTimeClviecRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SettingTimeCalamviecService()
        {

        }

        public SettingTimeCalamviecService(IRespository<SETTING_TIME_CA_LVIEC, int> respository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _settingTimeClviecRepository = respository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public SettingTimeCalamviecViewModel Add(SettingTimeCalamviecViewModel settingTimeVm)
        {
            settingTimeVm.UserCreated = GetUserId();
            var objSetting = _mapper.Map<SETTING_TIME_CA_LVIEC>(settingTimeVm);
            _settingTimeClviecRepository.Add(objSetting);
            return settingTimeVm;
        }

        public void Delete(int id)
        {
            _settingTimeClviecRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SettingTimeCalamviecViewModel> GetAll()
        {
            var lst = _settingTimeClviecRepository.FindAll().OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<SettingTimeCalamviecViewModel>>(lst);
        }

        public List<SettingTimeCalamviecViewModel> GetAll(string keyword, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties)
        {
            var lst = _settingTimeClviecRepository.FindAll(x => x.Id > 0, includeProperties).OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<SettingTimeCalamviecViewModel>>(lst);
        }

        public SettingTimeCalamviecViewModel GetById(int id, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties)
        {
            return _mapper.Map<SettingTimeCalamviecViewModel>(_settingTimeClviecRepository.FindById(id, includeProperties));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SettingTimeCalamviecViewModel settingTimeVm)
        {
            settingTimeVm.UserModified = GetUserId();
            var entity = _mapper.Map<SETTING_TIME_CA_LVIEC>(settingTimeVm);
            _settingTimeClviecRepository.Update(entity);
        }

        public void UpdateSingle(SettingTimeCalamviecViewModel settingTimeVm)
        {
            throw new NotImplementedException();
        }

        public SettingTimeCalamviecViewModel GetByStatus(string status, params Expression<Func<SETTING_TIME_CA_LVIEC, object>>[] includeProperties)
        {
            var obj = _settingTimeClviecRepository.FindAll(x => x.Status == status, includeProperties).OrderByDescending(x => x.DateModified).FirstOrDefault();
            return _mapper.Map<SettingTimeCalamviecViewModel>(obj);
        }
    }
}
