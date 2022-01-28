using HRMNS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class BoPhanDetailController : AdminBaseController
    {
        IBoPhanDetailService _boPhanDetailService;
        public BoPhanDetailController(IBoPhanDetailService boPhanService)
        {
            _boPhanDetailService = boPhanService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var bophans = _boPhanDetailService.GetAll(null);
            return new OkObjectResult(bophans);
        }
    }
}
