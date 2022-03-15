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
    public class DangKyChamCongDacBietService : BaseService, IDangKyChamCongDacBietService
    {
        private IRespository<DANGKY_CHAMCONG_DACBIET, int> _chamCongDbRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DangKyChamCongDacBietService(IRespository<DANGKY_CHAMCONG_DACBIET, int> chamCongDbRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _chamCongDbRepository = chamCongDbRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DangKyChamCongDacBietViewModel Add(DangKyChamCongDacBietViewModel chamCongVm)
        {
            chamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_DACBIET>(chamCongVm);
            _chamCongDbRepository.Add(entity);
            return chamCongVm;
        }

        public void Delete(int id)
        {
            _chamCongDbRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DangKyChamCongDacBietViewModel> GetAll(string keyword)
        {
            return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll().OrderByDescending(x => x.DateModified));
        }

        public List<DangKyChamCongDacBietViewModel> GetAll(params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties)
        {
            return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.Id > 0, includeProperties).OrderByDescending(x => x.DateModified));
        }

        public DangKyChamCongDacBietViewModel GetById(int id)
        {
            return _mapper.Map<DangKyChamCongDacBietViewModel>(_chamCongDbRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DangKyChamCongDacBietViewModel chamCongVm)
        {
            chamCongVm.UserModified = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_DACBIET>(chamCongVm);
            _chamCongDbRepository.Update(entity);
        }

        public List<DangKyChamCongDacBietViewModel> Search(string dept, string fromDate, string toDate, params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(dept))
            {
                if(string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    return GetAll(includeProperties);
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => string.Compare(x.NgayBatDau, fromDate) >= 0 && string.Compare(x.NgayKetThuc, toDate) <= 0, includeProperties).OrderByDescending(x => x.DateModified));
            }
            else
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept, includeProperties).OrderByDescending(x => x.DateModified));
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && string.Compare(x.NgayBatDau, fromDate) >= 0 && string.Compare(x.NgayKetThuc, toDate) <= 0, includeProperties).OrderByDescending(x => x.DateModified));
            }
        }

        public DangKyChamCongDacBietViewModel GetSingle(Expression<Func<DANGKY_CHAMCONG_DACBIET, bool>> predicate)
        {
            return _mapper.Map<DangKyChamCongDacBietViewModel>(_chamCongDbRepository.FindSingle(predicate));
        }
    }
}
