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
    public class CheDoBHService : ICheDoBHService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<HR_CHEDOBH, string> _cheDoBHRepository;
        private readonly IMapper _mapper;

        public CheDoBHService(IRespository<HR_CHEDOBH, string> cheDoBHRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cheDoBHRepository = cheDoBHRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<CheDoBaoHiemViewModel> GetAll(string filter)
        {
            var cheDoBH = _cheDoBHRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
            {
                cheDoBH = cheDoBH.Where(x => x.TenCheDo.Contains(filter));
            }

            return _mapper.ProjectTo<CheDoBaoHiemViewModel>(cheDoBH).ToList();
        }
    }
}
