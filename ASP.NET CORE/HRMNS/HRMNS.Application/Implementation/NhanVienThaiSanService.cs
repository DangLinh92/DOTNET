using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
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
    public class NhanVienThaiSanService : BaseService, INhanVienThaiSanService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IRespository<HR_THAISAN_CONNHO, int> _thaisanRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienThaiSanService(IRespository<HR_THAISAN_CONNHO, int> thaisanRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _thaisanRepository = thaisanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public NhanVienThaiSanViewModel Add(NhanVienThaiSanViewModel nhanVienVm)
        {
            nhanVienVm.UserCreated = GetUserId();
            nhanVienVm.UserModified = GetUserId();

            var obj = _mapper.Map<HR_THAISAN_CONNHO>(nhanVienVm);
            _thaisanRepository.Add(obj);
            return nhanVienVm;
        }

        public void Delete(int id)
        {
            _thaisanRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVienThaiSanViewModel> GetAll()
        {
           var lst = _thaisanRepository.FindAll(x=>x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
        }

        public NhanVienThaiSanViewModel GetById(int id, params Expression<Func<HR_THAISAN_CONNHO, object>>[] includeProperties)
        {
           var ent = _thaisanRepository.FindById(id, includeProperties);
            return _mapper.Map<NhanVienThaiSanViewModel>(ent);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<NhanVienThaiSanViewModel> Search(string maNV, string begin, string end)
        {
            if(string.IsNullOrEmpty(maNV) && string.IsNullOrEmpty(begin) && string.IsNullOrEmpty(end))
            {
                var lst = _thaisanRepository.FindAll(x => x.HR_NHANVIEN).OrderByDescending(x=>x.DateModified);
                return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
            }

            if (!string.IsNullOrEmpty(maNV))
            {
                if(!string.IsNullOrEmpty(begin) && !string.IsNullOrEmpty(end))
                {
                    var lst = _thaisanRepository.FindAll(x => x.MaNV == maNV && ((begin.CompareTo(x.FromDate) <= 0 && end.CompareTo(x.FromDate) >= 0) || (begin.CompareTo(x.ToDate) <= 0 && end.CompareTo(x.ToDate) >= 0)), x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
                }
                else
                {
                    var lst = _thaisanRepository.FindAll(x => x.MaNV == maNV, x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
                }
            }
            else
            {
                var lst = _thaisanRepository.FindAll(x => ((begin.CompareTo(x.FromDate) <= 0 && end.CompareTo(x.FromDate) >= 0) || (begin.CompareTo(x.ToDate) <= 0 && end.CompareTo(x.ToDate) >= 0)),x=>x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
            }
        }

        public void Update(NhanVienThaiSanViewModel nhanVienVm)
        {
            nhanVienVm.UserModified = GetUserId();
            var entity = _mapper.Map<HR_THAISAN_CONNHO>(nhanVienVm);
            _thaisanRepository.Update(entity);
        }
    }
}
