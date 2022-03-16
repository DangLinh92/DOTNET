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
    public class DM_DCChamCongService : BaseService, IDM_DCChamCongService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<DM_DIEUCHINH_CHAMCONG, int> _dmDCChamCongRepository;
        private readonly IMapper _mapper;

        public DM_DCChamCongService(IRespository<DM_DIEUCHINH_CHAMCONG, int> respository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _dmDCChamCongRepository = respository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public DMDieuChinhChamCongViewModel Add(DMDieuChinhChamCongViewModel dmDcChamCongVm)
        {
            dmDcChamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DM_DIEUCHINH_CHAMCONG>(dmDcChamCongVm);
            _dmDCChamCongRepository.Add(entity);
            return dmDcChamCongVm;
        }

        public void Delete(int id)
        {
            _dmDCChamCongRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DMDieuChinhChamCongViewModel> GetAll(string keyword, params Expression<Func<DM_DIEUCHINH_CHAMCONG, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _mapper.Map<List<DMDieuChinhChamCongViewModel>>(_dmDCChamCongRepository.FindAll(x => x.Id > 0, includeProperties).OrderBy(x=>x.DateModified));
            }
            else
            {
                return _mapper.Map<List<DMDieuChinhChamCongViewModel>>(_dmDCChamCongRepository.FindAll(x => x.TieuDe.Contains(keyword), includeProperties).OrderBy(x => x.DateModified));
            }
        }

        public DMDieuChinhChamCongViewModel GetById(int id, params Expression<Func<DM_DIEUCHINH_CHAMCONG, object>>[] includeProperties)
        {
            return _mapper.Map<DMDieuChinhChamCongViewModel>(_dmDCChamCongRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<DMDieuChinhChamCongViewModel> Search(string status, string dept, string timeFrom, string timeTo)
        {
            throw new NotImplementedException();
        }

        public void Update(DMDieuChinhChamCongViewModel dmDCChamCongVm)
        {
            dmDCChamCongVm.UserModified = GetUserId();
            var entity = _mapper.Map<DM_DIEUCHINH_CHAMCONG>(dmDCChamCongVm);
            _dmDCChamCongRepository.Update(entity);
        }

        public void UpdateSingle(DMDieuChinhChamCongViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }
    }
}
