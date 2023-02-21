using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
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

        public DangKyOTController(IOvertimeService overtimeService, INgayLeNamService ngayLeNamService, IDMucNgaylamviecService dmNgayLviecService, IWebHostEnvironment hostEnvironment, ILogger<DangKyOTController> logger)
        {
            _ngayLeNamService = ngayLeNamService;
            _dmNgayLviecService = dmNgayLviecService;
            _hostingEnvironment = hostEnvironment;
            _overtimeService = overtimeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<DangKyOTNhanVienViewModel> lst = _overtimeService.GetAll("", x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC).OrderByDescending(x => x.NgayOT).ToList();

            if (UserRole == CommonConstants.AssLeader_Role)
            {
                lst = lst.Where(x => x.HR_NHANVIEN.MaBoPhan == Department && x.ApproveLV3 != CommonConstants.Approved).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove1) // leader 
            {
                lst = lst.
                    Where(x => x.HR_NHANVIEN.MaBoPhan == Department && (x.Approve == CommonConstants.Request || x.ApproveLV2 == CommonConstants.Request || x.ApproveLV3 != CommonConstants.Approved)).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove2) // korea manager
            {
                lst = lst.
                    Where(x => x.HR_NHANVIEN.MaBoPhan == Department && x.Approve == CommonConstants.Approved && x.ApproveLV3 != CommonConstants.Approved).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole) // hr approve
            {
                lst = lst.Where(x => x.ApproveLV2 == CommonConstants.Approved && x.ApproveLV3 == CommonConstants.Request).OrderByDescending(x => x.DateModified).ToList();
            }

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
                overtime.SoGioOT = double.Parse(overtime.SoGioOT_1, System.Globalization.CultureInfo.InvariantCulture);
                if (action == "Add")
                {
                    DangKyOTNhanVienViewModel itemCheck = _overtimeService.CheckExist(0, overtime.MaNV, overtime.NgayOT, overtime.HeSoOT);
                    if (itemCheck != null)
                    {
                        itemCheck.NgayOT = overtime.NgayOT;
                        itemCheck.SoGioOT = overtime.SoGioOT;
                        itemCheck.HeSoOT = overtime.HeSoOT;
                        itemCheck.NoiDung = overtime.NoiDung;

                        itemCheck.Approve = CommonConstants.Approved;
                        itemCheck.ApproveLV2 = CommonConstants.Approved;
                        itemCheck.ApproveLV3 = CommonConstants.Request;
                        if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        {
                            itemCheck.ApproveLV3 = CommonConstants.Approved;
                        }

                        UpdateDMNgayLviec(itemCheck);
                        _overtimeService.Update(itemCheck);
                    }
                    else
                    {
                        #region 
                        //if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        //{
                        //    overtime.Approve = CommonConstants.Approved;
                        //    overtime.ApproveLV2 = CommonConstants.Approved;
                        //    overtime.ApproveLV3 = CommonConstants.Approved;
                        //}
                        //else if (UserRole == CommonConstants.roleApprove1) // group leader manager
                        //{
                        //    overtime.Approve = CommonConstants.Approved;
                        //    overtime.ApproveLV2 = CommonConstants.Request;
                        //    overtime.ApproveLV3 = CommonConstants.Request;
                        //}
                        //else if (UserRole == CommonConstants.roleApprove2) // korea manager
                        //{
                        //    overtime.Approve = CommonConstants.Approved;
                        //    overtime.ApproveLV2 = CommonConstants.Approved;
                        //    overtime.ApproveLV3 = CommonConstants.Request;
                        //} 
                        //else
                        //{
                        //    overtime.Approve = CommonConstants.Request;
                        //    overtime.ApproveLV2 = CommonConstants.Request;
                        //    overtime.ApproveLV3 = CommonConstants.Request;
                        //}
                        #endregion

                        overtime.Approve = CommonConstants.Approved;
                        overtime.ApproveLV2 = CommonConstants.Approved;
                        overtime.ApproveLV3 = CommonConstants.Request;
                        if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        {
                            overtime.ApproveLV3 = CommonConstants.Approved;
                        }

                        UpdateDMNgayLviec(overtime);
                        _overtimeService.Add(overtime);
                    }
                    _overtimeService.Save();
                    return new OkObjectResult(overtime);
                }
                else
                {
                    DangKyOTNhanVienViewModel itemCheck = _overtimeService.CheckExist(overtime.Id, overtime.MaNV, overtime.NgayOT, overtime.HeSoOT);
                    if (itemCheck != null)
                    {
                        itemCheck.NgayOT = overtime.NgayOT;
                        itemCheck.SoGioOT = overtime.SoGioOT;
                        itemCheck.HeSoOT = overtime.HeSoOT;
                        itemCheck.NoiDung = overtime.NoiDung;

                        itemCheck.Approve = CommonConstants.Approved;
                        itemCheck.ApproveLV2 = CommonConstants.Approved;
                        itemCheck.ApproveLV3 = CommonConstants.Request;
                        if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        {
                            itemCheck.ApproveLV3 = CommonConstants.Approved;
                        }

                        UpdateDMNgayLviec(itemCheck);
                    }
                    else
                    {
                        itemCheck = _overtimeService.CheckExist(0, overtime.MaNV, overtime.NgayOT, overtime.HeSoOT);

                        if (itemCheck == null)
                        {
                            #region 
                            //if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                            //{
                            //    overtime.Approve = CommonConstants.Approved;
                            //    overtime.ApproveLV2 = CommonConstants.Approved;
                            //    overtime.ApproveLV3 = CommonConstants.Approved;
                            //}
                            //else if (UserRole == CommonConstants.roleApprove1)
                            //{
                            //    overtime.Approve = CommonConstants.Approved;
                            //    overtime.ApproveLV2 = CommonConstants.Request;
                            //    overtime.ApproveLV3 = CommonConstants.Request;
                            //}
                            //else if (UserRole == CommonConstants.roleApprove2)
                            //{
                            //    overtime.Approve = CommonConstants.Approved;
                            //    overtime.ApproveLV2 = CommonConstants.Approved;
                            //    overtime.ApproveLV3 = CommonConstants.Request;
                            //} // korea manager
                            //else
                            //{
                            //    overtime.Approve = CommonConstants.Request;
                            //    overtime.ApproveLV2 = CommonConstants.Request;
                            //    overtime.ApproveLV3 = CommonConstants.Request;
                            //}
                            #endregion 

                            overtime.Approve = CommonConstants.Approved;
                            overtime.ApproveLV2 = CommonConstants.Approved;
                            overtime.ApproveLV3 = CommonConstants.Request;
                            if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                            {
                                overtime.ApproveLV3 = CommonConstants.Approved;
                            }

                            UpdateDMNgayLviec(overtime);
                            _overtimeService.Add(overtime);
                            _overtimeService.Save();
                            return new OkObjectResult(overtime);
                        }
                        else
                        {
                            itemCheck.NgayOT = overtime.NgayOT;
                            itemCheck.SoGioOT = overtime.SoGioOT;
                            itemCheck.HeSoOT = overtime.HeSoOT;
                            itemCheck.NoiDung = overtime.NoiDung;

                            itemCheck.Approve = CommonConstants.Approved;
                            itemCheck.ApproveLV2 = CommonConstants.Approved;
                            itemCheck.ApproveLV3 = CommonConstants.Request;
                            if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                            {
                                itemCheck.ApproveLV3 = CommonConstants.Approved;
                            }
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
            var lstNgayLeNam = _ngayLeNamService.GetAll(obj.NgayOT);
            var itemcheck = lstNgayLeNam.FirstOrDefault(x => x.Id == obj.NgayOT);
            var afterOneDay = DateTime.Parse(obj.NgayOT).AddDays(1).ToString("yyyy-MM-dd");
            var itemcheck2 = lstNgayLeNam.FirstOrDefault(x => x.Id == afterOneDay);

            if (itemcheck != null)
            {
                obj.DM_NgayLViec = CommonConstants.NgayLe;

                if (itemcheck.IslastHoliday == CommonConstants.Y)
                {
                    obj.DM_NgayLViec = CommonConstants.NgayLeCuoiCung;
                }
            }
            else if (itemcheck2 != null)
            {
                obj.DM_NgayLViec = CommonConstants.TruocNgayLe;
            }
            else if (DateTime.Parse(obj.NgayOT).DayOfWeek == DayOfWeek.Sunday)
            {
                obj.DM_NgayLViec = CommonConstants.ChuNhat;
            }
            else
            {
                obj.DM_NgayLViec = CommonConstants.NgayThuong;
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
        public IActionResult Approve(List<int> lstID, string action)
        {
            List<DangKyOTNhanVienViewModel> lstOT = _overtimeService.GetAll("").Where(x => lstID.Contains(x.Id)).ToList();

            if (action == "approve")
            {
                foreach (var item in lstOT)
                {
                    if (UserRole == CommonConstants.roleApprove1)
                    {
                        item.Approve = CommonConstants.Approved;
                    }
                    else if (UserRole == CommonConstants.roleApprove2)
                    {
                        item.ApproveLV2 = CommonConstants.Approved;
                    }
                    else if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                    {
                        item.ApproveLV3 = CommonConstants.Approved;
                    }
                }
            }
            else if (action == "unapprove")
            {
                foreach (var item in lstOT)
                {
                    if (UserRole == CommonConstants.roleApprove1)
                    {
                        item.Approve = CommonConstants.No_Approved;
                    }
                    else if (UserRole == CommonConstants.roleApprove2)
                    {
                        item.ApproveLV2 = CommonConstants.No_Approved;
                    }
                    else if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                    {
                        item.ApproveLV3 = CommonConstants.No_Approved;
                    }
                }
            }

            _overtimeService.UpdateRange(lstOT);
            _overtimeService.Save();
            return new OkObjectResult(lstID);
        }

        [HttpPost]
        public IActionResult Search(string department, string status, string timeFrom, string timeTo)
        {
            var lst = _overtimeService.Search(UserRole, department, status, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC);
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
                ResultDB result = _overtimeService.ImportExcel(filePath, UserRole);

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
