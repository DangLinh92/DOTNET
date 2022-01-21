using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Data.IRepositories;
using HRMNS.Utilities.Constants;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NhanVienService : BaseService,INhanVienService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienService(IRespository<HR_NHANVIEN, string> nhanVienRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienRepository = nhanVienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public NhanVienViewModel Add(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            nhanvien.Status = Status.Active.ToString();
            nhanvien.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            nhanvien.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            nhanvien.UserCreated = GetUserId();
            nhanvien.IsDelete = "N";
            _nhanvienRepository.Add(nhanvien);
            return nhanVienVm;
        }

        public void Delete(string id)
        {
            _nhanvienRepository.Remove(id);
        }

        public List<NhanVienViewModel> GetAll()
        {
            return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.IsDelete != CommonConstants.IsDelete)).OrderByDescending(x=>x.DateModified).ToList();
        }

        public List<NhanVienViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(keyword) || x.Id.Contains(keyword))).ToList();
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
            try
            {
                var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
                _nhanvienRepository.Update(nhanvien);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVienViewModel> Search(string id, string name, string dept)
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(dept))
            {
                return GetAll();
            }

            List<NhanVienViewModel> lstNV = new List<NhanVienViewModel>();

            if (!string.IsNullOrEmpty(id))
            {
                NhanVienViewModel nv = _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
                if (nv != null)
                {
                    lstNV.Add(nv);
                    return lstNV;
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                lstNV = _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name))).ToList();
            }
            else
            if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(name))
            {
                lstNV = _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name) && x.MaBoPhan.Contains(dept))).ToList();
            }

            return lstNV;
        }
    }
}
