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
    public class DCChamCongService : BaseService, IDCChamCongService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<DC_CHAM_CONG, int> _dcChamCongRepository;
        private readonly IMapper _mapper;

        public DCChamCongService(IRespository<DC_CHAM_CONG, int> respository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _dcChamCongRepository = respository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public DCChamCongViewModel Add(DCChamCongViewModel dmDcChamCongVm)
        {
            dmDcChamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DC_CHAM_CONG>(dmDcChamCongVm);
            _dcChamCongRepository.Add(entity);
            return dmDcChamCongVm;
        }

        public void Delete(int id)
        {
            _dcChamCongRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DCChamCongViewModel> GetAll(string keyword, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.Id > 0, includeProperties).OrderBy(x => x.DateModified));
            }
            else
            {
                return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.NoiDungDC.Contains(keyword), includeProperties).OrderBy(x => x.DateModified));
            }
        }

        public DCChamCongViewModel GetById(int id, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            return _mapper.Map<DCChamCongViewModel>(_dcChamCongRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<DCChamCongViewModel> Search(string status, string dept, string timeFrom, string timeTo, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(dept))
            {
                if (string.IsNullOrEmpty(status))
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return GetAll("", includeProperties);
                    }
                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x =>  string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(status))
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x =>x.HR_NHANVIEN.MaBoPhan == dept, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
            }
        }

        public void Update(DCChamCongViewModel dmDCChamCongVm)
        {
            dmDCChamCongVm.UserModified = GetUserId();
            var entity = _mapper.Map<DC_CHAM_CONG>(dmDCChamCongVm);
            _dcChamCongRepository.Update(entity);
        }
    }
}
