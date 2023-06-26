using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Sameple;
using System;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class LeadTimeSampleController : AdminBaseController
    {
        private IScheduleSampleService _scheduleSampleService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LeadTimeSampleController(IScheduleSampleService scheduleSampleService, IWebHostEnvironment hostingEnvironment)
        {
            _scheduleSampleService = scheduleSampleService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            LeadTimeSampleModel model = GetData(year, month, "0", "", "", "-9999");
            return View(model);
        }

        [HttpPost]
        public ActionResult GetWeekByMonth(string year, string month)
        {
            LeadTimeSampleModel models = new LeadTimeSampleModel(year);
            var weeks = models.GetWeeksByMonth(year, month);
            return new OkObjectResult(weeks);
        }

        [HttpPost]
        public ActionResult Search(string year, string month, string week, string code, string nguoiphutrach, string gap)
        {
            if (week == "0") week = "";
            LeadTimeSampleModel models = GetData(year, month, week, code, nguoiphutrach, gap);
            return View("Index", models);
        }

        private LeadTimeSampleModel GetData(string year, string month, string week, string code, string nguoiphutrach, string gap)
        {
            if (week == "0") week = "";

            LeadTimeSampleModel data = _scheduleSampleService.GetDataLeadTimeChart(year, month, week, code, nguoiphutrach, gap);
            return data;
        }
    }
}
