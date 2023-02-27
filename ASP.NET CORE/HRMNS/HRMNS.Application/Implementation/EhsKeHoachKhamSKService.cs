using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EhsKeHoachKhamSKService : BaseService, IEhsKeHoachKhamSKService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KE_HOACH_KHAM_SK, Guid> _EHSKeHoachKhamSKRepository;
        IRespository<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, int> _EHSNgayThucHienKhamSKRepository;
        IRespository<EHS_NHANVIEN_KHAM_SK, int> _EHNhanVienKhamSKRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachKhamSKService(IUnitOfWork unitOfWork, IMapper mapper, IRespository<EHS_KE_HOACH_KHAM_SK, Guid> eHSKeHoachKhamSKRepository,
            IRespository<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, int> eHSNgayThucHienKhamSKRepository, IRespository<EHS_NHANVIEN_KHAM_SK, int> eHNhanVienKhamSKRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _EHSKeHoachKhamSKRepository = eHSKeHoachKhamSKRepository;
            _EHSNgayThucHienKhamSKRepository = eHSNgayThucHienKhamSKRepository;
            _EHNhanVienKhamSKRepository = eHNhanVienKhamSKRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<EhsKeHoachKhamSKViewModel> GetList(string year)
        {
           return _mapper.Map<List<EhsKeHoachKhamSKViewModel>>(_EHSKeHoachKhamSKRepository.FindAll(x => x.Year == year, x => x.EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, y => y.EHS_NHANVIEN_KHAM_SK));
        }

        public EhsKeHoachKhamSKViewModel GetById(Guid Id)
        {
           return _mapper.Map<EhsKeHoachKhamSKViewModel>(_EHSKeHoachKhamSKRepository.FindById(Id,x=>x.EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK,y => y.EHS_NHANVIEN_KHAM_SK));
        }

        public EhsKeHoachKhamSKViewModel Add(EhsKeHoachKhamSKViewModel model)
        {
            model.MaDMKeHoach = Guid.Parse("ffe65d73-1066-4f1b-af5b-0c0e33d494dd");
            EHS_KE_HOACH_KHAM_SK en = _mapper.Map<EHS_KE_HOACH_KHAM_SK>(model);
            _EHSKeHoachKhamSKRepository.Add(en);
            return _mapper.Map<EhsKeHoachKhamSKViewModel>(en);
        }

        public EhsKeHoachKhamSKViewModel Update(EhsKeHoachKhamSKViewModel model)
        {
            _EHSKeHoachKhamSKRepository.Update(_mapper.Map<EHS_KE_HOACH_KHAM_SK>(model));
            return model;
        }

        public void Delete(Guid Id)
        {
            _EHSKeHoachKhamSKRepository.Remove(Id);
        }

        public EhsNgayThucHienChiTietKhamSKViewModel AddNgayKhamSK(EhsNgayThucHienChiTietKhamSKViewModel model)
        {
            EhsKeHoachKhamSKViewModel quantrac = GetById(model.MaKHKhamSK);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK en = _mapper.Map<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK>(model);
            _EHSNgayThucHienKhamSKRepository.Add(en);
            return _mapper.Map<EhsNgayThucHienChiTietKhamSKViewModel>(en);
        }

        public EhsNgayThucHienChiTietKhamSKViewModel UpdateNgayKhamSK(EhsNgayThucHienChiTietKhamSKViewModel model)
        {
            EhsKeHoachKhamSKViewModel quantrac = GetById(model.MaKHKhamSK);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            _EHSNgayThucHienKhamSKRepository.Update(_mapper.Map<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK>(model));
            return model;
        }

        public EhsNgayThucHienChiTietKhamSKViewModel GetNgayKhamSKById(int Id)
        {
            return _mapper.Map<EhsNgayThucHienChiTietKhamSKViewModel>(_EHSNgayThucHienKhamSKRepository.FindById(Id));
        }

        public void DeleteNgayKhamSK(int Id)
        {
            _EHSNgayThucHienKhamSKRepository.Remove(Id);
        }

        public EhsNhanVienKhamSucKhoe AddNhanVienKhamSK(EhsNhanVienKhamSucKhoe model)
        {
            EHS_NHANVIEN_KHAM_SK en = _mapper.Map<EHS_NHANVIEN_KHAM_SK>(model);
            _EHNhanVienKhamSKRepository.Add(en);
            return _mapper.Map<EhsNhanVienKhamSucKhoe>(en);
        }

        public EhsNhanVienKhamSucKhoe UpdateNhanVienKhamSK(EhsNhanVienKhamSucKhoe model)
        {
            _EHNhanVienKhamSKRepository.Update(_mapper.Map<EHS_NHANVIEN_KHAM_SK>(model));
            return model;
        }

        public void DeleteNhanVienKhamSK(int Id)
        {
            _EHNhanVienKhamSKRepository.Remove(Id);
        }

        public List<EhsNhanVienKhamSucKhoe> GetNhanVienKhamSKByKeHoach(Guid maKHKhamSK)
        {
           return _mapper.Map<List<EhsNhanVienKhamSucKhoe>>(_EHNhanVienKhamSKRepository.FindAll(x=>x.MaKHKhamSK.Equals(maKHKhamSK)));
        }

        /// <summary>
        /// Import nhân viên khám sức khỏe
        /// </summary>
        /// <param name="filePath"></param>
        public void ImportExcel(string filePath,string maKHKhamSK)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                EHS_NHANVIEN_KHAM_SK nhanvienKhamSK;

                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    nhanvienKhamSK = new EHS_NHANVIEN_KHAM_SK();
                    nhanvienKhamSK.MaKHKhamSK = Guid.Parse(maKHKhamSK);
                    nhanvienKhamSK.MaNV = worksheet.Cells[i, 1].Text.NullString();
                    nhanvienKhamSK.TenNV = worksheet.Cells[i, 2].Text.NullString();
                    nhanvienKhamSK.Section = worksheet.Cells[i, 3].Text.NullString();
                    nhanvienKhamSK.ThoiGianKhamSK = worksheet.Cells[i, 4].Text.NullString();
                    nhanvienKhamSK.Note = worksheet.Cells[i, 5].Text.NullString();

                    _EHNhanVienKhamSKRepository.Add(nhanvienKhamSK);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _unitOfWork.Commit();
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
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Vendor: " + nhaThau;
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
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Vendor: " + nhaThau;
                even.UserCreated = GetUserId();
                _EventScheduleParentRepository.Add(even);
            }
            return Guid.Parse(evenId);
        }

        public EhsNhanVienKhamSucKhoe GetNhanVienKhamSKById(int Id)
        {
            return _mapper.Map<EhsNhanVienKhamSucKhoe>(_EHNhanVienKhamSKRepository.FindById(Id));
        }
    }
}
