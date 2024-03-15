using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Constants;
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
    public class NhanVienThaiSanService : BaseService, INhanVienThaiSanService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IRespository<HR_THAISAN_CONNHO, int> _thaisanRepository;
        private IRespository<HOTRO_SINH_LY, int> _hotroSinhLyRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienThaiSanService(IRespository<HR_THAISAN_CONNHO, int> thaisanRepository,
            IRespository<HOTRO_SINH_LY, int> hotroSinhLyRepository,
            IRespository<HR_NHANVIEN, string> nhanvienRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _thaisanRepository = thaisanRepository;
            _hotroSinhLyRepository = hotroSinhLyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _nhanvienRepository = nhanvienRepository;
        }

        public NhanVienThaiSanViewModel Add(NhanVienThaiSanViewModel nhanVienVm)
        {
            nhanVienVm.UserCreated = GetUserId();
            nhanVienVm.UserModified = GetUserId();

            var obj = _mapper.Map<HR_THAISAN_CONNHO>(nhanVienVm);
            _thaisanRepository.Add(obj);
            return nhanVienVm;
        }

        public void Delete(int id)
        {
            _thaisanRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVienThaiSanViewModel> GetAll()
        {
            var lst = _thaisanRepository.FindAll(x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
        }

        public NhanVienThaiSanViewModel GetById(int id, params Expression<Func<HR_THAISAN_CONNHO, object>>[] includeProperties)
        {
            var ent = _thaisanRepository.FindById(id, includeProperties);
            return _mapper.Map<NhanVienThaiSanViewModel>(ent);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<NhanVienThaiSanViewModel> Search(string maNV, string begin, string end, string chedo)
        {
            if (string.IsNullOrEmpty(maNV) && string.IsNullOrEmpty(begin) && string.IsNullOrEmpty(end))
            {
                var lst = _thaisanRepository.FindAll(x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);

                if (chedo.NullString() != "")
                {
                    lst = lst.Where(x => x.CheDoThaiSan.Contains(chedo)).OrderByDescending(x => x.DateModified);
                }

                return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
            }

            if (!string.IsNullOrEmpty(maNV))
            {
                if (!string.IsNullOrEmpty(begin) && !string.IsNullOrEmpty(end))
                {
                    var lst = _thaisanRepository.FindAll(x => x.MaNV == maNV && (begin.CompareTo(x.ToDate) <= 0 && end.CompareTo(x.FromDate) >= 0), x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                    if (chedo.NullString() != "")
                    {
                        lst = lst.Where(x => x.CheDoThaiSan.Contains(chedo)).OrderByDescending(x => x.DateModified);
                    }

                    return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
                }
                else
                {
                    var lst = _thaisanRepository.FindAll(x => x.MaNV == maNV, x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                    if (chedo.NullString() != "")
                    {
                        lst = lst.Where(x => x.CheDoThaiSan.Contains(chedo)).OrderByDescending(x => x.DateModified);
                    }

                    return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
                }
            }
            else
            {
                var lst = _thaisanRepository.FindAll(x => (begin.CompareTo(x.ToDate) <= 0 && end.CompareTo(x.FromDate) >= 0), x => x.HR_NHANVIEN).OrderByDescending(x => x.DateModified);
                if (chedo.NullString() != "")
                {
                    lst = lst.Where(x => x.CheDoThaiSan.Contains(chedo)).OrderByDescending(x => x.DateModified);
                }

                return _mapper.Map<List<NhanVienThaiSanViewModel>>(lst);
            }
        }

        public void Update(NhanVienThaiSanViewModel nhanVienVm)
        {
            nhanVienVm.UserModified = GetUserId();
            var entity = _mapper.Map<HR_THAISAN_CONNHO>(nhanVienVm);
            _thaisanRepository.Update(entity);
        }

        public List<HoTroSinhLyViewModel> GetHoTroSinhLy(string month, string boPhan)
        {
            List<HR_NHANVIEN> lstNhanvien = _nhanvienRepository.FindAll().ToList();
            List<HOTRO_SINH_LY> lstHoTroSly = _hotroSinhLyRepository.FindAll(x => x.Month == month).ToList();

            if (boPhan.NullString() != "")
            {
                lstHoTroSly = lstHoTroSly.Where(x => x.BoPhan == boPhan).ToList();
            }

            List<HoTroSinhLyViewModel> data = new List<HoTroSinhLyViewModel>();
            HoTroSinhLyViewModel htro;

            foreach (var item in lstHoTroSly)
            {
                htro = new HoTroSinhLyViewModel()
                {
                    Id = item.Id,
                    MaNV = item.MaNV,
                    Month = item.Month,
                    ThoiGianChuaNghi = item.ThoiGianChuaNghi
                };

                htro.TenNV = lstNhanvien.FirstOrDefault(x => x.Id == item.MaNV).TenNV;
                htro.BoPhan = lstNhanvien.FirstOrDefault(x => x.Id == item.MaNV).MaBoPhan;
                data.Add(htro);
            }

            return data;
        }

        public List<HoTroSinhLyViewModel> GetHoTroSinhLyImport(string month, string boPhan)
        {
            string endMonth = DateTime.Parse(month).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<HR_THAISAN_CONNHO> ThaiSanConNho = new List<HR_THAISAN_CONNHO>();
            ThaiSanConNho = _thaisanRepository.FindAll(x => x.FromDate.CompareTo(month) <= 0 && x.ToDate.CompareTo(endMonth) >= 0 && (x.CheDoThaiSan == "ThaiSan" || x.CheDoThaiSan == "MangBau")).ToList();
            List<HR_NHANVIEN> lstNhanvien = _nhanvienRepository.FindAll(x => ((x.NgayNghiViec != "" && x.NgayNghiViec.CompareTo(month) >= 0) || x.NgayNghiViec == "" || x.NgayNghiViec == null) && x.GioiTinh == "Female").ToList();

            foreach (var item in lstNhanvien.ToList())
            {
                if (ThaiSanConNho.Any(x => x.MaNV == item.Id) || item.GioiTinh == "Male")
                {
                    lstNhanvien.Remove(item);
                }
            }

            if (boPhan.NullString() != "")
            {
                lstNhanvien = lstNhanvien.Where(x => x.MaBoPhan == boPhan).ToList();
            }

            List<HoTroSinhLyViewModel> data = new List<HoTroSinhLyViewModel>();
            HoTroSinhLyViewModel htro;
            foreach (var item in lstNhanvien)
            {
                htro = new HoTroSinhLyViewModel()
                {
                    MaNV = item.Id,
                    TenNV = item.TenNV,
                    Month = month,
                    ThoiGianChuaNghi = 0
                };

                htro.BoPhan = lstNhanvien.FirstOrDefault(x => x.Id == item.Id).MaBoPhan;
                data.Add(htro);
            }

            return data;
        }


        /// <summary>
        /// Import hô tro sinh ly
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultDB ImportExcel(string filePath, string param)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ResultDB resultDB = new ResultDB();
                try
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    HOTRO_SINH_LY hotrosly = null;

                    string maNV = "";
                    string month = "";

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        maNV = worksheet.Cells[i, 1].Text.NullString();
                        if (maNV == "")
                        {
                            resultDB.ReturnInt = -1;
                            return resultDB;
                        }

                        month = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString()).ToString("yyyy-MM-01");
                        hotrosly = _hotroSinhLyRepository.FindSingle(x => x.MaNV == maNV && x.Month == month);

                        if (hotrosly == null)
                        {
                            hotrosly = new HOTRO_SINH_LY();
                            hotrosly.MaNV = maNV;
                            hotrosly.BoPhan = worksheet.Cells[i, 3].Text.NullString();
                            hotrosly.ThoiGianChuaNghi = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                            hotrosly.Month = month;

                            _hotroSinhLyRepository.Add(hotrosly);
                        }
                        else
                        {
                            hotrosly.MaNV = maNV;
                            hotrosly.BoPhan = worksheet.Cells[i, 3].Text.NullString();
                            hotrosly.ThoiGianChuaNghi = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                            hotrosly.Month = month;
                            _hotroSinhLyRepository.Update(hotrosly);
                        }
                    }
                    resultDB.ReturnInt = 0;
                    return resultDB;
                }
                catch (Exception ex)
                {
                    resultDB.ReturnInt = -1;
                    resultDB.ReturnString = ex.Message;

                    return resultDB;
                }
            }
        }

        public ResultDB ImportThaiSanExcel(string filePath, string param)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ResultDB resultDB = new ResultDB();
                try
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    HR_THAISAN_CONNHO thaisan = null;

                    string maNV = "";
                    DateTime fromdate, todate;
                    List<HR_THAISAN_CONNHO> lstThaiSanConNho = new List<HR_THAISAN_CONNHO>();
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        maNV = worksheet.Cells[i, 1].Text.NullString();
                        if (maNV == "")
                        {
                            resultDB.ReturnInt = -1;
                            return resultDB;
                        }

                        thaisan = new HR_THAISAN_CONNHO();
                        thaisan.MaNV = maNV;
                        thaisan.CheDoThaiSan = worksheet.Cells[i, 3].Text.NullString();

                        if (DateTime.TryParse(worksheet.Cells[i, 4].Text.NullString(), out fromdate))
                        {
                            thaisan.FromDate = fromdate.ToString("yyyy-MM-dd");
                        }

                        if (DateTime.TryParse(worksheet.Cells[i, 5].Text.NullString(), out todate))
                        {
                            thaisan.ToDate = todate.ToString("yyyy-MM-dd");
                        }

                        lstThaiSanConNho.Add(thaisan);
                    }

                    if (lstThaiSanConNho.Count > 0)
                    {
                        _thaisanRepository.AddRange(lstThaiSanConNho);
                    }
                    resultDB.ReturnInt = 0;
                    return resultDB;
                }
                catch (Exception ex)
                {
                    resultDB.ReturnInt = -1;
                    resultDB.ReturnString = ex.Message;

                    return resultDB;
                }
            }
        }

        public HoTroSinhLyViewModel AddHotrosinhly(HoTroSinhLyViewModel model)
        {
            HOTRO_SINH_LY hotrosinhly = new HOTRO_SINH_LY();

            hotrosinhly.MaNV = model.MaNV;
            hotrosinhly.Month = model.Month;
            hotrosinhly.ThoiGianChuaNghi = model.ThoiGianChuaNghi;
            hotrosinhly.BoPhan = model.BoPhan;
            hotrosinhly.UserModified = GetUserId();

            if (_hotroSinhLyRepository.FindSingle(x => x.MaNV == hotrosinhly.MaNV && x.Month == hotrosinhly.Month) == null)
            {
                _hotroSinhLyRepository.Add(hotrosinhly);
            }
            else
            {
                HOTRO_SINH_LY obj = _hotroSinhLyRepository.FindSingle(x => x.MaNV == hotrosinhly.MaNV && x.Month == hotrosinhly.Month);
                obj.ThoiGianChuaNghi = hotrosinhly.ThoiGianChuaNghi;
                obj.BoPhan = hotrosinhly.BoPhan;
                obj.UserModified = GetUserId();

                _hotroSinhLyRepository.Update(obj);
            }

            Save();
            return model;
        }

        public HoTroSinhLyViewModel EditHotrosinhly(HoTroSinhLyViewModel model)
        {
            HOTRO_SINH_LY hOTRO_SINH_LY = _hotroSinhLyRepository.FindById(model.Id);

            if (hOTRO_SINH_LY != null)
            {
                hOTRO_SINH_LY.ThoiGianChuaNghi = model.ThoiGianChuaNghi;
                hOTRO_SINH_LY.UserModified = GetUserId();
                _hotroSinhLyRepository.Update(hOTRO_SINH_LY);

                Save();
            }
            return model;
        }

        public void DeleteHotrosinhly(int id)
        {
            HOTRO_SINH_LY hOTRO_SINH_LY = _hotroSinhLyRepository.FindById(id);
            if (hOTRO_SINH_LY != null)
            {
                _hotroSinhLyRepository.Remove(hOTRO_SINH_LY);
            }

            Save();
        }
    }
}
