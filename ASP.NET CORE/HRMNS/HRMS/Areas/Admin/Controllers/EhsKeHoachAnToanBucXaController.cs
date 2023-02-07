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
    public class EhsKeHoachAnToanBucXaController : AdminBaseController
    {
        private readonly IEhsKeHoachAnToanBucXaService EhsKeHoachATBXService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EhsKeHoachAnToanBucXaController(IEhsKeHoachAnToanBucXaService ehsKeHoachATBXService, IWebHostEnvironment hostingEnvironment)
        {
            EhsKeHoachATBXService = ehsKeHoachATBXService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object KeHoachATBX(DataSourceLoadOptions loadOptions)
        {
           var lstModel = EhsKeHoachATBXService.GetList();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new EhsKeHoachAnToanBucXaViewModel();
            JsonConvert.PopulateObject(values, khoach);

            EhsKeHoachATBXService.Add(khoach);
            EhsKeHoachATBXService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var kehoach = EhsKeHoachATBXService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsKeHoachATBXService.Update(kehoach);
            EhsKeHoachATBXService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(Guid key)
        {
            EhsKeHoachATBXService.Delete(key);
            EhsKeHoachATBXService.Save();
        }

        [HttpGet]
        public object GetThucHienATBX(DataSourceLoadOptions loadOptions,Guid key)
        {
            var kehoach = EhsKeHoachATBXService.GetById(key);
            return DataSourceLoader.Load(kehoach.EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA.OrderByDescending(x=>x.NgayBatDau).ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult AddThoiGianATBX(string values,Guid maKH)
        {
            var khoach = new EhsThoiGianThucHienAnToanBucXaViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKH_ATBX = maKH;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachATBXService.AddThoiGianBucXa(khoach);
            EhsKeHoachATBXService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateThoiGianATBX(int key, string values)
        {
            var kehoach = EhsKeHoachATBXService.GetThoiGianBucXaById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachATBXService.UpdateThoiGianBucXa(kehoach);
            EhsKeHoachATBXService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteThoiGianATBX(int key)
        {
            EhsKeHoachATBXService.DeleteThoiGianBucXa(key);
            EhsKeHoachATBXService.Save();
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

                EhsKeHoachATBXService.ImportExcel(filePath);
                EhsKeHoachATBXService.Save();

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
