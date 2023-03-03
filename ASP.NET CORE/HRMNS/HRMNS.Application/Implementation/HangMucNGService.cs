using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class HangMucNGService : BaseService, IHangMucNGService
    {
        private IRespository<EHS_HANGMUC_NG, int> _hangMucRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HangMucNGService(IRespository<EHS_HANGMUC_NG, int> eventParentRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _hangMucRepository = eventParentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<EhsHangMucNGViewModel> GetAll()
        {
            var lst = _hangMucRepository.FindAll();
            return _mapper.Map<List<EhsHangMucNGViewModel>>(lst);
        }

        public EhsHangMucNGViewModel Add(EhsHangMucNGViewModel item)
        {
            EHS_HANGMUC_NG en = _mapper.Map<EHS_HANGMUC_NG>(item);
            _hangMucRepository.Add(en);
            Save();
            return _mapper.Map<EhsHangMucNGViewModel>(en);
        }

        public EhsHangMucNGViewModel Update(EhsHangMucNGViewModel item)
        {
            EHS_HANGMUC_NG en = _mapper.Map<EHS_HANGMUC_NG>(item);
            _hangMucRepository.Update(en);
            return _mapper.Map<EhsHangMucNGViewModel>(en);
        }

        public void Delete(int id)
        {
            _hangMucRepository.Remove(id);
        }

        public List<EhsHangMucNGViewModel> GetByKey(string key)
        {
           var lst = _hangMucRepository.FindAll(x => x.MaNgayChiTiet == key);
            return _mapper.Map<List<EhsHangMucNGViewModel>>(lst);
        }

        public EhsHangMucNGViewModel GetById(int id)
        {
            var vm = _hangMucRepository.FindById(id);
            return _mapper.Map<EhsHangMucNGViewModel>(vm);
        }

        public List<EhsHangMucNGViewModel> GetByYear(string year)
        {
           var lst = _hangMucRepository.FindAll(x => x.DateCreated.StartsWith(year)).OrderByDescending(x=>x.DateCreated);
            return _mapper.Map<List<EhsHangMucNGViewModel>>(lst);
        }
    }
}
