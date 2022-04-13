using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Common;
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Areas.Admin.Controllers
{
    public class VocController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;

        public VocController(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index(int year)
        {
            VocInfomationsModel model = GetData(year, "", CommonConstants.WHC);
            return View(model.vOC_CHART);
        }

        [HttpGet]
        public IActionResult Search(int year, string customer, string side)
        {
            VocInfomationsModel model = GetData(year, customer.NullString(), side.NullString(), true);
            return new OkObjectResult(model.vOC_CHART);
        }

        private VocInfomationsModel GetData(int year, string customer, string side, bool isSearch = false)
        {
            VocInfomationsModel model = new VocInfomationsModel();

            if (!isSearch && (year <= 0 || year == DateTime.Now.Year))
            {
                string startTime = DateTime.Now.Year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));

                // REPORT BY MONTH
                model.vOCSiteModelByTimeLsts.AddRange(_vocMstService.ReportInit());

                // VE BIEU DO TOTAL THEO NAM
                model.totalVOCSitesView = _vocMstService.ReportByYear(DateTime.Now.Year.ToString(), customer);

                // VE BIEU DO PARETO CHO DEFECT NAME
                model.paretoDataDefectName.AddRange(_vocMstService.ReportDefectByYear(DateTime.Now.Year.ToString(), "SAW", customer, side));
                model.paretoDataDefectName.AddRange(_vocMstService.ReportDefectByYear(DateTime.Now.Year.ToString(), "Module", customer, side));

                model.vOC_CHART.totalVOCSitesView = model.totalVOCSitesView;
                model.vOC_CHART.vOCSiteModelByTimeLsts = model.vOCSiteModelByTimeLsts;
                model.vOC_CHART.paretoDataDefectName = model.paretoDataDefectName;

                model.vOC_CHART.vocProgessInfo = _vocMstService.GetProgressInfo(DateTime.Now.Year, customer, side);
                model.vOC_CHART.Year = DateTime.Now.Year;
                model.vOC_CHART.Customer = "";
                model.vOC_CHART.Side = CommonConstants.WHC;
            }
            else
            {
                string startTime = year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));

                // REPORT BY MONTH
                model.vOCSiteModelByTimeLsts.AddRange(_vocMstService.ReportByMonth(year.ToString(), customer, side));

                // VE BIEU DO TOTAL THEO NAM
                model.totalVOCSitesView = _vocMstService.ReportByYear(year.ToString(), customer);

                // VE BIEU DO PARETO CHO DEFECT NAME
                model.paretoDataDefectName.AddRange(_vocMstService.ReportDefectByYear(year.ToString(), "SAW", customer, side));
                model.paretoDataDefectName.AddRange(_vocMstService.ReportDefectByYear(year.ToString(), "Module", customer, side));

                model.vOC_CHART.totalVOCSitesView = model.totalVOCSitesView;
                model.vOC_CHART.vOCSiteModelByTimeLsts = model.vOCSiteModelByTimeLsts;
                model.vOC_CHART.paretoDataDefectName = model.paretoDataDefectName;

                model.vOC_CHART.vocProgessInfo = _vocMstService.GetProgressInfo(year, customer, side);
                model.vOC_CHART.Year = year;
                model.vOC_CHART.Customer = customer;
                model.vOC_CHART.Side = side;
            }

            return model;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var lst = _vocMstService.GetCustomer();
            return new OkObjectResult(lst);
        }

        [HttpGet]
        public IActionResult UploadList(int year)
        {
            VocInfomationsModel model = new VocInfomationsModel();

            if (year <= 0 || year == DateTime.Now.Year)
            {
                string startTime = DateTime.Now.Year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));
            }
            else
            {
                string startTime = year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetDefectType()
        {
            var lstType = _vocMstService.GetDefectType();
            return new OkObjectResult(lstType);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var obj = _vocMstService.GetById(id);
            return new OkObjectResult(obj);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _vocMstService.Delete(Id);
            _vocMstService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult SaveVoc(VOC_MSTViewModel model, [FromQuery] int Id, [FromQuery] string action)
        {
            if (action == "Add")
            {
                if (DateTime.TryParse(model.SPLReceivedDate, out _))
                {
                    model.SPLReceivedDateWeek = "W" + (DateTime.Parse(model.SPLReceivedDate).GetWeekOfYear() - 1).NullString();

                    if (DateTime.TryParse(model.VOCFinishingDate, out _))
                    {
                        model.VOC_TAT = DateTime.Parse(model.VOCFinishingDate).Subtract(DateTime.Parse(model.SPLReceivedDate)).Days.NullString();
                    }
                }
                else if (DateTime.TryParse(model.ReceivedDate, out _))
                {
                    model.SPLReceivedDateWeek = "W" + (DateTime.Parse(model.ReceivedDate).GetWeekOfYear() - 1).NullString();
                }

                _vocMstService.Add(model);
            }
            else
            {
                var item = _vocMstService.GetById(Id);
                item.CopyPropertiesFrom(model, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified", "VOCFinishingWeek", "SPLReceivedDateMonth" });

                if (DateTime.TryParse(item.SPLReceivedDate, out _))
                {
                    item.SPLReceivedDateWeek = "W" + (DateTime.Parse(item.SPLReceivedDate).GetWeekOfYear() - 1).NullString();

                    if (DateTime.TryParse(item.VOCFinishingDate, out _))
                    {
                        item.VOC_TAT = DateTime.Parse(item.VOCFinishingDate).Subtract(DateTime.Parse(item.SPLReceivedDate)).Days.NullString();
                    }
                }
                else if (DateTime.TryParse(item.ReceivedDate, out _))
                {
                    item.SPLReceivedDateWeek = "W" + (DateTime.Parse(item.ReceivedDate).GetWeekOfYear() - 1).NullString();
                }

                _vocMstService.Update(item);
            }

            _vocMstService.Save();
            return new OkObjectResult(model);
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
                ResultDB result = _vocMstService.ImportExcel(filePath, param);

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

            _logger.LogError("Upload file: " + CommonConstants.NotFoundObjectResult_Msg);
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpPost]
        public IActionResult ExportExcel(string year)
        {
            if (!int.TryParse(year, out _))
            {
                return new BadRequestObjectResult("Year :" + year + " invalid!");
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"voc_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            var vocs = _vocMstService.SearchByTime(startTime, endTime);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                worksheet.Cells["A1"].LoadFromCollection(vocs, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult UpLoadExcel(IList<IFormFile> files, [FromQuery] string vocId)
        {
            try
            {
                if (files != null && files.Count > 0)
                {
                    var file = files[0];
                    var filename = ContentDispositionHeaderValue
                                       .Parse(file.ContentDisposition)
                                       .FileName
                                       .Trim('"');

                    string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\voc_issue";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string newName = vocId + filename;
                    string filePath = Path.Combine(folder, newName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    string url = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        VOC_MSTViewModel voc = _vocMstService.GetById(int.Parse(vocId));

                        url = $"{Request.Scheme}://{Request.Host}/uploaded/voc_issue/{newName}";

                        voc.LinkReport = url;
                        _vocMstService.Update(voc);
                        _vocMstService.Save();
                    }

                    return new OkObjectResult(url);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            _logger.LogError("UpLoadExcel: " + CommonConstants.NotFoundObjectResult_Msg);
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
