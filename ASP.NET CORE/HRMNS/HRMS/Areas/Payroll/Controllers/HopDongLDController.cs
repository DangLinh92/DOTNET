using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Utilities.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    public class HopDongLDController : AdminBaseController
    {
        private IHopDongService _hopDongService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HopDongLDController(IHopDongService hopDongService, IWebHostEnvironment webHost)
        {
            _hopDongService = hopDongService;
            _hostingEnvironment = webHost;
        }
        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);
            SetSessionInpage(CommonConstants.OUT);
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_hopDongService.GetNVHetHanHD(), loadOptions);
        }
    }
}
