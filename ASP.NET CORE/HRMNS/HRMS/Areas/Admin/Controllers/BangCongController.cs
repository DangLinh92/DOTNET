using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class BangCongController : AdminBaseController
    {
        private IBangCongService _bangCongService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BangCongController(IBangCongService bangCongService, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger)
        {
            _bangCongService = bangCongService;
            _hostingEnvironment = hostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lst1 = _bangCongService.GetDataReport("2022-05","");
            return View(lst1);
        }

        [HttpPost]
        public IActionResult Search(string department,string timeTo)
        {
            var lst = _bangCongService.GetDataReport(timeTo, department);
            return PartialView("_gridBangCongPartialView", lst);
        }
    }
}
