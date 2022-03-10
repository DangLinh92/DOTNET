using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
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
    public class DangKyOTController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        IOvertimeService _overtimeService;
        IDMucNgaylamviecService _dmNgayLviecService;
        INgayLeNamService _ngayLeNamService;

        public DangKyOTController(IOvertimeService overtimeService, INgayLeNamService ngayLeNamService, IDMucNgaylamviecService dmNgayLviecService, IWebHostEnvironment hostEnvironment, ILogger<DangKyOTNhanVienViewModel> logger)
        {
            _ngayLeNamService = ngayLeNamService;
            _dmNgayLviecService = dmNgayLviecService;
            _hostingEnvironment = hostEnvironment;
            _overtimeService = overtimeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lst = _overtimeService.GetAll("", x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC);
            return View(lst);
        }

        [HttpPost]
        public IActionResult RegisterOvertime(DangKyOTNhanVienViewModel overtime, [FromQuery] string action)
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
                    DangKyOTNhanVienViewModel itemCheck = _overtimeService.CheckExist(0, overtime.MaNV, overtime.NgayOT);
                    if (itemCheck != null)
                    {
                        itemCheck.NgayOT = overtime.NgayOT;
                        UpdateDMNgayLviec(itemCheck);
                        _overtimeService.Update(itemCheck);
                    }
                    else
                    {
                        overtime.Approve = CommonConstants.No_Approved;
                        UpdateDMNgayLviec(overtime);
                        _overtimeService.Add(overtime);
                    }
                    _overtimeService.Save();
                    return new OkObjectResult(overtime);
                }
                else
                {
                    DangKyOTNhanVienViewModel itemCheck = _overtimeService.CheckExist(overtime.Id, overtime.MaNV, overtime.NgayOT);
                    if (itemCheck != null)
                    {
                        itemCheck.NgayOT = overtime.NgayOT;
                        UpdateDMNgayLviec(itemCheck);
                    }
                    else
                    {
                        itemCheck = _overtimeService.CheckExist(0, overtime.MaNV, overtime.NgayOT);

                        if (itemCheck == null)
                        {
                            overtime.Approve = CommonConstants.No_Approved;
                            UpdateDMNgayLviec(overtime);
                            _overtimeService.Add(overtime);
                            _overtimeService.Save();
                            return new OkObjectResult(overtime);
                        }
                        else
                        {
                            itemCheck.NgayOT = overtime.NgayOT;
                        }
                    }

                    _overtimeService.Update(itemCheck);
                    _overtimeService.Save();
                    return new OkObjectResult(itemCheck);
                }
            }
        }

        // Update Ngay lam viec là ngay le, ngay truoc le, ngay le cuoi cung,ngay thuong, chu nhat  ....
        private void UpdateDMNgayLviec(DangKyOTNhanVienViewModel obj)
        {
            var lstNgayLeNam = _ngayLeNamService.GetAll("");
            var itemcheck = lstNgayLeNam.FirstOrDefault(x => x.Id == obj.NgayOT);
            var afterOneDay = DateTime.Parse(obj.NgayOT).AddDays(1).ToString("yyyy-MM-dd");
            var itemcheck2 = lstNgayLeNam.FirstOrDefault(x => x.Id == afterOneDay);

            if (itemcheck != null)
            {
                obj.DM_NgayLViec = "NL";

                if (itemcheck.IslastHoliday == CommonConstants.Y)
                {
                    obj.DM_NgayLViec = "NLCC";
                }
            }
            else if (itemcheck2 != null)
            {
                obj.DM_NgayLViec = "TNL";
            }
            else if (DateTime.Parse(obj.NgayOT).DayOfWeek == DayOfWeek.Sunday)
            {
                obj.DM_NgayLViec = "CN";
            }
            else
            {
                obj.DM_NgayLViec = "NT";
            }
        }

        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var obj = _overtimeService.GetById(Id);
            return new OkObjectResult(obj);
        }

        [HttpPost]
        public IActionResult DeleteOvertime(int Id)
        {
            _overtimeService.Delete(Id);
            _overtimeService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult Approve(int Id,string dept, string status,string action)
        {
            if(Id > 0)// approve single
            {
                if (action == CommonConstants.Approved)
                {
                    _overtimeService.ApproveSingle(Id, true);
                }
                else
                {
                    _overtimeService.ApproveSingle(Id, false);
                }
            }
            else
            {
                if (action == CommonConstants.Approved)
                {
                    if (status != CommonConstants.No_Approved || string.IsNullOrEmpty(dept))
                    {
                        return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                    }

                    _overtimeService.Approve(dept, status, true);
                }
                else
                {
                    if (status != CommonConstants.Approved || string.IsNullOrEmpty(dept))
                    {
                        return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                    }

                    _overtimeService.Approve(dept, status, false);
                }
            }
            
            _overtimeService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult Search(string department, string status, string timeFrom, string timeTo)
        {
            var lst = _overtimeService.Search(department, status, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC);
            return PartialView("_gridOvertimePartialView", lst);
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
                ResultDB result = _overtimeService.ImportExcel(filePath, param);

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
    }
}
