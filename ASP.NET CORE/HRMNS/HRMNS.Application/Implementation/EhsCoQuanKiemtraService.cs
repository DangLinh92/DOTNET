using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EhsCoQuanKiemtraService : BaseService, IEhsCoQuanKiemtraService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_COQUAN_KIEMTRA, int> _coquanRepository;
        private IRespository<EHS_HANGMUC_NG, int> _hangMucRepository;
        private readonly IMapper _mapper;

        public EhsCoQuanKiemtraService(IRespository<EHS_HANGMUC_NG, int> hangMucRepository, IRespository<EHS_COQUAN_KIEMTRA, int> respository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _hangMucRepository = hangMucRepository;
            _unitOfWork = unitOfWork;
            _coquanRepository = respository;
            _mapper = mapper;
        }

        public EhsCoQuanKiemTraViewModel Add(EhsCoQuanKiemTraViewModel model)
        {
            var en = _mapper.Map<EHS_COQUAN_KIEMTRA>(model);
            _coquanRepository.Add(en);
            Save();
            return _mapper.Map<EhsCoQuanKiemTraViewModel>(en);
        }

        public void Delete(int Id)
        {
            _coquanRepository.Remove(Id);
        }

        public EhsCoQuanKiemTraViewModel GetById(int Id)
        {
            return _mapper.Map<EhsCoQuanKiemTraViewModel>(_coquanRepository.FindById(Id));
        }

        public List<EhsCoQuanKiemTraViewModel> GetList()
        {
            var lst = _coquanRepository.FindAll().OrderByDescending(x=>x.NgayKiemTra);
            return _mapper.Map<List<EhsCoQuanKiemTraViewModel>>(lst);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public EhsCoQuanKiemTraViewModel Update(EhsCoQuanKiemTraViewModel model)
        {
            var en = _mapper.Map<EHS_COQUAN_KIEMTRA>(model);
            _coquanRepository.Update(en);
            return _mapper.Map<EhsCoQuanKiemTraViewModel>(en);
        }

        public List<EhsCoQuanKiemTraViewModel> GetNG(string year)
        {
            var lst =_coquanRepository.FindAll(x => x.KetQua == "NG" && x.DateCreated.StartsWith(year));
            return _mapper.Map<List<EhsCoQuanKiemTraViewModel>>(lst);
        }
    }
}
