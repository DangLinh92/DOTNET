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
    public class EhsKeHoachPCCCService : BaseService, IEhsKeHoachPCCCService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KEHOACH_PCCC, Guid> _EHSKeHoachPCCCRepository;
        IRespository<EHS_THOIGIAN_THUC_HIEN_PCCC, int> _EHSNgayThucHienPCCCRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachPCCCService(IRespository<EHS_KEHOACH_PCCC, Guid> EHSKeHoachPCCCRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_PCCC, int> EHSNgayThucHienPCCCRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _EHSKeHoachPCCCRepository = EHSKeHoachPCCCRepository;
            _EHSNgayThucHienPCCCRepository = EHSNgayThucHienPCCCRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public Ehs_KeHoach_PCCCViewModel Add(Ehs_KeHoach_PCCCViewModel model)
        {
            // Id trong table EHS_DM_KEHOACH
            model.Id = Guid.NewGuid();
            model.MaDMKeHoach = Guid.Parse("8b5cb6e4-e925-4a8b-b14d-594b310b6a4f");
            EHS_KEHOACH_PCCC en = _mapper.Map<EHS_KEHOACH_PCCC>(model);
            _EHSKeHoachPCCCRepository.Add(en);
            Save();
            return _mapper.Map<Ehs_KeHoach_PCCCViewModel>(en);
        }

        public Ehs_KeHoach_PCCCViewModel Update(Ehs_KeHoach_PCCCViewModel model)
        {
            _EHSKeHoachPCCCRepository.Update(_mapper.Map<EHS_KEHOACH_PCCC>(model));

            var lstEvt = _EHSNgayThucHienPCCCRepository.FindAll(x => x.MaKH_PCCC == model.Id).Select(x => x.MaEvent).ToList();

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
            _EHSKeHoachPCCCRepository.Remove(Id);
        }

        public Ehs_KeHoach_PCCCViewModel GetById(Guid Id)
        {
            return _mapper.Map<Ehs_KeHoach_PCCCViewModel>(_EHSKeHoachPCCCRepository.FindById(Id, x => x.EHS_THOIGIAN_THUC_HIEN_PCCC));
        }

        public List<Ehs_KeHoach_PCCCViewModel> GetList(string year)
        {
            return _mapper.Map<List<Ehs_KeHoach_PCCCViewModel>>(_EHSKeHoachPCCCRepository.FindAll(x => x.Year == year, x => x.EHS_THOIGIAN_THUC_HIEN_PCCC).OrderBy(x => x.STT));
        }


        public EhsThoiGianThucHienPCCCViewModel AddThoiGianPCCC(EhsThoiGianThucHienPCCCViewModel model)
        {
            Ehs_KeHoach_PCCCViewModel kehoach = GetById(model.MaKH_PCCC);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string nhaThau = kehoach.NhaThau.NullString();
            model.NoiDung = kehoach.NoiDung.NullString();

            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), kehoach.HangMuc, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);
            EHS_THOIGIAN_THUC_HIEN_PCCC enModel = _mapper.Map<EHS_THOIGIAN_THUC_HIEN_PCCC>(model);
            _EHSNgayThucHienPCCCRepository.Add(enModel);

            Save();

            var en = _EHSKeHoachPCCCRepository.FindById(model.MaKH_PCCC);
            string days = "";
            int count = 0;
            foreach (var item in _EHSNgayThucHienPCCCRepository.FindAll(x => x.MaKH_PCCC.Equals(model.MaKH_PCCC)))
            {
                count += 1;

                if (count <= 5)
                {
                    days += item.NgayBatDau + ";";
                }
                else
                {
                    days += "..";
                }
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachPCCCRepository.Update(en);
            return _mapper.Map<EhsThoiGianThucHienPCCCViewModel>(enModel); ;
        }

        public EhsThoiGianThucHienPCCCViewModel UpdateThoiGianPCCC(EhsThoiGianThucHienPCCCViewModel model)
        {
            Ehs_KeHoach_PCCCViewModel kehoach = GetById(model.MaKH_PCCC);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string nhaThau = kehoach.NhaThau.NullString();
            model.NoiDung = kehoach.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), kehoach.HangMuc, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            _EHSNgayThucHienPCCCRepository.Update(_mapper.Map<EHS_THOIGIAN_THUC_HIEN_PCCC>(model));
            Save();

            var en = _EHSKeHoachPCCCRepository.FindById(model.MaKH_PCCC);
            string days = "";
            int count = 0;
            foreach (var item in _EHSNgayThucHienPCCCRepository.FindAll(x => x.MaKH_PCCC.Equals(model.MaKH_PCCC)))
            {
                count += 1;

                if (count <= 5)
                {
                    days += item.NgayBatDau + ";";
                }
                else
                {
                    days += "..";
                }
            }

            en.ThoiGianDaoTao = days.Substring(0, days.Length - 1);
            _EHSKeHoachPCCCRepository.Update(en);
            return model;
        }

        public void DeleteThoiGianPCCC(int Id)
        {
            _EHSNgayThucHienPCCCRepository.Remove(Id);
        }


        public EhsThoiGianThucHienPCCCViewModel GetThoiGianPCCCById(int Id)
        {
            return _mapper.Map<EhsThoiGianThucHienPCCCViewModel>(_EHSNgayThucHienPCCCRepository.FindById(Id));
        }

        public string ImportExcel(string filePath)
        {
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    EHS_KEHOACH_PCCC kehoach;
                    Guid kehoachId;

                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
                    int j = 0;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        kehoach = new EHS_KEHOACH_PCCC();
                        kehoachId = Guid.NewGuid();
                        kehoach.Id = kehoachId;
                        kehoach.MaDMKeHoach = Guid.Parse("8b5cb6e4-e925-4a8b-b14d-594b310b6a4f");
                        kehoach.STT = int.Parse(worksheet.Cells[i, 1].Text.NullString());
                        kehoach.HangMuc = worksheet.Cells[i, 2].Text.NullString();
                        kehoach.NoiDung = worksheet.Cells[i, 3].Text.NullString();
                        kehoach.ChuKyThucHien = worksheet.Cells[i, 4].Text.NullString();
                        kehoach.ThoiGianDaoTao = worksheet.Cells[i, 5].Text.NullString();
                        kehoach.Year = worksheet.Cells[i, 6].Text.NullString();
                        kehoach.NguoiPhuTrach = worksheet.Cells[i, 7].Text.NullString();
                        kehoach.NhaThau = worksheet.Cells[i, 8].Text.NullString();

                        if (int.Parse(kehoach.Year) < DateTime.Now.Year)
                        {
                            throw new Exception("Năm nhỏ hơn năm hiện tại là không phù hợp!");
                        }

                        kehoach.CostMonth_1 = double.Parse(worksheet.Cells[i, 9].Text.IfNullIsZero());
                        kehoach.CostMonth_2 = double.Parse(worksheet.Cells[i, 10].Text.IfNullIsZero());
                        kehoach.CostMonth_3 = double.Parse(worksheet.Cells[i, 11].Text.IfNullIsZero());
                        kehoach.CostMonth_4 = double.Parse(worksheet.Cells[i, 12].Text.IfNullIsZero());
                        kehoach.CostMonth_5 = double.Parse(worksheet.Cells[i, 13].Text.IfNullIsZero());
                        kehoach.CostMonth_6 = double.Parse(worksheet.Cells[i, 14].Text.IfNullIsZero());
                        kehoach.CostMonth_7 = double.Parse(worksheet.Cells[i, 15].Text.IfNullIsZero());
                        kehoach.CostMonth_8 = double.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                        kehoach.CostMonth_9 = double.Parse(worksheet.Cells[i, 17].Text.IfNullIsZero());
                        kehoach.CostMonth_10 = double.Parse(worksheet.Cells[i, 18].Text.IfNullIsZero());
                        kehoach.CostMonth_11 = double.Parse(worksheet.Cells[i, 19].Text.IfNullIsZero());
                        kehoach.CostMonth_12 = double.Parse(worksheet.Cells[i, 20].Text.IfNullIsZero());

                        _EHSKeHoachPCCCRepository.Add(kehoach);

                        List<string> lstDay = new List<string>();
                        lstDay.AddRange(kehoach.ThoiGianDaoTao.NullString().Split(","));

                        EHS_THOIGIAN_THUC_HIEN_PCCC ngaythuchien;
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
                                foreach (var item in _EHSNgayThucHienPCCCRepository.FindAll().ToList())
                                {
                                    if (DateTime.Parse(item.NgayBatDau).Year == int.Parse(kehoach.Year))
                                    {
                                        _EHSNgayThucHienPCCCRepository.Remove(item);
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

                            ngaythuchien = new EHS_THOIGIAN_THUC_HIEN_PCCC()
                            {
                                MaKH_PCCC = kehoachId,
                                NoiDung = kehoach.NoiDung,
                                NgayBatDau = day.NullString(),
                                NgayKetThuc = day.NullString(),
                                MaEvent = even.Id,
                                UserCreated = GetUserId()
                            };

                            _EHSNgayThucHienPCCCRepository.Add(ngaythuchien);
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
