using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Data.EF.Extensions;
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
            InventoryTicker.SetStatus(false, false, true, false);
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
            List<DateOffLineViewModel> DateOffLineViewModels = _dateOffLineService.GetDateOffLine(CommonConstants.WLP1, "");
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
        public IActionResult GetstayLotInfo(string model, string operation)
        {
            var data = _stayLotListService.GetStayLotListByModel(model, operation);
            return new OkObjectResult(data);
        }


        #region WLP2 Daily plan

        [HttpPost]
        public object UpdatePrioryWlp2([FromBody] List<DataChange> changes)
        {
            string sapcode = "";
            string operation = "";

            int i = 0;
            foreach (var change in changes)
            {
                 sapcode = change.Key.NullString().Split("^")[0];
                 operation = change.Key.NullString().Split("^")[1];

                var data = _stayLotListService
                    .GetStayLotListByModelWlp2(sapcode, operation)
                    .FirstOrDefault(x => x.SapCode + "^" + x.OperationId + "^" + x.Material + "^" + x.CassetteID + "^" + x.LotID == change.Key.NullString());

                if (data != null)
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), data);
                }

                _stayLotListService.UpdatePrioryLotIdWlp2(data, i++);
            }
            _stayLotListService.Save();
            return Ok(changes);
        }

        [HttpPost]
        public IActionResult GetstayLotInfoWlp2(string model, string operation)
        {
            var data = _stayLotListService.GetStayLotListByModelWlp2(model, operation);
            return new OkObjectResult(data);
        }

        public IActionResult DailyPlanWlp2()
        {
            ViewBag.DayPlanWLP2 = InventoryTicker.GetBeginDateOfWlp2().ToString("yyyy-MM-dd");
            InventoryTicker.SetStatus(false, false, false, true);
            return View();
        }

        [HttpPost]
        public IActionResult SetBeginDateWLP2(string beginDate)
        {
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            InventoryTicker.BeginDateOfWLP2 = beginDate;
            return new OkObjectResult(null);
        }
        #endregion
    }
}
