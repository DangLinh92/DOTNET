using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class ChamCongController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IChamCongService _chamCongService;
        private BioStarDBContext _bioStarDB;

        public ChamCongController(IChamCongService chamCongService, IWebHostEnvironment hostingEnvironment, BioStarDBContext bioStarDB)
        {
            _chamCongService = chamCongService;
            _bioStarDB = bioStarDB;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(new List<ChamCongLogViewModel>());
        }

        public IActionResult GetData()
        {
            List<ChamCongLogViewModel> chamCongLog = _chamCongService.GetAll("");
            return View("Index",chamCongLog);
        }

        [HttpGet]
        public IActionResult GetChamCongLog()
        {
            string maxDate = _chamCongService.GetMaxDate();
            if (string.IsNullOrEmpty(maxDate))
            {
                maxDate = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            string fromTime = DateTime.Parse(maxDate).AddDays(-40).ToString("yyyy-MM-dd");
            string toTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            ResultDB result = _bioStarDB.GetChamCongLogData(fromTime, toTime);
            if (result.ReturnInt == 0)
            {
                ResultDB result1 = _chamCongService.InsertLogData(result.ReturnDataSet.Tables[0]);

                if (result1.ReturnInt == 0)
                {
                    return new OkObjectResult(result1.ReturnString);
                }
                else
                {
                    return new BadRequestObjectResult(result1.ReturnString);
                }
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpGet]
        public IActionResult GetDepartment()
        {
            ResultDB result = _bioStarDB.GetDeparment();
            if (result.ReturnInt == 0)
            {
                string depts = DataTableToJson.DataTableToJSONWithJSONNet(result.ReturnDataSet.Tables[0]);
                return new OkObjectResult(depts);
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpPost]
        public IActionResult Search(string result, string dept, string fromTime, string toTime)
        {
            var lst = _chamCongService.Search(result, dept, fromTime, toTime);
            return PartialView("_gridChamCongPartialView", lst);
        }

        [HttpPost]
        public IActionResult GetLogByUserId(string userId,string time)
        {
            ResultDB result = _bioStarDB.ShowChamCongLogInDay(userId,time);
            if (result.ReturnInt == 0)
            {
                string data = DataTableToJson.DataTableToJSONWithJSONNet(result.ReturnDataSet.Tables[0]);
                return new OkObjectResult(data);
            }
            else
            {
                return new OkObjectResult(result.ReturnString);
            }
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
                ResultDB result = _chamCongService.ImportExcel(filePath, param);

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
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
