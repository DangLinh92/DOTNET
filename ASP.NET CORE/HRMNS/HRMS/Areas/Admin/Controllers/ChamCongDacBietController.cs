using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace HRMS.Areas.Admin.Controllers
{
    public class ChamCongDacBietController : AdminBaseController
    {
        private IDangKyChamCongDacBietService _chamCongDacBietService;
        private IDangKyChamCongChiTietService _chamCongChiTietService;
        private INhanVienService _nhanvienService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ChamCongDacBietController(IDangKyChamCongDacBietService chamCongService, IDangKyChamCongChiTietService chamCongChiTietService, INhanVienService nhanvienService, IWebHostEnvironment hostingEnvironment)
        {
            _chamCongDacBietService = chamCongService;
            _chamCongChiTietService = chamCongChiTietService;
            _nhanvienService = nhanvienService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<DangKyChamCongDacBietViewModel> lst = new List<DangKyChamCongDacBietViewModel>();
            if (UserRole == CommonConstants.AssLeader_Role)
            {
                lst = _chamCongDacBietService.GetAll(x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET).Where(x => x.HR_NHANVIEN.MaBoPhan == Department).OrderByDescending(x => x.DateModified).Take(1000).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove1) // leader 
            {
                lst = _chamCongDacBietService.GetAll(x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET).
                    Where(x => x.HR_NHANVIEN.MaBoPhan == Department &&
                    (x.Approve == CommonConstants.Request || x.ApproveLV2 == CommonConstants.Request || x.ApproveLV3 == CommonConstants.Request)).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove2) // korea manager
            {
                lst = _chamCongDacBietService.GetAll(x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET).
                    Where(x => x.HR_NHANVIEN.MaBoPhan == Department && x.Approve == CommonConstants.Approved && (x.ApproveLV2 == CommonConstants.Request || x.ApproveLV3 == CommonConstants.Request)).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole) // hr approve
            {
                lst = _chamCongDacBietService.GetAll(x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET).Where(x => x.ApproveLV2 == CommonConstants.Approved && x.ApproveLV3 == CommonConstants.Request).OrderByDescending(x => x.DateModified).ToList();
            }

            return View(lst);
        }

        [HttpPost]
        public IActionResult RegisterChamCongDB(List<DangKyChamCongDacBietViewModel> arrManv, [FromQuery] string action)
        {
            foreach (var data in arrManv.Where(x => x.MaNV != null || x.MaNV != "").ToList())
            {
                if (data != null && string.IsNullOrEmpty(data.NoiDung.NullString()))
                {
                    var chamCongct = _chamCongChiTietService.GetById((int)data.MaChamCong_ChiTiet);
                    if (chamCongct != null && CommonConstants.arrNghiPhep.Contains(chamCongct.KyHieuChamCong))
                    {
                        data.NoiDung = _nhanvienService.GetById(data.MaNV.NullString())?.TenNV + " " + chamCongct.TenChiTiet + "\n";
                    }
                }
                else
                {
                    if (!data.NoiDung.Contains(_nhanvienService.GetById(data.MaNV.NullString())?.TenNV))
                    {
                        data.NoiDung = _nhanvienService.GetById(data.MaNV.NullString())?.TenNV + " " + data.NoiDung + "\n";
                    }
                }

                if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                {
                    data.GiaTri = double.Parse(data.GiaTri1, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    data.GiaTri = 0;
                }

                if (action == "Add")
                {
                    var itemCheck = _chamCongDacBietService.GetSingle(x => x.MaNV == data.MaNV && x.NgayBatDau == data.NgayBatDau && x.NgayKetThuc == data.NgayKetThuc);

                    if (itemCheck != null)
                    {
                        itemCheck.MaChamCong_ChiTiet = data.MaChamCong_ChiTiet;
                        itemCheck.NgayBatDau = data.NgayBatDau;
                        itemCheck.NgayKetThuc = data.NgayKetThuc;
                        itemCheck.NoiDung = data.NoiDung;
                        itemCheck.GiaTri = data.GiaTri;

                        itemCheck.Approve = CommonConstants.Approved;
                        itemCheck.ApproveLV2 = CommonConstants.Approved;
                        itemCheck.ApproveLV3 = CommonConstants.Request;
                        if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        {
                            itemCheck.ApproveLV3 = CommonConstants.Approved;
                        }

                        _chamCongDacBietService.Update(itemCheck);
                    }
                    else
                    {
                        #region
                        //if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole) // HR
                        //{
                        //    data.Approve = CommonConstants.Approved;
                        //    data.ApproveLV2 = CommonConstants.Approved;
                        //    data.ApproveLV3 = CommonConstants.Approved;
                        //}
                        //else if (UserRole == CommonConstants.roleApprove1) // group leader
                        //{
                        //    data.Approve = CommonConstants.Approved;
                        //    data.ApproveLV2 = CommonConstants.Request;
                        //    data.ApproveLV3 = CommonConstants.Request;
                        //}
                        //else if (UserRole == CommonConstants.roleApprove2) // korea 
                        //{
                        //    data.Approve = CommonConstants.Approved;
                        //    data.ApproveLV2 = CommonConstants.Approved;
                        //    data.ApproveLV3 = CommonConstants.Request;
                        //}
                        //else // assys leader
                        //{
                        //    if(data.MaChamCong_ChiTiet == 5) // cham OT bo xung
                        //    {
                        //        data.Approve = CommonConstants.Approved;
                        //        data.ApproveLV2 = CommonConstants.Approved;
                        //        data.ApproveLV3 = CommonConstants.Approved;
                        //    }
                        //    else
                        //    {
                        //        data.Approve = CommonConstants.Request;
                        //        data.ApproveLV2 = CommonConstants.Request;
                        //        data.ApproveLV3 = CommonConstants.Request;
                        //    }
                        //}
                        #endregion

                        data.Approve = CommonConstants.Approved;
                        data.ApproveLV2 = CommonConstants.Approved;
                        data.ApproveLV3 = CommonConstants.Request;
                        if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                        {
                            data.ApproveLV3 = CommonConstants.Approved;
                        }

                        _chamCongDacBietService.Add(data);
                    }
                }
                else
                {
                    DangKyChamCongDacBietViewModel chamcongVm = _chamCongDacBietService.GetById(data.Id);
                    chamcongVm.MaChamCong_ChiTiet = data.MaChamCong_ChiTiet;
                    chamcongVm.NgayBatDau = data.NgayBatDau;
                    chamcongVm.NgayKetThuc = data.NgayKetThuc;
                    chamcongVm.NoiDung = data.NoiDung;
                    chamcongVm.GiaTri = data.GiaTri;

                    chamcongVm.Approve = CommonConstants.Approved;
                    chamcongVm.ApproveLV2 = CommonConstants.Approved;
                    chamcongVm.ApproveLV3 = CommonConstants.Request;
                    if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                    {
                        chamcongVm.ApproveLV3 = CommonConstants.Approved;
                    }

                    _chamCongDacBietService.Update(chamcongVm);
                }
            }
            _chamCongDacBietService.Save();
            return new OkObjectResult(arrManv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstID">Danh sach Id DANGKY_CHAMCONG_DACBIET</param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApproveAction(List<int> lstID, string action)
        {
            List<DangKyChamCongDacBietViewModel> lstChamCongDB = _chamCongDacBietService.GetAll().Where(x => lstID.Contains(x.Id)).ToList();

            if (action == "approve")
            {
                foreach (var item in lstChamCongDB)
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

                    if (item.NoiDung.NullString() != "" && !item.NoiDung.Contains(_nhanvienService.GetById(item.MaNV.NullString())?.TenNV))
                    {
                        item.NoiDung = _nhanvienService.GetById(item.MaNV.NullString())?.TenNV + " " + item.NoiDung + "\n";
                    }
                }
            }
            else if (action == "unapprove")
            {
                foreach (var item in lstChamCongDB)
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

                    if (item.NoiDung.NullString() != "" && !item.NoiDung.Contains(_nhanvienService.GetById(item.MaNV.NullString())?.TenNV))
                    {
                        item.NoiDung = _nhanvienService.GetById(item.MaNV.NullString())?.TenNV + " " + item.NoiDung + "\n";
                    }
                }
            }

            _chamCongDacBietService.UpdateRange(lstChamCongDB);
            _chamCongDacBietService.Save();
            return new OkObjectResult(lstID);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var en = _chamCongDacBietService.GetById(id);
            return new OkObjectResult(en);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _chamCongDacBietService.Delete(id);
            _chamCongDacBietService.Save();
            return new OkObjectResult(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <param name="timeFrom">Thoi gian tao from</param>
        /// <param name="timeTo">Thoi gian tao to</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Search(string department, string timeFrom, string timeTo)
        {
            var lst = _chamCongDacBietService.Search(department, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET);

            if (UserRole == CommonConstants.AssLeader_Role)
            {
                lst = lst.OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove1) // leader 
            {
                lst = lst.Where(x => x.Approve == CommonConstants.Request || x.Approve == CommonConstants.Approved || x.Approve == CommonConstants.No_Approved).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove2) // korea manager
            {
                lst = lst.
                    Where(x => x.Approve == CommonConstants.Approved &&
                    (x.ApproveLV2 == CommonConstants.Request ||
                    x.ApproveLV2 == CommonConstants.Approved ||
                    x.ApproveLV2 == CommonConstants.No_Approved)).
                    OrderByDescending(x => x.DateModified).ToList();
            }
            else if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole) // hr approve
            {
                lst = lst
                    .Where(x => x.ApproveLV2 == CommonConstants.Approved && (x.ApproveLV3 == CommonConstants.Request || x.ApproveLV3 == CommonConstants.Approved || x.ApproveLV3 == CommonConstants.No_Approved)).OrderByDescending(x => x.DateModified).ToList();
            }

            return PartialView("_gridChamCongDacBietPartialView", lst);
        }

        [HttpGet]
        public IActionResult GetDmucChamCongChiTiet()
        {
            var lst = _chamCongChiTietService.GetAll(x => x.DM_DANGKY_CHAMCONG);
            return new OkObjectResult(lst);
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
                ResultDB result = _chamCongDacBietService.ImportExcel(filePath, UserRole);

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
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }

            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpPost]
        public IActionResult ExportExcel(string department, string timeFrom, string timeTo)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"ChamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var data = _chamCongDacBietService.Search(department, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET);
            List<ChamCongDBViewModel> lstChamDB = new List<ChamCongDBViewModel>();
            ChamCongDBViewModel model;
            foreach (var item in data)
            {
                model = new ChamCongDBViewModel()
                {
                    UserId = item.MaNV,
                    UserName = item.HR_NHANVIEN.TenNV,
                    BoPhan = item.HR_NHANVIEN.MaBoPhan,
                    DanhMuc = item.DANGKY_CHAMCONG_CHITIET?.KyHieuChamCong,
                    Begin = item.NgayBatDau,
                    End = item.NgayKetThuc,
                    NoiDung = item.NoiDung
                };

                lstChamDB.Add(model);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ChamCongDB");
                worksheet.Cells["A1"].LoadFromCollection(lstChamDB, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }
    }

    public class ChamCongDBViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string BoPhan { get; set; }
        public string DanhMuc { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public string NoiDung { get; set; }
    }
}
