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
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class EhsKeHoachAnToanBucXaService : BaseService, IEhsKeHoachAnToanBucXaService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KEHOACH_ANTOAN_BUCXA, Guid> _EHSKeHoachBucXaRepository;
        IRespository<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA, int> _EHSNgayThucHienBucXaRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachAnToanBucXaService(IRespository<EHS_KEHOACH_ANTOAN_BUCXA, Guid> EHSKeHoachBucXaRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA, int> EHSNgayThucHienBucXaRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _EHSKeHoachBucXaRepository = EHSKeHoachBucXaRepository;
            _EHSNgayThucHienBucXaRepository = EHSNgayThucHienBucXaRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public EhsKeHoachAnToanBucXaViewModel Add(EhsKeHoachAnToanBucXaViewModel model)
        {
            // Id trong table EHS_DM_KEHOACH
            model.Id = Guid.NewGuid();
            model.MaDMKeHoach = Guid.Parse("7c60f914-c6d2-453a-841f-5afd0fb4a3bc");
            EHS_KEHOACH_ANTOAN_BUCXA en = _mapper.Map<EHS_KEHOACH_ANTOAN_BUCXA>(model);
            _EHSKeHoachBucXaRepository.Add(en);
            Save();
            return _mapper.Map<EhsKeHoachAnToanBucXaViewModel>(en);
        }

        public EhsKeHoachAnToanBucXaViewModel Update(EhsKeHoachAnToanBucXaViewModel model)
        {
            _EHSKeHoachBucXaRepository.Update(_mapper.Map<EHS_KEHOACH_ANTOAN_BUCXA>(model));

            var lstEvt = _EHSNgayThucHienBucXaRepository.FindAll(x => x.MaKH_ATBX == model.Id).Select(x => x.MaEvent).ToList();

            foreach (var evt in lstEvt)
            {
                var even = _EventScheduleParentRepository.FindById(evt);
                if (even != null)
                {
                    even.Subject = model.HangMuc;
                    even.Description = "Phụ Trách: " + model.NguoiPhuTrach + " || Vendor: " + model.NhaThau;
                    even.UserModified = GetUserId();
                    _EventScheduleParentRepository.Update(even);
                }
            }

            return model;
        }

        public void Delete(Guid Id)
        {
            _EHSKeHoachBucXaRepository.Remove(Id);
        }

        public EhsKeHoachAnToanBucXaViewModel GetById(Guid Id)
        {
            return _mapper.Map<EhsKeHoachAnToanBucXaViewModel>(_EHSKeHoachBucXaRepository.FindById(Id, x => x.EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA));
        }

        public List<EhsKeHoachAnToanBucXaViewModel> GetList(string year)
        {
            return _mapper.Map<List<EhsKeHoachAnToanBucXaViewModel>>(_EHSKeHoachBucXaRepository.FindAll(x=>x.Year ==year,x => x.EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA).OrderBy(x => x.STT));
        }

        public EhsThoiGianThucHienAnToanBucXaViewModel AddThoiGianBucXa(EhsThoiGianThucHienAnToanBucXaViewModel model)
        {
            EhsKeHoachAnToanBucXaViewModel kehoach = GetById(model.MaKH_ATBX);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string nhaThau = kehoach.NhaThau.NullString();
            model.NoiDung = kehoach.NoiDung.NullString();

            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), kehoach.HangMuc, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);
            EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA enModel = _mapper.Map<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA>(model);
            _EHSNgayThucHienBucXaRepository.Add(enModel);

            Save();

            var en = _EHSKeHoachBucXaRepository.FindById(model.MaKH_ATBX);
            en.ThoiGianCapL1 = "";
            en.ThoiGianCapLai_L1 = "";
            en.ThoiGianCapLai_L2 = "";
            en.ThoiGianCapLai_L3 = "";
            en.ThoiGianCapLai_L4 = "";

            string days = "";
            int count = 0;
            foreach (var item in _EHSNgayThucHienBucXaRepository.FindAll(x => x.MaKH_ATBX.Equals(model.MaKH_ATBX)).OrderBy(x => x.NgayBatDau))
            {
                if (!DateTime.TryParse(item.NgayBatDau, out _))
                {
                    continue;
                }

                count += 1;
                if (count == 1)
                {
                    en.ThoiGianCapL1 = item.NgayBatDau;
                }
                else
                if (DateTime.Now.CompareTo(DateTime.Parse(item.NgayBatDau)) <= 0)
                {
                    en.ThoiGianCapLai_L4 += item.NgayBatDau + ",";
                }
                else
                {
                    if (en.ThoiGianCapLai_L1 == "")
                    {
                        en.ThoiGianCapLai_L1 = item.NgayBatDau;
                    }

                    if (en.ThoiGianCapLai_L2 == "")
                    {
                        en.ThoiGianCapLai_L2 = item.NgayBatDau;
                    }

                    if (en.ThoiGianCapLai_L3 == "")
                    {
                        en.ThoiGianCapLai_L3 = item.NgayBatDau;
                    }
                }

                days += item.NgayBatDau + ",";
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachBucXaRepository.Update(en);
            return _mapper.Map<EhsThoiGianThucHienAnToanBucXaViewModel>(enModel); ;
        }

        public EhsThoiGianThucHienAnToanBucXaViewModel UpdateThoiGianBucXa(EhsThoiGianThucHienAnToanBucXaViewModel model)
        {
            EhsKeHoachAnToanBucXaViewModel kehoach = GetById(model.MaKH_ATBX);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string nhaThau = kehoach.NhaThau.NullString();
            model.NoiDung = kehoach.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), kehoach.HangMuc, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            _EHSNgayThucHienBucXaRepository.Update(_mapper.Map<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA>(model));
            Save();

            var en = _EHSKeHoachBucXaRepository.FindById(model.MaKH_ATBX);
            en.ThoiGianCapL1 = "";
            en.ThoiGianCapLai_L1 = "";
            en.ThoiGianCapLai_L2 = "";
            en.ThoiGianCapLai_L3 = "";
            en.ThoiGianCapLai_L4 = "";

            string days = "";
            int count = 0;
            foreach (var item in _EHSNgayThucHienBucXaRepository.FindAll(x => x.MaKH_ATBX.Equals(model.MaKH_ATBX)).OrderBy(x => x.NgayBatDau))
            {
                if (!DateTime.TryParse(item.NgayBatDau, out _))
                {
                    continue;
                }

                count += 1;
                if (count == 1)
                {
                    en.ThoiGianCapL1 = item.NgayBatDau;
                }
                else
                if (DateTime.Now.CompareTo(DateTime.Parse(item.NgayBatDau)) <= 0)
                {
                    en.ThoiGianCapLai_L4 += item.NgayBatDau + ",";
                }
                else
                {
                    if (en.ThoiGianCapLai_L1 == "")
                    {
                        en.ThoiGianCapLai_L1 = item.NgayBatDau;
                    }

                    if (en.ThoiGianCapLai_L2 == "")
                    {
                        en.ThoiGianCapLai_L2 = item.NgayBatDau;
                    }

                    if (en.ThoiGianCapLai_L3 == "")
                    {
                        en.ThoiGianCapLai_L3 = item.NgayBatDau;
                    }
                }

                days += item.NgayBatDau + ",";
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachBucXaRepository.Update(en);
            return model;
        }

        public void DeleteThoiGianBucXa(int Id)
        {
            _EHSNgayThucHienBucXaRepository.Remove(Id);
        }

        public EhsThoiGianThucHienAnToanBucXaViewModel GetThoiGianBucXaById(int Id)
        {
            return _mapper.Map<EhsThoiGianThucHienAnToanBucXaViewModel>(_EHSNgayThucHienBucXaRepository.FindById(Id));
        }

        public string ImportExcel(string filePath)
        {
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    EHS_KEHOACH_ANTOAN_BUCXA kehoach;
                    Guid kehoachId;

                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
                    int j = 0;

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        kehoach = new EHS_KEHOACH_ANTOAN_BUCXA();
                        kehoachId = Guid.NewGuid();
                        kehoach.Id = kehoachId;
                        kehoach.MaDMKeHoach = Guid.Parse("7c60f914-c6d2-453a-841f-5afd0fb4a3bc");

                        kehoach.STT = int.Parse(worksheet.Cells[i, 1].Text.NullString());
                        kehoach.HangMuc = worksheet.Cells[i, 2].Text.NullString();
                        kehoach.NoiDung = worksheet.Cells[i, 3].Text.NullString();
                        kehoach.MaHieu = worksheet.Cells[i, 4].Text.NullString();
                        kehoach.ChuKyThucHien = worksheet.Cells[i, 5].Text.NullString();
                        kehoach.ThoiGianCapL1 = worksheet.Cells[i, 6].Text.NullString();
                        kehoach.ThoiGianCapLai_L1 = worksheet.Cells[i, 7].Text.NullString();
                        kehoach.ThoiGianCapLai_L2 = worksheet.Cells[i, 8].Text.NullString();
                        kehoach.ThoiGianCapLai_L3 = worksheet.Cells[i, 9].Text.NullString();
                        kehoach.ThoiGianCapLai_L4 = worksheet.Cells[i, 10].Text.NullString();

                        kehoach.ThoiGianDaoTao = kehoach.ThoiGianCapL1 + "," + kehoach.ThoiGianCapLai_L1 + "," + kehoach.ThoiGianCapLai_L2 + "," + kehoach.ThoiGianCapLai_L3 + "," + kehoach.ThoiGianCapLai_L4;

                        kehoach.YeuCau = worksheet.Cells[i, 11].Text.NullString();
                        kehoach.QuyDinhVBPL = worksheet.Cells[i, 12].Text.NullString();
                        kehoach.NguoiPhuTrach = worksheet.Cells[i, 13].Text.NullString();
                        kehoach.NhaThau = worksheet.Cells[i, 14].Text.NullString();

                        kehoach.CostMonth_1 = double.Parse(worksheet.Cells[i, 15].Text.IfNullIsZero());
                        kehoach.CostMonth_2 = double.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                        kehoach.CostMonth_3 = double.Parse(worksheet.Cells[i, 17].Text.IfNullIsZero());
                        kehoach.CostMonth_4 = double.Parse(worksheet.Cells[i, 18].Text.IfNullIsZero());
                        kehoach.CostMonth_5 = double.Parse(worksheet.Cells[i, 19].Text.IfNullIsZero());
                        kehoach.CostMonth_6 = double.Parse(worksheet.Cells[i, 20].Text.IfNullIsZero());
                        kehoach.CostMonth_7 = double.Parse(worksheet.Cells[i, 21].Text.IfNullIsZero());
                        kehoach.CostMonth_8 = double.Parse(worksheet.Cells[i, 22].Text.IfNullIsZero());
                        kehoach.CostMonth_9 = double.Parse(worksheet.Cells[i, 23].Text.IfNullIsZero());
                        kehoach.CostMonth_10 = double.Parse(worksheet.Cells[i, 24].Text.IfNullIsZero());
                        kehoach.CostMonth_11 = double.Parse(worksheet.Cells[i, 25].Text.IfNullIsZero());
                        kehoach.CostMonth_12 = double.Parse(worksheet.Cells[i, 26].Text.IfNullIsZero());
                        kehoach.Year = worksheet.Cells[i, 27].Text.NullString();

                        if (int.Parse(kehoach.Year) < DateTime.Now.Year)
                        {
                            throw new Exception("Năm nhỏ hơn năm hiện tại là không phù hợp!");
                        }

                        _EHSKeHoachBucXaRepository.Add(kehoach);

                        List<string> lstDay = new List<string>();
                        lstDay.AddRange(kehoach.ThoiGianDaoTao.NullString().Split(","));

                        EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA ngaythuchien;
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
                                foreach (var item in _EHSNgayThucHienBucXaRepository.FindAll().ToList())
                                {
                                    if (DateTime.Parse(item.NgayBatDau).Year == int.Parse(kehoach.Year))
                                    {
                                        _EHSNgayThucHienBucXaRepository.Remove(item);
                                    }
                                }
                            }

                            even = new EVENT_SHEDULE_PARENT();
                            even.Id = Guid.NewGuid();
                            even.Subject = kehoach.HangMuc;
                            even.StartEvent = day.NullString();
                            even.EndEvent = day.NullString();
                            even.StartTime = DateTime.Parse(day.NullString());
                            even.EndTime = DateTime.Parse(day.NullString()).AddDays(1);
                            even.IsAllDay = true;
                            even.Description = "Phụ Trách: " + kehoach.NguoiPhuTrach + " || Vendor: " + kehoach.NhaThau;
                            even.UserCreated = GetUserId();
                            _EventScheduleParentRepository.Add(even);

                            ngaythuchien = new EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA()
                            {
                                MaKH_ATBX = kehoachId,
                                NoiDung = kehoach.NoiDung,
                                NgayBatDau = day.NullString(),
                                NgayKetThuc = day.NullString(),
                                MaEvent = even.Id,
                                UserCreated = GetUserId()
                            };

                            _EHSNgayThucHienBucXaRepository.Add(ngaythuchien);
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
    }
}
