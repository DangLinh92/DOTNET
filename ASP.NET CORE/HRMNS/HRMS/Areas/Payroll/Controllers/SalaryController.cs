using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    public class SalaryController : AdminBaseController
    {
        private readonly ISalaryService _salaryService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SalaryController(ISalaryService salaryService, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _salaryService = salaryService;
        }

        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);
            SetSessionInpage(CommonConstants.OUT);
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions, int year)
        {
            return DataSourceLoader.Load(_salaryService.GetAllSalary(year), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            HR_SALARY model = new HR_SALARY();
            JsonConvert.PopulateObject(values, model);

            _salaryService.AddSalary(model);
            //_salaryService.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var entity = _salaryService.GetById(key);

            JsonConvert.PopulateObject(values, entity);

            _salaryService.UpdateSalary(entity);

            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _salaryService.DeleteSalary(key);
        }

        /// <summary>
        /// Salary information
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

                ResultDB rs = _salaryService.ImportExcel(filePath);
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

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportIncentiveExcel(IList<IFormFile> files)
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

                ResultDB rs = _salaryService.ImportIncentiveExcel(filePath);
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

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportSoTaiKhoan(IList<IFormFile> files)
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

                ResultDB rs = _salaryService.ImportTaiKhoanNHExcel(filePath);
                if (rs.ReturnInt == 0)
                    _salaryService.Save();
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

        #region Chi Phi Phat Sinh
        public IActionResult ChiPhiPhatSinh()
        {
            return View();
        }

        [HttpGet]
        public object GetPhatSinh(DataSourceLoadOptions loadOptions, int year)
        {
            return DataSourceLoader.Load(_salaryService.GetAllSalaryPhatSinh(year), loadOptions);
        }

        [HttpPost]
        public IActionResult PostPhatSinh(string values)
        {
            HR_SALARY_PHATSINH model = new HR_SALARY_PHATSINH();
            JsonConvert.PopulateObject(values, model);

            _salaryService.AddSalaryPhatSinh(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult PutPhatSinh(int key, string values)
        {
            var entity = _salaryService.GetPhatSinhById(key);

            JsonConvert.PopulateObject(values, entity);
            _salaryService.UpdateSalaryPhatSinh(entity);

            return Ok();
        }

        [HttpDelete]
        public void DeletePhatSinh(int key)
        {
            _salaryService.DeleteSalaryPhatSinh(key);
        }

        [HttpGet]
        public object GetDanhMucPhatSinh(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_salaryService.GetDanhMucPhatSinh(), loadOptions);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportPhatSinhExcel(IList<IFormFile> files)
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

                ResultDB rs = _salaryService.ImportPhatSinhExcel(filePath);
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
        #endregion


    }
}
