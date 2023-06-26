using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class HoldLotListController : AdminBaseController
    {
        private IStayLotListService _StayLotListService;
        public HoldLotListController(IStayLotListService StayLotListService)
        {
            _StayLotListService = StayLotListService;
        }

        public IActionResult Index()
        {
            var model = _StayLotListService.GetStayLotList();
            return View(model);
        }

        [HttpPut]
        public IActionResult UpdateLotStay(string key, string values)
        {
            var models = _StayLotListService.GetStayLotList();

            var lots = models.StayLotList_Ex_ViewModels.First(o => o.Key == key);

            JsonConvert.PopulateObject(values, lots);

            _StayLotListService.UpdateLotInfo(lots, models);

            return Ok(lots);
        }

        [HttpGet]
        public object GetLotStay(DataSourceLoadOptions loadOptions)
        {
            var model = _StayLotListService.GetStayLotList();
            return DataSourceLoader.Load(model.StayLotList_Ex_ViewModels, loadOptions);
        }

        public IActionResult HoldLotHistory()
        {
            var data = _StayLotListService.GetStayLotListHistory("", "", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_DATA = data,
                FromTime = DateTime.Now.ToString("yyyy-MM-dd"),
                ToTime = DateTime.Now.ToString("yyyy-MM-dd")
            };

            return View(models);
        }

        [HttpPost]
        public IActionResult GetLotHoldHistory(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            var data = _StayLotListService.GetStayLotListHistory(cassetteId, lotId, timeFrom, timeTo);
            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_DATA = data,
                CasseteId = cassetteId,
                LotId = lotId,
                FromTime = timeFrom,
                ToTime = timeTo
            };
            return View("HoldLotHistory", models);
        }

        #region WLP2 Hold lot list
        public IActionResult HoldWlp2()
        {
            var model = _StayLotListService.GetStayLotListWlp2();
            return View(model);
        }

        [HttpGet]
        public object GetLotStayWlp2(DataSourceLoadOptions loadOptions)
        {
            var model = _StayLotListService.GetStayLotListWlp2();
            return DataSourceLoader.Load(model.StayLotList_Ex_ViewModels, loadOptions);
        }

        [HttpPut]
        public IActionResult UpdateLotStayWlp2(string key, string values)
        {
            var models = _StayLotListService.GetStayLotListWlp2();

            var lots = models.StayLotList_Ex_ViewModels.First(o => o.Key == key);

            JsonConvert.PopulateObject(values, lots);

            _StayLotListService.UpdateLotInfoWlp2(lots, models);

            return Ok(lots);
        }

        public IActionResult HoldLotHistoryWlp2()
        {
            var data = _StayLotListService.GetStayLotListHistoryWlp2("", "", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_WLP2_DATA = data,
                FromTime = DateTime.Now.ToString("yyyy-MM-dd"),
                ToTime = DateTime.Now.ToString("yyyy-MM-dd")
            };

            return View(models);
        }

        [HttpPost]
        public IActionResult GetLotHoldHistoryWlp2(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            var data = _StayLotListService.GetStayLotListHistoryWlp2(cassetteId, lotId, timeFrom, timeTo);
            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_WLP2_DATA = data,
                CasseteId = cassetteId,
                LotId = lotId,
                FromTime = timeFrom,
                ToTime = timeTo
            };
            return View("HoldLotHistoryWlp2", models);
        }
        #endregion


        #region SAMPLE Hold lot list
        public IActionResult HoldSample()
        {
            var model = _StayLotListService.GetStayLotListSample();
            return View(model);
        }

        [HttpGet]
        public object GetLotStaySample(DataSourceLoadOptions loadOptions)
        {
            var model = _StayLotListService.GetStayLotListSample();
            return DataSourceLoader.Load(model.StayLotList_Ex_ViewModels, loadOptions);
        }

        [HttpPut]
        public IActionResult UpdateLotStaySample(string key, string values)
        {
            var models = _StayLotListService.GetStayLotListSample();

            var lots = models.StayLotList_Ex_ViewModels.First(o => o.Key == key);

            JsonConvert.PopulateObject(values, lots);

            _StayLotListService.UpdateLotInfoSample(lots, models);

            return Ok(lots);
        }

        public IActionResult HoldLotHistorySample()
        {
            var data = _StayLotListService.GetStayLotListHistorySample("", "", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_SAMPLE_DATA = data,
                FromTime = DateTime.Now.ToString("yyyy-MM-dd"),
                ToTime = DateTime.Now.ToString("yyyy-MM-dd")
            };

            return View(models);
        }

        [HttpPost]
        public IActionResult GetLotHoldHistorySample(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            var data = _StayLotListService.GetStayLotListHistorySample(cassetteId, lotId, timeFrom, timeTo);
            ViewHistoryHoldLotModel models = new ViewHistoryHoldLotModel()
            {
                STAY_LOT_LIST_HISTORY_SAMPLE_DATA = data,
                CasseteId = cassetteId,
                LotId = lotId,
                FromTime = timeFrom,
                ToTime = timeTo
            };
            return View("HoldLotHistorySample", models);
        }
        #endregion
    }
}
