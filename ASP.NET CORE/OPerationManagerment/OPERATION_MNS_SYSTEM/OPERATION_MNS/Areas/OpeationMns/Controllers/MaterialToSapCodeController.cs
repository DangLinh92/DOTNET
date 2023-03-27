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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class MaterialToSapCodeController : AdminBaseController
    {
        private IMaterialToSapCodeService _materialToSapCodeService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _memoryCache;
        public MaterialToSapCodeController(IMaterialToSapCodeService materialToSapCodeService, IWebHostEnvironment hostingEnvironment, ILogger<MaterialToSapCodeController> logger, IMemoryCache memoryCache)
        {
            _materialToSapCodeService = materialToSapCodeService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IActionResult Index(string bophan)
        {
            _memoryCache.Remove("MaterialToSap_Dep");
            _memoryCache.Set("MaterialToSap_Dep", bophan);

            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            _memoryCache.TryGetValue("MaterialToSap_Dep", out string deparment);
            if (!string.IsNullOrEmpty(deparment))
            {
                return DataSourceLoader.Load(_materialToSapCodeService.GetAllData(deparment), loadOptions);
            }

            return DataSourceLoader.Load(_materialToSapCodeService.GetAllData("WLP1"), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            MaterialToSapViewModel model = new MaterialToSapViewModel();
            JsonConvert.PopulateObject(values, model);

            _memoryCache.TryGetValue("MaterialToSap_Dep", out string deparment);

            if (!string.IsNullOrEmpty(deparment))
            {
                model.Department = deparment;
                _materialToSapCodeService.Add(model);
                _materialToSapCodeService.Save();
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _materialToSapCodeService.GetById(key);

            JsonConvert.PopulateObject(values, model);
            _materialToSapCodeService.Update(model);
            _materialToSapCodeService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _materialToSapCodeService.Delete(key);
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

                _memoryCache.TryGetValue("MaterialToSap_Dep", out string deparment);

                if (string.IsNullOrEmpty(deparment))
                {
                    _logger.LogInformation("deparment null");
                    return new NotFoundObjectResult("deparment null");
                }

                ResultDB rs = _materialToSapCodeService.ImportExcel(filePath, deparment);

                if (rs.ReturnInt == 0)
                {
                    _materialToSapCodeService.Save();
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
