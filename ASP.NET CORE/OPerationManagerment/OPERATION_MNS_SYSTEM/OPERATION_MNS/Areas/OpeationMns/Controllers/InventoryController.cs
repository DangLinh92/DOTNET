using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
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
        private readonly IWebHostEnvironment _hostingEnvironment;
        public InventoryController(IInventoryService service, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _InventoryService = service;
        }

        public IActionResult Index()
        {
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
    }
}
