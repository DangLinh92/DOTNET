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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class ThicknessWlp2Controller : AdminBaseController
    {
        private readonly IThicknessWlp2Service _thicknessWlp2Service;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ThicknessWlp2Controller(IThicknessWlp2Service thicknessWlp2Service, IWebHostEnvironment hostingEnvironment, ILogger<ThicknessWlp2Controller> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _thicknessWlp2Service = thicknessWlp2Service;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_thicknessWlp2Service.GetAllData(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            ThickNetModelWlp2ViewModel model = new ThickNetModelWlp2ViewModel();
            JsonConvert.PopulateObject(values, model);

            _thicknessWlp2Service.Add(model);
            _thicknessWlp2Service.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _thicknessWlp2Service.GetById(key);

            JsonConvert.PopulateObject(values, model);
            _thicknessWlp2Service.Update(model);
            _thicknessWlp2Service.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _thicknessWlp2Service.Delete(key);
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


                ResultDB rs = _thicknessWlp2Service.ImportExcel(filePath, "");

                if (rs.ReturnInt == 0)
                {
                    _thicknessWlp2Service.Save();
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
