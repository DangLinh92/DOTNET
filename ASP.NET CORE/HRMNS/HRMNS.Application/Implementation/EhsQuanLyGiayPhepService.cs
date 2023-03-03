using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EhsQuanLyGiayPhepService : BaseService,IEhsQuanLyGiayPhepService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_QUANLY_GIAY_PHEP, int> _quanlygiayphepRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private IRespository<EHS_HANGMUC_NG, int> _hangMucRepository;
        private readonly IMapper _mapper;

        public EhsQuanLyGiayPhepService(IRespository<EVENT_SHEDULE_PARENT, Guid>  eventScheduleParentRepository, IHttpContextAccessor httpContextAccessor,
            IRespository<EHS_QUANLY_GIAY_PHEP, int> quanlygiayphepRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _quanlygiayphepRepository = quanlygiayphepRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public EhsQuanLyGiayPhepViewModel Add(EhsQuanLyGiayPhepViewModel model)
        {
            var en = _mapper.Map<EHS_QUANLY_GIAY_PHEP>(model);
            en.MaEvent = AddNewEvent(Guid.Empty.ToString(), en.NoiDung, en.ThoiGianThucHien, en.ThoiGianThucHien, en.NguoiThucHien, "");
            _quanlygiayphepRepository.Add(en);
            Save();
            return _mapper.Map<EhsQuanLyGiayPhepViewModel>(en);
        }

        public void Delete(int Id)
        {
            _quanlygiayphepRepository.Remove(Id);
        }

        public EhsQuanLyGiayPhepViewModel GetById(int Id)
        {
            var en = _quanlygiayphepRepository.FindById(Id);
            return _mapper.Map<EhsQuanLyGiayPhepViewModel>(en);
        }

        public List<EhsQuanLyGiayPhepViewModel> GetList()
        {
           var lst = _quanlygiayphepRepository.FindAll().OrderByDescending(x=>x.ThoiGianThucHien);
            return _mapper.Map<List<EhsQuanLyGiayPhepViewModel>>(lst);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public EhsQuanLyGiayPhepViewModel Update(EhsQuanLyGiayPhepViewModel model)
        {
            var en = _mapper.Map<EHS_QUANLY_GIAY_PHEP>(model);
            en.MaEvent = AddNewEvent(en.MaEvent.ToString(), en.NoiDung, en.ThoiGianThucHien, en.ThoiGianThucHien, en.NguoiThucHien, "");
            _quanlygiayphepRepository.Update(en);
            return _mapper.Map<EhsQuanLyGiayPhepViewModel>(en);
        }

        private Guid AddNewEvent(string evenId, string noidung, string beginTime, string endTime, string nguoiPhuTrach, string nhaThau)
        {
            Guid Id = Guid.Parse(evenId);

            var even = _EventScheduleParentRepository.FindById(Id);

            if (even != null)
            {
                even.Subject = noidung;
                even.StartEvent = beginTime;
                even.EndEvent = endTime;
                even.StartTime = DateTime.Parse(beginTime);
                even.EndTime = DateTime.Parse(endTime).AddDays(1); // thêm 1 ngày vì thời gian sẽ là 00:00:00, thời gian tính đến hết 23:59:59 ngày hôm trước
                even.IsAllDay = true;
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Noi Dung: " + noidung;
                even.UserModified = GetUserId();
                _EventScheduleParentRepository.Update(even);
            }
            else
            {
                evenId = Guid.NewGuid().ToString();

                even = new EVENT_SHEDULE_PARENT();
                even.Id = Guid.Parse(evenId);
                even.Subject = noidung;
                even.StartEvent = beginTime;
                even.EndEvent = endTime;
                even.StartTime = DateTime.Parse(beginTime);
                even.EndTime = DateTime.Parse(endTime).AddDays(1);
                even.IsAllDay = true;
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Noi Dung: " + noidung;
                even.UserCreated = GetUserId();
                _EventScheduleParentRepository.Add(even);
            }
            return Guid.Parse(evenId);
        }
    }
}
