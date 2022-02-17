using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class BHXHService : BaseService, IBHXHService
    {
        private IRespository<HR_BHXH, string> _bhxhRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BHXHService(IRespository<HR_BHXH, string> bhxhRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _bhxhRepository = bhxhRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public BHXHViewModel Add(BHXHViewModel bhxhVm)
        {
            var bhxh = _mapper.Map<BHXHViewModel, HR_BHXH>(bhxhVm);
            bhxh.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bhxh.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bhxh.UserCreated = GetUserId();
            _bhxhRepository.Add(bhxh);
            return bhxhVm;
        }

        public List<BHXHViewModel> GetAll(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
                return _mapper.Map<List<BHXHViewModel>>(_bhxhRepository.FindAll(x => x.Id.Contains(filter)));
            else
                return _mapper.Map<List<BHXHViewModel>>(_bhxhRepository.FindAll());
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BHXHViewModel bhxhVm)
        {
            var bhxh = _mapper.Map<BHXHViewModel, HR_BHXH>(bhxhVm);
            bhxh.UserModified = GetUserId();
            _bhxhRepository.Update(bhxh);
        }

        public BHXHViewModel GetById(string id)
        {
            return _mapper.Map<HR_BHXH, BHXHViewModel>(_bhxhRepository.FindById(id));
        }

        public void Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                _bhxhRepository.Remove(id);
        }

        public string GetUserLogin()
        {
            return GetUserId();
        }
    }
}
