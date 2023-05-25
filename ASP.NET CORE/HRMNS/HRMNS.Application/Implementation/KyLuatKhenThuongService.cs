using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class KyLuatKhenThuongService : BaseService, IKyLuatKhenThuongService
    {
        private IRespository<HR_KY_LUAT_KHENTHUONG, int> _kyluatkhenthuongRepository;
        private IUnitOfWork _unitOfWork;

        public KyLuatKhenThuongService(IRespository<HR_KY_LUAT_KHENTHUONG, int> kyluatkhenthuongRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _kyluatkhenthuongRepository = kyluatkhenthuongRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public HR_KY_LUAT_KHENTHUONG Add(HR_KY_LUAT_KHENTHUONG nhanVienLVVm)
        {
            nhanVienLVVm.UserCreated = GetUserId();
            _kyluatkhenthuongRepository.Add(nhanVienLVVm);
            _unitOfWork.Commit();
            return nhanVienLVVm;
        }

        public void Delete(int id)
        {
            _kyluatkhenthuongRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public HR_KY_LUAT_KHENTHUONG FindById(int id)
        {
            return _kyluatkhenthuongRepository.FindById(id);
        }

        public List<HR_KY_LUAT_KHENTHUONG> GetAll()
        {
            return _kyluatkhenthuongRepository.FindAll(x => x.HR_NHANVIEN,x =>x.HR_NHANVIEN.HR_BO_PHAN_DETAIL).ToList();
        }

        public void Update(HR_KY_LUAT_KHENTHUONG nhanVienLVVm)
        {
            nhanVienLVVm.UserCreated = GetUserId();
            _kyluatkhenthuongRepository.Update(nhanVienLVVm);
            _unitOfWork.Commit();
        }
    }
}
