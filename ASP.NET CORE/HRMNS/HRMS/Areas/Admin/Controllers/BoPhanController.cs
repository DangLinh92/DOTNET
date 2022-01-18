using HRMNS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class BoPhanController : AdminBaseController
    {
        IBoPhanService _boPhanService;
        public BoPhanController(IBoPhanService boPhanService)
        {
            _boPhanService = boPhanService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var bophans = _boPhanService.GetAll(null);
            return new OkObjectResult(bophans);
        }
    }
}
