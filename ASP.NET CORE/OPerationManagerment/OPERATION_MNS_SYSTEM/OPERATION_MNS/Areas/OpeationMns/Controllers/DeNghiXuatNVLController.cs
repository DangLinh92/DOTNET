using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class DeNghiXuatNVLController : AdminBaseController
    {
        private IDeNghiXuatNVLService _IDeNghiXuatNVLService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DeNghiXuatNVLController(IDeNghiXuatNVLService DeNghiXuatNVLService, IWebHostEnvironment hostingEnvironment, ILogger<DeNghiXuatNVLController> logger)
        {
            _IDeNghiXuatNVLService = DeNghiXuatNVLService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions, string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 23)
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                else
                {
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
            }

            return DataSourceLoader.Load(_IDeNghiXuatNVLService.GetAllData(date), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            BoPhanDeNghiXuatNVLViewModel model = new BoPhanDeNghiXuatNVLViewModel();
            JsonConvert.PopulateObject(values, model);
            model.NgayDeNghi = model.NgayDeNghi2.ToString("yyyy-MM-dd");

            _IDeNghiXuatNVLService.Add(model);
            _IDeNghiXuatNVLService.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _IDeNghiXuatNVLService.GetById(key);

            JsonConvert.PopulateObject(values, model);

            model.NgayDeNghi = model.NgayDeNghi2.ToString("yyyy-MM-dd");
            _IDeNghiXuatNVLService.Update(model);
            _IDeNghiXuatNVLService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _IDeNghiXuatNVLService.Delete(key);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files)
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
                string fName = CorrelationIdGenerator.GetNextId() + filename;
                string filePath = Path.Combine(folder, fName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                ResultDB rs = _IDeNghiXuatNVLService.ImportExcel(filePath, "");

                if (rs.ReturnInt == 0)
                {
                    _IDeNghiXuatNVLService.Save();
                }
                else
                {
                    _logger.LogInformation("Import error:" + rs.ReturnString);
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                _logger.LogInformation("Import success");
                return new OkObjectResult(filePath);
            }

            _logger.LogInformation("File null");
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
