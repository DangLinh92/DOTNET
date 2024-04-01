using CarMNS.Application.Implementation;
using CarMNS.Application.Interfaces;
using CarMNS.Application.ViewModels;
using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using CarMNS.Extensions;
using CarMNS.Services;
using CarMNS.Utilities.Common;
using CarMNS.Utilities.Constants;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.CodeParser;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace CarMNS.Areas.Admin.Controllers
{
    public class DangKyXeController : AdminBaseController
    {
        private IDangKyXeService _DangKyXeService;
        private IDriverAndCarService _XeAndLaiXeService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;
        public DangKyXeController(IEmailSender emailSender, IDangKyXeService dangKyXeService, IDriverAndCarService xeAndLaiXeService, IWebHostEnvironment hostEnvironment)
        {
            _DangKyXeService = dangKyXeService;
            _XeAndLaiXeService = xeAndLaiXeService;
            _hostingEnvironment = hostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Taxi()
        {
            return View();
        }

        #region Đăng Ký Xe Taxi
        [HttpGet]
        public object GetDangKyXeAll_Taxi(DataSourceLoadOptions loadOptions)
        {
            var lstXe = _DangKyXeService.GetAllDangKyXe_Taxi(UserRole, Department);
            return DataSourceLoader.Load(lstXe, loadOptions);
        }

        [HttpGet]
        public object GetHistoryByUser(DataSourceLoadOptions loadOptions, string maNV)
        {
            var lstXe = _DangKyXeService.GetHistoryByUser(maNV);
            return DataSourceLoader.Load(lstXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostDangKyXe_Taxi(string values)
        {
            var xe = new DANG_KY_XE_TAXI();
            JsonConvert.PopulateObject(values, xe);

            xe.MaBill = xe.MaBill.NullString();
            xe = _DangKyXeService.AddDangKyXe_Taxi(xe, UserRole);

            if (xe != null)
            {
                var dic = _DangKyXeService.GetUserSendMailTaxi(xe.Id, true, false, false);
                if (dic["SEND_NEXT"].Count > 0)
                {
                    var message = new Message(dic["SEND_NEXT"], "REGISTER TO USE TAXI", "You have a request need approve, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }

            return Ok(xe);
        }

        [HttpPut]
        public IActionResult PutDangKyXe_Taxi(int key, string values)
        {
            var xe = _DangKyXeService.GetDangKyXeTaxiById(key);

            if (xe != null)
            {
                JsonConvert.PopulateObject(values, xe);
                xe = _DangKyXeService.UpdateDangKyXeTaxi(xe);

                return Ok(xe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteDangKyXe_Taxi(int key)
        {
            _DangKyXeService.DeleteDangKyXeTaxi(key);
        }

        /// <summary>
        /// Approve đăng ký xe
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApproveTaxi(int key)
        {
            bool rs = _DangKyXeService.ApproveTaxi(key, UserRole);
            if (rs)
            {
                var dic = _DangKyXeService.GetUserSendMailTaxi(key, false, true, false);

                if (dic["SEND_NEXT"].Count > 0)
                {
                    var message = new Message(dic["SEND_NEXT"], "REGISTER TO USE TAXI", "You have a request need approve, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
                if (dic["SEND_PRE"].Count > 0)
                {
                    var message = new Message(dic["SEND_PRE"], "REGISTER TO USE TAXI", "You have a request approved, please check: http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }
            return Ok(key);
        }

        /// <summary>
        /// UnApprove đăng ký xe
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UnApproveTaxi(int key)
        {
            var rs = _DangKyXeService.UnApproveTaxi(key, UserRole);

            if (rs)
            {
                var dic = _DangKyXeService.GetUserSendMailTaxi(key, false, false, true);

                if (dic["SEND_PRE"].Count > 0)
                {
                    var message = new Message(dic["SEND_PRE"], "REGISTER TO USE TAXI", "You have a request unapproved, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }
            return Ok(key);
        }

        public IActionResult ViewHistoryTaxi()
        {
            return View();
        }

        [HttpGet]
        public object Get_HistoryTaxi(DataSourceLoadOptions loadOptions, string fromTime, string toTime, string bophan)
        {
            var lst = _DangKyXeService.GetDangKyXeTaxiHistory();
            if (!string.IsNullOrEmpty(fromTime))
            {
                lst = lst.Where(x => fromTime.NullString().CompareTo(x.NgaySuDung?.ToString("yyyy-MM-dd")) <= 0).ToList();
            }

            if (!string.IsNullOrEmpty(toTime))
            {
                lst = lst.Where(x => toTime.NullString().CompareTo(x.NgaySuDung?.ToString("yyyy-MM-dd")) >= 0).ToList();
            }

            if (!string.IsNullOrEmpty(bophan))
            {
                lst = lst.Where(x => x.BoPhan == bophan).ToList();
            }

            return DataSourceLoader.Load(lst.OrderByDescending(x => x.NgaySuDung).ThenBy(x => x.MaBill.NullString()), loadOptions);
        }

        public IActionResult TaxiReport()
        {
            return View();
        }

        [HttpGet]
        public object ReportTaxiByBoPhan(DataSourceLoadOptions loadOptions, string fromTime, string toTime, string bophan)
        {
            var lst = _DangKyXeService.GetReportTaxi(fromTime, toTime, bophan);
            return DataSourceLoader.Load(lst, loadOptions);
        }

        [HttpGet]
        public object ReportTaxiTotalByBoPhan(DataSourceLoadOptions loadOptions, string fromTime, string toTime, string bophan)
        {
            var lst = _DangKyXeService.GetReportTaxi(fromTime, toTime, bophan);

            List<TongHopBoPhan> lstTotal = new List<TongHopBoPhan>();
            TongHopBoPhan totalBP_Sotien;
            TongHopBoPhan totalBP_SoNgay;
            foreach (var item in lst)
            {
                if (lstTotal.Any(x => x.BoPhan == item.BoPhan))
                {
                    totalBP_Sotien = lstTotal.FirstOrDefault(x => x.RowTital == "금액" && x.BoPhan == item.BoPhan);
                    totalBP_SoNgay = lstTotal.FirstOrDefault(x => x.RowTital == "횟수" && x.BoPhan == item.BoPhan);
                }
                else
                {
                    totalBP_Sotien = new TongHopBoPhan()
                    {
                        RowTital = "금액",
                        BoPhan = item.BoPhan,
                        SoTien_SD = 0
                    };

                    totalBP_SoNgay = new TongHopBoPhan()
                    {
                        RowTital = "횟수",
                        BoPhan = item.BoPhan,
                        SoTien_SD = 0
                    };
                }

                totalBP_Sotien.SoTien_SD += item.SoTien;
                totalBP_SoNgay.SoTien_SD += item.SoLanSuDung;

                if (!lstTotal.Any(x => x.BoPhan == item.BoPhan))
                {
                    lstTotal.Add(totalBP_Sotien);
                    lstTotal.Add(totalBP_SoNgay);
                }
            }

            return DataSourceLoader.Load(lstTotal, loadOptions);
        }

        [HttpGet]
        public object ReportTaxiTotalByBoPhanInYear(DataSourceLoadOptions loadOptions, string year)
        {
            string fromTime = year + "-01-01";
            string toTime = DateTime.Parse(fromTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            var lst = _DangKyXeService.GetReportTaxiInYear(fromTime, toTime);

            List<TongHopBoPhan> lstTotal = new List<TongHopBoPhan>();
            TongHopBoPhan totalBP_Sotien;
            foreach (var item in lst)
            {
                if (lstTotal.Any(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2)))
                {
                    totalBP_Sotien = lstTotal.FirstOrDefault(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2));
                }
                else
                {
                    totalBP_Sotien = new TongHopBoPhan()
                    {
                        Month = item.ThangSuDung.Substring(5, 2), // yyyy-MM
                        RowTital = item.BoPhan,
                        ColumnTital = item.ThangSuDung.Substring(5, 2),
                        BoPhan = item.BoPhan,
                        SoTien_SD = 0
                    };
                }

                totalBP_Sotien.SoTien_SD += item.SoTien;

                if (!lstTotal.Any(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2)))
                {
                    lstTotal.Add(totalBP_Sotien);
                }
            }

            return DataSourceLoader.Load(lstTotal.OrderByDescending(x => x.SoTien_SD), loadOptions);
        }

        public object DataReportTaxiTotalByBoPhanInYear(string year)
        {
            string fromTime = year + "-01-01";
            string toTime = DateTime.Parse(fromTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            var lst = _DangKyXeService.GetReportTaxiInYear(fromTime, toTime);

            List<TongHopBoPhan> lstTotal = new List<TongHopBoPhan>();
            TongHopBoPhan totalBP_Sotien;
            foreach (var item in lst)
            {
                if (lstTotal.Any(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2)))
                {
                    totalBP_Sotien = lstTotal.FirstOrDefault(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2));
                }
                else
                {
                    totalBP_Sotien = new TongHopBoPhan()
                    {
                        Month = item.ThangSuDung.Substring(5, 2), // yyyy-MM
                        RowTital = item.BoPhan,
                        ColumnTital = item.ThangSuDung.Substring(5, 2),
                        BoPhan = item.BoPhan,
                        SoTien_SD = 0
                    };
                }

                totalBP_Sotien.SoTien_SD += item.SoTien;

                if (!lstTotal.Any(x => x.BoPhan == item.BoPhan && x.Month == item.ThangSuDung.Substring(5, 2)))
                {
                    lstTotal.Add(totalBP_Sotien);
                }
            }

            List<ChartReportTaxiItem> chartData = new List<ChartReportTaxiItem>();
            ChartReportTaxiItem chartItem;
            foreach (var item in lstTotal)
            {
                if (chartData.Any(x => x.Month == item.Month))
                {
                    chartItem = chartData.FirstOrDefault(x => x.Month == item.Month);
                }
                else
                {
                    chartItem = new ChartReportTaxiItem()
                    {
                        Month = item.Month
                    };

                    chartData.Add(chartItem);
                }

                if (item.BoPhan == "Account")
                {
                    chartItem.Account = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "CSP")
                {
                    chartItem.CSP = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "EHS")
                {
                    chartItem.EHS = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "GOC")
                {
                    chartItem.GOC = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "GUEST")
                {
                    chartItem.Guest = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "HR")
                {
                    chartItem.HR = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "KHO")
                {
                    chartItem.Warehouse = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "LFEM")
                {
                    chartItem.LFEM = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "PI")
                {
                    chartItem.PI = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "Purchase")
                {
                    chartItem.Purchase = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "QC")
                {
                    chartItem.QC = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "SMT")
                {
                    chartItem.SMT = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "SP")
                {
                    chartItem.Support = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "UTILITY")
                {
                    chartItem.UTILITY = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "WLP1")
                {
                    chartItem.WLP1 = Math.Round(item.SoTien_SD, 0);
                }

                if (item.BoPhan == "WLP2")
                {
                    chartItem.WLP2 = Math.Round(item.SoTien_SD, 0);
                }
            }

            return chartData.OrderBy(x => x.Month).ToList();
        }
        #endregion

        #region Dang Ky Xe Oto
        [HttpGet]
        public object GetBoPhan(DataSourceLoadOptions loadOptions)
        {
            var bophans = _DangKyXeService.GetBoPhan();
            return DataSourceLoader.Load(bophans, loadOptions);
        }

        [HttpGet]
        public object GetCardInfo(DataSourceLoadOptions loadOptions)
        {
            var cardInfos = _DangKyXeService.GetBoCardInfo();
            return DataSourceLoader.Load(cardInfos, loadOptions);
        }

        [HttpGet]
        public object GetMucDichSD(DataSourceLoadOptions loadOptions)
        {
            var cardInfos = _DangKyXeService.GetMucDichSD();
            return DataSourceLoader.Load(cardInfos, loadOptions);
        }

        [HttpGet]
        public object GetBoPhan2(DataSourceLoadOptions loadOptions)
        {
            var bophans = _DangKyXeService.GetBoPhan();
            bophans.Insert(0, new BOPHAN() { Id = "", TenBoPhan = "ALL" });
            return DataSourceLoader.Load(bophans, loadOptions);
        }

        [HttpGet]
        public object GetDangKyXeAll(DataSourceLoadOptions loadOptions)
        {
            var lstXe = _DangKyXeService.GetAllDangKyXe(UserRole, Department);
            return DataSourceLoader.Load(lstXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostDangKyXe(string values)
        {
            var xe = new DANG_KY_XE();
            JsonConvert.PopulateObject(values, xe);

            xe = _DangKyXeService.AddDangKyXe(xe, UserRole);

            if (xe != null)
            {
                var dic = _DangKyXeService.GetUserSendMail(xe.Id, true, false, false);
                if (dic["SEND_NEXT"].Count > 0)
                {
                    var message = new Message(dic["SEND_NEXT"], "REGISTER TO USE CAR", "You have a request need approve, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }

            return Ok(xe);
        }

        [HttpPut]
        public IActionResult PutDangKyXe(int key, string values)
        {
            var xe = _DangKyXeService.GetDangKyXeById(key);

            if (xe != null)
            {
                JsonConvert.PopulateObject(values, xe);
                xe = _DangKyXeService.UpdateDangKyXe(xe);

                return Ok(xe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteDangKyXe(int key)
        {
            _DangKyXeService.DeleteDangKyXe(key);
        }
        #endregion

        #region Thông tin điều xe
        [HttpGet]
        public object GetDieuXeAll(DataSourceLoadOptions loadOptions, int maDangKy)
        {
            var lstXe = _DangKyXeService.GetXe(maDangKy);
            return DataSourceLoader.Load(lstXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostDieuXe(string values)
        {
            var xe = new DIEUXE_DANGKY();
            JsonConvert.PopulateObject(values, xe);
            xe = _DangKyXeService.AddXe(xe);
            return Ok(xe);
        }

        [HttpPut]
        public IActionResult PutDieuXe(int key, string values)
        {
            var xe = _DangKyXeService.GetDieuXeById(key);

            if (xe != null)
            {
                JsonConvert.PopulateObject(values, xe);
                xe = _DangKyXeService.UpdateXe(xe);
                return Ok(xe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteDieuXe(int key)
        {
            _DangKyXeService.DeleteXe(key);
        }
        #endregion

        [HttpGet]
        public object GetXeInfo(DataSourceLoadOptions loadOptions)
        {
            var cars = _XeAndLaiXeService.GetAllCar();
            return DataSourceLoader.Load(cars, loadOptions);
        }

        [HttpGet]
        public object GetLaiXeInfo(DataSourceLoadOptions loadOptions)
        {
            var dricars = _XeAndLaiXeService.GetAllLaiXe();
            return DataSourceLoader.Load(dricars, loadOptions);
        }

        /// <summary>
        /// Approve đăng ký xe
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Approve(int key)
        {
            bool rs = _DangKyXeService.Approve(key, UserRole);
            if (rs)
            {
                var dic = _DangKyXeService.GetUserSendMail(key, false, true, false);

                if (dic["SEND_NEXT"].Count > 0)
                {
                    var message = new Message(dic["SEND_NEXT"], "REGISTER TO USE CAR", "You have a request need approve, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
                if (dic["SEND_PRE"].Count > 0)
                {
                    var message = new Message(dic["SEND_PRE"], "REGISTER TO USE CAR", "You have a request approved, please check: http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }
            return Ok(key);
        }

        /// <summary>
        /// UnApprove đăng ký xe
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UnApprove(int key)
        {
            var rs = _DangKyXeService.UnApprove(key, UserRole);

            if (rs)
            {
                var dic = _DangKyXeService.GetUserSendMail(key, false, false, true);

                if (dic["SEND_PRE"].Count > 0)
                {
                    var message = new Message(dic["SEND_PRE"], "REGISTER TO USE CAR", "You have a request unapproved, please check : http://10.70.10.97:8083/admin");
                    EmailSender.MailFrom = UserEmail;
                    _emailSender.SendEmail(message);
                }
            }
            return Ok(key);
        }


        [HttpPost]
        public ActionResult Upload(IFormFile myFile, [FromQuery] int maDangKy)
        {
            try
            {
                if (myFile != null)
                {
                    var filename = ContentDispositionHeaderValue
                                       .Parse(myFile.ContentDisposition)
                                       .FileName
                                       .Trim('"');
                    //string SavePath = Path.Combine(Directory.GetCurrentDirectory(), (@"C:\", filename));
                    string folder = "D:\\HRMS_DRAF_CAR";//_hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, maDangKy + "_" + filename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        myFile.CopyTo(fs);
                        fs.Flush();
                    }

                    var xe = _DangKyXeService.GetDangKyXeById(maDangKy);
                    xe.UrlDraf = filePath;
                    _DangKyXeService.UpdateDangKyXe(xe);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        public FileResult DownloadFile(int id) // can also be IActionResult
        {
            var xe = _DangKyXeService.GetDangKyXeById(id);
            if (xe != null)
            {
                // Get the MIMEType for the File
                var mimeType = GetMimeType(xe.UrlDraf);
                //string file = System.IO.Path.Combine(env.WebRootPath, "test.txt");
                return File(new FileStream(xe.UrlDraf, FileMode.Open), mimeType, Path.GetFileName(xe.UrlDraf)); // could have specified the downloaded file name again here
            }

            return null;
        }

        public string GetMimeType(string file)
        {
            var mimeTypes = MimeTypes.GetMimeTypes();
            var extension = Path.GetExtension(file).ToLowerInvariant();
            return mimeTypes[extension];
        }

        public IActionResult ViewHistory()
        {
            return View();
        }

        [HttpGet]
        public object Get_History(DataSourceLoadOptions loadOptions, string fromTime, string toTime, string bophan)
        {
            var lst = _DangKyXeService.GetDangKyXeHistory();
            if (!string.IsNullOrEmpty(fromTime))
            {
                lst = lst.Where(x => fromTime.NullString().CompareTo(x.NgaySuDung?.ToString("yyyy-MM-dd")) <= 0).ToList();
            }

            if (!string.IsNullOrEmpty(toTime))
            {
                lst = lst.Where(x => toTime.NullString().CompareTo(x.NgaySuDung?.ToString("yyyy-MM-dd")) >= 0).ToList();
            }

            if (!string.IsNullOrEmpty(bophan))
            {
                lst = lst.Where(x => x.BoPhan == bophan).ToList();
            }

            return DataSourceLoader.Load(lst.OrderByDescending(x => x.NgaySuDung), loadOptions);
        }

        public IActionResult ViepMap()
        {
            return View();
        }

        [HttpGet]
        public object GetTime(DataSourceLoadOptions loadOptions)
        {
            var times = _DangKyXeService.GetListTime();
            return DataSourceLoader.Load(times, loadOptions);
        }

        [HttpPost]
        public IActionResult ExportTaxiCost(string fromDate, string toDate)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files/sr");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"Taxi Draff Form" + $"_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/sr/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "Taxi Draff Form_Tmp.xlsx"));
            if (file.Exists)
            {
                file.Delete();
            }

            if (fileSrc.Exists)
            {
                fileSrc.CopyTo(file.FullName, true);
            }

            List<TaxiCostReportViewModel> Data = _DangKyXeService.TaxiCostReportData(fromDate, toDate);
            string cellFrom, cellTo;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                package.Workbook.CalcMode = ExcelCalcMode.Automatic;

                string date1 = DateTime.Parse(toDate).ToString("MM/yyyy");
                string date2 = DateTime.Parse(fromDate).ToString("dd/MM/yyyy");
                string date3 = DateTime.Parse(toDate).ToString("dd/MM/yyyy");

                worksheet.Cells["C5"].Value = "BẢNG KÊ PHÍ TAXI MAI LINH THÁNG " + date1 + " ( " + date2 + " - " + date3 + " )";

                worksheet.Cells["S2"].Value = "Đi làm\r\n출근";
                worksheet.Cells["S3"].Value = "Đi làm sớm\r\n일찍 출근";
                worksheet.Cells["S4"].Value = "Đi làm muộn\r\n지각";
                worksheet.Cells["S5"].Value = "Tan ca\r\n퇴근";
                worksheet.Cells["S6"].Value = "Tan ca sớm\r\n조퇴";
                worksheet.Cells["S7"].Value = "Tăng ca\r\n잔업";
                worksheet.Cells["S8"].Value = "công tác bên ngoài\r\n외근";
                worksheet.Cells["S9"].Value = "Liên hoan\r\n회식";
                worksheet.Cells["S10"].Value = "Trực\r\n당직";
                worksheet.Cells["S11"].Value = "Làm chủ nhật, ngày lễ\r\n일요일 특근";
                worksheet.Cells["S12"].Value = "Khác\r\n기타";

                int beginIndex = 8;
                for (int i = 0; i < Data.Count; i++)
                {
                    worksheet.Cells["C" + beginIndex].Value = i + 1;
                    worksheet.Cells["D" + beginIndex].Value = Data[i].CardNo;
                    worksheet.Cells["E" + beginIndex].Value = Data[i].CardName;
                    worksheet.Cells["F" + beginIndex].Value = Data[i].UserName;
                    worksheet.Cells["G" + beginIndex].Value = Data[i].Department;
                    worksheet.Cells["H" + beginIndex].Value = Data[i].Date;
                    worksheet.Cells["I" + beginIndex].Value = Data[i].DeparturePlace;
                    worksheet.Cells["J" + beginIndex].Value = Data[i].ArrivalPlace;
                    worksheet.Cells["K" + beginIndex].Value = Data[i].BillNo;
                    worksheet.Cells["L" + beginIndex].Value = Data[i].Amount1;
                    worksheet.Cells["M" + beginIndex].Value = Data[i].Amount2;
                    worksheet.Cells["N" + beginIndex].Value = Data[i].Note;

                    if (i > 284 && i < Data.Count - 1)
                    {
                        cellFrom = "C" + beginIndex + ":N" + beginIndex;
                        cellTo = "C" + (beginIndex + 1) + ":N" + (beginIndex + 1);
                        worksheet.Cells[cellFrom].Copy(worksheet.Cells[cellTo]);
                    }
                    beginIndex += 1;
                }

                package.Save(); //Save the workbook.
            }

            return new OkObjectResult(fileUrl);
        }
    }
}
