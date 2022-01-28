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
    public class BoPhanDetailService : IBoPhanDetailService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<HR_BO_PHAN_DETAIL, int> _boPhanDetailRepository;
        private readonly IMapper _mapper;

        public BoPhanDetailService(IRespository<HR_BO_PHAN_DETAIL, int> boPhanRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _boPhanDetailRepository = boPhanRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BoPhanDetailViewModel> GetAll(string filter)
        {
            var boPhan = _boPhanDetailRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
            {
                boPhan = boPhan.Where(x => x.TenBoPhanChiTiet.Contains(filter));
            }

            return _mapper.ProjectTo<BoPhanDetailViewModel>(boPhan).ToList();
        }
    }
}
