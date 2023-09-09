using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class InventoryController : AdminBaseController
    {
        IInventoryService _InventoryService;
        ISMTReturnWlp2Service _ISMTReturnWlp2Service;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public InventoryController(IInventoryService service, IWebHostEnvironment hostingEnvironment, ISMTReturnWlp2Service ISMTReturnWlp2Service)
        {
            _hostingEnvironment = hostingEnvironment;
            _InventoryService = service;
            _ISMTReturnWlp2Service = ISMTReturnWlp2Service;
        }

        public IActionResult Index()
        {
            InventoryTicker.SetStatus(true, false);
            ViewBag.ViewOption_Actual = string.IsNullOrEmpty(InventoryTicker.ViewOption_Actual) ? CommonConstants.CHIP : InventoryTicker.ViewOption_Actual;
            return View();
        }

        [HttpPost]
        public IActionResult SetViewOption(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                option = CommonConstants.CHIP;
            }

            InventoryTicker.ViewOption_Actual = option;
            return new OkObjectResult(option);
        }

        public IActionResult Wlp2Stock()
        {
            WLP2Ticker.SetStatus(true, false);
            return View();
        }

        // update smt return
        [HttpPost]
        public object Batch([FromBody] List<DataChange> changes)
        {
            foreach (var change in changes)
            {
                SmtReturnWlp2ViewModel smtReturn;

                if (change.Type == "update")
                {
                    var key = change.Key.NullString();

                    StockHoldPositionViewModel dataGrid = new StockHoldPositionViewModel()
                    {
                        SapCode = key
                    };

                    smtReturn = _ISMTReturnWlp2Service.FindSingle(key);

                    JsonConvert.PopulateObject(change.Data.ToString(), dataGrid);

                    if(smtReturn != null)
                    {
                        smtReturn.SmtReturn = dataGrid.SmtReturn;
                        _ISMTReturnWlp2Service.Update(smtReturn);
                    }
                    else
                    {
                        smtReturn = new SmtReturnWlp2ViewModel()
                        {
                            SapCode = key,
                            SmtReturn = dataGrid.SmtReturn
                        };
                        _ISMTReturnWlp2Service.Insert(smtReturn);
                    }
                }
            }

            _ISMTReturnWlp2Service.Save();

            return Ok(changes);
        }
    }

    public class DataChange
    {
        [JsonProperty("key")]
        public object Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
