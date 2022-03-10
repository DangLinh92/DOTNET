using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class DMucNgaylamviecService : BaseService, IDMucNgaylamviecService
    {
        private IRespository<DM_NGAY_LAMVIEC, string> _dmNgayLamViecRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DMucNgaylamviecService(IRespository<DM_NGAY_LAMVIEC, string> respository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dmNgayLamViecRepository = respository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DMucNgayLamViecViewModel Add(DMucNgayLamViecViewModel ngayLVVm)
        {
            ngayLVVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DM_NGAY_LAMVIEC>(ngayLVVm);
            _dmNgayLamViecRepository.Add(entity);
            return ngayLVVm;
        }

        public void Delete(string id)
        {
            _dmNgayLamViecRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DMucNgayLamViecViewModel> GetAll()
        {
            return _mapper.Map<List<DMucNgayLamViecViewModel>>(_dmNgayLamViecRepository.FindAll());
        }

        public DMucNgayLamViecViewModel GetById(string id, params Expression<Func<DM_NGAY_LAMVIEC, object>>[] includeProperties)
        {
            return _mapper.Map< DMucNgayLamViecViewModel >(_dmNgayLamViecRepository.FindById(id, includeProperties));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
