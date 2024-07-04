using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace HRMS.Areas.Payroll.Controllers
{
    public class ConNhoMnsController : AdminBaseController
    {
        private IConNhoMnsService _conNhoService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ConNhoMnsController(IConNhoMnsService conNhoService, IWebHostEnvironment hostEnvironment)
        {
            _conNhoService = conNhoService;
            _hostingEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetConNho(DataSourceLoadOptions loadOptions)
        {
            var data = _conNhoService.GetConNhos();
            return DataSourceLoader.Load(data, loadOptions);
        }

        [HttpPost]
        public IActionResult PostConNho(string values)
        {
            var conNho = new HR_CON_NHO();
            JsonConvert.PopulateObject(values, conNho);

            if (!TryValidateModel(conNho))
                return BadRequest("Error: lỗi input");

            _conNhoService.PostConNho(conNho);
            _conNhoService.Save();
            return Ok();
        }

        [HttpPut]
        public IActionResult PutConNho(int key, string values)
        {
            var data = _conNhoService.GetConNhoById(key);
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest("Error: lỗi cập nhật");

            _conNhoService.PutConNho(data);
            _conNhoService.Save();

            return Ok();
        }

        /// <summary>
        /// Import danh mục chuyển đổi code wlp
        /// </summary>
        /// <param name="files"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportConNho(IList<IFormFile> files, [FromQuery] string param)
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
                ResultDB rs = _conNhoService.ImportConNhoExcel(filePath, param);

                if (rs.ReturnInt == 0)
                {
                    _conNhoService.Save();
                }
                else
                {
                    return new NotFoundObjectResult(rs.ReturnString);
                }

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
