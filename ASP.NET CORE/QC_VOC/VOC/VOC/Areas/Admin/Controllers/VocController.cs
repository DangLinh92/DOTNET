using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Constants;

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
                model.totalVOCSitesView = _vocMstService.ReportByYear(DateTime.Now.Year.ToString(),customer);

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
                model.totalVOCSitesView = _vocMstService.ReportByYear(year.ToString(),customer);

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
    }
}
