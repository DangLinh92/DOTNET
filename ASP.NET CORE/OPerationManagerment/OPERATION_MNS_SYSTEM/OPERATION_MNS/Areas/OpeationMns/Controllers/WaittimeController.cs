using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class WaittimeController : AdminBaseController
    {
        IWaitimeService _WaitimeService;

        public WaittimeController(IWaitimeService WaitimeService)
        {
            _WaitimeService = WaitimeService;
        }

        public IActionResult Index()
        {
            var models = _WaitimeService.GetWaitTime_WLP1();
            return View(models);
        }
    }
}
