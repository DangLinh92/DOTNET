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
    public class EhsKeHoachKiemDinhMayMocService : BaseService, IEhsKeHoachKiemDinhMayMocService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KEHOACH_KIEMDINH_MAYMOC, Guid> _EHSKeHoachKiemDinhMayMocRepository;
        IRespository<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM, int> _EHSNgayThucHienKiemDinhMayMocRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachKiemDinhMayMocService(IRespository<EHS_KEHOACH_KIEMDINH_MAYMOC, Guid> EHSKeHoachKiemDinhMayMocRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM, int> EHSNgayThucHienKiemDinhMayMocRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _EHSKeHoachKiemDinhMayMocRepository = EHSKeHoachKiemDinhMayMocRepository;
            _EHSNgayThucHienKiemDinhMayMocRepository = EHSNgayThucHienKiemDinhMayMocRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public EhsKeHoachKiemDinhMayMocViewModel Add(EhsKeHoachKiemDinhMayMocViewModel model)
        {
            // Id trong table EHS_DM_KEHOACH
            model.Id = Guid.NewGuid();
            model.MaDMKeHoach = Guid.Parse("5bbdc8cc-fc22-4be9-a3b8-f468ef04efe0");
            EHS_KEHOACH_KIEMDINH_MAYMOC en = _mapper.Map<EHS_KEHOACH_KIEMDINH_MAYMOC>(model);
            _EHSKeHoachKiemDinhMayMocRepository.Add(en);
            Save();
            return _mapper.Map<EhsKeHoachKiemDinhMayMocViewModel>(en);
        }

        public EhsKeHoachKiemDinhMayMocViewModel Update(EhsKeHoachKiemDinhMayMocViewModel model)
        {
            _EHSKeHoachKiemDinhMayMocRepository.Update(_mapper.Map<EHS_KEHOACH_KIEMDINH_MAYMOC>(model));

            var lstEvt = _EHSNgayThucHienKiemDinhMayMocRepository.FindAll(x => x.MaKH_KDMM == model.Id).Select(x => x.MaEvent).ToList();

            foreach (var evt in lstEvt)
            {
                var even = _EventScheduleParentRepository.FindById(evt);
                if (even != null)
                {
                    even.Subject = "Kiểm Định :" + model.TenMayMoc;
                    even.Description = "Phụ Trách: " + model.NguoiPhuTrach + " || Vị Trí[위치]: " + model.ViTri;
                    even.UserModified = GetUserId();
                    _EventScheduleParentRepository.Update(even);
                }
            }

            return model;
        }

        public void Delete(Guid Id)
        {
            _EHSKeHoachKiemDinhMayMocRepository.Remove(Id);
        }

        public EhsKeHoachKiemDinhMayMocViewModel GetById(Guid Id)
        {
            return _mapper.Map<EhsKeHoachKiemDinhMayMocViewModel>(_EHSKeHoachKiemDinhMayMocRepository.FindById(Id, x => x.EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM));
        }

        public List<EhsKeHoachKiemDinhMayMocViewModel> GetList(string year)
        {
            return _mapper.Map<List<EhsKeHoachKiemDinhMayMocViewModel>>(_EHSKeHoachKiemDinhMayMocRepository.FindAll(x=>x.Year == year,x => x.EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM).OrderBy(x => x.STT));
        }

        public EhsThoiGianKiemDinhMayMocViewModel AddThoiGianKiemDinhMayMoc(EhsThoiGianKiemDinhMayMocViewModel model)
        {
            EhsKeHoachKiemDinhMayMocViewModel kehoach = GetById(model.MaKH_KDMM);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string vitri = kehoach.ViTri.NullString();
            model.NoiDung = kehoach.TenMayMoc.NullString();

            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, vitri);
            EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM enModel = _mapper.Map<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM>(model);
            _EHSNgayThucHienKiemDinhMayMocRepository.Add(enModel);

            Save();

            var en = _EHSKeHoachKiemDinhMayMocRepository.FindById(model.MaKH_KDMM);
            en.LanKiemDinhKeTiep = "";
            en.LanKiemDinhKeTiep1 = "";
            en.LanKiemDinhKeTiep2 = "";
            en.LanKiemDinhKeTiep3 = "";

            foreach (var item in _EHSNgayThucHienKiemDinhMayMocRepository.FindAll(x => x.MaKH_KDMM.Equals(model.MaKH_KDMM)).OrderByDescending(x => x.NgayBatDau))
            {
                if (!DateTime.TryParse(item.NgayBatDau, out _))
                {
                    continue;
                }

                if (DateTime.Now.CompareTo(DateTime.Parse(item.NgayBatDau)) <= 0)
                {
                    en.LanKiemDinhKeTiep3 += item.NgayBatDau + ",";
                }
                else
                {
                    if (en.LanKiemDinhKeTiep2 == "")
                    {
                        en.LanKiemDinhKeTiep2 += item.NgayBatDau;
                    }

                    if (en.LanKiemDinhKeTiep1 == "")
                    {
                        en.LanKiemDinhKeTiep1 += item.NgayBatDau;
                    }

                    if (en.LanKiemDinhKeTiep == "")
                    {
                        en.LanKiemDinhKeTiep += item.NgayBatDau;
                    }
                }
            }

            _EHSKeHoachKiemDinhMayMocRepository.Update(en);
            return _mapper.Map<EhsThoiGianKiemDinhMayMocViewModel>(enModel); ;
        }

        public EhsThoiGianKiemDinhMayMocViewModel UpdateThoiGianKiemDinhMayMoc(EhsThoiGianKiemDinhMayMocViewModel model)
        {
            EhsKeHoachKiemDinhMayMocViewModel kehoach = GetById(model.MaKH_KDMM);
            string nguoiPhuTrach = kehoach.NguoiPhuTrach.NullString();
            string vitri = kehoach.ViTri.NullString();
            model.NoiDung = kehoach.TenMayMoc.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, vitri);
            _EHSNgayThucHienKiemDinhMayMocRepository.Update(_mapper.Map<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM>(model));
            Save();

            var en = _EHSKeHoachKiemDinhMayMocRepository.FindById(model.MaKH_KDMM);
            en.LanKiemDinhKeTiep = "";
            en.LanKiemDinhKeTiep1 = "";
            en.LanKiemDinhKeTiep2 = "";
            en.LanKiemDinhKeTiep3 = "";

            foreach (var item in _EHSNgayThucHienKiemDinhMayMocRepository.FindAll(x => x.MaKH_KDMM.Equals(model.MaKH_KDMM)).OrderByDescending(x => x.NgayBatDau))
            {
                if (!DateTime.TryParse(item.NgayBatDau, out _))
                {
                    continue;
                }

                if (DateTime.Now.CompareTo(DateTime.Parse(item.NgayBatDau)) <= 0)
                {
                    en.LanKiemDinhKeTiep3 += item.NgayBatDau + ",";
                }
                else
                {
                    if (en.LanKiemDinhKeTiep2 == "")
                    {
                        en.LanKiemDinhKeTiep2 += item.NgayBatDau;
                    }

                    if (en.LanKiemDinhKeTiep1 == "")
                    {
                        en.LanKiemDinhKeTiep1 += item.NgayBatDau;
                    }

                    if (en.LanKiemDinhKeTiep == "")
                    {
                        en.LanKiemDinhKeTiep += item.NgayBatDau;
                    }
                }
            }

            _EHSKeHoachKiemDinhMayMocRepository.Update(en);
            return model;
        }

        public void DeleteThoiKiemDinhMayMoc(int Id)
        {
            _EHSNgayThucHienKiemDinhMayMocRepository.Remove(Id);
        }

        public EhsThoiGianKiemDinhMayMocViewModel GetThoiGianKiemDinhMayMocById(int Id)
        {
            return _mapper.Map<EhsThoiGianKiemDinhMayMocViewModel>(_EHSNgayThucHienKiemDinhMayMocRepository.FindById(Id));
        }

        public string ImportExcel(string filePath)
        {
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    EHS_KEHOACH_KIEMDINH_MAYMOC kehoach;
                    Guid kehoachId;
                    int j = 0;
                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        kehoach = new EHS_KEHOACH_KIEMDINH_MAYMOC();
                        kehoachId = Guid.NewGuid();
                        kehoach.Id = kehoachId;
                        kehoach.MaDMKeHoach = Guid.Parse("5bbdc8cc-fc22-4be9-a3b8-f468ef04efe0");

                        kehoach.STT = int.Parse(worksheet.Cells[i, 1].Text.NullString());
                        kehoach.TenMayMoc = worksheet.Cells[i, 2].Text.NullString();
                        kehoach.ChuKyKiemDinh = worksheet.Cells[i, 3].Text.NullString();
                        kehoach.ViTri = worksheet.Cells[i, 4].Text.NullString();

                        kehoach.SoLuongThietBi = int.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());

                        kehoach.LanKiemDinhKeTiep = worksheet.Cells[i, 6].Text.NullString();
                        kehoach.LanKiemDinhKeTiep1 = worksheet.Cells[i, 7].Text.NullString();
                        kehoach.LanKiemDinhKeTiep2 = worksheet.Cells[i, 8].Text.NullString();
                        kehoach.LanKiemDinhKeTiep3 = worksheet.Cells[i, 9].Text.NullString();

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
                        kehoach.Year = worksheet.Cells[i, 23].Text.NullString();

                        if (int.Parse(kehoach.Year) < DateTime.Now.Year)
                        {
                            throw new Exception("Năm nhỏ hơn năm hiện tại là không phù hợp!");
                        }

                        _EHSKeHoachKiemDinhMayMocRepository.Add(kehoach);

                        List<string> lstDay = new List<string>();
                        lstDay.AddRange(kehoach.LanKiemDinhKeTiep.NullString().Split(","));
                        lstDay.AddRange(kehoach.LanKiemDinhKeTiep1.NullString().Split(","));
                        lstDay.AddRange(kehoach.LanKiemDinhKeTiep2.NullString().Split(","));
                        lstDay.AddRange(kehoach.LanKiemDinhKeTiep3.NullString().Split(","));

                        EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM ngaythuchien;
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
                                foreach (var item in _EHSNgayThucHienKiemDinhMayMocRepository.FindAll().ToList())
                                {
                                    if (DateTime.Parse(item.NgayBatDau).Year == int.Parse(kehoach.Year))
                                    {
                                        _EHSNgayThucHienKiemDinhMayMocRepository.Remove(item);
                                    }
                                }
                            }

                            even = new EVENT_SHEDULE_PARENT();
                            even.Id = Guid.NewGuid();
                            even.Subject ="Kiểm Định:" + kehoach.TenMayMoc;
                            even.StartEvent = day.NullString();
                            even.EndEvent = day.NullString();
                            even.StartTime = DateTime.Parse(day.NullString());
                            even.EndTime = DateTime.Parse(day.NullString()).AddDays(1);
                            even.IsAllDay = true;
                            even.Description = "Phụ Trách: " + kehoach.NguoiPhuTrach + " || Vị trí: " + kehoach.ViTri;
                            even.UserCreated = GetUserId();
                            _EventScheduleParentRepository.Add(even);

                            ngaythuchien = new EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM()
                            {
                                MaKH_KDMM = kehoachId,
                                NoiDung = kehoach.TenMayMoc,
                                NgayBatDau = day.NullString(),
                                NgayKetThuc = day.NullString(),
                                MaEvent = even.Id,
                                UserCreated = GetUserId()
                            };

                            _EHSNgayThucHienKiemDinhMayMocRepository.Add(ngaythuchien);
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
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Vị trí[위치]: " + nhaThau;
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
                even.Description = "Phụ Trách: " + nguoiPhuTrach + " || Vị trí[위치]: " + nhaThau;
                even.UserCreated = GetUserId();
                _EventScheduleParentRepository.Add(even);
            }
            return Guid.Parse(evenId);
        }
    }
}
