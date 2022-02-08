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
    public class PhepNamService : BaseService, IPhepNamService
    {
        private IRespository<HR_PHEP_NAM, int> _phepNamRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhepNamService(IRespository<HR_PHEP_NAM, int> phepNamRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _phepNamRepository = phepNamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public PhepNamViewModel Add(PhepNamViewModel phepNamVm)
        {
            var phepNam = _mapper.Map<PhepNamViewModel, HR_PHEP_NAM>(phepNamVm);
            phepNam.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            phepNam.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            phepNam.UserCreated = GetUserId();
            _phepNamRepository.Add(phepNam);
            return phepNamVm;
        }

        public void Delete(int id)
        {
            _phepNamRepository.Remove(id);
        }

        public List<PhepNamViewModel> GetAll(string keyword)
        {
            return _mapper.ProjectTo<PhepNamViewModel>(_phepNamRepository.FindAll()).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(PhepNamViewModel phepNamVm)
        {
            var phepNam = _mapper.Map<PhepNamViewModel, HR_PHEP_NAM>(phepNamVm);
            phepNam.UserModified = GetUserId();
            _phepNamRepository.Update(phepNam);
        }

        public void UpdateSingle(PhepNamViewModel phepNamVm)
        {
            throw new NotImplementedException();
        }

        public PhepNamViewModel GetById(int id)
        {
            return _mapper.Map<HR_PHEP_NAM, PhepNamViewModel>(_phepNamRepository.FindById(id));
        }
    }
}
