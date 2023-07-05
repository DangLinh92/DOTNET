using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class PhuCapLuongService : BaseService, IPhuCapLuongService
    {
        private IRespository<PHUCAP_DOC_HAI, int> _phucapDocHaiRepository;
        private IRespository<HR_SALARY_GRADE, string> _gradeRepository;
        private IRespository<BOPHAN, string> _bophanRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhuCapLuongService(IRespository<PHUCAP_DOC_HAI, int> phucapDocHaiRepository, IRespository<BOPHAN, string> bophanRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRespository<HR_SALARY_GRADE, string> gradeRepository)
        {
            _phucapDocHaiRepository = phucapDocHaiRepository;
            _bophanRepository = bophanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _gradeRepository = gradeRepository;
        }

        public PHUCAP_DOC_HAI AddDH(PHUCAP_DOC_HAI en)
        {
            en.UserCreated = GetUserId();
            _phucapDocHaiRepository.Add(en);
            _unitOfWork.Commit();
            return en;
        }

        public void DeleteDH(int id)
        {
            _phucapDocHaiRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<PHUCAP_DOC_HAI> GetAll()
        {
            return _phucapDocHaiRepository.FindAll().ToList();
        }

        public PHUCAP_DOC_HAI GetAllById(int Id)
        {
            return _phucapDocHaiRepository.FindById(Id);
        }

        public PHUCAP_DOC_HAI UpdateDH(PHUCAP_DOC_HAI en)
        {
            en.UserCreated = GetUserId();
            _phucapDocHaiRepository.Update(en);
            _unitOfWork.Commit();
            return en;
        }

        public List<BOPHAN> GetBoPhanAll()
        {
            return _bophanRepository.FindAll().ToList();
        }

        public List<HR_SALARY_GRADE> GetAllGrade()
        {
            return _gradeRepository.FindAll().ToList();
        }

        public HR_SALARY_GRADE AddGrade(HR_SALARY_GRADE en)
        {
            en.UserCreated = GetUserId();
            _gradeRepository.Add(en);
            _unitOfWork.Commit();
            return en;
        }

        public HR_SALARY_GRADE UpdateGrade(HR_SALARY_GRADE en)
        {
            en.UserCreated = GetUserId();
            _gradeRepository.Update(en);
            _unitOfWork.Commit();
            return en;
        }

        public void DeleteGrade(string Id)
        {
            _gradeRepository.Remove(Id);
            _unitOfWork.Commit();
        }

        public HR_SALARY_GRADE GetGradeById(string Id)
        {
            return _gradeRepository.FindById(Id); 
        }
    }
}
