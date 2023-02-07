using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class DanhMucKeHoachService : BaseService, IDanhMucKeHoachService
    {
        private IRespository<EHS_DM_KEHOACH, Guid> _ehsDanhMucKHRespository;
        private IRespository<EHS_LUATDINH_KEHOACH, int> _ehsLuatDinhKHRespository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _eventScheduleParentRepository;

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhMucKeHoachService(
            IRespository<EHS_DM_KEHOACH, Guid> ehsDanhMucKHRespository,
            IRespository<EHS_LUATDINH_KEHOACH, int> ehsLuatDinhKHRespository,         
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _ehsDanhMucKHRespository = ehsDanhMucKHRespository;
            _ehsLuatDinhKHRespository = ehsLuatDinhKHRespository;         
            _eventScheduleParentRepository = eventScheduleParentRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public EhsDanhMucKeHoachPageViewModel GetDataDanhMucKeHoachPage(Guid? maKeHoach)
        {
            EhsDanhMucKeHoachPageViewModel model = new EhsDanhMucKeHoachPageViewModel();
            model.EhsDMKeHoachViewModels = GetDMKehoach();

            if (maKeHoach == null)
            {
                maKeHoach = model.EhsDMKeHoachViewModels.FirstOrDefault()?.Id;
            }

            model.MaKeHoachActive = maKeHoach;
            return model;
        }

        public EhsLuatDinhKeHoachViewModel DeleteLuatDinhKeHoach(int id)
        {
            throw new NotImplementedException();
        }

      
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<EhsDMKeHoachViewModel> GetDMKehoach()
        {
            return _mapper.Map<List<EhsDMKeHoachViewModel>>(_ehsDanhMucKHRespository.FindAll(x => x.EHS_LUATDINH_KEHOACH));
        }

        public List<EhsLuatDinhDeMucKeHoachViewModel> GetLuatDinhDeMucKeHoach()
        {
            throw new NotImplementedException();
        }

        public List<EhsLuatDinhKeHoachViewModel> GetLuatDinhKeHoach(Guid? maKeHoach)
        {
            return _mapper.Map<List<EhsLuatDinhKeHoachViewModel>>(_ehsLuatDinhKHRespository.FindAll(x => maKeHoach.Equals(x.MaKeHoach)));
        }

        public EhsDMKeHoachViewModel UpdateDMKeHoach(EhsDMKeHoachViewModel kehoach)
        {
            var khEn = _ehsDanhMucKHRespository.FindById(kehoach.Id);
            var en = _mapper.Map<EHS_DM_KEHOACH>(kehoach);
            if (khEn == null)
            {
                _ehsDanhMucKHRespository.Add(en);
            }
            else
            {
                _ehsDanhMucKHRespository.Update(en);
            }
            return kehoach;
        }

        public EhsLuatDinhKeHoachViewModel UpdateLuatDinhKeHoach(EhsLuatDinhKeHoachViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Guid DeleteDMKeHoach(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
