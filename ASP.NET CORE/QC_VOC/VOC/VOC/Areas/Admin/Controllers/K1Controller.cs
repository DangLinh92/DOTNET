using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Constants;

namespace VOC.Areas.Admin.Controllers
{
    public class K1Controller : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;

        public K1Controller(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index(int year)
        {
            VocInfomationsModel model = GetData(year, CommonConstants.CSP);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="site">CSP/LFEM</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Search(int year, string site)
        {
            VocInfomationsModel model = GetData(year, site.NullString(), true);
            return new OkObjectResult(model);
        }

        private VocInfomationsModel GetData(int year, string site, bool isSearch = false)
        {
            VocInfomationsModel model = new VocInfomationsModel();

            // Ve Bieu Do PPM
            List<VOCPPM_Ex> pMDataCharts;
            if (!isSearch && (year <= 0 || year == DateTime.Now.Year))
            {
                model.VocPPMView = _vocMstService.ReportPPMByYear(DateTime.Now.Year.ToString(), site, out pMDataCharts);
            }
            else
            {
                model.VocPPMView = _vocMstService.ReportPPMByYear(year.ToString(), site, out pMDataCharts);
            }
            model.vOCPPMs = pMDataCharts;
            return model;
        }

        [HttpGet]
        public IActionResult UploadList()
        {
            GmesDataViewModel gmes = _vocMstService.GetGmesData();
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
