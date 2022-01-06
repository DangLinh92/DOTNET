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

        public NhanVienService(IRespository<HR_NHANVIEN, string> nhanVienRepository, IUnitOfWork unitOfWork)
        {
            _nhanvienRepository = nhanVienRepository;
            _unitOfWork = unitOfWork;
        }

        public NhanVienViewModel Add(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = Mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            _nhanvienRepository.Add(nhanvien);
            return nhanVienVm;
        }

        public void Delete(string id)
        {
            _nhanvienRepository.Remove(id);
        }

        public List<NhanVienViewModel> GetAll()
        {
            return _nhanvienRepository.FindAll().ProjectTo<NhanVienViewModel>().ToList(); ;
        }

        public List<NhanVienViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _nhanvienRepository.FindAll(x => x.TenNV.Contains(keyword)).ProjectTo<NhanVienViewModel>().ToList();
            else
                return _nhanvienRepository.FindAll()
                    .ProjectTo<NhanVienViewModel>()
                    .ToList();
        }

        public NhanVienViewModel GetById(string id)
        {
            return Mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NhanVienViewModel nhanVienVm)
        {
            throw new NotImplementedException();
        }
    }
}
