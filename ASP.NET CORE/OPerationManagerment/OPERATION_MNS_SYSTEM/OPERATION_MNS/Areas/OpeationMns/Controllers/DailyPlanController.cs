using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class DailyPlanController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDateOffLineService _dateOffLineService;
        private readonly IStayLotListService _stayLotListService;
        private IHttpContextAccessor _httpContextAccessor;
        public DailyPlanController(IWebHostEnvironment hostingEnvironment, IDateOffLineService dateOffLineService, IStayLotListService stayLotListService, IHttpContextAccessor httpContextAccessor)
        {
            _stayLotListService = stayLotListService;
            _hostingEnvironment = hostingEnvironment;
            _dateOffLineService = dateOffLineService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.DayPlan = GetDaysPlan();
            ViewBag.ViewOption = string.IsNullOrEmpty(InventoryTicker.ViewOption) ? CommonConstants.WAFER : InventoryTicker.ViewOption;
            return View();
        }

        [HttpPost]
        public IActionResult SetBeginDate(string beginDate)
        {
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            InventoryTicker.BeginDate = beginDate;
            return new OkObjectResult(GetDaysPlan());
        }

        [HttpPost]
        public IActionResult SetViewOption(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                option = CommonConstants.WAFER;
            }

            InventoryTicker.ViewOption = option;
            return new OkObjectResult(option);
        }

        private List<string> GetDaysPlan()
        {
            List<DateOffLineViewModel> DateOffLineViewModels = _dateOffLineService.GetDateOffLine();
            List<string> Days = new List<string>();
            int leadTime = _dateOffLineService.GetLeadTime();
            int index = 1;
            while (Days.ToList().Count < leadTime - 1)
            {
                if (!DateOffLineViewModels.Any(x => x.ItemValue == InventoryTicker.GetBeginDate().AddDays(index).ToString("yyyy-MM-dd")))
                {
                    Days.Add(InventoryTicker.GetBeginDate().AddDays(index).ToString("yyyy-MM-dd"));
                }
                index += 1;
            }

            Days.Sort();
            Days.Insert(0, InventoryTicker.GetBeginDate().ToString("yyyy-MM-dd"));
            return Days;
        }

        [HttpPost]
        public IActionResult GetstayLotInfo(string model,string operation)
        {
            var data = _stayLotListService.GetStayLotListByModel(model, operation);
            return new OkObjectResult(data);
        }
    }
}
