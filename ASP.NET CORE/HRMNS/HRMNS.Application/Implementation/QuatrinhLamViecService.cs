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
    public class QuatrinhLamViecService : BaseService, IQuatrinhLamViecService
    {
        private IRespository<HR_QUATRINHLAMVIEC, int> _quatrinhLviecRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuatrinhLamViecService(IRespository<HR_QUATRINHLAMVIEC, int> qtrinhLvRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _quatrinhLviecRepository = qtrinhLvRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public QuaTrinhLamViecViewModel Add(QuaTrinhLamViecViewModel qtrinhLviecVm)
        {
            var qtrinhLv = _mapper.Map<QuaTrinhLamViecViewModel, HR_QUATRINHLAMVIEC>(qtrinhLviecVm);
            qtrinhLv.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            qtrinhLv.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            qtrinhLv.UserCreated = GetUserId();
            _quatrinhLviecRepository.Add(qtrinhLv);
            return qtrinhLviecVm;
        }

        public void Delete(int id)
        {
            _quatrinhLviecRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<QuaTrinhLamViecViewModel> GetAll()
        {
            var er = _quatrinhLviecRepository.FindAll().OrderBy(x => x.ThơiGianBatDau).ToList();
            return (List<QuaTrinhLamViecViewModel>)_mapper.Map(er, typeof(List<HR_QUATRINHLAMVIEC>), typeof(List<QuaTrinhLamViecViewModel>));
        }

        public List<QuaTrinhLamViecViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return (List<QuaTrinhLamViecViewModel>)_mapper.Map(_quatrinhLviecRepository.FindAll(x => x.MaNV.Contains(keyword)).OrderBy(x => x.ThơiGianBatDau).ToList(), typeof(List<HR_QUATRINHLAMVIEC>), typeof(List<QuaTrinhLamViecViewModel>));
            else
                return (List<QuaTrinhLamViecViewModel>)_mapper.Map(_quatrinhLviecRepository.FindAll().OrderBy(x => x.ThơiGianBatDau).ToList(), typeof(List<HR_QUATRINHLAMVIEC>), typeof(List<QuaTrinhLamViecViewModel>));
        }

        public QuaTrinhLamViecViewModel GetById(int id)
        {
            return _mapper.Map<HR_QUATRINHLAMVIEC, QuaTrinhLamViecViewModel>(_quatrinhLviecRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(QuaTrinhLamViecViewModel qtrinhLviecVm)
        {
            var qtrinhLv = _mapper.Map<QuaTrinhLamViecViewModel, HR_QUATRINHLAMVIEC>(qtrinhLviecVm);
            qtrinhLv.UserModified = GetUserId();
            _quatrinhLviecRepository.Update(qtrinhLv);
        }
    }
}
