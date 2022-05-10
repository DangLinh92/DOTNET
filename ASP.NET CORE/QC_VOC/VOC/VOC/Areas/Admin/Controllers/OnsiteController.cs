﻿using System;
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
    public class OnsiteController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocOnsiteSevice _vocOnsiteService;

        public OnsiteController(IVocOnsiteSevice vocOnsiteService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocOnsiteService = vocOnsiteService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lst = GetData(DateTime.Now.Year, CommonConstants.SEV_ALL, "", CommonConstants.ALL, "");
            return View(lst);
        }

        [HttpPost]
        public IActionResult Search(int year, string customer_sev, string customer_sevt, string part, string wisolModel)
        {
            var lst = GetData(year, customer_sev, customer_sevt, part, wisolModel);
            return View("Index", lst);
        }

        private VocOnsiteList GetData(int year, string customer_sev, string customer_sevt, string part, string wisolModel)
        {
            string customer = "";
            if (!string.IsNullOrEmpty(customer_sev))
            {
                customer = customer_sev;
            }

            if (!string.IsNullOrEmpty(customer_sevt))
            {
                customer = customer_sevt;
            }

            VocOnsiteList vocOnsiteList = new VocOnsiteList()
            {
                Customer = customer,
                Year = year,
                Part = part,
                WisolModel = wisolModel.NullString()
            };

            vocOnsiteList.vocOnsiteModels = _vocOnsiteService.SumDataOnsite(year, customer, part);

            var groups = vocOnsiteList.vocOnsiteModels.GroupBy(x => (x.Week, x.Part)).Select(gr => (gr, Total: gr.Sum(x => x.Qty)));
            VocOnsiteSumWeek sumWeek;
            foreach (var group in groups)
            {
                sumWeek = new VocOnsiteSumWeek()
                {
                    Time = group.gr.Key.Week,
                    QTY = group.Total,
                    Part = group.gr.Key.Part,
                    OK = 0,
                    NG = 0
                };

                foreach (var item in group.gr)
                {
                    sumWeek.OK += item.OK;
                    sumWeek.NG += item.NG;
                    sumWeek.Month = item.Month;
                }

                vocOnsiteList.vocOnsiteSumWeeks.Add(sumWeek);
            }
            vocOnsiteList.vocOnsiteSumWeeks = vocOnsiteList.vocOnsiteSumWeeks.OrderBy(x => x.Time).ToList();

            ViewBag.UpdateDayOnsite = GetUpdateDay();

            return vocOnsiteList;
        }

        public IActionResult UploadList()
        {
            string startTime = DateTime.Now.Year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            var lst = _vocOnsiteService.GetAllOnsiteByTime(startTime, endTime);
            return View(lst);
        }

        [HttpPost]
        public IActionResult SearchUpload(string fromTime, string toTime)
        {
            if (string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
            {
                string startTime = DateTime.Now.Year + "-01-01";
                string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                var lst = _vocOnsiteService.GetAllOnsiteByTime(startTime, endTime);
                ViewBag.SearchTime = fromTime + "^" + toTime;
                return View("UploadList", lst);
            }
            else
            {
                var lst = _vocOnsiteService.GetAllOnsiteByTime(fromTime, toTime);
                ViewBag.SearchTime = fromTime + "^" + toTime;
                return View("UploadList", lst);
            }
        }

        [HttpPost]
        public IActionResult SaveData(VocOnsiteViewModel model, [FromQuery] string action)
        {
            if (action == "Add")
            {
                _vocOnsiteService.AddVocOnsite(model);
            }
            else
            {
                var entity = _vocOnsiteService.FindById(model.Id);

                entity.CopyPropertiesFrom(model, new List<string>()
                {
                    "Id,","DateCreated","DateModified","UserCreated","UserModified"
                });

                _vocOnsiteService.UpdateVocOnsite(entity);
            }
            _vocOnsiteService.Save();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetOnsiteById(int id)
        {
            var item = _vocOnsiteService.FindById(id);
            return new OkObjectResult(item);
        }

        [HttpPost]
        public IActionResult DeleteOnsite(int id)
        {
            _vocOnsiteService.DeleteVocOnsite(id);
            _vocOnsiteService.Save();
            return new OkObjectResult(id);
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
            string sFileName = $"onsite_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            var lst = _vocOnsiteService.GetAllOnsiteByTime(startTime, endTime);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Onsite Master LIST");
                worksheet.Cells["A1"].LoadFromCollection(lst, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
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
                ResultDB result = _vocOnsiteService.ImportExcel(filePath, param);

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

        [HttpGet]
        public IActionResult GetModel()
        {
            var lst = _vocOnsiteService.GetModel();
            return new OkObjectResult(lst);
        }

        [HttpPost]
        public IActionResult UpdateDay(string date)
        {
            string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\updateDay";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, "updateLastDayOnsite.txt");
            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                file.Delete();
            }

            using (StreamWriter sw = file.CreateText())
            {
                sw.WriteLine(date);
            }

            return new OkObjectResult(date);
        }

        public string GetUpdateDay()
        {
            string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\updateDay";
            string filePath = Path.Combine(folder, "updateLastDayOnsite.txt");
            FileInfo file = new FileInfo(filePath);

            string date = "";
            if (file.Exists)
            {
                using (StreamReader sr = file.OpenText())
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        date += s;
                    }
                }
            }

            return date.Trim();
        }
    }
}
