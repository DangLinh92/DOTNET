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
    public class TinhTrangHoSoService : BaseService, ITinhTrangHoSoService
    {
        private IRespository<HR_TINHTRANGHOSO, int> _ttHSoRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TinhTrangHoSoService(IRespository<HR_TINHTRANGHOSO, int> ttHsoRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _ttHSoRepository = ttHsoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public TinhTrangHoSoViewModel Add(TinhTrangHoSoViewModel ttHsoVm)
        {
            var tthso = _mapper.Map<TinhTrangHoSoViewModel, HR_TINHTRANGHOSO>(ttHsoVm);
            tthso.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tthso.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tthso.UserCreated = GetUserId();
            _ttHSoRepository.Add(tthso);
            return ttHsoVm;
        }

        public void Delete(int id)
        {
            _ttHSoRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<TinhTrangHoSoViewModel> GetAll()
        {
            return (List<TinhTrangHoSoViewModel>)_mapper.Map(_ttHSoRepository.FindAll().ToList(), typeof(List<HR_TINHTRANGHOSO>), typeof(List<TinhTrangHoSoViewModel>));
        }

        public TinhTrangHoSoViewModel GetById(int id)
        {
            return _mapper.Map<HR_TINHTRANGHOSO, TinhTrangHoSoViewModel>(_ttHSoRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(TinhTrangHoSoViewModel ttHsoVm)
        {
            var tthso = _mapper.Map<TinhTrangHoSoViewModel, HR_TINHTRANGHOSO>(ttHsoVm);
            tthso.UserModified = GetUserId();
            _ttHSoRepository.Update(tthso);
        }

        public TinhTrangHoSoViewModel GetByMaNV(string id)
        {
            return _mapper.Map<HR_TINHTRANGHOSO, TinhTrangHoSoViewModel>(_ttHSoRepository.FindSingle(x=>x.MaNV == id));
        }
    }
}
