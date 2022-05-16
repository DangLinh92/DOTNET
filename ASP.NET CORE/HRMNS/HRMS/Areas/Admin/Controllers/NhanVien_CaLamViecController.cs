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

namespace HRMS.Areas.Admin.Controllers
{
    public class NhanVien_CaLamViecController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        INhanVien_CalamviecService _nvienCalamviecService;
        ISettingTimeCalamviecService _settingTimeCalamviec;

        public NhanVien_CaLamViecController(INhanVien_CalamviecService nhanVien_CalamviecService, ISettingTimeCalamviecService settingTimeCalamviec, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger)
        {
            _hostingEnvironment = hostEnvironment;
            _nvienCalamviecService = nhanVien_CalamviecService;
            _logger = logger;
            _settingTimeCalamviec = settingTimeCalamviec;
        }

        public IActionResult Index()
        {
            List<NhanVien_CalamViecViewModel> nvCalamviecs = _nvienCalamviecService.GetAll("", x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC);
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
                if (action == "Add")
                {
                    NhanVien_CalamViecViewModel itemCheck = _nvienCalamviecService.CheckExist(0, calamviec.MaNV, calamviec.Danhmuc_CaLviec, calamviec.BatDau_TheoCa, calamviec.KetThuc_TheoCa);
                    if (itemCheck != null)
                    {
                        itemCheck.Danhmuc_CaLviec = calamviec.Danhmuc_CaLviec;
                        itemCheck.BatDau_TheoCa = calamviec.BatDau_TheoCa;
                        itemCheck.KetThuc_TheoCa = calamviec.KetThuc_TheoCa;

                        var calaviecActive = _settingTimeCalamviec.GetByCaLamViecAndStatus(Status.Active.ToString(), itemCheck.Danhmuc_CaLviec);
                        if (calaviecActive != null)
                        {
                            if (itemCheck.KetThuc_TheoCa.CompareDateTime(itemCheck.BatDau_TheoCa) >= 0 && 
                                itemCheck.BatDau_TheoCa.InRangeDateTime(calaviecActive.NgayBatDau, calaviecActive.NgayKetThuc) && 
                                itemCheck.KetThuc_TheoCa.InRangeDateTime(calaviecActive.NgayBatDau, calaviecActive.NgayKetThuc))
                            {
                                _nvienCalamviecService.Update(itemCheck);
                                _nvienCalamviecService.Save();
                            }
                        }
                        else
                        {
                            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                        }
                    }
                    else
                    {
                        calamviec.Approved = CommonConstants.No_Approved;
                        _nvienCalamviecService.Add(calamviec);
                        _nvienCalamviecService.Save();
                    }

                    return new OkObjectResult(calamviec);
                }
                else
                {
                    NhanVien_CalamViecViewModel itemCheck = _nvienCalamviecService.CheckExist(calamviec.Id, calamviec.MaNV, calamviec.Danhmuc_CaLviec, calamviec.BatDau_TheoCa, calamviec.KetThuc_TheoCa);
                    itemCheck.Danhmuc_CaLviec = calamviec.Danhmuc_CaLviec;
                    itemCheck.BatDau_TheoCa = calamviec.BatDau_TheoCa;
                    itemCheck.KetThuc_TheoCa = calamviec.KetThuc_TheoCa;

                    var calaviecActive = _settingTimeCalamviec.GetByCaLamViecAndStatus(Status.Active.ToString(), itemCheck.Danhmuc_CaLviec);
                    if (calaviecActive != null)
                    {
                        if (itemCheck.KetThuc_TheoCa.CompareDateTime(itemCheck.BatDau_TheoCa) >= 0 &&
                            itemCheck.BatDau_TheoCa.InRangeDateTime(calaviecActive.NgayBatDau, calaviecActive.NgayKetThuc) &&
                            itemCheck.KetThuc_TheoCa.InRangeDateTime(calaviecActive.NgayBatDau, calaviecActive.NgayKetThuc))
                        {
                            _nvienCalamviecService.Update(itemCheck);
                            _nvienCalamviecService.Save();
                        }
                    }
                    else
                    {
                        return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                    }

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

        [HttpPost]
        public IActionResult RegisNewShift(SettingTimeCalamviecViewModel shift, [FromQuery] string action)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var obj = _settingTimeCalamviec.GetById(shift.Id);
                if (action == "Add")
                {
                    if (obj != null)
                    {
                        obj.CaLamViec = shift.CaLamViec;
                        obj.NgayBatDau = shift.NgayBatDau;
                        obj.NgayKetThuc = shift.NgayKetThuc;

                        obj.NgayBatDauDangKy = shift.NgayBatDauDangKy;
                        obj.NgayKetThucDangKy = shift.NgayKetThucDangKy;
                        obj.Status = shift.Status;
                        _settingTimeCalamviec.Update(obj);
                    }
                    else
                    {
                        _settingTimeCalamviec.Add(shift);
                    }
                }
                else
                {
                    obj.CaLamViec = shift.CaLamViec;
                    obj.NgayBatDau = shift.NgayBatDau;
                    obj.NgayKetThuc = shift.NgayKetThuc;

                    obj.NgayBatDauDangKy = shift.NgayBatDauDangKy;
                    obj.NgayKetThucDangKy = shift.NgayKetThucDangKy;
                    obj.Status = shift.Status;
                    _settingTimeCalamviec.Update(obj);
                }

                if(obj.NgayKetThuc.CompareDateTime(obj.NgayBatDau) >= 0 && obj.NgayKetThucDangKy.CompareDateTime(obj.NgayBatDauDangKy) >= 0)
                {
                    _settingTimeCalamviec.Save();
                    return new OkObjectResult(shift);
                }
                else
                {
                    return new BadRequestObjectResult(CommonConstants.InvalidParam);
                }
            }
        }

        [HttpGet]
        public IActionResult GetTimeSettingCaLamViec()
        {
            var lst = _settingTimeCalamviec.GetAll("", x => x.DM_CA_LVIEC).OrderByDescending(x=>x.NgayBatDau).Take(10);
            return new OkObjectResult(lst);
        }

        [HttpGet]
        public IActionResult GetActiveTime()
        {
            var obj = _settingTimeCalamviec.GetByStatus(Status.Active.ToString());
            return new OkObjectResult(obj);
        }

        [HttpPost]
        public IActionResult Approve(string dept, string status, int Id)
        {
            if (Id > 0)
            {
                _nvienCalamviecService.ApproveSingle(Id, true);
            }
            else
            {
                if (status != CommonConstants.No_Approved || string.IsNullOrEmpty(dept))
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }

                _nvienCalamviecService.Approve(dept, status, true);
            }

            _nvienCalamviecService.Save();
            return new OkObjectResult(true);
        }

        [HttpPost]
        public IActionResult UnApprove(string dept, string status, int Id)
        {
            if (Id > 0)
            {
                _nvienCalamviecService.ApproveSingle(Id, false);
            }
            else
            {
                if (status != CommonConstants.Approved || string.IsNullOrEmpty(dept))
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }

                _nvienCalamviecService.Approve(dept, status, false);
            }

            _nvienCalamviecService.Save();
            return new OkObjectResult(null);
        }
    }
}
