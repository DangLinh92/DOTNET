using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EhsKeHoachDaoTaoATVSLDService : BaseService, IEhsKeHoachDaoTaoATVSLDService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD, Guid> _EHSKeHoachDaoTaoATVSLDRepository;
        IRespository<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD, int> _EHSNgayThucHienATVSLDRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachDaoTaoATVSLDService(IRespository<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD, Guid> EHSKeHoachDaoTaoATVSLDRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD, int> EHSNgayThucHienATVSLDRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _EHSKeHoachDaoTaoATVSLDRepository = EHSKeHoachDaoTaoATVSLDRepository;
            _EHSNgayThucHienATVSLDRepository = EHSNgayThucHienATVSLDRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public EhsKeHoachDaoTaoATLDViewModel Add(EhsKeHoachDaoTaoATLDViewModel model)
        {
            // Id trong table EHS_DM_KEHOACH
            model.Id = Guid.NewGuid();
            model.MaDMKeHoach = Guid.Parse("5f2ad5b3-8e86-4cd8-be84-ef4e7d82212b");
            EHS_KEHOACH_DAOTAO_ANTOAN_VSLD en = _mapper.Map<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD>(model);
            _EHSKeHoachDaoTaoATVSLDRepository.Add(en);
            Save();
            return _mapper.Map<EhsKeHoachDaoTaoATLDViewModel>(en);
        }

        public void Delete(Guid Id)
        {
            _EHSKeHoachDaoTaoATVSLDRepository.Remove(Id);
        }

        public List<EhsKeHoachDaoTaoATLDViewModel> GetList(string year)
        {
            return _mapper.Map<List<EhsKeHoachDaoTaoATLDViewModel>>(_EHSKeHoachDaoTaoATVSLDRepository.FindAll(x => x.Year == year, x => x.EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD).OrderBy(x => x.STT));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public EhsKeHoachDaoTaoATLDViewModel Update(EhsKeHoachDaoTaoATLDViewModel model)
        {
            _EHSKeHoachDaoTaoATVSLDRepository.Update(_mapper.Map<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD>(model));

            var lstEvt = _EHSNgayThucHienATVSLDRepository.FindAll(x => x.MaKHDaoTaoATLD == model.Id).Select(x => x.MaEvent).ToList();

            foreach (var evt in lstEvt)
            {
                var even = _EventScheduleParentRepository.FindById(evt);
                if (even != null)
                {
                    even.Subject = model.NoiDung;
                    even.Description = "Phụ Trách: " + model.NguoiPhuTrach + " || Vendor: " + model.NhaThau;
                    even.UserModified = GetUserId();
                    _EventScheduleParentRepository.Update(even);
                }
            }

            return model;
        }

        public EhsKeHoachDaoTaoATLDViewModel GetById(Guid Id)
        {
            return _mapper.Map<EhsKeHoachDaoTaoATLDViewModel>(_EHSKeHoachDaoTaoATVSLDRepository.FindById(Id, x => x.EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD));
        }

        public EhsThoiGianThucHienDaoTaoATVSViewModel UpdateThoiGianATVSLD(EhsThoiGianThucHienDaoTaoATVSViewModel model)
        {
            EhsKeHoachDaoTaoATLDViewModel quantrac = GetById(model.MaKHDaoTaoATLD);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            _EHSNgayThucHienATVSLDRepository.Update(_mapper.Map<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD>(model));
            Save();

            var en = _EHSKeHoachDaoTaoATVSLDRepository.FindById(model.MaKHDaoTaoATLD);
            string days = "";
            foreach (var item in _EHSNgayThucHienATVSLDRepository.FindAll(x => x.MaKHDaoTaoATLD.Equals(model.MaKHDaoTaoATLD)))
            {
                days += item.NgayBatDau + ";";
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachDaoTaoATVSLDRepository.Update(en);
            return model;
        }

        public EhsThoiGianThucHienDaoTaoATVSViewModel AddThoiGianATVSLD(EhsThoiGianThucHienDaoTaoATVSViewModel model)
        {
            EhsKeHoachDaoTaoATLDViewModel quantrac = GetById(model.MaKHDaoTaoATLD);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();

            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);
            EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD enModel = _mapper.Map<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD>(model);
            _EHSNgayThucHienATVSLDRepository.Add(enModel);

            Save();

           var en = _EHSKeHoachDaoTaoATVSLDRepository.FindById(model.MaKHDaoTaoATLD);
            string days = "";
            foreach (var item in _EHSNgayThucHienATVSLDRepository.FindAll(x=>x.MaKHDaoTaoATLD.Equals(model.MaKHDaoTaoATLD)))
            {
                days += item.NgayBatDau + ";";
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachDaoTaoATVSLDRepository.Update(en);
            return _mapper.Map<EhsThoiGianThucHienDaoTaoATVSViewModel>(enModel);
        }

        public void DeleteThoiGianATVSLD(int Id)
        {
            _EHSNgayThucHienATVSLDRepository.Remove(Id);
        }

        public EhsThoiGianThucHienDaoTaoATVSViewModel GetThoiGianATVSLDById(int Id)
        {
            return _mapper.Map<EhsThoiGianThucHienDaoTaoATVSViewModel>(_EHSNgayThucHienATVSLDRepository.FindById(Id));
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

        public string ImportExcel(string filePath)
        {
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();

                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    EHS_KEHOACH_DAOTAO_ANTOAN_VSLD kehoach;
                    Guid kehoachId;
                    int j = 0;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        kehoach = new EHS_KEHOACH_DAOTAO_ANTOAN_VSLD();
                        kehoachId = Guid.NewGuid();
                        kehoach.Id = kehoachId;
                        kehoach.MaDMKeHoach = Guid.Parse("5f2ad5b3-8e86-4cd8-be84-ef4e7d82212b");
                        kehoach.STT = int.Parse(worksheet.Cells[i, 1].Text.NullString());
                        kehoach.NoiDung = worksheet.Cells[i, 2].Text.NullString();
                        kehoach.NguoiThamGia = worksheet.Cells[i, 3].Text.NullString();
                        kehoach.ChuKyThucHien = worksheet.Cells[i, 4].Text.NullString();
                        kehoach.ThoiGianCapLanDau = worksheet.Cells[i, 5].Text.NullString();
                        kehoach.ThoiGianHuanLuyenLanDau = worksheet.Cells[i, 6].Text.NullString();
                        kehoach.ThoiGianHuanLuyenLai = worksheet.Cells[i, 7].Text.NullString();
                        kehoach.ThoiGianDaoTao = worksheet.Cells[i, 8].Text.NullString();
                        kehoach.Year = worksheet.Cells[i, 9].Text.NullString();

                        if (int.Parse(kehoach.Year) < DateTime.Now.Year)
                        {
                            throw new Exception("Năm nhỏ hơn năm hiện tại là không phù hợp!");
                        }

                        kehoach.NguoiPhuTrach = worksheet.Cells[i, 10].Text.NullString();

                        kehoach.CostMonth_1 = double.Parse(worksheet.Cells[i, 11].Text.IfNullIsZero());
                        kehoach.CostMonth_2 = double.Parse(worksheet.Cells[i, 12].Text.IfNullIsZero());
                        kehoach.CostMonth_3 = double.Parse(worksheet.Cells[i, 13].Text.IfNullIsZero());
                        kehoach.CostMonth_4 = double.Parse(worksheet.Cells[i, 14].Text.IfNullIsZero());
                        kehoach.CostMonth_5 = double.Parse(worksheet.Cells[i, 15].Text.IfNullIsZero());
                        kehoach.CostMonth_6 = double.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                        kehoach.CostMonth_7 = double.Parse(worksheet.Cells[i, 17].Text.IfNullIsZero());
                        kehoach.CostMonth_8 = double.Parse(worksheet.Cells[i, 18].Text.IfNullIsZero());
                        kehoach.CostMonth_9 = double.Parse(worksheet.Cells[i, 19].Text.IfNullIsZero());
                        kehoach.CostMonth_10 = double.Parse(worksheet.Cells[i, 20].Text.IfNullIsZero());
                        kehoach.CostMonth_11 = double.Parse(worksheet.Cells[i, 21].Text.IfNullIsZero());
                        kehoach.CostMonth_12 = double.Parse(worksheet.Cells[i, 22].Text.IfNullIsZero());

                        _EHSKeHoachDaoTaoATVSLDRepository.Add(kehoach);

                        List<string> lstDay = new List<string>();
                        lstDay.AddRange(kehoach.ThoiGianDaoTao.NullString().Split(","));

                        EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD ngaythuchien;
                        EVENT_SHEDULE_PARENT even;
                        foreach (var day in lstDay.Where(x => x != ""))
                        {
                            if (!DateTime.TryParse(day.NullString(), out _))
                            {
                                continue;
                            }

                            if (DateTime.Parse(day.NullString()).Year < DateTime.Now.Year)
                            {
                                continue;
                            }
                            j += 1;

                            if (j == 1)
                            {
                                foreach (var item in _EHSNgayThucHienATVSLDRepository.FindAll().ToList())
                                {
                                    if (DateTime.Parse(item.NgayBatDau).Year == int.Parse(kehoach.Year))
                                    {
                                        _EHSNgayThucHienATVSLDRepository.Remove(item);
                                    }
                                }
                            }

                            even = new EVENT_SHEDULE_PARENT();
                            even.Id = Guid.NewGuid();
                            even.Subject = kehoach.NoiDung;
                            even.StartEvent = day.NullString();
                            even.EndEvent = day.NullString();
                            even.StartTime = DateTime.Parse(day.NullString());
                            even.EndTime = DateTime.Parse(day.NullString()).AddDays(1);
                            even.IsAllDay = true;
                            even.Description = "Phụ Trách: " + kehoach.NguoiPhuTrach + " || Vendor: " + kehoach.NhaThau;
                            even.UserCreated = GetUserId();
                            _EventScheduleParentRepository.Add(even);

                            ngaythuchien = new EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD()
                            {
                                MaKHDaoTaoATLD = kehoachId,
                                NoiDung = kehoach.NoiDung,
                                NgayBatDau = day.NullString(),
                                NgayKetThuc = day.NullString(),
                                MaEvent = even.Id,
                                UserCreated = GetUserId()
                            };

                            _EHSNgayThucHienATVSLDRepository.Add(ngaythuchien);
                        }

                        Save();
                    }

                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.CommitTransaction();
                    return "";
                }
            }
            catch (Exception ex)
            {
                ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.RollbackTransaction();
                return ex.Message;
            }

        }
    }
}
