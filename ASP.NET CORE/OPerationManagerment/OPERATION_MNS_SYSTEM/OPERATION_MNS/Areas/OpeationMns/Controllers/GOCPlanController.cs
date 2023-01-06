using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// https://ddonline.webhook.office.com/webhookb2/9a0a3222-d752-47b6-b56a-d62400e9540d@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/987d88cc310f47749af818ceb3c59b42/a41df1e6-6192-480f-94cd-f1021a999840
namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class GOCPlanController : AdminBaseController
    {
        IGocPlanService _GocPlanService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public GOCPlanController(IGocPlanService GocPlanService, IWebHostEnvironment hostingEnvironment, ILogger<GOCPlanController> logger)
        {
            _GocPlanService = GocPlanService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            var lst = _GocPlanService.GetByTime(CommonConstants.CHIP, fromDate, toDate);
            return View(lst);
        }

        public IActionResult Inputgoc()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            var lst = _GocPlanService.GetByTime(CommonConstants.CHIP, fromDate, toDate);
            return View(lst);
        }

        [HttpPost]
        public IActionResult DeleteGocPlan(int standarQtyId, string from, string to)
        {
            _GocPlanService.DeleteGocModel(standarQtyId, from, to);
            return new OkObjectResult(standarQtyId);
        }

        [HttpPost]
        public IActionResult Search(string chipWafer, string fromTime, string toTime)
        {
            if (string.IsNullOrEmpty(chipWafer) || string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
            {
                if (!string.IsNullOrEmpty(chipWafer) && string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
                {
                    string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                    string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                    List<string> lstDate1 = new List<string>();

                    foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
                    {
                        lstDate1.Add(item.ToString("yyyy-MM-dd"));
                    }

                    ViewBag.daysSearch = lstDate1;
                    var lst1 = _GocPlanService.GetByTime(chipWafer, fromDate, toDate);
                    return PartialView("_GocPlanGridView", lst1);
                }
                else
                {
                    ViewBag.daysSearch = new List<string>();
                    return PartialView("_GocPlanGridView", new List<GocPlanViewModelEx>());
                }
            }

            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            var lst = _GocPlanService.GetByTime(chipWafer, fromTime, toTime);
            return PartialView("_GocPlanGridView", lst);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, [FromQuery] string param)
        {
            _logger.LogInformation("Begin import exce");
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
                string fName = CorrelationIdGenerator.GetNextId() + filename;
                string filePath = Path.Combine(folder, fName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                ResultDB rs = _GocPlanService.ImportExcel(filePath, param);

                if (rs.ReturnInt == 0)
                {
                    _GocPlanService.Save();
                }
                else
                {
                    _logger.LogInformation("Import error:" + rs.ReturnString);
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                _logger.LogInformation("Import success");
                return new OkObjectResult(filePath);
            }

            _logger.LogInformation("File null");
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        public IActionResult ProcActualPlanChart()
        {
            var data = _GocPlanService.GetProcActualPlanModel(DateTime.Now.ToString("yyyy-MM"));
            return View(data);
        }

        [HttpPost]
        public IActionResult GetDataChart(string year, string month)
        {
            string m = year + "-" + month;
            var data = _GocPlanService.GetProcActualPlanModel(m);
            return View("ProcActualPlanChart", data);
        }

        // FAB
        public IActionResult WLP1_InputFAB()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearchFAB = lstDate;
            var lst = _GocPlanService.GetByTime_fab(CommonConstants.CHIP, fromDate, toDate);
            return View(lst);
        }

        [HttpPost]
        public IActionResult SearchWLP_FAB(string chipWafer, string fromTime, string toTime)
        {
            if (string.IsNullOrEmpty(chipWafer) || string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
            {
                if (!string.IsNullOrEmpty(chipWafer) && string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
                {
                    string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                    string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                    List<string> lstDate1 = new List<string>();

                    foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
                    {
                        lstDate1.Add(item.ToString("yyyy-MM-dd"));
                    }

                    ViewBag.daysSearchFAB = lstDate1;
                    var lst1 = _GocPlanService.GetByTime_fab(chipWafer, fromDate, toDate);
                    return PartialView("_InputfabPlanGridView", lst1);
                }
                else
                {
                    ViewBag.daysSearchFAB = new List<string>();
                    return PartialView("_InputfabPlanGridView", new List<GocPlanViewModelEx>());
                }
            }

            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearchFAB = lstDate;
            var lst = _GocPlanService.GetByTime_fab(chipWafer, fromTime, toTime);
            return PartialView("_InputfabPlanGridView", lst);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult Import_WLP1_InputFAB(IList<IFormFile> files, [FromQuery] string param)
        {
            _logger.LogInformation("Begin import exce");
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

                ResultDB rs = _GocPlanService.ImportExcel(filePath, "FAB_WLP1");

                if (rs.ReturnInt == 0)
                {
                    _GocPlanService.Save();
                }
                else
                {
                    _logger.LogInformation("Import error:" + rs.ReturnString);
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                _logger.LogInformation("Import success");
                return new OkObjectResult(filePath);
            }

            _logger.LogInformation("File null");
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        // view control chart
        public IActionResult ViewControlChart()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string toDate = date;
            ViewControlChartDataModel model = _GocPlanService.GetDataControlChart(date, toDate, "OP64000", "");

            model.FromDay = DateTime.Now.ToString("yyyy-MM-dd");
            model.ToDay = DateTime.Now.ToString("yyyy-MM-dd");

            model.Operation = "OP64000";
            model.MatertialID = "";

            return View(model);
        }

        [HttpPost]
        public IActionResult GetDataControlChart(string fromDay, string toDay,string operation, string matertial)
        {
            ViewControlChartDataModel model = _GocPlanService.GetDataControlChart(fromDay, toDay, operation.NullString(), matertial.NullString());

            model.FromDay = fromDay;
            model.ToDay = toDay;

            model.Operation = operation;
            model.MatertialID = matertial;
            
            return View("ViewControlChart", model);
        }

        // CTQ Setting
        public IActionResult CTQSetting()
        {
            return View();
        }

        [HttpGet]
        public object GetCTQ(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_GocPlanService.GetCTQ(), loadOptions);
        }

        [HttpPost]
        public IActionResult PostCTQ(string values)
        {
            var ctq = new CTQSettingViewModel();
            JsonConvert.PopulateObject(values, ctq);

            _GocPlanService.PostCTQ(ctq);
            _GocPlanService.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult PutCTQ(int key, string values)
        {
            var ctq = _GocPlanService.GetCTQ_Id(key);
            JsonConvert.PopulateObject(values, ctq);
            _GocPlanService.PutCTQ(ctq);
            _GocPlanService.Save();

            return Ok();
        }

        [HttpDelete]
        public void DeleteCTQ(int key)
        {
            var ctq = _GocPlanService.GetCTQ_Id(key);
            _GocPlanService.DeleteCTQ(ctq);
            _GocPlanService.Save();
        }
    }
}
