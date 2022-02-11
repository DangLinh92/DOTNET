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
    public class HopDongService : BaseService, IHopDongService
    {
        private IRespository<HR_HOPDONG, int> _hopDongRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HopDongService(IRespository<HR_HOPDONG, int> hopDongRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _hopDongRepository = hopDongRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public HopDongViewModel Add(HopDongViewModel hopDongVm)
        {
            var hopDong = _mapper.Map<HopDongViewModel, HR_HOPDONG>(hopDongVm);
            hopDong.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hopDong.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hopDong.UserCreated = GetUserId();
            _hopDongRepository.Add(hopDong);
            return hopDongVm;
        }

        public void Delete(int id)
        {
            _hopDongRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<HopDongViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public HopDongViewModel GetById(int id)
        {
            return _mapper.Map<HR_HOPDONG, HopDongViewModel>(_hopDongRepository.FindById(id));
        }

        public HopDongViewModel GetByMaHD(string maHd)
        {
            return _mapper.Map<HR_HOPDONG, HopDongViewModel>(((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().HrHopDong.FirstOrDefault(x=>x.MaHD == maHd));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(HopDongViewModel hopDongVm)
        {
            var hopDong = _mapper.Map<HopDongViewModel, HR_HOPDONG>(hopDongVm);
            hopDong.UserModified = GetUserId();
            _hopDongRepository.Update(hopDong);
        }

        public void UpdateSingle(HopDongViewModel hopDongVm)
        {
            throw new NotImplementedException();
        }
    }
}
