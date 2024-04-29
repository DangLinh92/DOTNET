using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRMS.Areas.Payroll.Controllers
{
    public class PhuCapLuongController : AdminBaseController
    {
        private IPhuCapLuongService _phuCapLuongService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PhuCapLuongController(IPhuCapLuongService phuCapLuongService, IWebHostEnvironment hostingEnvironment)
        {
            _phuCapLuongService = phuCapLuongService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetPCDH(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _phuCapLuongService.GetAll();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpGet]
        public object GetBoPhanAll(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _phuCapLuongService.GetBoPhanAll();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult InsertPCDH(string values)
        {
            var phucap = new PHUCAP_DOC_HAI();
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.AddDH(phucap);

            return Ok(phucap);
        }

        [HttpPut]
        public IActionResult UpdatePCDH(int key, string values)
        {
            var phucap = _phuCapLuongService.GetAllById(key);
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.UpdateDH(phucap);

            return Ok(phucap);
        }

        [HttpDelete]
        public void DeleteDH(int key)
        {
            _phuCapLuongService.DeleteDH(key);
        }

        // thong tin cấp bậc
        [HttpGet]
        public object GetPCGrade(DataSourceLoadOptions loadOptions,int year)
        {
            var lstModel = _phuCapLuongService.GetAllGrade(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult InsertGrade(string values)
        {
            var phucap = new HR_SALARY_GRADE();
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.AddGrade(phucap);

            return Ok(phucap);
        }

        [HttpPut]
        public IActionResult UpdateGrade(string key, string values)
        {
            var phucap = _phuCapLuongService.GetGradeById(key);

            if(phucap != null)
            {
                JsonConvert.PopulateObject(values, phucap);
                phucap = _phuCapLuongService.UpdateGrade(phucap);
                return Ok(phucap);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteGrade(string key)
        {
            _phuCapLuongService.DeleteGrade(key);
        }

        /// <summary>
        /// Salary grade information
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
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

                ResultDB rs = _phuCapLuongService.ImportExcel(filePath);

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
