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
    public class HRLoaiHopDongService : BaseService, IHRLoaiHopDongService
    {
        private IRespository<HR_LOAIHOPDONG, int> _loaiHopDongRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HRLoaiHopDongService(IRespository<HR_LOAIHOPDONG, int> loaiHDRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _loaiHopDongRepository = loaiHDRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<LoaiHopDongViewModel> GetAll()
        {
            var er = _loaiHopDongRepository.FindAll().ToList();
            return (List<LoaiHopDongViewModel>)_mapper.Map(er, typeof(List<HR_LOAIHOPDONG>), typeof(List<LoaiHopDongViewModel>));
        }
    }
}
