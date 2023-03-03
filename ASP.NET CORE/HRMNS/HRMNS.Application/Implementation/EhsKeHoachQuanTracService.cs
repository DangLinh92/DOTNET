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
    public class EhsKeHoachQuanTracService : BaseService, IEhsKeHoachQuanTracService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<EHS_KEHOACH_QUANTRAC, int> _EHSKeHoachQuanTracRepository;
        IRespository<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC, int> _EHSNgayThucHienQuanTracRepository;
        IRespository<EHS_DM_KEHOACH, Guid> _EHSDMKeHoachRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _EventScheduleParentRepository;
        private readonly IMapper _mapper;

        public EhsKeHoachQuanTracService(IRespository<EHS_KEHOACH_QUANTRAC, int> EHSKeHoachQuanTracRepository,
            IRespository<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC, int> EHSNgayThucHienQuanTracRepository,
            IRespository<EHS_DM_KEHOACH, Guid> eHSDMKeHoachRepository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _EHSNgayThucHienQuanTracRepository = EHSNgayThucHienQuanTracRepository;
            _EHSKeHoachQuanTracRepository = EHSKeHoachQuanTracRepository;
            _EHSDMKeHoachRepository = eHSDMKeHoachRepository;
            _EventScheduleParentRepository = eventScheduleParentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public EhsKeHoachQuanTracViewModel Add(EhsKeHoachQuanTracViewModel model)
        {
            // Id trong table EHS_DM_KEHOACH
            model.MaDMKeHoach = Guid.Parse("44ba2130-8336-4853-b226-7234e592c52c");

            EHS_KEHOACH_QUANTRAC en = _mapper.Map<EHS_KEHOACH_QUANTRAC>(model);
            _EHSKeHoachQuanTracRepository.Add(en);
            Save();
            return _mapper.Map<EhsKeHoachQuanTracViewModel>(en);
        }

        public void Delete(int Id)
        {
            _EHSKeHoachQuanTracRepository.Remove(Id);
        }

        public List<EhsKeHoachQuanTracViewModel> GetList(string year)
        {
            return _mapper.Map<List<EhsKeHoachQuanTracViewModel>>(_EHSKeHoachQuanTracRepository.FindAll(x => x.Year == year, x => x.EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC).OrderBy(x => x.STT));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public EhsKeHoachQuanTracViewModel Update(EhsKeHoachQuanTracViewModel model)
        {
            _EHSKeHoachQuanTracRepository.Update(_mapper.Map<EHS_KEHOACH_QUANTRAC>(model));

            var lstEvt = _EHSNgayThucHienQuanTracRepository.FindAll(x => x.MaKHQuanTrac == model.Id).Select(x => x.MaEvent).ToList();

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

        public EhsKeHoachQuanTracViewModel GetById(int Id)
        {
            return _mapper.Map<EhsKeHoachQuanTracViewModel>(_EHSKeHoachQuanTracRepository.FindById(Id, x => x.EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC));
        }

        public EhsNgayThucHienChiTietQuanTrac UpdateNgayQuanTrac(EhsNgayThucHienChiTietQuanTrac model)
        {
            EhsKeHoachQuanTracViewModel quantrac = GetById(model.MaKHQuanTrac);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();
            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            _EHSNgayThucHienQuanTracRepository.Update(_mapper.Map<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC>(model));
            return model;
        }

        public EhsNgayThucHienChiTietQuanTrac AddNgayQuanTrac(EhsNgayThucHienChiTietQuanTrac model)
        {
            EhsKeHoachQuanTracViewModel quantrac = GetById(model.MaKHQuanTrac);
            string nguoiPhuTrach = quantrac.NguoiPhuTrach.NullString();
            string nhaThau = quantrac.NhaThau.NullString();
            model.NoiDung = quantrac.NoiDung.NullString();

            model.MaEvent = AddNewEvent(model.MaEvent.ToString(), model.NoiDung, model.NgayBatDau, model.NgayKetThuc, nguoiPhuTrach, nhaThau);

            EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC en = _mapper.Map<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC>(model);
            _EHSNgayThucHienQuanTracRepository.Add(en);
            Save();
            return _mapper.Map<EhsNgayThucHienChiTietQuanTrac>(en);
        }

        public void DeleteNgayQuanTrac(int Id)
        {
            _EHSNgayThucHienQuanTracRepository.Remove(Id);
        }

        public EhsNgayThucHienChiTietQuanTrac GetNgayQuanTracById(int Id)
        {
            return _mapper.Map<EhsNgayThucHienChiTietQuanTrac>(_EHSNgayThucHienQuanTracRepository.FindById(Id));
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

                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    EHS_KEHOACH_QUANTRAC kehoachQuanTrac;
                    int j = 0;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        kehoachQuanTrac = new EHS_KEHOACH_QUANTRAC();
                        kehoachQuanTrac.MaDMKeHoach = Guid.Parse("44ba2130-8336-4853-b226-7234e592c52c");
                        kehoachQuanTrac.STT = int.Parse(worksheet.Cells[i, 1].Text.NullString());
                        kehoachQuanTrac.Demuc = worksheet.Cells[i, 2].Text.NullString();
                        kehoachQuanTrac.LuatDinhLienQuan = worksheet.Cells[i, 3].Text.NullString();
                        kehoachQuanTrac.NoiDung = worksheet.Cells[i, 4].Text.NullString();
                        kehoachQuanTrac.ChuKyThucHien = worksheet.Cells[i, 5].Text.NullString();
                        kehoachQuanTrac.KhuVucLayMau = worksheet.Cells[i, 6].Text.NullString();
                        kehoachQuanTrac.Year = worksheet.Cells[i, 7].Text.NullString();

                        if (int.Parse(kehoachQuanTrac.Year) < DateTime.Now.Year)
                        {
                            throw new Exception("Năm nhỏ hơn năm hiện tại là không phù hợp!");
                        }

                        kehoachQuanTrac.NguoiPhuTrach = worksheet.Cells[i, 8].Text.NullString();
                        kehoachQuanTrac.NhaThau = worksheet.Cells[i, 9].Text.NullString();

                        kehoachQuanTrac.Month_1 = worksheet.Cells[i, 10].Text.NullString() != "";
                        kehoachQuanTrac.Month_2 = worksheet.Cells[i, 11].Text.NullString() != "";
                        kehoachQuanTrac.Month_3 = worksheet.Cells[i, 12].Text.NullString() != "";
                        kehoachQuanTrac.Month_4 = worksheet.Cells[i, 13].Text.NullString() != "";
                        kehoachQuanTrac.Month_5 = worksheet.Cells[i, 14].Text.NullString() != "";
                        kehoachQuanTrac.Month_6 = worksheet.Cells[i, 15].Text.NullString() != "";
                        kehoachQuanTrac.Month_7 = worksheet.Cells[i, 16].Text.NullString() != "";
                        kehoachQuanTrac.Month_8 = worksheet.Cells[i, 17].Text.NullString() != "";
                        kehoachQuanTrac.Month_9 = worksheet.Cells[i, 18].Text.NullString() != "";
                        kehoachQuanTrac.Month_10 = worksheet.Cells[i, 19].Text.NullString() != "";
                        kehoachQuanTrac.Month_11 = worksheet.Cells[i, 20].Text.NullString() != "";
                        kehoachQuanTrac.Month_12 = worksheet.Cells[i, 21].Text.NullString() != "";

                        kehoachQuanTrac.LayMau_Month_1 = worksheet.Cells[i, 22].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_2 = worksheet.Cells[i, 23].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_3 = worksheet.Cells[i, 24].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_4 = worksheet.Cells[i, 25].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_5 = worksheet.Cells[i, 26].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_6 = worksheet.Cells[i, 27].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_7 = worksheet.Cells[i, 28].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_8 = worksheet.Cells[i, 29].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_9 = worksheet.Cells[i, 30].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_10 = worksheet.Cells[i, 31].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_11 = worksheet.Cells[i, 32].Text.NullString();
                        kehoachQuanTrac.LayMau_Month_12 = worksheet.Cells[i, 33].Text.NullString();

                        kehoachQuanTrac.CostMonth_1 = double.Parse(worksheet.Cells[i, 34].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_2 = double.Parse(worksheet.Cells[i, 35].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_3 = double.Parse(worksheet.Cells[i, 36].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_4 = double.Parse(worksheet.Cells[i, 37].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_5 = double.Parse(worksheet.Cells[i, 38].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_6 = double.Parse(worksheet.Cells[i, 39].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_7 = double.Parse(worksheet.Cells[i, 40].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_8 = double.Parse(worksheet.Cells[i, 41].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_9 = double.Parse(worksheet.Cells[i, 42].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_10 = double.Parse(worksheet.Cells[i, 43].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_11 = double.Parse(worksheet.Cells[i, 44].Text.IfNullIsZero());
                        kehoachQuanTrac.CostMonth_12 = double.Parse(worksheet.Cells[i, 45].Text.IfNullIsZero());

                        _EHSKeHoachQuanTracRepository.Add(kehoachQuanTrac);
                        Save();

                        int maxId = _EHSKeHoachQuanTracRepository.GetMaxSequenceValue();

                        List<string> lstDay = new List<string>();
                        lstDay.AddRange(worksheet.Cells[i, 10].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 11].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 12].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 13].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 14].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 15].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 16].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 17].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 18].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 19].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 20].Text.NullString().Split(","));
                        lstDay.AddRange(worksheet.Cells[i, 21].Text.NullString().Split(","));

                        EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC ngayQuanTrac;
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
                                foreach (var item in _EHSNgayThucHienQuanTracRepository.FindAll().ToList())
                                {
                                    if (DateTime.Parse(item.NgayBatDau).Year == int.Parse(kehoachQuanTrac.Year))
                                    {
                                        _EHSNgayThucHienQuanTracRepository.Remove(item);
                                    }
                                }
                            }

                            even = new EVENT_SHEDULE_PARENT();
                            even.Id = Guid.NewGuid();
                            even.Subject = kehoachQuanTrac.NoiDung;
                            even.StartEvent = day.NullString();
                            even.EndEvent = day.NullString();
                            even.StartTime = DateTime.Parse(day.NullString());
                            even.EndTime = DateTime.Parse(day.NullString()).AddDays(1);
                            even.IsAllDay = true;
                            even.Description = "Phụ Trách: " + kehoachQuanTrac.NguoiPhuTrach + " || Vendor: " + kehoachQuanTrac.NhaThau;
                            even.UserCreated = GetUserId();
                            _EventScheduleParentRepository.Add(even);

                            ngayQuanTrac = new EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC()
                            {
                                MaKHQuanTrac = maxId,
                                NoiDung = kehoachQuanTrac.NoiDung,
                                NgayBatDau = day.NullString(),
                                NgayKetThuc = day.NullString(),
                                MaEvent = even.Id,
                                UserCreated = GetUserId()
                            };

                            _EHSNgayThucHienQuanTracRepository.Add(ngayQuanTrac);
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
