using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NhanVienService : INhanVienService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienService(IRespository<HR_NHANVIEN, string> nhanVienRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _nhanvienRepository = nhanVienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public NhanVienViewModel Add(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            _nhanvienRepository.Add(nhanvien);
            return nhanVienVm;
        }

        public void Delete(string id)
        {
            _nhanvienRepository.Remove(id);
        }

        public List<NhanVienViewModel> GetAll()
        {
            return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll()).ToList();
        }

        public List<NhanVienViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.ProjectTo < NhanVienViewModel >(_nhanvienRepository.FindAll(x => x.TenNV.Contains(keyword))).ToList();
            else
                return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll()).ToList();
        }

        public NhanVienViewModel GetById(string id)
        {
            return _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NhanVienViewModel nhanVienVm)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
