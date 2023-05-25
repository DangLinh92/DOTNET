using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRMS.Areas.Payroll.Controllers
{
    public class CapBacNhanVienController : AdminBaseController
    {
        private readonly ICapBacNhanVienService _capBacNhanVienService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CapBacNhanVienController(ICapBacNhanVienService capBacNhanVienService, IWebHostEnvironment hostingEnvironment)
        {
            _capBacNhanVienService = capBacNhanVienService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);
            SetSessionInpage(CommonConstants.OUT);
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions,int year)
        {
            return DataSourceLoader.Load(_capBacNhanVienService.GetAll(year), loadOptions);
        }

        [HttpPost]
        public IActionResult PostCB(string values)
        {
            NHANVIEN_INFOR_EX model = new NHANVIEN_INFOR_EX();
            JsonConvert.PopulateObject(values, model);
            _capBacNhanVienService.Add(model);
            return Ok();
        }

        [HttpPut]
        public IActionResult PutCB(int key, string values)
        {
            var entity = _capBacNhanVienService.GetCapBacById(key);
            JsonConvert.PopulateObject(values, entity);
            _capBacNhanVienService.Update(entity);

            return Ok();
        }

        [HttpDelete]
        public void DeleteCB(int key)
        {
            _capBacNhanVienService.Delete(key);
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

                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                ResultDB rs = _capBacNhanVienService.ImportCapBacExcel(filePath);
                if (rs.ReturnInt != 0)
                    return new NotFoundObjectResult(rs.ReturnString);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                return new OkObjectResult(filePath);
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
