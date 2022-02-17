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
    public class KeKhaiBaoHiemService : BaseService, IKeKhaiBaoHiemService
    {
        private IRespository<HR_KEKHAIBAOHIEM, int> _kekhaiBHRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KeKhaiBaoHiemService(IRespository<HR_KEKHAIBAOHIEM, int> kekhaiBHRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _kekhaiBHRepository = kekhaiBHRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public KeKhaiBaoHiemViewModel Add(KeKhaiBaoHiemViewModel bhxhVm)
        {
            var kkBH = _mapper.Map<KeKhaiBaoHiemViewModel, HR_KEKHAIBAOHIEM>(bhxhVm);
            kkBH.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            kkBH.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            kkBH.UserCreated = GetUserId();
            _kekhaiBHRepository.Add(kkBH);
            return bhxhVm;
        }

        public void Delete(int id)
        {
            _kekhaiBHRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<KeKhaiBaoHiemViewModel> GetAll(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
                return _mapper.Map<List<KeKhaiBaoHiemViewModel>>(_kekhaiBHRepository.FindAll(x => x.MaNV.Contains(filter)));
            else
                return _mapper.Map<List<KeKhaiBaoHiemViewModel>>(_kekhaiBHRepository.FindAll());
        }

        public KeKhaiBaoHiemViewModel GetById(int id)
        {
            return _mapper.Map<KeKhaiBaoHiemViewModel>(_kekhaiBHRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(KeKhaiBaoHiemViewModel bhxhVm)
        {
            var nhanvien = _mapper.Map<KeKhaiBaoHiemViewModel, HR_KEKHAIBAOHIEM>(bhxhVm);
            nhanvien.UserModified = GetUserId();
            _kekhaiBHRepository.Update(nhanvien);
        }
    }
}
