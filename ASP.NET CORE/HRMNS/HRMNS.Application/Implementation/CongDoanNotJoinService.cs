using AutoMapper;
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
    public class CongDoanNotJoinService :BaseService, ICongDoanNotJoinService
    {
        private IUnitOfWork _unitOfWork;
        private IRespository<CONGDOAN_NOT_JOIN, int> _congDoanRepository;
        private readonly IMapper _mapper;

        public CongDoanNotJoinService(IUnitOfWork unitOfWork, IRespository<CONGDOAN_NOT_JOIN, int> congDoanRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _congDoanRepository = congDoanRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public CONGDOAN_NOT_JOIN Add(CONGDOAN_NOT_JOIN item)
        {
            item.UserCreated = GetUserId();
            _congDoanRepository.Add(item);
            _unitOfWork.Commit();
            return item;

        }

        public void Delete(int id)
        {
            _congDoanRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<CONGDOAN_NOT_JOIN> GetAll()
        {
            return _congDoanRepository.FindAll(x=>x.HR_NHANVIEN,z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL).ToList();
        }
    }
}
