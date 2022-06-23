﻿using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class ChamCongController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IChamCongService _chamCongService;
        private INhanVienService _nhanVienService;
        private INhanVien_CalamviecService _nhanVien_CalamviecService;
        private BioStarDBContext _bioStarDB;
        private readonly IMemoryCache _memoryCache;

        public ChamCongController(IChamCongService chamCongService, INhanVienService nhanVienService, INhanVien_CalamviecService nhanVien_CalamviecService, IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment, BioStarDBContext bioStarDB, ILogger<ChamCongController> logger)
        {
            _chamCongService = chamCongService;
            _bioStarDB = bioStarDB;
            _hostingEnvironment = hostingEnvironment;
            _nhanVienService = nhanVienService;
            _nhanVien_CalamviecService = nhanVien_CalamviecService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ResultDB result = _memoryCache.GetOrCreate(CacheKeys.BoPhanInBiosStar.ToString(), entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return _bioStarDB.GetDeparment();
            });
            return View(new List<ChamCongLogViewModel>());
        }

        public IActionResult GetData()
        {
            List<ChamCongLogViewModel> chamCongLog = _chamCongService.GetAll("");

            var lst = UpdateShifts(chamCongLog);
            _memoryCache.Remove("SearchData");
            _memoryCache.Set("SearchData", lst);

            return View("Index", lst);
        }

        [HttpGet]
        public IActionResult GetChamCongLog()
        {
            string maxDate = _chamCongService.GetMaxDate();
            if (string.IsNullOrEmpty(maxDate))
            {
                maxDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            string fromTime = DateTime.Parse(maxDate).AddDays(-40).ToString("yyyy-MM-dd");
            string toTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            ResultDB result = _bioStarDB.GetChamCongLogData(fromTime, toTime);
            _logger.LogInformation("GetChamCongLog: "+result.ReturnString);
            if (result.ReturnInt == 0)
            {
                ResultDB result1 = _chamCongService.InsertLogData(result.ReturnDataSet.Tables[0]);

                if (result1.ReturnInt == 0)
                {
                    return new OkObjectResult(result1.ReturnString);
                }
                else
                {
                    return new BadRequestObjectResult(result1.ReturnString);
                }
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        /// <summary>
        /// Get du lieu cham cong loi
        /// </summary>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetChamCongAbsenceLog(string fromTime,string toTime,string dept)
        {
            ResultDB result = _bioStarDB.GetChamCongAbsenceLogData(fromTime, toTime,dept);
            _logger.LogInformation("GetChamCongAbsenceLog: " + result.ReturnString);
            if (result.ReturnInt == 0)
            {
                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string directory = Path.Combine(sWebRootFolder, "export-files");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string sFileName = $"DataAbsenceChamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
                string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
                FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data-LoiChamCong");
                    worksheet.Cells["A1"].LoadFromDataTable(result.ReturnDataSet.Tables[0], true, TableStyles.Light11);
                    worksheet.Cells.AutoFitColumns();
                    package.Save(); //Save the workbook.
                }

                return new OkObjectResult(fileUrl);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpGet]
        public IActionResult GetDepartment()
        {
            ResultDB result = _memoryCache.GetOrCreate(CacheKeys.BoPhanInBiosStar.ToString(), entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return _bioStarDB.GetDeparment();
            });
            _logger.LogInformation("GetDepartment: " + result.ReturnString);
            if (result.ReturnInt == 0)
            {
                DataTable data = result.ReturnDataSet.Tables[0];
                if (data.Rows.Count == 0)
                {
                    ResultDB result1 = _bioStarDB.GetDeparment();
                    _logger.LogInformation("GetDepartment: " + result1.ReturnString);
                    if (result1.ReturnInt == 0)
                    {
                        data = result1.ReturnDataSet.Tables[0];
                    }
                }
                string depts = DataTableToJson.DataTableToJSONWithJSONNet(data);
                return new OkObjectResult(depts);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpPost]
        public IActionResult Search(string result, string dept, string fromTime, string toTime, string maNV)
        {
            string status = "";

            if (result == "Normal" || result == "Absence")
            {
                status = result;
                result = "";
            }

            _chamCongService.SetDepartment(Department);
            var lst = _chamCongService.Search(result, dept, fromTime, toTime);
            if (!string.IsNullOrEmpty(maNV.NullString()) && lst.Any(x => maNV != null && maNV.Contains(x.ID_NV)))
            {
                lst = lst.Where(x => maNV.Contains(x.ID_NV)).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                lst = lst.Where(x => x.Status.Contains(status)).ToList();
            }

            if (!string.IsNullOrEmpty(Department) && Department != CommonConstants.SUPPORT_DEPT)
            {
                var lstNv = _nhanVienService.GetAll().Where(x => x.MaBoPhan == Department && x.Status != Status.InActive.ToString());

                foreach (var item in lst.ToList())
                {
                    if (!lstNv.Any(x => x.Id.Contains(item.ID_NV)))
                    {
                        lst.Remove(item);
                    }
                }
            }
            else
            {
                var lstNv = _nhanVienService.GetAll().Where(x => x.Status != Status.InActive.ToString());

                foreach (var item in lst.ToList())
                {
                    if (!lstNv.Any(x => x.Id.Contains(item.ID_NV)))
                    {
                        lst.Remove(item);
                    }
                }
            }

            var lstData = UpdateShifts(lst);
            _memoryCache.Remove("SearchData");
            _memoryCache.Set("SearchData", lstData);

            return PartialView("_gridChamCongPartialView", lstData);
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            List<ChamCongLogViewModel> chamCongs = new List<ChamCongLogViewModel>();
            _memoryCache.TryGetValue("SearchData", out chamCongs);
            if (!_memoryCache.TryGetValue("SearchData", out chamCongs) || chamCongs.Count == 0)
            {
                return new BadRequestObjectResult("Not found data!");
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"DataChamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("DataChamCong");
                worksheet.Cells["A1"].LoadFromCollection(chamCongs, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }

            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public IActionResult GetLogByUserId(string userId, string time)
        {
            ResultDB result = _bioStarDB.ShowChamCongLogInDay(userId, time);
            if (result.ReturnInt == 0)
            {
                string data = DataTableToJson.DataTableToJSONWithJSONNet(result.ReturnDataSet.Tables[0]);
                return new OkObjectResult(data);
            }
            else
            {
                return new OkObjectResult(result.ReturnString);
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

                ResultDB rs = _bioStarDB.GetAllNVInfo();
                if (rs.ReturnInt == 0)
                {
                    ResultDB result = _chamCongService.ImportExcel(filePath, rs.ReturnDataSet.Tables[0]);

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
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpPost]
        public IActionResult UpdateTimeChamCong(string firstTime, string lastTime, string maNV, string ngayChamCong, string tenNV)
        {
            if (firstTime.Split(":").Length == 2)
            {
                firstTime += ":00";
            }

            if (lastTime.Split(":").Length == 2)
            {
                lastTime += ":00";
            }

            ChamCongLogViewModel model = new ChamCongLogViewModel()
            {
                ID_NV = maNV,
                Ten_NV = tenNV,
                Ngay_ChamCong = ngayChamCong,
                FirstIn_Time = firstTime,
                Last_Out_Time = lastTime,
                UserModified = UserName
            };
            _chamCongService.Update(model);
            _chamCongService.Save();
            return new OkObjectResult(model);
        }

        private List<ChamCongLogViewModel> UpdateShifts(List<ChamCongLogViewModel> chamCongs)
        {
            NhanVien_CalamViecViewModel clv;
            foreach (var item in chamCongs)
            {
                clv = _nhanVien_CalamviecService.FindCaLamViecByDay(item.ID_NV, item.Ngay_ChamCong);
                if (clv != null)
                {
                    item.Shift = clv.DM_CA_LVIEC.TenCaLamViec;
                }
            }
            return chamCongs;
        }
    }
}
