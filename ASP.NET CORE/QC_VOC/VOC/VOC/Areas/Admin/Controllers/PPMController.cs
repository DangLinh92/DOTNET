using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;

namespace VOC.Areas.Admin.Controllers
{
    public class PPMController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;

        public PPMController(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            GmesDataViewModel gmes = _vocMstService.GetGmesData(0,0);
            return View(gmes);
        }

        [HttpPost]
        public IActionResult UpdatePPMByYear(VocPPMYearViewModel model, [FromQuery] string action)
        {
            bool isAdd = action == "Add";
            _vocMstService.UpdatePPMByYear(isAdd, model);
            _vocMstService.Save();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetPPMByYear(int id)
        {
            VocPPMYearViewModel model = _vocMstService.GetPPMByYear(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetTargetByYear()
        {
            int year = DateTime.Now.Year;
            double target = _vocMstService.GetTargetPPMByYear(year);
            return new OkObjectResult(target);
        }

        [HttpGet]
        public IActionResult GetPPMByYearMonth(int id)
        {
            VocPPMViewModel model = _vocMstService.GetPPMByYearMonth(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdatePPM(VocPPMViewModel model, [FromQuery] string action)
        {
            bool isAdd = action == "Add";
            _vocMstService.UpdatePPMByYearMonth(isAdd, model);
            _vocMstService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult DeletePPMByYear(int Id)
        {
            _vocMstService.DeletePPMByYear(Id);
            _vocMstService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult DeletePPMByYearMonth(int Id)
        {
            _vocMstService.DeletePPMByYearMonth(Id);
            _vocMstService.Save();
            return new OkObjectResult(Id);
        }
    }
}
