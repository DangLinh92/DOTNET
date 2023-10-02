using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
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
        private readonly ILotTestHistoryLFemService _lotTestHistoryLFemService;
        private readonly ISMTDailyPlanService _SMTDailyPlanService;
        private IHttpContextAccessor _httpContextAccessor;
        public DailyPlanController(ISMTDailyPlanService SMTDailyPlanService, ILotTestHistoryLFemService lotTestHistoryLFemService, IWebHostEnvironment hostingEnvironment, IDateOffLineService dateOffLineService, IStayLotListService stayLotListService, IHttpContextAccessor httpContextAccessor)
        {
            _stayLotListService = stayLotListService;
            _hostingEnvironment = hostingEnvironment;
            _dateOffLineService = dateOffLineService;
            _httpContextAccessor = httpContextAccessor;
            _lotTestHistoryLFemService = lotTestHistoryLFemService;
            _SMTDailyPlanService = SMTDailyPlanService;
        }

        public IActionResult Index()
        {
            ViewBag.DayPlan = GetDaysPlan();
            ViewBag.ViewOption = string.IsNullOrEmpty(InventoryTicker.ViewOption) ? CommonConstants.WAFER : InventoryTicker.ViewOption;
            InventoryTicker.SetStatus(false, true);
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
        public object UpdateDailyPlanWlp2([FromBody] List<DataChange> changes)
        {
            string sapcode = "";
            foreach (var change in changes)
            {
                sapcode = change.Key.NullString();
                string datePlan = WLP2Ticker.GetBeginDateOfWlp2().ToString("yyyy-MM-dd");

                DailyPlanWlp2ViewModel model = new DailyPlanWlp2ViewModel();
                JsonConvert.PopulateObject(change.Data.ToString(), model);

                var data = _stayLotListService.GetDailyPlanWlp2Update(sapcode, datePlan);

                if (data != null)
                {
                    data.BackGrinding = model.BackGrinding;
                    _stayLotListService.UpdateDailyPlanWlp2Update(data, true);
                }
                else
                {
                    data = new DAILY_PLAN_WLP2();
                    data.BackGrinding = model.BackGrinding;
                    data.Model = sapcode;
                    data.DatePlan = datePlan;
                    _stayLotListService.UpdateDailyPlanWlp2Update(data, false);
                }
            }

            return Ok(changes);
        }

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

                // get row data in DB
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
            ViewBag.DayPlanWLP2 = WLP2Ticker.GetBeginDateOfWlp2().ToString("yyyy-MM-dd");
            WLP2Ticker.SetStatus(false, true);
            return View();
        }

        [HttpPost]
        public IActionResult SetBeginDateWLP2(string beginDate)
        {
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            WLP2Ticker.BeginDateOfWLP2 = beginDate;
            return new OkObjectResult(null);
        }
        #endregion

        #region LFEM
        public IActionResult DailyPlanLFEM()
        {
            string date = LFEMTicker.GetBeginDateOfLFEM().ToString("yyyy-MM-dd");
            ViewBag.DayPlanLFEM = date;
            LFEMTicker.SetStatus(true, false);
            ViewBag.NextDay = LFEMTicker.GetNexDayLFEM();
            ViewBag.NextDaySMT = _SMTDailyPlanService.GetNexDaySMT(date);
            return View();
        }

        [HttpPost]
        public IActionResult GetstayLotInfoLfem(string model, string operation)
        {
            var data = _stayLotListService.GetStayLotListByModelLFEM(model, operation);
            return new OkObjectResult(data);
        }

        [HttpPost]
        public IActionResult SetBeginDateLFEM(string beginDate)
        {
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            LFEMTicker.BeginDateOfLFEM = beginDate;
            return new OkObjectResult(null);
        }

        [HttpPost]
        public object UpdatePrioryLfem([FromBody] List<DataChange> changes)
        {
            string sapcode = "";
            string operation = "";

            int i = 0;
            foreach (var change in changes)
            {
                sapcode = change.Key.NullString().Split("^")[0];
                operation = change.Key.NullString().Split("^")[1];

                // GET row data in DB
                var data = _stayLotListService
                    .GetStayLotListByModelLFEM(sapcode, operation)
                    .FirstOrDefault(x => x.MesItem + "^" + x.OperationId + "^" + x.LotID == change.Key.NullString());

                if (data != null)
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), data);
                }

                _stayLotListService.UpdatePrioryLotIdLFEM(data, i++);
            }
            _stayLotListService.Save();
            return Ok(changes);
        }
        #endregion

        #region LFEM WIP : Tồn Kho hiện tại
        public IActionResult ViewWipLfem()
        {
            return View();
        }

        [HttpGet]
        public object GetViewWipLfem(DataSourceLoadOptions loadOptions)
        {
            List<WipLotListLFEMViewModel> data = _lotTestHistoryLFemService.GetWIPLotListLfem();
            return DataSourceLoader.Load(data, loadOptions);
        }

        #endregion

        #region Lịch sử lot test line LFEM
        public IActionResult LotTestHistory()
        {
            return View();
        }

        [HttpGet]
        public object GetLotTest(DataSourceLoadOptions loadOptions, string fromTime, string toTime)
        {
            var data = _lotTestHistoryLFemService.GetAllData();
            if (fromTime.NullString() != "")
            {
                data = data.Where(x => x.Date.Value.ToString("yyyy-MM-dd").CompareTo(fromTime) >= 0).ToList();
            }

            if (toTime.NullString() != "")
            {
                data = data.Where(x => x.Date.Value.ToString("yyyy-MM-dd").CompareTo(toTime) <= 0).ToList();
            }

            return DataSourceLoader.Load(data, loadOptions);
        }

        [HttpGet]
        public object GetLotTestView(DataSourceLoadOptions loadOptions)
        {
            var data = _lotTestHistoryLFemService.GetAllData().Where(x => x.KetQua != "OK").OrderByDescending(x => x.Date);
            return DataSourceLoader.Load(data, loadOptions);
        }

        [HttpPost]
        public IActionResult PostLotTest(string values)
        {
            var lot = new LOT_TEST_HISTOTY_LFEM();
            JsonConvert.PopulateObject(values, lot);

            lot = _lotTestHistoryLFemService.PostData(lot);
            return Ok(lot);
        }

        [HttpPut]
        public IActionResult PutLotTest(int key, string values)
        {
            var lot = _lotTestHistoryLFemService.FindById(key);
            if (lot != null)
            {
                JsonConvert.PopulateObject(values, lot);
                lot = _lotTestHistoryLFemService.PutData(lot);
                return Ok(lot);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteLotTest(int key)
        {
            _lotTestHistoryLFemService.DeleteData(key);
        }

        #endregion

        #region RUNTIME
        public IActionResult RuntimeLfem()
        {
            LFEMTicker.SetStatus(false, true);
            return View();
        }
        #endregion

        #region SMT
        public IActionResult DailyPlanSMT()
        {
            ViewBag.DayPlanSMT = SMTTicker.GetBeginDateOfSMT().ToString("yyyy-MM-dd");
            ViewBag.NextDaySMT = SMTTicker.GetNexDaySMT();
            return View();
        }

        [HttpPost]
        public IActionResult SetBeginDateSMT(string beginDate)
        {
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            SMTTicker.BeginDateOfDailyPlanSMT = beginDate;
            return new OkObjectResult(null);
        }

        /// <summary>
        /// Dùng bên page dailyplan LFEM
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetDailyPlanSMT(DataSourceLoadOptions loadOptions,string date)
        {
            var data = _SMTDailyPlanService.GetDailyPlanSMT(date);
            return DataSourceLoader.Load(data, loadOptions);
        }

        /// <summary>
        /// Lấy tồn hiện tại SMT
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetInventorySmt(string materialID)
        {
            var data = _stayLotListService.GetInventoryCurrentSmt(materialID);
            return new OkObjectResult(data);
        }
        #endregion
    }
}
