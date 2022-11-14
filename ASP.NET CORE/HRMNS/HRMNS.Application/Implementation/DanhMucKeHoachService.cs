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
        private IRespository<EHS_DEMUC_KEHOACH, Guid> _ehsDemucKHRespository;
        private IRespository<EHS_NOIDUNG, Guid> _ehsNoiDungRespository;
        private IRespository<EHS_NOIDUNG_KEHOACH, Guid> _ehsNoiDungKeHoachRespository;
        private IRespository<EHS_LUATDINH_DEMUC_KEHOACH, int> _ehsLuatDinhDemucKHRespository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _eventScheduleParentRepository;

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhMucKeHoachService(
            IRespository<EHS_DM_KEHOACH, Guid> ehsDanhMucKHRespository,
            IRespository<EHS_LUATDINH_KEHOACH, int> ehsLuatDinhKHRespository,
            IRespository<EHS_DEMUC_KEHOACH, Guid> ehsDemucKHRespository,
            IRespository<EHS_NOIDUNG, Guid> ehsNoiDungRespository,
            IRespository<EHS_LUATDINH_DEMUC_KEHOACH, int> ehsLuatDinhDemucKHRespository,
            IRespository<EHS_NOIDUNG_KEHOACH, Guid> ehsNoiDungKeHoachRespository,
            IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _ehsDanhMucKHRespository = ehsDanhMucKHRespository;
            _ehsLuatDinhKHRespository = ehsLuatDinhKHRespository;
            _ehsDemucKHRespository = ehsDemucKHRespository;
            _ehsNoiDungRespository = ehsNoiDungRespository;
            _ehsLuatDinhDemucKHRespository = ehsLuatDinhDemucKHRespository;
            _ehsNoiDungKeHoachRespository = ehsNoiDungKeHoachRespository;
            _eventScheduleParentRepository = eventScheduleParentRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Guid DeleteDeMucKeHoach(Guid Id)
        {
            var en = _ehsDemucKHRespository.FindById(Id);
            if (en != null)
            {
                _ehsDemucKHRespository.Remove(en);
            }

            return Id;
        }

        public Guid DeleteDMKeHoach(Guid id)
        {
            _ehsDanhMucKHRespository.Remove(id);
            return id;
        }

        public EhsLuatDinhDeMucKeHoachViewModel DeleteLuatDinhDemucKeHoach(int id)
        {
            throw new NotImplementedException();
        }

        public EhsLuatDinhKeHoachViewModel DeleteLuatDinhKeHoach(int id)
        {
            throw new NotImplementedException();
        }

        public Guid DeleteNoiDung(Guid maNoiDung)
        {
            var nd = _ehsNoiDungRespository.FindById(maNoiDung);
            if (nd != null)
            {
                _ehsNoiDungRespository.Remove(nd);
            }

            return maNoiDung;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<EhsDeMucKeHoachViewModel> GetDeMucKeHoachByKeHoach(Guid? maKeHoach)
        {
            var lstNoidung = _mapper.Map<List<EHS_NOIDUNG>>(_ehsNoiDungRespository.FindAll(x => x.EHS_DM_KEHOACH.Id.Equals(maKeHoach), x => x.EHS_DM_KEHOACH, y => y.EHS_DEMUC_KEHOACH));
            List<EHS_DEMUC_KEHOACH> lstDemucKH = new List<EHS_DEMUC_KEHOACH>();
            foreach (var item in lstNoidung)
            {
                lstDemucKH.Add(item.EHS_DEMUC_KEHOACH);
            }

            return _mapper.Map<List<EhsDeMucKeHoachViewModel>>(lstDemucKH);
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

        public List<EhsNoiDungViewModel> GetNoiDungByKeHoach(Guid maKeHoach)
        {
            return _mapper.Map<List<EhsNoiDungViewModel>>(_ehsNoiDungRespository.FindAll(x => maKeHoach.Equals(x.MaKeHoach)));
        }

        public Guid UpdateDeMucKeHoach(Guid maDemuc, string tenDemuc)
        {
            var en = _ehsDemucKHRespository.FindById(maDemuc);
            if (en != null)
            {
                en.TenKeDeMuc_VN = tenDemuc;
                en.TenKeDeMuc_KR = tenDemuc;
                _ehsDemucKHRespository.Update(en);
            }

            return maDemuc;
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

        public Guid UpdateLuatDinhDeMucKeHoach(Guid maDemuc, string luatDinh)
        {
            var ld = _ehsLuatDinhDemucKHRespository.FindAll(x => x.MaDeMuc.Equals(maDemuc)).FirstOrDefault();
            if (ld != null)
            {
                ld.LuatDinhLienQuan = luatDinh;
                _ehsLuatDinhDemucKHRespository.Update(ld);
            }
            return maDemuc;
        }

        public EhsLuatDinhKeHoachViewModel UpdateLuatDinhKeHoach(EhsLuatDinhKeHoachViewModel model)
        {
            throw new NotImplementedException();
        }

        public Guid UpdateNoiDung(Guid maNoiDung, string noidung)
        {
            var nd = _ehsNoiDungRespository.FindById(maNoiDung);
            if (nd != null)
            {
                nd.NoiDung = noidung;
                _ehsNoiDungRespository.Update(nd);
            }

            return maNoiDung;
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

            if (model.MaKeHoachActive != null && model.EhsDMKeHoachViewModels.Any(x => x.Id == maKeHoach))
            {
                var lstDemuc = GetDeMucKeHoachByKeHoach(model.MaKeHoachActive);
                NoiDungKeHoachModel noidung;
                foreach (var item in lstDemuc)
                {
                    foreach (var nd in item.EHS_NOIDUNG)
                    {
                        noidung = new NoiDungKeHoachModel()
                        {
                            MaDeMucKH = nd.MaDeMucKH,
                            MaKeHoach = nd.MaKeHoach,
                            MaNoiDung = nd.Id,
                            NoiDung = nd.NoiDung,
                            TenKeDeMuc_VN = nd.EHS_DEMUC_KEHOACH.TenKeDeMuc_VN,
                            TenKeDeMuc_KR = nd.EHS_DEMUC_KEHOACH.TenKeDeMuc_KR
                        };
                        model.NoiDungKeHoachViewModels.Add(noidung);
                    }
                }
            }

            return model;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public ResultDB ImportExcel(string filePath, string maKH)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                if (maKH != "IMPORT_NOIDUNG_CHITIET")
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                        List<EHS_DEMUC_KEHOACH> lstDemucKeHoach = new List<EHS_DEMUC_KEHOACH>();
                        List<EHS_NOIDUNG> lstNoiDung = new List<EHS_NOIDUNG>();
                        List<EHS_LUATDINH_DEMUC_KEHOACH> lstLuatDinhDM = new List<EHS_LUATDINH_DEMUC_KEHOACH>();

                        string demuc = "";
                        string luatDinh = "";
                        string noidung = "";
                        Guid maDemuc = Guid.Empty;

                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {

                            demuc = worksheet.Cells[i, 1].Text.NullString();
                            luatDinh = worksheet.Cells[i, 2].Text.NullString();
                            noidung = worksheet.Cells[i, 3].Text.NullString();

                            if (!string.IsNullOrEmpty(demuc))
                            {
                                maDemuc = Guid.NewGuid();
                                EHS_DEMUC_KEHOACH DemucEn = new EHS_DEMUC_KEHOACH(maDemuc, demuc, demuc);
                                lstDemucKeHoach.Add(DemucEn);
                            }

                            EHS_LUATDINH_DEMUC_KEHOACH LuatDinhEn = new EHS_LUATDINH_DEMUC_KEHOACH();
                            LuatDinhEn.MaDeMuc = maDemuc;
                            LuatDinhEn.LuatDinhLienQuan = luatDinh;
                            lstLuatDinhDM.Add(LuatDinhEn);

                            EHS_NOIDUNG NoiDungEn = new EHS_NOIDUNG(Guid.NewGuid(), noidung, Guid.Parse(maKH), maDemuc);
                            lstNoiDung.Add(NoiDungEn);
                        }

                        _ehsDemucKHRespository.AddRange(lstDemucKeHoach);
                        _ehsLuatDinhDemucKHRespository.AddRange(lstLuatDinhDM);
                        _ehsNoiDungRespository.AddRange(lstNoiDung);

                        Save();

                        resultDB.ReturnInt = 0;
                        resultDB.ReturnString = "MSG_COM_004";

                        return resultDB;
                    }
                }
                else // IMPORT_NOIDUNG_CHITIET
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                        List<EHS_NOIDUNG_KEHOACH> lstNoiDungKH = new List<EHS_NOIDUNG_KEHOACH>();
                        List<EVENT_SHEDULE_PARENT> lstEvent = new List<EVENT_SHEDULE_PARENT>();

                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            if (worksheet.Cells[i, 1].Text.NullString() == "")
                            {
                                continue;
                            }

                            foreach (var ngayKDinh in worksheet.Cells[i, 9].Text.NullString().Split(",")) // 2022-06-10,2022-09-10
                            {
                                if (ngayKDinh.NullString() == "")
                                {
                                    continue;
                                }

                                EHS_NOIDUNG_KEHOACH noiDungKH = new EHS_NOIDUNG_KEHOACH();
                                noiDungKH.MaNoiDung = Guid.Parse(worksheet.Cells[i, 1].Text.NullString());
                                noiDungKH.NhaThau = worksheet.Cells[i, 3].Text.NullString();

                                noiDungKH.ChuKy = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero()) + "/" + worksheet.Cells[i, 5].Text.NullString();

                                noiDungKH.MaHieuMayKiemTra = worksheet.Cells[i, 6].Text.NullString();
                                noiDungKH.SoLuong = double.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());
                                noiDungKH.ViTri = worksheet.Cells[i, 8].Text.NullString();

                                if (ValidateCommon.DateTimeValid(ngayKDinh.NullString()))
                                {
                                    noiDungKH.NgayThucHien = ngayKDinh.NullString();
                                }
                                else
                                {
                                    throw new Exception("Ngày kiểm định phải định dạng : yyyy-MM-dd");
                                }

                                noiDungKH.ThoiGian_ThucHien = worksheet.Cells[i, 10].Text.IfNullIsZero();
                                noiDungKH.YeuCau = worksheet.Cells[i, 11].Text.NullString();
                                noiDungKH.NgayKhaiBaoThietBi = worksheet.Cells[i, 12].Text.NullString();
                                noiDungKH.ThoiGianThongBao = worksheet.Cells[i, 13].Text.IfNullIsZero() + "/" + worksheet.Cells[i, 14].Text.NullString();

                                noiDungKH.TienDoHoanThanh = double.Parse(worksheet.Cells[i, 15].Text.IfNullIsZero());
                                noiDungKH.SoTien = double.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                                noiDungKH.KetQua = worksheet.Cells[i, 17].Text.NullString();

                                noiDungKH.Id = Guid.NewGuid();
                                noiDungKH.Year = DateTime.Parse(ngayKDinh.NullString()).Year.NullString();
                                noiDungKH.UserCreated = GetUserId();
                                noiDungKH.UserModified = GetUserId();
                                lstNoiDungKH.Add(noiDungKH);

                                EVENT_SHEDULE_PARENT even = new EVENT_SHEDULE_PARENT();
                                even.Subject = worksheet.Cells[i, 2].Text.NullString();
                                even.MaNoiDungKH = noiDungKH.MaNoiDung;
                                even.StartEvent = noiDungKH.NgayThucHien;
                                even.EndEvent = noiDungKH.NgayThucHien;
                                even.StartTime = DateTime.Parse(noiDungKH.NgayThucHien);
                                even.EndTime = DateTime.Parse(noiDungKH.NgayThucHien);
                                even.TimeAlert = noiDungKH.ThoiGianThongBao;
                                even.Location = noiDungKH.ViTri;
                                even.IsAllDay = true;
                                even.Description = "Vendor: " + noiDungKH.NhaThau + "\n" + " || Date: " + noiDungKH.NgayThucHien + "\n" + " || Cost: " + noiDungKH.SoTien + "\n" + " || Result: " + noiDungKH.KetQua;
                                lstEvent.Add(even);
                            }
                        }

                        _ehsNoiDungKeHoachRespository.AddRange(lstNoiDungKH);
                        _eventScheduleParentRepository.AddRange(lstEvent);
                        Save();

                        resultDB.ReturnInt = 0;
                        resultDB.ReturnString = "MSG_COM_004";
                        return resultDB;
                    }
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public DeMucLuatDinh GetTenDeMucKeHoach(Guid maDemuc)
        {
            var en = _ehsDemucKHRespository.FindById(maDemuc);

            if (en != null)
            {
                var ld = _ehsLuatDinhDemucKHRespository.FindAll(x => x.MaDeMuc.Equals(en.Id)).FirstOrDefault();
                return new DeMucLuatDinh()
                {
                    TenDemuc = en.TenKeDeMuc_VN,
                    LuatDinh = ld?.LuatDinhLienQuan
                };
            }

            return null;
        }

        public string GetNoiDungKeHoach(Guid maNoiDung)
        {
            var en = _ehsNoiDungRespository.FindById(maNoiDung);
            if (en != null)
            {
                return en.NoiDung.NullString();
            }

            return "";
        }

        public List<EhsNoiDungKeHoachViewModel> GetNoiDungKeHoachByMaNoiDung(string maNoiDung)
        {
            return _mapper.Map<List<EhsNoiDungKeHoachViewModel>>(_ehsNoiDungKeHoachRespository.FindAll(x => Guid.Parse(maNoiDung).Equals(x.MaNoiDung), x => x.EHS_NOIDUNG));
        }

        public EhsNoiDungKeHoachViewModel GetNoiDungKeHoachById(string Id)
        {
            return _mapper.Map<EhsNoiDungKeHoachViewModel>(_ehsNoiDungKeHoachRespository.FindById(Guid.Parse(Id), x => x.EHS_NOIDUNG));
        }

        public EhsNoiDungKeHoachViewModel UpdateNoiDungKeHoach(EhsNoiDungKeHoachViewModel model)
        {
            model.UserModified = GetUserId();
            var en = _mapper.Map<EHS_NOIDUNG_KEHOACH>(model);
            _ehsNoiDungKeHoachRespository.Update(en);

            var even = _eventScheduleParentRepository.FindSingle(x => x.MaNoiDungKH.Equals(model.Id));
            string noidung = _ehsNoiDungRespository.FindById(model.MaNoiDung)?.NoiDung.NullString();
            if (even != null)
            {
                even.Subject = noidung;
                even.StartEvent = model.NgayThucHien;
                even.EndEvent = model.NgayThucHien;
                even.StartTime = DateTime.Parse(model.NgayThucHien);
                even.EndTime = DateTime.Parse(model.NgayThucHien);
                even.TimeAlert = model.ThoiGianThongBao;
                even.Location = model.ViTri;
                even.IsAllDay = true;
                even.Description = "Vendor: " + model.NhaThau + "\n" + " || Date: " + model.NgayThucHien + "\n" + " || Cost: " + model.SoTien + "\n" + " || Result: " + model.KetQua;
                even.UserModified = GetUserId();
                _eventScheduleParentRepository.Update(even);
            }
            else
            {
                even = new EVENT_SHEDULE_PARENT();
                even.Subject = noidung;
                even.MaNoiDungKH = model.Id;
                even.StartEvent = model.NgayThucHien;
                even.EndEvent = model.NgayThucHien;
                even.StartTime = DateTime.Parse(model.NgayThucHien);
                even.EndTime = DateTime.Parse(model.NgayThucHien);
                even.TimeAlert = model.ThoiGianThongBao;
                even.Location = model.ViTri;
                even.IsAllDay = true;
                even.Description = "Vendor: " + model.NhaThau + "\n" + " || Date: " + model.NgayThucHien + "\n" + " || Cost: " + model.SoTien + "\n" + " || Result: " + model.KetQua;
                even.UserCreated = GetUserId();
                _eventScheduleParentRepository.Add(even);
            }

            return model;
        }

        public EhsNoiDungKeHoachViewModel UpdateNoiDungKeHoachSingle(EhsNoiDungKeHoachViewModel model)
        {
            model.UserModified = GetUserId();
            var en = _mapper.Map<EHS_NOIDUNG_KEHOACH>(model);
            _ehsNoiDungKeHoachRespository.Update(en);
            return model;
        }

        public string DeleteNoiDungKeHoach(string Id)
        {
            var en = _ehsNoiDungKeHoachRespository.FindById(Guid.Parse(Id));
            _ehsNoiDungKeHoachRespository.Remove(en);
            return Id;
        }

        public TongHopKeHoachALL TongHopKeHoachByYear(string year)
        {
            TongHopKeHoachALL tongHopKeHoachALL = new TongHopKeHoachALL();
            List<TongHopKeHoachViewModel> result = new List<TongHopKeHoachViewModel>();
            var lstDMKehoach = GetDMKehoach();
            var lstEhsDemucKeHoach = _ehsDemucKHRespository.FindAll(x => x.EHS_NOIDUNG, y => y.EHS_LUATDINH_DEMUC_KEHOACH);
            var lstNoiDung = _ehsNoiDungRespository.FindAll(x => x.EHS_DM_KEHOACH, y => y.EHS_DEMUC_KEHOACH, h => h.EHS_NOIDUNG_KEHOACH, k => k.EHS_DEMUC_KEHOACH.EHS_LUATDINH_DEMUC_KEHOACH);

            TongHopKeHoachViewModel kehoachTongHop;
            foreach (var kehoach in lstDMKehoach)
            {
                kehoachTongHop = new TongHopKeHoachViewModel()
                {
                    MaKeHoach = kehoach.Id,
                    TenKeHoach = kehoach.TenKeHoach_VN + "\n" + kehoach.TenKeHoach_KR,
                    Year = year,
                    OrderItem = kehoach.OrderDM,
                    LuatDinhKeHoach = kehoach.EHS_LUATDINH_KEHOACH.FirstOrDefault()?.NoiDungLuatDinh.NullString()
                };

                foreach (var demucKH in lstNoiDung.ToList().
                                                    Where(x => x.MaKeHoach.Equals(kehoach.Id)).
                                                    Select(x => x.EHS_DEMUC_KEHOACH).Distinct())
                {
                    if (!kehoachTongHop.lstDeMucNoiDung.Any(x => x.MaDeMucKH.Equals(demucKH.Id)))
                    {
                        DemucKeHoach demuc = new DemucKeHoach()
                        {
                            MaKeHoach = kehoach.Id,
                            MaDeMucKH = demucKH.Id,
                            TenDemuc = demucKH.TenKeDeMuc_VN,
                            LuatDinh = demucKH.EHS_LUATDINH_DEMUC_KEHOACH.FirstOrDefault()?.LuatDinhLienQuan.NullString()
                        };

                        foreach (var noidung in lstNoiDung.ToList().Where(x => x.MaKeHoach.Equals(kehoachTongHop.MaKeHoach) && x.MaDeMucKH.Equals(demuc.MaDeMucKH)))
                        {
                            foreach (var item in noidung.EHS_NOIDUNG_KEHOACH)
                            {
                                item.EHS_NOIDUNG = null;
                            }

                            NoiDungDeMucKH noiDungDeMuc = new NoiDungDeMucKH()
                            {
                                MaNoiDung = noidung.Id,
                                NoiDung = noidung.NoiDung,
                                NoiDungChiTiets = _mapper.Map<List<EhsNoiDungKeHoachViewModel>>(noidung.EHS_NOIDUNG_KEHOACH.Where(x => x.Year == year))
                            };

                            if (noiDungDeMuc.NoiDungChiTiets.Count > 0)
                                demuc.NoiDungs.Add(noiDungDeMuc);
                        }

                        if (demuc.NoiDungs.Count > 0)
                            kehoachTongHop.lstDeMucNoiDung.Add(demuc);
                    }
                }

                if (kehoachTongHop.lstDeMucNoiDung.Count > 0)
                    result.Add(kehoachTongHop);
            }
            tongHopKeHoachALL.TongHopKeHoachViewModels = result;
            TotalByYear totalByYear = new TotalByYear()
            {
                Year = year
            };
            TotalAllItemByYear totalAllItem;
            foreach (var item in result)
            {
                foreach (var demuc in item.lstDeMucNoiDung)
                {
                    foreach (var noidung in demuc.NoiDungs)
                    {
                        if (!totalByYear.ItemByYears.Any(x => x.MaKeHoach.Equals(demuc.MaKeHoach) && x.MaDeMuc.Equals(demuc.MaDeMucKH) && x.MaNoiDung.Equals(noidung.MaNoiDung)))
                        {
                            totalAllItem = new TotalAllItemByYear()
                            {
                                MaKeHoach = demuc.MaKeHoach,
                                MaDeMuc = demuc.MaDeMucKH,
                                TenDeMuc = demuc.TenDemuc,
                                TenNoiDung = noidung.NoiDung,
                                MaNoiDung = noidung.MaNoiDung,
                                ChuKy = noidung.NoiDungChiTiets.FirstOrDefault().ChuKy,
                                NhaThau = noidung.NoiDungChiTiets.FirstOrDefault().NhaThau,
                                OrderItem = item.OrderItem
                            };
                        }
                        else
                        {
                            totalAllItem = totalByYear.ItemByYears.FirstOrDefault(x => x.MaKeHoach.Equals(demuc.MaKeHoach) && x.MaDeMuc.Equals(demuc.MaDeMucKH) && x.MaNoiDung.Equals(noidung.MaNoiDung));
                        }

                        foreach (var ndChitiet in noidung.NoiDungChiTiets)
                        {
                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "01") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_1 += ndChitiet.SoTien;
                                totalByYear.TMonth_1 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "02") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_2 += ndChitiet.SoTien;
                                totalByYear.TMonth_2 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "03") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_3 += ndChitiet.SoTien;
                                totalByYear.TMonth_3 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "04") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_4 += ndChitiet.SoTien;
                                totalByYear.TMonth_4 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "05") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_5 += ndChitiet.SoTien;
                                totalByYear.TMonth_5 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "06") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_6 += ndChitiet.SoTien;
                                totalByYear.TMonth_6 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "07") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_7 += ndChitiet.SoTien;
                                totalByYear.TMonth_7 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "08") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_8 += ndChitiet.SoTien;
                                totalByYear.TMonth_8 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "09") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_9 += ndChitiet.SoTien;
                                totalByYear.TMonth_9 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "10") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_10 += ndChitiet.SoTien;
                                totalByYear.TMonth_10 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "11") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_11 += ndChitiet.SoTien;
                                totalByYear.TMonth_11 += ndChitiet.SoTien;
                            }

                            if (ndChitiet.NgayThucHien.NullString().Contains("-") && ndChitiet.NgayThucHien.Split("-")[1].NullString() == "12") // yyyy-MM-dd 
                            {
                                totalAllItem.Month_12 += ndChitiet.SoTien;
                                totalByYear.TMonth_12 += ndChitiet.SoTien;
                            }
                        }

                        totalByYear.ItemByYears.Add(totalAllItem);
                    }
                }
            }
            totalByYear.ItemByYears.Sort((a, b) => a.OrderItem.CompareTo(b.OrderItem));
            tongHopKeHoachALL.TotalByYear = totalByYear;
            return tongHopKeHoachALL;
        }
    }
}
