using Microsoft.AspNetCore.Mvc;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Implementation;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Sameple;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class LotcardSampleController : AdminBaseController
    {
        private ITCardSampleService _TCardSampleService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _memoryCache;
        public LotcardSampleController(ITCardSampleService TCardSampleService, IWebHostEnvironment hostingEnvironment, ILogger<MaterialToSapCodeController> logger, IMemoryCache memoryCache)
        {
            _TCardSampleService = TCardSampleService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_TCardSampleService.GetAllData(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            TCARD_SAMPLE model = new TCARD_SAMPLE();
            JsonConvert.PopulateObject(values, model);
            _TCardSampleService.Add(model);
            _TCardSampleService.Save();
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _TCardSampleService.GetById(key);

            JsonConvert.PopulateObject(values, model);
            _TCardSampleService.Update(model);
            _TCardSampleService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _TCardSampleService.Delete(key);
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

                ResultDB rs = _TCardSampleService.ImportExcel(filePath, filename);

                if (rs.ReturnInt == 0)
                {
                    _TCardSampleService.Save();
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

                _logger.LogInformation("Import tcard success");
                return new OkObjectResult(filePath);
            }

            _logger.LogInformation("File tcard null");
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
