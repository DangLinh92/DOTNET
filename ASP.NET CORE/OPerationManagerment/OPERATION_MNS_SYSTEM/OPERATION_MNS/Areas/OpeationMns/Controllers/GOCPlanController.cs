using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Office.Utils;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
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
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
            ViewBag.ViewType = "";
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
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime(CommonConstants.CHIP, fromDate, toDate);
            return View(lst);
        }

        [HttpPost]
        public IActionResult DeleteGocPlan(int standarQtyId, string from, string to)
        {
            _GocPlanService.DeleteGocModel(standarQtyId, from, to, CommonConstants.WLP1);
            return new OkObjectResult(new { standarQtyId = standarQtyId });
        }


        [HttpPost]
        public IActionResult Search(string chipWafer, string fromTime, string toTime, string actualPlanGap, string wlp, string danhmuc, string inputWlp2)
        {
            danhmuc = danhmuc.NullString();

            if (wlp == CommonConstants.WLP1)
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
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        var lst1 = _GocPlanService.GetByTime(chipWafer, fromDate, toDate);
                        return PartialView("_GocPlanGridView", lst1);
                    }
                    else
                    {
                        ViewBag.daysSearch = new List<string>();
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        return PartialView("_GocPlanGridView", new List<GocPlanViewModelEx>());
                    }
                }

                List<string> lstDate = new List<string>();

                foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
                {
                    lstDate.Add(item.ToString("yyyy-MM-dd"));
                }

                ViewBag.daysSearch = lstDate;
                ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Parse(fromTime).ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
                ViewBag.ViewType = actualPlanGap.NullString();
                var lst = _GocPlanService.GetByTime(chipWafer, fromTime, toTime);
                return PartialView("_GocPlanGridView", lst);
            }
            else if (wlp == CommonConstants.WLP2)
            {
                string partialView = "_GocPlanGridViewWlp2";

                if (inputWlp2 == "InputWlp2")
                {
                    partialView = "_InputGocPlanGridViewWlp2";
                }

                if (string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
                {
                    if (string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
                    {
                        string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                        string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        List<string> lstDate1 = new List<string>();

                        foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
                        {
                            lstDate1.Add(item.ToString("yyyy-MM-dd"));
                        }

                        ViewBag.daysSearch = lstDate1;
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        var lst1 = _GocPlanService.GetByTime(chipWafer, fromDate, toDate, CommonConstants.WLP2, danhmuc);
                        return PartialView(partialView, lst1);
                    }
                    else
                    {
                        ViewBag.daysSearch = new List<string>();
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        return PartialView(partialView, new List<GocPlanViewModelEx>());
                    }
                }

                List<string> lstDate = new List<string>();

                foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
                {
                    lstDate.Add(item.ToString("yyyy-MM-dd"));
                }

                ViewBag.daysSearch = lstDate;
                ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Parse(fromTime).ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, danhmuc);
                ViewBag.ViewType = actualPlanGap.NullString();
                var lst = _GocPlanService.GetByTime(chipWafer, fromTime, toTime, CommonConstants.WLP2, danhmuc);
                return PartialView(partialView, lst);
            }
            else if (wlp == CommonConstants.SMT || danhmuc == CommonConstants.SMT)
            {
                string partialView = "_GocPlanGridViewSMT";

                if (inputWlp2 == "InputSMT")
                {
                    partialView = "_InputGocPlanGridViewSMT";
                }

                danhmuc = CommonConstants.KHSX;

                if (string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
                {
                    if (string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
                    {
                        string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                        string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        List<string> lstDate1 = new List<string>();

                        foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
                        {
                            lstDate1.Add(item.ToString("yyyy-MM-dd"));
                        }

                        ViewBag.daysSearch = lstDate1;
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.SMT, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        var lst1 = _GocPlanService.GetByTime("", fromDate, toDate, CommonConstants.SMT, danhmuc);
                        return PartialView(partialView, lst1);
                    }
                    else
                    {
                        ViewBag.daysSearch = new List<string>();
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.SMT, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        return PartialView(partialView, new List<GocPlanViewModelEx>());
                    }
                }

                List<string> lstDate = new List<string>();

                foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
                {
                    lstDate.Add(item.ToString("yyyy-MM-dd"));
                }

                ViewBag.daysSearch = lstDate;
                ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.SMT, danhmuc);
                ViewBag.ViewType = actualPlanGap.NullString();
                var lst = _GocPlanService.GetByTime("", fromTime, toTime, CommonConstants.SMT, danhmuc);
                return PartialView(partialView, lst);
            }
            else if (wlp == CommonConstants.LFEM)
            {
                string partialView = "_GocPlanGridViewLFEM";

                if (inputWlp2 == "InputLfem")
                {
                    partialView = "_InputGocPlanGridViewLFEM";
                }

                if (string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
                {
                    if (string.IsNullOrEmpty(fromTime) && string.IsNullOrEmpty(toTime))
                    {
                        string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                        string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        List<string> lstDate1 = new List<string>();

                        foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
                        {
                            lstDate1.Add(item.ToString("yyyy-MM-dd"));
                        }

                        ViewBag.daysSearch = lstDate1;
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        var lst1 = _GocPlanService.GetByTime("", fromDate, toDate, CommonConstants.LFEM, danhmuc);
                        return PartialView(partialView, lst1);
                    }
                    else
                    {
                        ViewBag.daysSearch = new List<string>();
                        ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, danhmuc);
                        ViewBag.ViewType = actualPlanGap.NullString();
                        return PartialView(partialView, new List<GocPlanViewModelEx>());
                    }
                }

                List<string> lstDate = new List<string>();

                foreach (var item in EachDay.EachDays(DateTime.Parse(fromTime), DateTime.Parse(toTime)))
                {
                    lstDate.Add(item.ToString("yyyy-MM-dd"));
                }

                ViewBag.daysSearch = lstDate;
                ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, danhmuc);
                ViewBag.ViewType = actualPlanGap.NullString();
                var lst = _GocPlanService.GetByTime("", fromTime, toTime, CommonConstants.LFEM, danhmuc);
                return PartialView(partialView, lst);
            }

            return PartialView("_GocPlanGridView", new List<GocPlanViewModelEx>());
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
            ViewBag.dayOffLine = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP1, CommonConstants.WLP1);
            var data = _GocPlanService.GetProcActualPlanModel(DateTime.Now.ToString("yyyy-MM"));
            return View(data);
        }

        [HttpPost]
        public IActionResult GetDataChart(string year, string month)
        {
            string m = year + "-" + month;
            ViewBag.dayOffLine = _GocPlanService.DateOffLine(year, CommonConstants.WLP1, CommonConstants.WLP1);
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
        public IActionResult GetDataControlChart(string fromDay, string toDay, string operation, string matertial)
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

        #region WLP2
        // vẽ chart Actual & Prod. Plan
        [HttpPost]
        public IActionResult GetDataActualPlanChartWlp2(string fromDay, string toDay, string danhmuc)
        {
            string fromDate = fromDay;
            string toDate = toDay;
            if (string.IsNullOrEmpty(fromDay))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            }

            if (string.IsNullOrEmpty(toDay))
            {
                toDate = DateTime.Parse(fromDate.Substring(0, 7) + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }

            var data = _GocPlanService.GetDataByDay(fromDate, toDate, danhmuc).OrderBy(x => x.Model).ThenBy(x => x.DatePlan).ToList();
            List<GocPlanViewModel> newData = new List<GocPlanViewModel>();
            foreach (var item in data)
            {
                if (!newData.Exists(x => x.Model == item.Model))
                {
                    newData.Add(item);
                }
                else
                {
                    GocPlanViewModel pl = newData.FirstOrDefault(x => x.Model == item.Model);
                    pl.QuantityPlan += item.QuantityPlan;
                    pl.QuantityActual += item.QuantityActual;
                    pl.QuantityGap = item.QuantityGap;
                }
            }
            return new OkObjectResult(newData.OrderBy(x => x.Model).ToList());
        }

        public IActionResult Wlp2()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, CommonConstants.NHAP_KHO);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime(CommonConstants.CHIP, fromDate, toDate, CommonConstants.WLP2, CommonConstants.NHAP_KHO);
            return View(lst);
        }

        [HttpPost]
        public IActionResult GetDataChartWlp2(string year, string month, string danhmuc)
        {
            string m = year + "-" + month;
            ViewBag.dayOffLine = _GocPlanService.DateOffLine(year, CommonConstants.WLP2, CommonConstants.WLP2, danhmuc);
            var data = _GocPlanService.GetProcActualPlanWlp2Model(m, danhmuc);
            return View("ProcActualPlanChartWlp2", data);
        }

        public IActionResult ProcActualPlanChartWlp2()
        {
            ViewBag.dayOffLine = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, CommonConstants.NHAP_KHO);
            var data = _GocPlanService.GetProcActualPlanWlp2Model(DateTime.Now.ToString("yyyy-MM"), CommonConstants.NHAP_KHO);
            return View(data);
        }

        public IActionResult Inputgocwlp2()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), CommonConstants.WLP2, CommonConstants.WLP2, CommonConstants.NHAP_KHO);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime(CommonConstants.CHIP, fromDate, toDate, CommonConstants.WLP2, CommonConstants.NHAP_KHO);
            return View(lst);
        }

        [HttpPost]
        public IActionResult DeleteGocPlanWlp2(string model, string from, string to, string danhmuc)
        {
            _GocPlanService.DeleteGocModelWlp2(model, from, to, danhmuc);
            return new OkObjectResult(new { model = model });
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel_Wlp2(IList<IFormFile> files, [FromQuery] string param)
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
                ResultDB rs = _GocPlanService.ImportExcel_Wlp2(filePath, param);

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

        // View control chart wlp2
        public IActionResult ViewControlChartWlp2()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string toDate = date;
            ViewControlChartDataModel model = _GocPlanService.GetDataControlChartWLP2(date, toDate, "OP77000", "");

            model.FromDay = DateTime.Now.ToString("yyyy-MM-dd");
            model.ToDay = DateTime.Now.ToString("yyyy-MM-dd");

            model.Operation = "OP77000";
            model.MatertialID = "";

            return View(model);
        }

        [HttpPost]
        public IActionResult GetDataControlChartWlp2(string fromDay, string toDay, string operation, string matertial)
        {
            ViewControlChartDataModel model = _GocPlanService.GetDataControlChartWLP2(fromDay, toDay, operation.NullString(), matertial.NullString());

            model.FromDay = fromDay;
            model.ToDay = toDay;

            model.Operation = operation;
            model.MatertialID = matertial;

            return View("ViewControlChartWlp2", model);
        }

        // CTQ WLP2
        public IActionResult CTQSettingWlp2()
        {
            return View();
        }

        [HttpGet]
        public object GetCTQ_WLP2(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_GocPlanService.GetCTQ_Wlp2(), loadOptions);
        }

        [HttpPost]
        public IActionResult PostCTQ_WLP2(string values)
        {
            var ctq = new CTQSettingWLP2ViewModel();
            JsonConvert.PopulateObject(values, ctq);

            _GocPlanService.PostCTQ_Wlp2(ctq);
            _GocPlanService.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult PutCTQ_WLP2(int key, string values)
        {
            var ctq = _GocPlanService.GetCTQ_Wlp2_Id(key);
            JsonConvert.PopulateObject(values, ctq);
            _GocPlanService.PutCTQ_Wlp2(ctq);
            _GocPlanService.Save();

            return Ok();
        }

        [HttpDelete]
        public void DeleteCTQ_WLP2(int key)
        {
            var ctq = _GocPlanService.GetCTQ_Wlp2_Id(key);
            _GocPlanService.DeleteCTQ_Wlp2(ctq);
            _GocPlanService.Save();
        }
        #endregion

        #region LFEM

        public IActionResult ProcActualPlanChartLfem()
        {
            ViewBag.dayOffLine = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, CommonConstants.KHSX);
            var data = _GocPlanService.GetProcActualPlanLfemModel(DateTime.Now.ToString("yyyy-MM"), "", CommonConstants.KHSX, "Month");
            return View(data);
        }

        [HttpPost]
        public IActionResult GetDataChartLfem(string display, string year, string danhmuc, string dateFrom, string dateTo)
        {
            if(danhmuc == CommonConstants.SMT)
            {
                ViewBag.dayOffLine = _GocPlanService.DateOffLine(year, "", CommonConstants.SMT, CommonConstants.KHSX);
            }
            else
            {
                ViewBag.dayOffLine = _GocPlanService.DateOffLine(year, "", CommonConstants.LFEM, danhmuc);
            }

            if (danhmuc == CommonConstants.SMT)
            {
                danhmuc = CommonConstants.KHSX;
                List<ProcActualPlanModel> data = new List<ProcActualPlanModel>();
                if (display == "Month")
                {
                    string m = dateFrom.Substring(0, 7);
                     data = _GocPlanService.GetProcActualPlanSMTModel(m, "", danhmuc, display);
                  
                }
                else if (display == "Week")
                {
                     data = _GocPlanService.GetProcActualPlanSMTModel(dateFrom, dateTo, danhmuc, display);
                }
                else
                {
                     data = _GocPlanService.GetProcActualPlanSMTModel(dateFrom, dateTo, danhmuc, display);
                }

                foreach (var item in data)
                {
                    item.DanhMuc = CommonConstants.SMT;
                }

                return View("ProcActualPlanChartLfem", data);
            }
            else // LFEM
            {
                if (display == "Month")
                {
                    string m = dateFrom.Substring(0, 7);
                    var data = _GocPlanService.GetProcActualPlanLfemModel(m, "", danhmuc, display);
                    return View("ProcActualPlanChartLfem", data);
                }
                else if (display == "Week")
                {
                    var data = _GocPlanService.GetProcActualPlanLfemModel(dateFrom, dateTo, danhmuc, display);
                    return View("ProcActualPlanChartLfem", data);
                }
                else
                {
                    var data = _GocPlanService.GetProcActualPlanLfemModel(dateFrom, dateTo, danhmuc, display);
                    return View("ProcActualPlanChartLfem", data);
                }
            }
            
        }

        public IActionResult Lfem()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, CommonConstants.KHSX);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime("", fromDate, toDate, CommonConstants.LFEM, CommonConstants.KHSX);
            return View(lst);
        }

        public IActionResult InputGocLfem()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.LFEM, CommonConstants.KHSX);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime("", fromDate, toDate, CommonConstants.LFEM, CommonConstants.KHSX);
            return View(lst);
        }

        [HttpPost]
        public IActionResult GetDataActualPlanChartLfem(string dateFrom, string danhmuc)
        {
            if (string.IsNullOrEmpty(dateFrom))
            {
                dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            }

            var data = _GocPlanService.GetDataByDayLfem(dateFrom, danhmuc).OrderBy(x => x.MesItemId).ToList();

            List<GOC_PLAN_LFEM> newData = new List<GOC_PLAN_LFEM>();
            foreach (var item in data)
            {
                if (!newData.Exists(x => x.MesItemId == item.MesItemId))
                {
                    newData.Add(item);
                }
                else
                {
                    GOC_PLAN_LFEM pl = newData.FirstOrDefault(x => x.MesItemId == item.MesItemId);
                    pl.QuantityPlan += item.QuantityPlan;
                    pl.QuantityActual += item.QuantityActual;
                    pl.QuantityGap = item.QuantityGap;
                }
            }
            return new OkObjectResult(newData.OrderBy(x => x.MesItemId).ToList());
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel_LFEM(IList<IFormFile> files, [FromQuery] string param)
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
                ResultDB rs = _GocPlanService.ImportExcel_Lfem(filePath, param);

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

        [HttpPost]
        public IActionResult DeleteGocPlanLFEM(string model, string from, string to, string danhmuc)
        {
            _GocPlanService.DeleteGocModelLfem(model, from, to, danhmuc);
            return new OkObjectResult(new { model = model });
        }

        public IActionResult ShortageShipping()
        {
            string month = DateTime.Now.ToString("yyyy-MM") + "-01";
            ViewBag.MonthViewSHG = month;
            var data = _GocPlanService.GetSanXuatXuatHang(month);
            return View(data);
        }

        [HttpPost]
        public IActionResult SearchShortageShipping(string month)
        {
            month = month + "-01";
            ViewBag.MonthViewSHG = month;
            var data = _GocPlanService.GetSanXuatXuatHang(month);
            return View("ShortageShipping", data);
        }
        #endregion

        #region SMT

        public IActionResult InputGocSMT()
        {
            string fromDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            string toDate = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            List<string> lstDate = new List<string>();

            foreach (var item in EachDay.EachDays(DateTime.Parse(fromDate), DateTime.Parse(toDate)))
            {
                lstDate.Add(item.ToString("yyyy-MM-dd"));
            }

            ViewBag.daysSearch = lstDate;
            ViewBag.dayOff = _GocPlanService.DateOffLine(DateTime.Now.ToString("yyyy"), "", CommonConstants.SMT, CommonConstants.KHSX);
            ViewBag.ViewType = "";
            var lst = _GocPlanService.GetByTime("", fromDate, toDate, CommonConstants.SMT, CommonConstants.KHSX);
            return View(lst);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel_SMT(IList<IFormFile> files, [FromQuery] string param)
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
                ResultDB rs = _GocPlanService.ImportExcel_SMT(filePath, param);

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

        [HttpPost]
        public IActionResult DeleteGocPlanSMT(string model, string from, string to, string danhmuc)
        {
            _GocPlanService.DeleteGocModelSMT(model, from, to, danhmuc);
            return new OkObjectResult(new { model = model });
        }
        #endregion
    }
}
