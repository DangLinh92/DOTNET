using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
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

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsKeHoachDaoTaoATVSLDController : AdminBaseController
    {
        private readonly IEhsKeHoachDaoTaoATVSLDService EhsATLDService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EhsKeHoachDaoTaoATVSLDController(IEhsKeHoachDaoTaoATVSLDService ehsATLDService, IWebHostEnvironment hostingEnvironment)
        {
            EhsATLDService = ehsATLDService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object ATVSLD(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = EhsATLDService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }


        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new EhsKeHoachDaoTaoATLDViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach = EhsATLDService.Add(khoach);
            EhsATLDService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var kehoach = EhsATLDService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsATLDService.Update(kehoach);
            EhsATLDService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(Guid key)
        {
            EhsATLDService.Delete(key);
            EhsATLDService.Save();
        }

        [HttpGet]
        public object GetThucHienATVSLD(DataSourceLoadOptions loadOptions, Guid key)
        {
            var kehoach = EhsATLDService.GetById(key);
            if(kehoach == null)
            {
                return DataSourceLoader.Load(new List<EhsThoiGianThucHienDaoTaoATVSViewModel>(), loadOptions);
            }
            return DataSourceLoader.Load(kehoach.EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD.ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult AddThoiGianATVSLD(string values, Guid maKH)
        {
            var khoach = new EhsThoiGianThucHienDaoTaoATVSViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKHDaoTaoATLD = maKH;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            khoach = EhsATLDService.AddThoiGianATVSLD(khoach);
            EhsATLDService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateThoiGianATVSLD(int key, string values)
        {
            var kehoach = EhsATLDService.GetThoiGianATVSLDById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsATLDService.UpdateThoiGianATVSLD(kehoach);
            EhsATLDService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteThoiGianATVSLD(int key)
        {
            EhsATLDService.DeleteThoiGianATVSLD(key);
            EhsATLDService.Save();
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

                string err = EhsATLDService.ImportExcel(filePath);
                if (err == "")
                    EhsATLDService.Save();
                else
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(filePath);
                    }

                    return new NotFoundObjectResult(err);
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
