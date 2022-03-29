using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Common;
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;


        public HomeController(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index(int year)
        {
            VocInfomationsModel model = new VocInfomationsModel();
            if (year <= 0 || year == DateTime.Now.Year)
            {
                string startTime = DateTime.Now.Year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));

                model.vOCSiteModelByTimeLsts.AddRange(_vocMstService.ReportInit());
                model.totalVOCSitesView = _vocMstService.ReportByYear(DateTime.Now.Year.ToString());
            }
            else
            {
                string startTime = year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));

                int endWeek = DateTime.Parse(endTime).GetWeekOfYear() - 1;

                model.vOCSiteModelByTimeLsts.AddRange(_vocMstService.ReportByWeek(endWeek - 4, endWeek, year.ToString()));
                model.totalVOCSitesView = _vocMstService.ReportByYear(year.ToString());
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

                _vocMstService.Add(model);
            }
            else
            {
                var item = _vocMstService.GetById(Id);
                item.CopyPropertiesFrom(model, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified" });

                if (DateTime.TryParse(item.SPLReceivedDate, out _))
                {
                    item.SPLReceivedDateWeek = "W" + (DateTime.Parse(item.SPLReceivedDate).GetWeekOfYear() - 1).NullString();

                    if (DateTime.TryParse(item.VOCFinishingDate, out _))
                    {
                        item.VOC_TAT = DateTime.Parse(item.VOCFinishingDate).Subtract(DateTime.Parse(item.SPLReceivedDate)).Days.NullString();
                    }
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
            if (!int.TryParse(year,out _))
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
    }
}
