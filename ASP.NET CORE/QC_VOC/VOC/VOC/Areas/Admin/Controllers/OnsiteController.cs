using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Constants;

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
            var lst = GetData(DateTime.Now.Year, CommonConstants.SEV, CommonConstants.ALL);
            return View(lst);
        }

        [HttpPost]
        public IActionResult Search(int year, string customer, string part)
        {
            var lst = GetData(year, customer, part);
            return View("Index", lst);
        }

        private VocOnsiteList GetData(int year, string customer, string part)
        {
            VocOnsiteList vocOnsiteList = new VocOnsiteList()
            {
                Customer = customer,
                Year = year,
                Part = part
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
    }
}
