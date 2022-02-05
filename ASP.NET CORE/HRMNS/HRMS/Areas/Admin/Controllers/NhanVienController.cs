﻿using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class NhanVienController : AdminBaseController
    {
        INhanVienService _nhanvienService;
        IBoPhanService _boPhanService;
        IBoPhanDetailService _boPhanDetailService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NhanVienController(INhanVienService nhanVienService, IBoPhanService boPhanService, IBoPhanDetailService boPhanDetailService, IWebHostEnvironment hostingEnvironment)
        {
            _nhanvienService = nhanVienService;
            _boPhanService = boPhanService;
            _hostingEnvironment = hostingEnvironment;
            _boPhanDetailService = boPhanDetailService;
        }

        public IActionResult Index()
        {
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return View(nhanviens);
        }

        [HttpGet]
        public IActionResult OnGetPartialData()
        {
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return PartialView("_NhanVienGridPartial", nhanviens);
        }

        [HttpPost]
        public IActionResult SaveEmployee(NhanVienCustomizeViewModel nhanvienVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                bool isAdd = nhanvienVm.Action == CommonConstants.Add_Action;
                bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == nhanvienVm.NhanVien.Id);
                //bool notExist = nv == null;

                if (isAdd && notExist)
                {
                    _nhanvienService.Add(nhanvienVm.NhanVien);
                }
                else if (isAdd && !notExist)
                {
                    return new ConflictObjectResult(CommonConstants.ConflictObjectResult_Msg);
                }
                else if (!isAdd && notExist)
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }
                else if (!isAdd && !notExist)
                {
                    NhanVienViewModel nhanVien = _nhanvienService.GetById(nhanvienVm.NhanVien.Id);
                    nhanVien.TenNV = nhanvienVm.NhanVien.TenNV;
                    nhanVien.GioiTinh = nhanvienVm.NhanVien.GioiTinh;
                    nhanVien.Email = nhanvienVm.NhanVien.Email;
                    nhanVien.SoDienThoai = nhanvienVm.NhanVien.SoDienThoai;
                    nhanVien.NgayVao = nhanvienVm.NhanVien.NgayVao;
                    nhanVien.MaBoPhan = nhanvienVm.NhanVien.MaBoPhan;
                    nhanVien.MaChucDanh = nhanvienVm.NhanVien.MaChucDanh;

                    _nhanvienService.UpdateSingle(nhanVien);
                }

                _nhanvienService.Save();

                return new OkObjectResult(nhanvienVm.NhanVien);
            }
        }

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _nhanvienService.ImportExcel(filePath);
                _nhanvienService.Save();

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }
                return new OkObjectResult(filePath);
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Nhanvien_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var nhanviens = _nhanvienService.GetAll().Where(x => x.Status.NullString() != Status.InActive.ToString()).Select(x => new
            {
                x.Id,
                x.TenNV,
                x.MaBoPhan,
                x.MaChucDanh,
                x.GioiTinh,
                x.NgaySinh,
                x.NoiSinh,
                x.TinhTrangHonNhan,
                x.DanToc,
                x.TonGiao,
                x.DiaChiThuongTru,
                x.SoDienThoai,
                x.SoDienThoaiNguoiThan,
                x.Email,
                x.CMTND,
                x.SoTaiKhoanNH,
                x.TenNganHang,
                x.TruongDaoTao,
                x.NgayVao,
                x.NguyenQuan,
                x.DChiHienTai,
                x.MaBHXH,
                x.MaSoThue,
                x.Status
            });
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                worksheet.Cells["A1"].LoadFromCollection(nhanviens, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == Id);
            if (notExist)
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
            _nhanvienService.Delete(Id);
            _nhanvienService.Save();
            return new OkObjectResult(null);
        }

        [HttpGet]
        public IActionResult GetById(string Id)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            return new OkObjectResult(nhanVien);
        }

        [HttpGet]
        public IActionResult Profile(string Id)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id,
                                                                    i => i.BOPHAN,
                                                                    x => x.HR_BO_PHAN_DETAIL,
                                                                    y => y.HR_TINHTRANGHOSO,
                                                                    o => o.HR_BHXH,
                                                                    p => p.HR_QUATRINHLAMVIEC,
                                                                    q => q.HR_PHEP_NAM,
                                                                    k => k.HR_HOPDONG);
            if (nhanVien != null)
            {
                NhanVienProfileModel profileModel = new NhanVienProfileModel();

                // Thong tin chung
                profileModel.MaNhanVien = nhanVien.Id;
                profileModel.Avartar = nhanVien.Image;
                profileModel.TenNhanVien = nhanVien.TenNV;
                profileModel.BoPhan = nhanVien.MaBoPhan;
                profileModel.BoPhanDetail = nhanVien.HR_BO_PHAN_DETAIL?.TenBoPhanChiTiet;
                profileModel.ChucDanh = nhanVien.MaChucDanh;
                profileModel.NgayVaoCongTy = nhanVien.NgayVao;
                profileModel.Phone = nhanVien.SoDienThoai;
                profileModel.Email = nhanVien.Email;
                profileModel.Birthday = nhanVien.NgaySinh;
                profileModel.DCHienTai = nhanVien.DChiHienTai;
                profileModel.GioiTinh = nhanVien.GioiTinh;
                profileModel.Status = nhanVien.Status.ToString();

                // So yeu li lich
                profileModel.NoiSinh = nhanVien.NoiSinh;
                profileModel.NguyenQuan = nhanVien.NguyenQuan;
                profileModel.DiaChiThuongTru = nhanVien.DiaChiThuongTru;
                profileModel.DanToc = nhanVien.DanToc;
                profileModel.TonGiao = nhanVien.TonGiao;
                profileModel.CMTND = nhanVien.CMTND;
                profileModel.NgayCapCMTND = nhanVien.NgayCapCMTND;
                profileModel.NoiCapCMTND = nhanVien.NoiCapCMTND;
                profileModel.MaSoThue = nhanVien.MaSoThue;
                profileModel.SoNguoiGiamTru = nhanVien.SoNguoiGiamTru;
                profileModel.TinhTrangHonNhan = nhanVien.TinhTrangHonNhan;
                profileModel.TruongDaoTao = nhanVien.TruongDaoTao;
                profileModel.Note = nhanVien.Note;

                // Nghỉ viêc 
                profileModel.NgayNghiViec = nhanVien.NgayNghiViec;

                // Lien lac
                profileModel.SoDienThoaiNguoiThan = nhanVien.SoDienThoaiNguoiThan;
                profileModel.QuanHeNguoiThan = nhanVien.QuanHeNguoiThan;

                // Bank Info
                profileModel.TenNganHang = nhanVien.TenNganHang;
                profileModel.SoTaiKhoanNH = nhanVien.SoTaiKhoanNH;

                // Tinh Trang Ho So --
                TinhTrangHoSoViewModel tthsModel = nhanVien.HR_TINHTRANGHOSO.FirstOrDefault();

                if (tthsModel != null)
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        AnhThe = tthsModel.AnhThe,
                        SoYeuLyLich = tthsModel.SoYeuLyLich,
                        CMTND = tthsModel.CMTND,
                        SoHoKhau = tthsModel.SoHoKhau,
                        GiayKhaiSinh = tthsModel.GiayKhaiSinh,
                        BangTotNghiep = tthsModel.BangTotNghiep,
                        XacNhanDanSu = tthsModel.XacNhanDanSu
                    };
                }

                // Ky Luat Lao Dong 
                profileModel.KyLuatLaoDong = nhanVien.KyLuatLD;

                // Bao Hiem
                BHXHViewModel bHXH = nhanVien.HR_BHXH.FirstOrDefault();
                if (bHXH != null)
                {
                    profileModel.bHXHs = new BHXHViewModel()
                    {
                        NgayThamGia = bHXH.NgayThamGia,
                        NgayKetThuc = bHXH.NgayKetThuc,
                        Id = bHXH.Id
                    };
                }

                // Bang cap , chung chi
                profileModel.chungChis = nhanVien.HR_CHUNGCHI_NHANVIEN.ToList();

                // Qua Trinh Lam Viec --
                profileModel.quaTrinhLamViecs = nhanVien.HR_QUATRINHLAMVIEC.ToList();

                profileModel.phepNams = nhanVien.HR_PHEP_NAM.ToList();

                // --
                profileModel.hopDongs = nhanVien.HR_HOPDONG.ToList();

                return View(profileModel);
            }
            else
            {
                return Redirect("/Admin/Error/Index?id=" + CommonConstants.NotFound);
            }
        }

        [HttpPost]
        public IActionResult SaveAvatar(string Id, string url)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            nhanVien.Image = url;
            _nhanvienService.UpdateSingle(nhanVien);
            _nhanvienService.Save();

            return new OkObjectResult(null);
        }

        [HttpPost]
        public IActionResult UpdateProfileBasic(NhanVienProfileModel profileBasic, string model)
        {
            string partialName = "";
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == profileBasic.MaNhanVien);
                if (notExist)
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }
                else
                {
                    NhanVienViewModel nhanVien = _nhanvienService.GetById(profileBasic.MaNhanVien);

                    // Update for basic info
                    if (model == "1")
                    {
                        nhanVien.TenNV = profileBasic.TenNhanVien;
                        if (DateTime.TryParse(profileBasic.Birthday, out var ngaysinh))
                        {
                            nhanVien.NgaySinh = ngaysinh.ToString("dd/MM/yyyy");
                            profileBasic.Birthday = ngaysinh.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            nhanVien.NgaySinh = "";
                        }

                        nhanVien.GioiTinh = profileBasic.GioiTinh;
                        nhanVien.Email = profileBasic.Email;
                        nhanVien.SoDienThoai = profileBasic.Phone;

                        if (DateTime.TryParse(profileBasic.NgayVaoCongTy, out var ngayvao))
                        {
                            nhanVien.NgayVao = ngayvao.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            nhanVien.NgayVao = "";
                        }

                        nhanVien.MaBoPhan = profileBasic.BoPhan;
                        nhanVien.MaChucDanh = profileBasic.ChucDanh;
                        nhanVien.MaBoPhanChiTiet = profileBasic.MaBoPhanDetail;

                        profileBasic.BoPhanDetail = _boPhanDetailService.GetAll(null).FirstOrDefault(x => x.Id == profileBasic.MaBoPhanDetail).TenBoPhanChiTiet;

                        nhanVien.DChiHienTai = profileBasic.DCHienTai;
                        nhanVien.TinhTrangHonNhan = profileBasic.TinhTrangHonNhan;
                        nhanVien.Status = profileBasic.Status;

                        partialName = "_profilebasicPartial";
                    }
                    else if (model == "2") // Update for personal info
                    {
                        nhanVien.CMTND = profileBasic.CMTND;

                        if (DateTime.TryParse(profileBasic.NgayCapCMTND, out var ngaycap))
                        {
                            nhanVien.NgayCapCMTND = ngaycap.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            nhanVien.NgayCapCMTND = "";
                        }

                        nhanVien.NoiCapCMTND = profileBasic.NoiCapCMTND;
                        nhanVien.DanToc = profileBasic.DanToc;
                        nhanVien.TonGiao = profileBasic.TonGiao;
                        nhanVien.MaSoThue = profileBasic.MaSoThue;
                        nhanVien.NoiSinh = profileBasic.NoiSinh;
                        nhanVien.NguyenQuan = profileBasic.NguyenQuan;
                        nhanVien.DiaChiThuongTru = profileBasic.DiaChiThuongTru;
                        nhanVien.TruongDaoTao = profileBasic.TruongDaoTao;

                        partialName = "_profilePersonalInfoPartial";
                    }
                    else if (model == "3") // Update Bank Info
                    {
                        nhanVien.TenNganHang = profileBasic.TenNganHang;
                        nhanVien.SoTaiKhoanNH = profileBasic.SoTaiKhoanNH;

                        partialName = "_profileBankInfoPartial";
                    }
                    else if(model == "4") // Update liên lạc người thân
                    {
                        nhanVien.QuanHeNguoiThan = profileBasic.QuanHeNguoiThan;
                        nhanVien.SoDienThoaiNguoiThan = profileBasic.SoDienThoaiNguoiThan;

                        partialName = "_profileEmergencyContactPartial";
                    }

                    _nhanvienService.UpdateSingle(nhanVien);
                }

                _nhanvienService.Save();
            }

            return PartialView(partialName, profileBasic);
        }

        [HttpGet]
        public IActionResult GetProfile(string Id)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id,
                                                                   i => i.BOPHAN,
                                                                   x => x.HR_BO_PHAN_DETAIL,
                                                                   y => y.HR_TINHTRANGHOSO,
                                                                   o => o.HR_BHXH,
                                                                   p => p.HR_QUATRINHLAMVIEC,
                                                                   q => q.HR_PHEP_NAM,
                                                                   k => k.HR_HOPDONG);
            if (nhanVien != null)
            {
                NhanVienProfileModel profileModel = new NhanVienProfileModel();

                // Thong tin chung
                profileModel.MaNhanVien = nhanVien.Id;
                profileModel.Avartar = nhanVien.Image;
                profileModel.TenNhanVien = nhanVien.TenNV;
                profileModel.BoPhan = nhanVien.MaBoPhan;
                profileModel.MaBoPhanDetail = nhanVien.MaBoPhanChiTiet;
                profileModel.ChucDanh = nhanVien.MaChucDanh;
                profileModel.NgayVaoCongTy = nhanVien.NgayVao;
                profileModel.Phone = nhanVien.SoDienThoai;
                profileModel.Email = nhanVien.Email;
                profileModel.Birthday = nhanVien.NgaySinh;
                profileModel.DCHienTai = nhanVien.DChiHienTai;
                profileModel.GioiTinh = nhanVien.GioiTinh;
                profileModel.Status = nhanVien.Status.ToString();

                // So yeu li lich
                profileModel.NoiSinh = nhanVien.NoiSinh;
                profileModel.NguyenQuan = nhanVien.NguyenQuan;
                profileModel.DiaChiThuongTru = nhanVien.DiaChiThuongTru;
                profileModel.DanToc = nhanVien.DanToc;
                profileModel.TonGiao = nhanVien.TonGiao;
                profileModel.CMTND = nhanVien.CMTND;
                profileModel.NgayCapCMTND = nhanVien.NgayCapCMTND;
                profileModel.NoiCapCMTND = nhanVien.NoiCapCMTND;
                profileModel.MaSoThue = nhanVien.MaSoThue;
                profileModel.SoNguoiGiamTru = nhanVien.SoNguoiGiamTru;
                profileModel.TinhTrangHonNhan = nhanVien.TinhTrangHonNhan;
                profileModel.TruongDaoTao = nhanVien.TruongDaoTao;
                profileModel.Note = nhanVien.Note;

                // Nghỉ viêc 
                profileModel.NgayNghiViec = nhanVien.NgayNghiViec;

                // Lien lac
                profileModel.SoDienThoaiNguoiThan = nhanVien.SoDienThoaiNguoiThan;
                profileModel.QuanHeNguoiThan = nhanVien.QuanHeNguoiThan;

                // Bank Info
                profileModel.TenNganHang = nhanVien.TenNganHang;
                profileModel.SoTaiKhoanNH = nhanVien.SoTaiKhoanNH;

                // Tinh Trang Ho So --
                TinhTrangHoSoViewModel tthsModel = nhanVien.HR_TINHTRANGHOSO.FirstOrDefault();

                if (tthsModel != null)
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        AnhThe = tthsModel.AnhThe,
                        SoYeuLyLich = tthsModel.SoYeuLyLich,
                        CMTND = tthsModel.CMTND,
                        SoHoKhau = tthsModel.SoHoKhau,
                        GiayKhaiSinh = tthsModel.GiayKhaiSinh,
                        BangTotNghiep = tthsModel.BangTotNghiep,
                        XacNhanDanSu = tthsModel.XacNhanDanSu
                    };
                }

                // Ky Luat Lao Dong 
                profileModel.KyLuatLaoDong = nhanVien.KyLuatLD;

                // Bao Hiem
                BHXHViewModel bHXH = nhanVien.HR_BHXH.FirstOrDefault();
                if (bHXH != null)
                {
                    profileModel.bHXHs = new BHXHViewModel()
                    {
                        NgayThamGia = bHXH.NgayThamGia,
                        NgayKetThuc = bHXH.NgayKetThuc,
                        Id = bHXH.Id
                    };
                }

                // Bang cap , chung chi
                profileModel.chungChis = nhanVien.HR_CHUNGCHI_NHANVIEN.ToList();

                // Qua Trinh Lam Viec --
                profileModel.quaTrinhLamViecs = nhanVien.HR_QUATRINHLAMVIEC.ToList();

                profileModel.phepNams = nhanVien.HR_PHEP_NAM.ToList();

                // --
                profileModel.hopDongs = nhanVien.HR_HOPDONG.ToList();

                return new OkObjectResult(profileModel);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }
    }
}
