using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class DMucCalamviecService : BaseService, IDMucCalamviecService
    {
        private IRespository<DM_CA_LVIEC, string> _dmCaLamViecRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DMucCalamviecService(IRespository<DM_CA_LVIEC, string> dmCalamviec, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dmCaLamViecRepository = dmCalamviec;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DMCalamviecViewModel Add(DMCalamviecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DMCalamviecViewModel> GetAll()
        {
           var lst = _dmCaLamViecRepository.FindAll();
           return _mapper.Map<List<DMCalamviecViewModel>>(lst);
        }

        public List<DMCalamviecViewModel> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public DMCalamviecViewModel GetById(string id, params Expression<Func<DMCalamviecViewModel, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public List<DMCalamviecViewModel> Search(string id, string name, string dept)
        {
            throw new NotImplementedException();
        }

        public void Update(DMCalamviecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateSingle(DMCalamviecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }
    }
}
