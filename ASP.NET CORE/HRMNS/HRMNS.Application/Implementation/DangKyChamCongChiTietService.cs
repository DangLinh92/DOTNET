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
    public class DangKyChamCongChiTietService : BaseService, IDangKyChamCongChiTietService
    {
        private IRespository<DANGKY_CHAMCONG_CHITIET, int> _chamCongChiTietRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DangKyChamCongChiTietService(IRespository<DANGKY_CHAMCONG_CHITIET, int> chamCongChiTietRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _chamCongChiTietRepository = chamCongChiTietRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DangKyChamCongChiTietViewModel Add(DangKyChamCongChiTietViewModel chamCongVm)
        {
            chamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_CHITIET>(chamCongVm);
            _chamCongChiTietRepository.Add(entity);
            return chamCongVm;
        }

        public void Delete(int id)
        {
            _chamCongChiTietRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DangKyChamCongChiTietViewModel> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _mapper.Map<List<DangKyChamCongChiTietViewModel>>(_chamCongChiTietRepository.FindAll());
            }
            else
            {
                return _mapper.Map<List<DangKyChamCongChiTietViewModel>>(_chamCongChiTietRepository.FindAll(x=>x.TenChiTiet.Contains(keyword)));
            }
        }

        public List<DangKyChamCongChiTietViewModel> GetAll(params Expression<Func<DANGKY_CHAMCONG_CHITIET, object>>[] includeProperties)
        {
            var lst = _chamCongChiTietRepository.FindAll(x => x.Id > 0, includeProperties);
            return _mapper.Map<List<DangKyChamCongChiTietViewModel>>(lst);
        }

        public DangKyChamCongChiTietViewModel GetById(int id)
        {
            return _mapper.Map<DangKyChamCongChiTietViewModel>(_chamCongChiTietRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DangKyChamCongChiTietViewModel chamCongVm)
        {
            chamCongVm.UserModified = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_CHITIET>(chamCongVm);
            _chamCongChiTietRepository.Update(entity);
        }
    }
}
