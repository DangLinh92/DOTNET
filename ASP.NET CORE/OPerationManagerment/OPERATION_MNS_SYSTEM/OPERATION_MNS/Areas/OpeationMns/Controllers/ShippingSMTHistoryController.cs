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
    public class ShippingSMTHistoryController : AdminBaseController
    {
        private IShippingSMTHistoryService _shippingSMTHistoryService;
        public ShippingSMTHistoryController(IShippingSMTHistoryService shippingSMTHistoryService)
        {
            _shippingSMTHistoryService = shippingSMTHistoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetSMTShipByLot(DataSourceLoadOptions loadOptions,string fromTime,string toTime)
        {
            var model = _shippingSMTHistoryService.GetHistoryByTime(fromTime, toTime);
            return DataSourceLoader.Load(model[0].shippingSMTHistoryByLotIdViewModels, loadOptions);
        }

        [HttpGet]
        public object GetSMTShipByTime(DataSourceLoadOptions loadOptions, string fromTime, string toTime)
        {
            var model = _shippingSMTHistoryService.GetHistoryByTime(fromTime, toTime);
            return DataSourceLoader.Load(model[0].shippingSMTHistoryBySapCodeViewModels, loadOptions);
        }
    }
}
