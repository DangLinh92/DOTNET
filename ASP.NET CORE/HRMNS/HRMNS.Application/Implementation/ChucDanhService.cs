using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Implementation
{
    public class ChucDanhService : IChucDanhService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<HR_CHUCDANH, string> _chucDanhRepository;
        private readonly IMapper _mapper;

        public ChucDanhService(IRespository<HR_CHUCDANH, string> chucDanhRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _chucDanhRepository = chucDanhRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ChucDanhViewModel> GetAll(string filter)
        {
            var chucDanh = _chucDanhRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
            {
                chucDanh = chucDanh.Where(x => x.TenChucDanh.Contains(filter));
            }

            return _mapper.ProjectTo<ChucDanhViewModel>(chucDanh).ToList();

        }
    }
}
