using HRMNS.Application.Interfaces;
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
using System.Globalization;
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
        IBHXHService _bhxhService;
        ITinhTrangHoSoService _ttrangHoSoService;
        IHopDongService _hopDongService;
        IHRLoaiHopDongService _hrLoaiHDService;
        IQuatrinhLamViecService _quatrinhLamViecService;
        IPhepNamService _phepNamService;
        IKeKhaiBaoHiemService _keKhaiBHService;
        ICheDoBHService _cheDoBHService;
        ITrainingListService _trainingListService;
        ITrainingTypeService _trainingTypeService;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public NhanVienController(INhanVienService nhanVienService,
            IBoPhanService boPhanService,
            IBoPhanDetailService boPhanDetailService,
            IBHXHService bHXHService,
            ITinhTrangHoSoService tinhTrangHoSoService,
            IHopDongService hopDongService,
            IHRLoaiHopDongService hrLoaiHDService,
            IQuatrinhLamViecService qtrinhLvService,
            IPhepNamService phepNamService,
            IKeKhaiBaoHiemService keKhaiBaoHiemService,
            ICheDoBHService cheDoBHService,
             ITrainingListService trainingListService,
             ITrainingTypeService trainingTypeService,
        IWebHostEnvironment hostingEnvironment)
        {
            _trainingListService = trainingListService;
            _trainingTypeService = trainingTypeService;
            _nhanvienService = nhanVienService;
            _boPhanService = boPhanService;
            _hostingEnvironment = hostingEnvironment;
            _boPhanDetailService = boPhanDetailService;
            _bhxhService = bHXHService;
            _ttrangHoSoService = tinhTrangHoSoService;
            _hopDongService = hopDongService;
            _hrLoaiHDService = hrLoaiHDService;
            _quatrinhLamViecService = qtrinhLvService;
            _phepNamService = phepNamService;
            _keKhaiBHService = keKhaiBaoHiemService;
            _cheDoBHService = cheDoBHService;
        }

        public IActionResult Index()
        {
            string status = Status.Active.ToString();
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll(x=>x.HR_HOPDONG).Where(x => x.Status == status && x.MaBoPhan != "KOREA").ToList();
            var section = _boPhanDetailService.GetAll(null);
            foreach (var item in nhanviens)
            {
                if (section.Any(x => x.Id == item.MaBoPhanChiTiet))
                {
                    item.HR_BO_PHAN_DETAIL = section.Find(x => x.Id == item.MaBoPhanChiTiet);
                }
            }

            return View(nhanviens);
        }
        /// <summary>
        /// Danh sách nhan viên nghỉ việc
        /// </summary>
        /// <returns></returns>
        public IActionResult NhanVienNghiViec()
        {
            ViewData["Title"] = "Danh sách nhân viên nghỉ việc";
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            string status = Status.InActive.ToString();
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll(x => x.HR_HOPDONG).Where(x=>x.Status == status).ToList();
            var section = _boPhanDetailService.GetAll(null);
            foreach (var item in nhanviens)
            {
                if (section.Any(x => x.Id == item.MaBoPhanChiTiet))
                {
                    item.HR_BO_PHAN_DETAIL = section.Find(x => x.Id == item.MaBoPhanChiTiet);
                }
            }

            return View("Index",nhanviens);
        }

        /// <summary>
        /// Danh sách nhan viên hàn quốc
        /// </summary>
        /// <returns></returns>
        public IActionResult NhanVienKorea()
        {
            string datetime = DateTime.Now.ToString("yyyy-MM");
            datetime = datetime + "-01";
            ViewData["Title"] = "Danh sách người Hàn";
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            string status = Status.InActive.ToString();
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll(x => x.HR_HOPDONG).Where(x => x.MaBoPhan == "KOREA" && (x.Status != status || string.Compare(datetime, x.NgayNghiViec) <= 0)).ToList();
            var section = _boPhanDetailService.GetAll(null);
            foreach (var item in nhanviens)
            {
                if (section.Any(x => x.Id == item.MaBoPhanChiTiet))
                {
                    item.HR_BO_PHAN_DETAIL = section.Find(x => x.Id == item.MaBoPhanChiTiet);
                }
            }

            return View("Index", nhanviens);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return new OkObjectResult(nhanviens);
        }

        [HttpGet]
        public IActionResult GetAllActive()
        {
            string datetime = DateTime.Now.ToString("yyyy-MM");
            datetime = datetime + "-01";
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll().Where(x=>x.Status != Status.InActive.NullString() || string.Compare(datetime,x.NgayNghiViec) <= 0).ToList();
            return new OkObjectResult(nhanviens);
        }

        [HttpGet]
        public IActionResult OnGetPartialData()
        {
            string status = Status.Active.ToString();
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll(x => x.HR_HOPDONG).Where(x => x.Status == status).ToList();
            var section = _boPhanDetailService.GetAll(null);
            foreach (var item in nhanviens)
            {
                if (section.Any(x => x.Id == item.MaBoPhanChiTiet))
                {
                    item.HR_BO_PHAN_DETAIL = section.Find(x => x.Id == item.MaBoPhanChiTiet);
                }
            }
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
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, [FromQuery] string param)
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
                _nhanvienService.ImportExcel(filePath, param);
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
                worksheet.Cells["A1"].LoadFromCollection(nhanviens, true, TableStyles.Light11);
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
                                                                    k => k.HR_HOPDONG,
                                                                    l => l.HR_KEKHAIBAOHIEM,
                                                                    h=>h.TRAINING_NHANVIEN);
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
                TinhTrangHoSoViewModel tthsModel = nhanVien.HR_TINHTRANGHOSO.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                if (tthsModel != null)
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        Id = tthsModel.Id,
                        MaNV = nhanVien.Id,
                        AnhThe = tthsModel.AnhThe,
                        SoYeuLyLich = tthsModel.SoYeuLyLich,
                        CMTND = tthsModel.CMTND,
                        SoHoKhau = tthsModel.SoHoKhau,
                        GiayKhaiSinh = tthsModel.GiayKhaiSinh,
                        BangTotNghiep = tthsModel.BangTotNghiep,
                        XacNhanDanSu = tthsModel.XacNhanDanSu
                    };
                }
                else
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        MaNV = nhanVien.Id,
                        AnhThe = false,
                        SoYeuLyLich = false,
                        CMTND = false,
                        SoHoKhau = false,
                        GiayKhaiSinh = false,
                        BangTotNghiep = false,
                        XacNhanDanSu = false
                    };
                }

                // Ky Luat Lao Dong 
                profileModel.KyLuatLaoDong = nhanVien.KyLuatLD;

                // Bao Hiem
                BHXHViewModel bHXH = nhanVien.HR_BHXH.ToList().OrderByDescending(x => x.DateModified).FirstOrDefault();
                if (bHXH != null)
                {
                    profileModel.bHXHs = new BHXHViewModel()
                    {
                        NgayThamGia = bHXH.NgayThamGia,
                        NgayKetThuc = bHXH.NgayKetThuc,
                        Id = bHXH.Id
                    };

                    profileModel.MaBHXH = bHXH.Id;
                    profileModel.NgayThamGia = bHXH.NgayThamGia;
                    profileModel.NgayKetThuc = bHXH.NgayKetThuc;
                }

                // Bang cap , chung chi
                profileModel.chungChis = nhanVien.HR_CHUNGCHI_NHANVIEN.ToList();

                // Qua Trinh Lam Viec --
                profileModel.quaTrinhLamViecs = nhanVien.HR_QUATRINHLAMVIEC.OrderByDescending(x => x.ThơiGianBatDau).ToList();
                if (profileModel.quaTrinhLamViecs.Count() == 0)
                {
                    profileModel.quaTrinhLamViecs = new List<QuaTrinhLamViecViewModel>()
                    {   new QuaTrinhLamViecViewModel()
                        {
                            Id= 0,
                            ThơiGianBatDau = "",
                            TieuDe = "",
                            Note = "",
                            MaNV = profileModel.MaNhanVien
                        }
                    };
                }

                profileModel.phepNams = nhanVien.HR_PHEP_NAM.OrderByDescending(x => x.Year).Take(2).ToList();
                if (profileModel.phepNams.Count() == 0)
                {
                    profileModel.phepNams = new List<PhepNamViewModel>()
                    {
                        new PhepNamViewModel()
                        {
                            MaNhanVien = Id,
                            SoPhepNam = 0,
                            SoPhepConLai = 0,
                            Year = DateTime.Now.Year
                        },
                        new PhepNamViewModel()
                        {
                            MaNhanVien = Id,
                            SoPhepNam = 0,
                            SoPhepConLai = 0,
                            Year = DateTime.Now.Year - 1
                        }
                    };
                }

                // --
                profileModel.hopDongs = nhanVien.HR_HOPDONG.OrderByDescending(x => x.NgayKy).ToList();
                if (profileModel.hopDongs == null || profileModel.hopDongs.Count() == 0)
                {
                    profileModel.hopDongs = new List<HopDongViewModel>()
                    {
                        new HopDongViewModel()
                        {
                            MaNV =profileModel.MaNhanVien
                        }
                    };
                }

                profileModel.kekhaibaohiems = nhanVien.HR_KEKHAIBAOHIEM.ToList();
                if (profileModel.kekhaibaohiems == null || profileModel.kekhaibaohiems.Count() == 0)
                {
                    profileModel.kekhaibaohiems = new List<KeKhaiBaoHiemViewModel>()
                    {
                        new KeKhaiBaoHiemViewModel()
                        {
                            MaNV =profileModel.MaNhanVien
                        }
                    };
                }

                var chedo = _cheDoBHService.GetAll(null);
                foreach (var item in profileModel.kekhaibaohiems)
                {
                    item.HR_CHEDOBH = chedo.Find(x => x.Id == item.CheDoBH);
                }

                profileModel.training_NhanVienViewModels = nhanVien.TRAINING_NHANVIEN.ToList();

                if (profileModel.training_NhanVienViewModels == null || profileModel.training_NhanVienViewModels.Count() == 0)
                {
                    profileModel.training_NhanVienViewModels = new List<Training_NhanVienViewModel>()
                    {
                        new Training_NhanVienViewModel(profileModel.MaNhanVien,Guid.NewGuid())
                    };
                }
                else
                {
                    foreach (var nvTrain in profileModel.training_NhanVienViewModels)
                    {
                       nvTrain.HR_TRAINING = _trainingListService.GetById(nvTrain.TrainnigId);
                    }
                }


                ViewBag.LoaiHD = _hrLoaiHDService.GetAll();

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
                            nhanVien.NgaySinh = ngaysinh.ToString("yyyy-MM-dd");
                            profileBasic.Birthday = ngaysinh.ToString("yyyy-MM-dd");
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
                            nhanVien.NgayVao = ngayvao.ToString("yyyy-MM-dd");
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
                            nhanVien.NgayCapCMTND = ngaycap.ToString("yyyy-MM-dd");
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
                    else if (model == "4") // Update liên lạc người thân
                    {
                        nhanVien.QuanHeNguoiThan = profileBasic.QuanHeNguoiThan;
                        nhanVien.SoDienThoaiNguoiThan = profileBasic.SoDienThoaiNguoiThan;

                        partialName = "_profileEmergencyContactPartial";
                    }
                    else if (model == "5") // Update BHXH
                    {
                        nhanVien.MaBHXH = profileBasic.MaBHXH;
                        BHXHViewModel bhxhViewModel = _bhxhService.GetById(profileBasic.MaBHXH);
                        if (bhxhViewModel == null)
                        {
                            _bhxhService.Delete(_bhxhService.GetAll(null).FirstOrDefault()?.Id);
                            _bhxhService.Save();

                            bhxhViewModel = new BHXHViewModel()
                            {
                                Id = profileBasic.MaBHXH,
                                MaNV = profileBasic.MaNhanVien,
                                NgayThamGia = profileBasic.NgayThamGia,
                                NgayKetThuc = profileBasic.NgayKetThuc,
                                UserCreated = _bhxhService.GetUserLogin()
                            };

                            nhanVien.HR_BHXH = new List<BHXHViewModel>() { bhxhViewModel };
                            partialName = "_profileSocialInsurancePartial";
                        }
                        else
                        {
                            bhxhViewModel.MaNV = profileBasic.MaNhanVien;
                            bhxhViewModel.NgayThamGia = profileBasic.NgayThamGia;
                            bhxhViewModel.NgayKetThuc = profileBasic.NgayKetThuc;

                            _bhxhService.Update(bhxhViewModel);
                            _bhxhService.Save();
                            partialName = "_profileSocialInsurancePartial";
                            return PartialView(partialName, profileBasic);
                        }
                    }
                    else if (model == "6") // Quitwork
                    {
                        nhanVien.NgayNghiViec = profileBasic.NgayNghiViec;
                        if (!string.IsNullOrEmpty(profileBasic.NgayNghiViec) && DateTime.TryParse(profileBasic.NgayNghiViec, out var dNgaynghi))
                        {
                            nhanVien.Status = Status.InActive.ToString();
                        }

                        partialName = "_profileQuitWorkPartial";
                    }
                    else if (model == "7") // ky luat lao dong
                    {
                        nhanVien.KyLuatLD = profileBasic.KyLuatLaoDong;
                        partialName = "_profileLabordisciplinePartial";
                    }

                    _nhanvienService.UpdateSingle(nhanVien);
                }

                _nhanvienService.Save();
            }

            return PartialView(partialName, profileBasic);
        }

        [HttpPost]
        public IActionResult UpdateTinhTrangHoSo(TinhTrangHoSoViewModel ttHso)
        {
            TinhTrangHoSoViewModel tinhTrangHs = _ttrangHoSoService.GetByMaNV(ttHso.MaNV);

            if (tinhTrangHs == null)
            {
                SetTinhTrangHoSo(ttHso);
                _ttrangHoSoService.Add(ttHso);
            }
            else
            {
                tinhTrangHs.AnhThe = ttHso.AnhThe;
                tinhTrangHs.SoYeuLyLich = ttHso.SoYeuLyLich;
                tinhTrangHs.CMTND = ttHso.CMTND;
                tinhTrangHs.SoHoKhau = ttHso.SoHoKhau;
                tinhTrangHs.GiayKhaiSinh = ttHso.GiayKhaiSinh;
                tinhTrangHs.BangTotNghiep = ttHso.BangTotNghiep;
                tinhTrangHs.XacNhanDanSu = ttHso.XacNhanDanSu;
                SetTinhTrangHoSo(tinhTrangHs);
                _ttrangHoSoService.Update(tinhTrangHs);
            }
            _ttrangHoSoService.Save();
            return PartialView("_profileTinhTrangHoSoPartial", ttHso);
        }

        [HttpPost]
        public IActionResult UpdateHopDong(HopDongViewModel hopDong)
        {
            HopDongViewModel hopDongViewModel = _hopDongService.GetById(hopDong.Id);
            if (hopDongViewModel == null)
            {
                hopDong.NgayTao = hopDong.NgayKy;
                _hopDongService.Add(hopDong);
            }
            else
            {
                hopDongViewModel.MaHD = hopDong.MaHD;
                hopDongViewModel.TenHD = hopDong.TenHD;
                hopDongViewModel.LoaiHD = hopDong.LoaiHD;
                hopDongViewModel.NgayKy = hopDong.NgayKy;
                hopDongViewModel.NgayTao = hopDong.NgayKy;
                hopDongViewModel.NgayHieuLuc = hopDong.NgayHieuLuc;
                hopDongViewModel.NgayHetHieuLuc = hopDong.NgayHetHieuLuc;
                hopDongViewModel.DayNumberNoti = hopDong.DayNumberNoti;
                _hopDongService.Update(hopDongViewModel);
            }
            _hopDongService.Save();
            List<HopDongViewModel> lstHD = new List<HopDongViewModel>()
            {
                hopDong
            };
            ViewBag.LoaiHD = _hrLoaiHDService.GetAll();
            return PartialView("_profileContractInfoPartial", lstHD);
        }

        [HttpPost]
        public IActionResult UpdateViewQuatrinhCtac(List<QuaTrinhLamViecViewModel> lstQtrinhCtac, string id)
        {
            string maNV = "";
            if (lstQtrinhCtac.Count() > 0)
            {
                maNV = lstQtrinhCtac[0].MaNV;
            }

            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int newId) && lstQtrinhCtac.Count() > 0)
            {
                if (newId == -9999)//  add area
                {
                    int nId = lstQtrinhCtac.OrderBy(x => x.Id).FirstOrDefault().Id - 1;

                    if (nId == -9999)
                    {
                        return PartialView("_profileQuaTrinhCtacModel", lstQtrinhCtac);
                    }

                    lstQtrinhCtac.Add(new QuaTrinhLamViecViewModel()
                    {
                        Id = nId,
                        MaNV = maNV
                    });
                }
                else // delete area
                {
                    var qtct = lstQtrinhCtac.FirstOrDefault(x => x.Id == newId);
                    lstQtrinhCtac.Remove(qtct);
                }
            }

            if (lstQtrinhCtac.Count() == 0)
            {
                lstQtrinhCtac.Add(new QuaTrinhLamViecViewModel()
                {
                    Id = 0,
                    MaNV = maNV
                });
            }

            ModelState.Clear();
            return PartialView("_profileQuaTrinhCtacModel", lstQtrinhCtac);
        }

        [HttpPost]
        public IActionResult UpdateQuatrinhCtac(List<QuaTrinhLamViecViewModel> lstQtrinhCtac, string id)
        {
            var qtCtacAll = _quatrinhLamViecService.GetAll(id);

            // Find object add new , update , delete
            List<QuaTrinhLamViecViewModel> lstAdd = new List<QuaTrinhLamViecViewModel>();
            List<QuaTrinhLamViecViewModel> lstUpdate = new List<QuaTrinhLamViecViewModel>();
            List<QuaTrinhLamViecViewModel> lstDelete = new List<QuaTrinhLamViecViewModel>();

            foreach (var item in lstQtrinhCtac)
            {
                var updateItem = qtCtacAll.Find(x => x.Id == item.Id);
                if (updateItem == null)
                {
                    lstAdd.Add(item);
                }
                else
                {
                    lstUpdate.Add(item);
                }
            }

            foreach (var item in qtCtacAll)
            {
                var deleteItem = lstQtrinhCtac.Find(x => x.Id == item.Id);
                if (deleteItem == null)
                {
                    lstDelete.Add(item);
                }
            }

            // Update data
            foreach (var item in lstUpdate)
            {
                QuaTrinhLamViecViewModel updateModel = qtCtacAll.Find(x => x.Id == item.Id);
                updateModel.TieuDe = item.TieuDe;
                updateModel.ThơiGianBatDau = item.ThơiGianBatDau;
                updateModel.ThoiGianKetThuc = item.ThoiGianKetThuc;
                updateModel.Note = item.Note;
                _quatrinhLamViecService.Update(updateModel);
            }

            foreach (var item in lstDelete)
            {
                _quatrinhLamViecService.Delete(item.Id);
            }

            foreach (var item in lstAdd)
            {
                item.Id = 0;
                item.MaNV = id;
                _quatrinhLamViecService.Add(item);
            }

            _quatrinhLamViecService.Save();

            return PartialView("_profileQuaTrinhCtacPartial", lstQtrinhCtac);
        }

        private void SetTinhTrangHoSo(TinhTrangHoSoViewModel ttHso)
        {
            if (ttHso.AnhThe && ttHso.SoYeuLyLich && ttHso.CMTND && ttHso.SoHoKhau && ttHso.GiayKhaiSinh && ttHso.BangTotNghiep && ttHso.XacNhanDanSu)
            {
                ttHso.Status = CommonConstants.Y;
            }
            else
            {
                ttHso.Status = CommonConstants.N;
            }
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
                                                                   k => k.HR_HOPDONG,
                                                                   l => l.HR_KEKHAIBAOHIEM,
                                                                   h =>h.TRAINING_NHANVIEN);
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
                TinhTrangHoSoViewModel tthsModel = nhanVien.HR_TINHTRANGHOSO.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                if (tthsModel != null)
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        Id = tthsModel.Id,
                        MaNV = nhanVien.Id,
                        AnhThe = tthsModel.AnhThe,
                        SoYeuLyLich = tthsModel.SoYeuLyLich,
                        CMTND = tthsModel.CMTND,
                        SoHoKhau = tthsModel.SoHoKhau,
                        GiayKhaiSinh = tthsModel.GiayKhaiSinh,
                        BangTotNghiep = tthsModel.BangTotNghiep,
                        XacNhanDanSu = tthsModel.XacNhanDanSu
                    };
                }
                else
                {
                    profileModel.tinhTrangHoSo = new TinhTrangHoSoViewModel()
                    {
                        MaNV = nhanVien.Id,
                        AnhThe = false,
                        SoYeuLyLich = false,
                        CMTND = false,
                        SoHoKhau = false,
                        GiayKhaiSinh = false,
                        BangTotNghiep = false,
                        XacNhanDanSu = false
                    };
                }

                // Ky Luat Lao Dong 
                profileModel.KyLuatLaoDong = nhanVien.KyLuatLD;

                // Bao Hiem
                BHXHViewModel bHXH = nhanVien.HR_BHXH.ToList().OrderByDescending(x => x.DateModified).FirstOrDefault();
                if (bHXH != null)
                {
                    profileModel.bHXHs = new BHXHViewModel()
                    {
                        NgayThamGia = bHXH.NgayThamGia,
                        NgayKetThuc = bHXH.NgayKetThuc,
                        Id = bHXH.Id
                    };

                    profileModel.MaBHXH = bHXH.Id;
                    profileModel.NgayThamGia = bHXH.NgayThamGia;
                    profileModel.NgayKetThuc = bHXH.NgayKetThuc;
                }

                // Bang cap , chung chi
                profileModel.chungChis = nhanVien.HR_CHUNGCHI_NHANVIEN.ToList();

                // Qua Trinh Lam Viec --
                profileModel.quaTrinhLamViecs = nhanVien.HR_QUATRINHLAMVIEC.OrderByDescending(x => x.ThơiGianBatDau).ToList();
                if (profileModel.quaTrinhLamViecs.Count() == 0)
                {
                    profileModel.quaTrinhLamViecs = new List<QuaTrinhLamViecViewModel>()
                    {
                        new QuaTrinhLamViecViewModel()
                        {
                            Id = 0,
                            ThơiGianBatDau = "",
                            TieuDe = "",
                            Note = "",
                            MaNV = profileModel.MaNhanVien
                        }
                    };
                }

                profileModel.phepNams = nhanVien.HR_PHEP_NAM.OrderByDescending(x => x.Year).Take(2).ToList();
                if (profileModel.phepNams.Count() == 0)
                {
                    profileModel.phepNams = new List<PhepNamViewModel>()
                    {
                        new PhepNamViewModel()
                        {
                            MaNhanVien = Id,
                            SoPhepNam = 0,
                            SoPhepConLai = 0,
                            Year = DateTime.Now.Year
                        },
                        new PhepNamViewModel()
                        {
                            MaNhanVien = Id,
                            SoPhepNam = 0,
                            SoPhepConLai = 0,
                            Year = DateTime.Now.Year - 1
                        }
                    };
                }

                // --
                profileModel.hopDongs = nhanVien.HR_HOPDONG.OrderByDescending(x => x.NgayKy).ToList();
                if (profileModel.hopDongs == null || profileModel.hopDongs.Count() == 0)
                {
                    profileModel.hopDongs = new List<HopDongViewModel>()
                    {
                        new HopDongViewModel()
                        {
                            MaNV =profileModel.MaNhanVien
                        }
                    };
                }

                profileModel.kekhaibaohiems = nhanVien.HR_KEKHAIBAOHIEM.ToList();
                if (profileModel.kekhaibaohiems == null || profileModel.kekhaibaohiems.Count() == 0)
                {
                    profileModel.kekhaibaohiems = new List<KeKhaiBaoHiemViewModel>()
                    {
                        new KeKhaiBaoHiemViewModel()
                        {
                            MaNV =profileModel.MaNhanVien
                        }
                    };
                }

                var chedo = _cheDoBHService.GetAll(null);
                foreach (var item in profileModel.kekhaibaohiems)
                {
                    item.HR_CHEDOBH = chedo.Find(x => x.Id == item.CheDoBH);
                }

                profileModel.training_NhanVienViewModels = nhanVien.TRAINING_NHANVIEN.ToList();

                if (profileModel.training_NhanVienViewModels == null || profileModel.training_NhanVienViewModels.Count() == 0)
                {
                    profileModel.training_NhanVienViewModels = new List<Training_NhanVienViewModel>()
                    {
                        new Training_NhanVienViewModel(profileModel.MaNhanVien,Guid.NewGuid())
                    };
                }
                else
                {
                    foreach (var nvTrain in profileModel.training_NhanVienViewModels)
                    {
                        nvTrain.HR_TRAINING = _trainingListService.GetById(nvTrain.TrainnigId);
                    }
                }

                ViewBag.LoaiHD = _hrLoaiHDService.GetAll();

                return new OkObjectResult(profileModel);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpPost]
        public IActionResult UpdateMaNV(string Id, string newId)
        {
            var currentNv = _nhanvienService.GetById(Id);
            var newNhanVien = _nhanvienService.GetById(newId);

            if (currentNv != null)
            {
                if (newNhanVien == null)
                {
                    newNhanVien = (NhanVienViewModel)currentNv.CloneObject();
                    newNhanVien.Id = newId;

                    _nhanvienService.Add(newNhanVien);
                    _nhanvienService.Save();

                    // update hop dong
                    var lstHd = _hopDongService.GetAll().Where(x => x.MaNV == Id);
                    foreach (var item in lstHd)
                    {
                        item.MaNV = newId;
                        _hopDongService.Update(item);
                    };

                    _hopDongService.Save();

                    //update BHXH
                    var lstBHXH = _bhxhService.GetAll(null).Where(x => x.MaNV == Id);
                    foreach (var item in lstBHXH)
                    {
                        item.MaNV = newId;
                        _bhxhService.Update(item);
                    }

                    _bhxhService.Save();

                    // update tinh trang hso
                    var lstTTHSo = _ttrangHoSoService.GetAll().Where(x => x.MaNV == Id);
                    foreach (var item in lstTTHSo)
                    {
                        item.MaNV = newId;
                        _ttrangHoSoService.Update(item);
                    }
                    _ttrangHoSoService.Save();

                    // update qua trinh lam viec
                    var lstquatrinhLviec = _quatrinhLamViecService.GetAll().Where(x => x.MaNV == Id);
                    foreach (var item in lstquatrinhLviec)
                    {
                        item.MaNV = newId;
                        _quatrinhLamViecService.Update(item);
                    }
                    _quatrinhLamViecService.Save();

                    // update phep nam
                    var lstPhepNam = _phepNamService.GetAll(null).Where(x => x.MaNhanVien == Id);
                    foreach (var item in lstPhepNam)
                    {
                        item.MaNhanVien = newId;
                        _phepNamService.Update(item);
                    }
                    _phepNamService.Save();

                    // update chi tra BH
                    var lstChiTraBH = _keKhaiBHService.GetAll(Id);
                    foreach (var item in lstChiTraBH)
                    {
                        item.MaNV = newId;
                        _keKhaiBHService.Update(item);
                    }
                    _keKhaiBHService.Save();

                    // delete old nhan vien
                    _nhanvienService.Delete(Id);
                    _nhanvienService.Save();
                }
                else
                {
                    return new ConflictObjectResult(CommonConstants.ConflictObjectResult_Msg);
                }
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
            return new OkObjectResult(newId);
        }

        [HttpGet]
        public IActionResult GetCheDoBH()
        {
            var chedo = _cheDoBHService.GetAll(null);
            return new OkObjectResult(chedo);
        }

        /// <summary>
        /// Update ke khai bao hiem
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateKeKhaiBH(KeKhaiBaoHiemViewModel data)
        {
            var model = _keKhaiBHService.GetById(data.Id);
            if (model == null)
            {
                _keKhaiBHService.Add(data);
            }
            else
            {
                model.CheDoBH = data.CheDoBH;
                model.NgayBatDau = data.NgayBatDau;
                model.NgayKetThuc = data.NgayKetThuc;
                model.NgayThanhToan = data.NgayThanhToan;
                model.SoTienThanhToan = data.SoTienThanhToan;

                _keKhaiBHService.Update(model);
            }
            _keKhaiBHService.Save();
            return new OkObjectResult(data);
        }

        [HttpGet]
        public IActionResult GetKeKhaiBH(int Id)
        {
            var obj = _keKhaiBHService.GetById(Id);
            return new OkObjectResult(obj);
        }

        [HttpPost]
        public IActionResult DeleteKeKhaiBH(int Id)
        {
            _keKhaiBHService.Delete(Id);
            _keKhaiBHService.Save();
            return new OkObjectResult(Id);
        }
    }
}
