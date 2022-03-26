using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Utilities.Common;
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;


        public HomeController(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            VocInfomationsModel model = new VocInfomationsModel();
            string startTime = DateTime.Now.Year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            model.vOC_MSTViews.AddRange(_vocMstService.SearchByTime(startTime, endTime));

            model.vOCSiteModelByTimeLsts.AddRange(_vocMstService.ReportInit());
            model.totalVOCSitesView = _vocMstService.ReportByYear(DateTime.Now.Year.ToString());

            return View(model);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, [FromQuery] string param)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                ResultDB result = _vocMstService.ImportExcel(filePath, param);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                if (result.ReturnInt == 0)
                {
                    return new OkObjectResult(filePath);
                }
                else
                {
                    _logger.LogError(result.ReturnString);
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }

            _logger.LogError("Upload file: " + CommonConstants.NotFoundObjectResult_Msg);
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
