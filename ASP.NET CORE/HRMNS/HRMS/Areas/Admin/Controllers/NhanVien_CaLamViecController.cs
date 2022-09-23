using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRMS.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace HRMS.Areas.Admin.Controllers
{
    public class NhanVien_CaLamViecController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        INhanVien_CalamviecService _nvienCalamviecService;

        public NhanVien_CaLamViecController(INhanVien_CalamviecService nhanVien_CalamviecService, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger)
        {
            _hostingEnvironment = hostEnvironment;
            _nvienCalamviecService = nhanVien_CalamviecService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<NhanVien_CalamViecViewModel> nvCalamviecs;
            if (Department != "")
            {
                nvCalamviecs = _nvienCalamviecService.GetAll("", x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC).Where(x => x.HR_NHANVIEN.MaBoPhan == Department && x.Status != Status.InActive.ToString()).ToList();
            }
            else
            {
                nvCalamviecs = _nvienCalamviecService.GetAll("", x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC).Where(x => x.Status != Status.InActive.ToString()).ToList(); ;
            }
            var calviec = _nvienCalamviecService.GetDMCalamViec();
            HttpContext.Session.Set("ssDMCaLviec", calviec);
            return View(nvCalamviecs);
        }

        [HttpGet]
        public IActionResult GetAllDMCaLamViec()
        {
            var calviec = _nvienCalamviecService.GetDMCalamViec();
            return new OkObjectResult(calviec);
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
                ResultDB result = _nvienCalamviecService.ImportExcel(filePath, param);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                if (result.ReturnInt == 0)
                {
                    return new OkObjectResult(filePath);
                }
                else
                {
                    _logger.LogError(result.ReturnString);
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpGet]
        public IActionResult FindId(int Id)
        {
            var obj = _nvienCalamviecService.GetById(Id);
            if (obj != null)
            {
                return new OkObjectResult(obj);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        /// <summary>
        /// Dang ky ca lam viec cho nhan vien
        /// </summary>
        /// <param name="calamviec"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterNhanVienCalamViec(NhanVien_CalamViecViewModel calamviec, [FromQuery] string action)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (calamviec.Danhmuc_CaLviec == "CD_CN") // ca dem con nho
                {
                    calamviec.Danhmuc_CaLviec = "CD_WHC";
                    calamviec.CaLV_DB = "CD_CN";
                }
                else if (calamviec.Danhmuc_CaLviec == "CN_CN") // ca ngay con nho
                {
                    calamviec.Danhmuc_CaLviec = "CN_WHC";
                    calamviec.CaLV_DB = "CN_CN";
                }
                else if (calamviec.Danhmuc_CaLviec == "VP_CN") // ca ngay con nho
                {
                    calamviec.Danhmuc_CaLviec = "CN_WHC";
                    calamviec.CaLV_DB = "VP_CN";
                }
                else if (calamviec.Danhmuc_CaLviec == "TS")
                {
                    calamviec.Danhmuc_CaLviec = "CN_WHC";
                    calamviec.CaLV_DB = "TS";
                }
                else
                {
                    calamviec.CaLV_DB = "";
                }

                if (action == "Add")
                {
                    DateTime dStart = DateTime.Parse(calamviec.BatDau_TheoCa);
                    DateTime dEnd = DateTime.Parse(calamviec.KetThuc_TheoCa);
                    string dateCheck;
                    foreach (DateTime day in EachDay.EachDays(dStart, dEnd))
                    {
                        dateCheck = day.ToString("yyyy-MM-dd");
                        if (_nvienCalamviecService.GetAll().FindAll(x => x.MaNV == calamviec.MaNV && string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0).Count() > 0)
                        {
                            return new NotFoundObjectResult("Ca làm việc bị trùng ngày: " + dateCheck + " Mã NV: " + calamviec.MaNV);
                        }
                    }

                    NhanVien_CalamViecViewModel itemCheck = _nvienCalamviecService.CheckExist(0, calamviec.MaNV, calamviec.BatDau_TheoCa, calamviec.KetThuc_TheoCa);
                    if (itemCheck != null)
                    {
                        itemCheck.Danhmuc_CaLviec = calamviec.Danhmuc_CaLviec;
                        itemCheck.BatDau_TheoCa = calamviec.BatDau_TheoCa;
                        itemCheck.KetThuc_TheoCa = calamviec.KetThuc_TheoCa;
                        itemCheck.CaLV_DB = calamviec.CaLV_DB;

                        _nvienCalamviecService.Update(itemCheck);
                        _nvienCalamviecService.Save();
                    }
                    else
                    {
                        calamviec.Approved = CommonConstants.Approved;// CommonConstants.No_Approved;
                        _nvienCalamviecService.Add(calamviec);
                        _nvienCalamviecService.Save();
                    }

                    return new OkObjectResult(calamviec);
                }
                else
                {
                    DateTime dStart = DateTime.Parse(calamviec.BatDau_TheoCa);
                    DateTime dEnd = DateTime.Parse(calamviec.KetThuc_TheoCa);
                    string dateCheck;
                    foreach (DateTime day in EachDay.EachDays(dStart, dEnd))
                    {
                        dateCheck = day.ToString("yyyy-MM-dd");
                        if (_nvienCalamviecService.GetAll().FindAll(x => x.MaNV == calamviec.MaNV && string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0).Count() > 1)
                        {
                            return new NotFoundObjectResult("Ca làm việc bị trùng ngày: " + dateCheck + " Mã NV: " + calamviec.MaNV);
                        }
                    }

                    NhanVien_CalamViecViewModel itemCheck = _nvienCalamviecService.CheckExist(calamviec.Id, calamviec.MaNV, calamviec.BatDau_TheoCa, calamviec.KetThuc_TheoCa);
                    itemCheck.Danhmuc_CaLviec = calamviec.Danhmuc_CaLviec;
                    itemCheck.BatDau_TheoCa = calamviec.BatDau_TheoCa;
                    itemCheck.KetThuc_TheoCa = calamviec.KetThuc_TheoCa;
                    itemCheck.CaLV_DB = calamviec.CaLV_DB;

                    _nvienCalamviecService.Update(itemCheck);
                    _nvienCalamviecService.Save();

                    return new OkObjectResult(itemCheck);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _nvienCalamviecService.Delete(Id);
            _nvienCalamviecService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult Search(string department, string status, string timeFrom, string timeTo)
        {
            var lst = _nvienCalamviecService.Search(department, status, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC);
            return PartialView("_gridNhanVienCaLamViecPartialView", lst);
        }

        //[HttpPost]
        //public IActionResult RegisNewShift(SettingTimeCalamviecViewModel shift, [FromQuery] string action)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
        //        return new BadRequestObjectResult(allErrors);
        //    }
        //    else
        //    {
        //        var obj = _settingTimeCalamviec.GetById(shift.Id);
        //        if (action == "Add")
        //        {
        //            if (obj != null)
        //            {
        //                if (obj.NgayKetThuc.CompareDateTime(obj.NgayBatDau) < 0 || obj.NgayKetThucDangKy.CompareDateTime(obj.NgayBatDauDangKy) < 0)
        //                {
        //                    return new BadRequestObjectResult(CommonConstants.InvalidParam);
        //                }

        //                obj.CaLamViec = shift.CaLamViec;
        //                obj.NgayBatDau = shift.NgayBatDau;
        //                obj.NgayKetThuc = shift.NgayKetThuc;

        //                obj.NgayBatDauDangKy = shift.NgayBatDauDangKy;
        //                obj.NgayKetThucDangKy = shift.NgayKetThucDangKy;
        //                obj.Status = shift.Status;
        //                _settingTimeCalamviec.Update(obj);
        //            }
        //            else
        //            {
        //                _settingTimeCalamviec.Add(shift);
        //            }
        //        }
        //        else
        //        {
        //            if (obj.NgayKetThuc.CompareDateTime(obj.NgayBatDau) < 0 || obj.NgayKetThucDangKy.CompareDateTime(obj.NgayBatDauDangKy) < 0)
        //            {
        //                return new BadRequestObjectResult(CommonConstants.InvalidParam);
        //            }

        //            obj.CaLamViec = shift.CaLamViec;
        //            obj.NgayBatDau = shift.NgayBatDau;
        //            obj.NgayKetThuc = shift.NgayKetThuc;

        //            obj.NgayBatDauDangKy = shift.NgayBatDauDangKy;
        //            obj.NgayKetThucDangKy = shift.NgayKetThucDangKy;
        //            obj.Status = shift.Status;
        //            _settingTimeCalamviec.Update(obj);
        //        }

        //        _settingTimeCalamviec.Save();
        //        return new OkObjectResult(shift);
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetTimeSettingCaLamViec()
        //{
        //    var lst = _settingTimeCalamviec.GetAll("", x => x.DM_CA_LVIEC).OrderByDescending(x => x.NgayBatDau).Take(2);
        //    return new OkObjectResult(lst);
        //}

        //[HttpGet]
        //public IActionResult GetActiveTime()
        //{
        //    var obj = _settingTimeCalamviec.GetByStatus(Status.Active.ToString());
        //    return new OkObjectResult(obj);
        //}

        [HttpPost]
        public IActionResult Approve(List<int> lstID, string action)
        {
            List<NhanVien_CalamViecViewModel> lstCalamviec = _nvienCalamviecService.GetAllWithoutStatus().Where(x => lstID.Contains(x.Id)).ToList();

            if (action == "approve")
            {
                foreach (var item in lstCalamviec)
                {
                    if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                    {
                        item.Approved = CommonConstants.Approved;
                    }
                }
            }
            else if (action == "unapprove")
            {
                foreach (var item in lstCalamviec)
                {
                    if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                    {
                        item.Approved = CommonConstants.No_Approved;
                    }
                }
            }

            _nvienCalamviecService.UpdateRange(lstCalamviec);
            _nvienCalamviecService.Save();
            return new OkObjectResult(lstID);
        }

        [HttpPost]
        public IActionResult ExportExcel(string department, string status, string timeFrom, string timeTo)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Nhanvien_calamviec_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            var data = _nvienCalamviecService.Search(department, status, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC);
            List<NhanVienCaLamViecView> lstData = new List<NhanVienCaLamViecView>();
            NhanVienCaLamViecView clv;
            foreach (var item in data)
            {
                clv = new NhanVienCaLamViecView()
                {
                    MaNV = item.MaNV,
                    TenNV = item.HR_NHANVIEN.TenNV,
                    BoPhan = item.HR_NHANVIEN.MaBoPhan,
                    CaLamViec = item.DM_CA_LVIEC.TenCaLamViec,
                    FromTime = item.BatDau_TheoCa,
                    ToTime = item.KetThuc_TheoCa,
                    Approve = item.Approved
                };

                lstData.Add(clv);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Calamviec");
                worksheet.Cells["A1"].LoadFromCollection(lstData, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }
    }

    public class NhanVienCaLamViecView
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string BoPhan { get; set; }
        public string CaLamViec { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Approve { get; set; }
    }
}
