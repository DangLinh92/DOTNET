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
    public class BoPhanService : IBoPhanService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<BOPHAN, string> _boPhanRepository;
        private readonly IMapper _mapper;

        public BoPhanService(IRespository<BOPHAN, string> boPhanRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _boPhanRepository = boPhanRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BoPhanViewModel> GetAll(string filter)
        {
            var boPhan = _boPhanRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
            {
                boPhan = boPhan.Where(x => x.TenBoPhan.Contains(filter));
            }

            return _mapper.ProjectTo<BoPhanViewModel>(boPhan).ToList();

        }
    }
}
