using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
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
using Newtonsoft.Json;
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
            _memoryCache.GetOrCreate(CacheKeys.BoPhanInBiosStar.ToString(), entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                DataTableCollection datas = _bioStarDB.GetDeparment().ReturnDataSet.Tables;
                if (datas.Count > 0)
                {
                    return datas[0];
                }
                return new DataTable();
            });
            return View(new List<ChamCongLogViewModel>());
        }

        public IActionResult GetData()
        {
            List<ChamCongLogViewModel> chamCongLog = _chamCongService.GetAll("");

            string fromTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            var lstNv = _nhanVienService.GetAll().Where(x => x.Status != Status.InActive.ToString() || fromTime.NullString().CompareTo(x.NgayNghiViec) <= 0);

            foreach (var item in chamCongLog.ToList())
            {
                if (!lstNv.Any(x => x.Id.ToUpper() == "H" + item.ID_NV))
                {
                    chamCongLog.Remove(item);
                }
            }

            var lst = UpdateShifts(chamCongLog);
            _memoryCache.Remove("SearchData");
            _memoryCache.Set("SearchData", lst);

            return View("Index", lst);
        }

        [HttpGet]
        public IActionResult GetChamCongLog()
        {

            ResultDB result = _chamCongService.GetLogDataCurrentDay();
            _logger.LogInformation("GetChamCongLog: " + result.ReturnString);
            if (result.ReturnInt == 0)
            {
                return new OkObjectResult(result.ReturnString);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpGet]
        public IActionResult GetChamCongLogBiostar()
        {
            string maxDate = DateTime.Now.ToString("yyyy-MM-dd"); // _chamCongService.GetMaxDate();
            //if (string.IsNullOrEmpty(maxDate))
            //{
            //    maxDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            //}
            string fromTime = DateTime.Now.AddMonths(-5).ToString("yyyy-MM") + "-01"; // DateTime.Parse(maxDate).AddDays(-60).ToString("yyyy-MM-dd");
            string toTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            ResultDB result = _bioStarDB.GetChamCongLogData(fromTime, toTime);
            _logger.LogInformation("GetChamCongLogBiostar: " + result.ReturnString);
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
        public IActionResult GetChamCongAbsenceLog(string fromTime, string toTime, string dept)
        {
            List<ChamCongLogViewModel> lstLog = _chamCongService.GetByTime(fromTime, toTime).Where(x => x.FirstIn.NullString() == "" || x.LastOut.NullString() == "").ToList();
            //ResultDB result = _bioStarDB.GetChamCongAbsenceLogData(fromTime, toTime, dept);
            //_logger.LogInformation("GetChamCongAbsenceLog: " + result.ReturnString);
            if (lstLog.Count > 0)
            {
                var lstNv = _nhanVienService.GetAll().Where(x => x.Status != Status.InActive.ToString() || fromTime.NullString().CompareTo(x.NgayNghiViec) <= 0);

                //List<ChamCongLogViewModel> lstLog = new List<ChamCongLogViewModel>();
                //ChamCongLogViewModel model;
                //foreach (DataRow row in result.ReturnDataSet.Tables[0].Rows)
                //{
                //    model = new ChamCongLogViewModel()
                //    {
                //        ID_NV = row["userId"].NullString(),
                //        Ngay_ChamCong = row["Date_Check"].NullString(),
                //        FirstIn_Time = row["First_In_Time"].NullString(),
                //        Last_Out_Time = row["Last_Out_Time"].NullString(),
                //        FirstIn = row["First_In"].NullString(),
                //        LastOut = row["Last_Out"].NullString(),
                //        Ten_NV = row["userName"].NullString()
                //    };
                //    lstLog.Add(model);
                //}

                foreach (var item in lstLog.ToList())
                {
                    if (!lstNv.Any(x => x.Id.Contains(item.ID_NV)))
                    {
                        lstLog.Remove(item);
                    }
                }

                var newlst = UpdateShifts(lstLog);
                List<ChamCongSimpleModel> lstResult = new List<ChamCongSimpleModel>();
                ChamCongSimpleModel simpleModel;
                foreach (var item in newlst)
                {
                    simpleModel = new ChamCongSimpleModel()
                    {
                        ID_NV = item.ID_NV,
                        Ten_NV = item.Ten_NV,
                        CaLamViec = item.Shift,
                        FirstIn_Time = item.FirstIn_Time,
                        Last_Out_Time = item.Last_Out_Time,
                        Ngay_ChamCong = item.Ngay_ChamCong,
                    };

                    if (item.Shift.NullString().Contains("주간"))
                    {
                        if (item.FirstIn == "" && item.LastOut == "")
                        {
                            simpleModel.Status = "Không chấm công";
                        }
                        else if (item.FirstIn == "" && item.LastOut != "")
                        {
                            simpleModel.Status = "Không chấm đến";
                        }
                        else if (item.FirstIn != "" && item.LastOut == "" && DateTime.Now.ToString("yyyy-MM-dd") != item.Ngay_ChamCong)
                        {
                            simpleModel.Status = "Không chấm về";
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        simpleModel.Last_Out_Time = item.Last_Out_Time_Update;

                        if (DateTime.Now.ToString("yyyy-MM-dd") == item.Ngay_ChamCong)
                        {
                            continue;
                        }
                        else
                        {
                            if (item.FirstIn_Time == "00:00:00" && item.Last_Out_Time_Update == "00:00:00")
                            {
                                simpleModel.Status = "Không chấm công";
                            }
                            else
                            if (item.FirstIn_Time == "00:00:00")
                            {
                                simpleModel.Status = "Không chấm đến";
                            }
                            else if (item.Last_Out_Time_Update == "00:00:00")
                            {
                                simpleModel.Status = "Không chấm về";
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    lstResult.Add(simpleModel);
                }

                foreach (var item in lstResult)
                {
                    item.BoPhan = lstNv.FirstOrDefault(x => x.Id.Contains(item.ID_NV))?.MaBoPhan;
                }

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
                    worksheet.Cells["A1"].LoadFromCollection(lstResult.OrderBy(x => x.Ngay_ChamCong), true, TableStyles.Light11);
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
            DataTable data = _memoryCache.GetOrCreate(CacheKeys.BoPhanInBiosStar.ToString(), entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return _bioStarDB.GetDeparment().ReturnDataSet.Tables[0];
            });

            _logger.LogInformation("GetDepartment: " + data.Rows.Count);
            if (data.Rows.Count > 0)
            {
                string depts = DataTableToJson.DataTableToJSONWithJSONNet(data);
                return new OkObjectResult(depts);
            }
            else
            {
                string jsonDept = "[{\"sName\":\"Support\"},{\"sName\":\"QualityControl\"},{\"sName\":\"SMT\"},{\"sName\":\"LFEM\"},{\"sName\":\"BoardofDirector\"},{\"sName\":\"Cleaner\"},{\"sName\":\"Security\"},{\"sName\":\"Driver\"},{\"sName\":\"Bep\"},{\"sName\":\"Utility\"},{\"sName\":\"Warehouse\"},{\"sName\":\"WLP2\"},{\"sName\":\"KoreanVN\"},{\"sName\":\"WLP1\"},{\"sName\":\"CSP\"},{\"sName\":\"Baove&Vesin\"}]";
                return new OkObjectResult(jsonDept);
            }
        }

        [HttpPost]
        public IActionResult Search(string result, string dept, string fromTime, string toTime, string maNV, string requestApprove)
        {
             if (result.NullString() == "" && dept.NullString() == "" && fromTime.NullString() == "" && toTime.NullString() == "" && maNV.NullString() == "")
            {
                return PartialView("_gridChamCongPartialView", new List<ChamCongLogViewModel>());
            }
            string status = "";

            if (result == "Normal" || result == "Absence")
            {
                status = result;
                result = "";
            }

            _chamCongService.SetDepartment(Department);
            var lst = _chamCongService.Search(result, dept, requestApprove, ref fromTime, ref toTime);
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
                var lstNv = _nhanVienService.GetAll().Where(x => x.MaBoPhan == Department && (x.Status != Status.InActive.ToString() || fromTime.NullString().CompareTo(x.NgayNghiViec) <= 0));

                foreach (var item in lst.ToList())
                {
                    if (!lstNv.Any(x => x.Id.ToUpper() == "H" + item.ID_NV))
                    {
                        lst.Remove(item);
                    }
                }
            }
            else
            {
                var lstNv = _nhanVienService.GetAll().Where(x => x.Status != Status.InActive.ToString() || fromTime.NullString().CompareTo(x.NgayNghiViec) <= 0);

                foreach (var item in lst.ToList())
                {
                    if (!lstNv.Any(x => x.Id.ToUpper() == "H" + item.ID_NV))
                    {
                        lst.Remove(item);
                    }
                    else
                    {
                        item.Department = lstNv.FirstOrDefault(x => x.Id.ToUpper() == "H" + item.ID_NV)?.MaBoPhan;
                    }
                }
            }

            var lstData = UpdateShifts(lst);

            if (requestApprove.NullString() != "")
            {
                lstData = lstData.FindAll(x => x.ApproveRequest == requestApprove);
            }

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
                UserModified = UserName
            };

            if (UserRole == "Admin" || UserRole == "HR")
            {
                model.FirstIn_Time = firstTime;
                model.Last_Out_Time = lastTime;
                model.FirstIn_Time_Request = firstTime;
                model.Last_Out_Time_Request = lastTime;
                model.ApproveRequest = "Approve";
                _chamCongService.Update(model);
            }
            else
            {
                model.FirstIn_Time_Request = firstTime;
                model.Last_Out_Time_Request = lastTime;
                model.ApproveRequest = "Request";
                _chamCongService.UpdateRequest(model);
            }

            _chamCongService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult ApproveAction(List<long> lstID, string action)
        {
            List<ChamCongLogViewModel> lstChamCongLog = _chamCongService.GetAll("").Where(x => lstID.Contains(x.Id)).OrderBy(x => x.Ngay_ChamCong).ToList();

            if (action == "approve")
            {
                if (UserRole == CommonConstants.roleApprove3 || UserRole == CommonConstants.AppRole.AdminRole)
                {
                    foreach (var item in lstChamCongLog)
                    {
                        item.ApproveRequest = "Approve";
                        item.FirstIn_Time = item.FirstIn_Time_Request;
                        item.Last_Out_Time = item.Last_Out_Time_Request;
                        _chamCongService.UpdateApprove(item);
                    }
                    _chamCongService.Save();

                    _chamCongService.UpdateAfterApprove(lstChamCongLog);
                    _chamCongService.Save();
                }
            }

            return new OkObjectResult(lstID);
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
