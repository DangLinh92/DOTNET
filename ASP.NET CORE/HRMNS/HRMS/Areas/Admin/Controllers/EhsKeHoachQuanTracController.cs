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
    public class EhsKeHoachQuanTracController : AdminBaseController
    {
        private readonly IEhsKeHoachQuanTracService EhsKeHoachQuanTracService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EhsKeHoachQuanTracController(IEhsKeHoachQuanTracService ehsKeHoachQuanTracService, IWebHostEnvironment hostingEnvironment)
        {
            EhsKeHoachQuanTracService = ehsKeHoachQuanTracService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object QuanTracs(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = EhsKeHoachQuanTracService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }


        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new EhsKeHoachQuanTracViewModel();
            JsonConvert.PopulateObject(values, khoach);

            EhsKeHoachQuanTracService.Add(khoach);
            EhsKeHoachQuanTracService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(int key, string values)
        {
            var kehoach = EhsKeHoachQuanTracService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsKeHoachQuanTracService.Update(kehoach);
            EhsKeHoachQuanTracService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(int key)
        {
            EhsKeHoachQuanTracService.Delete(key);
            EhsKeHoachQuanTracService.Save();
        }

        [HttpGet]
        public object GetNgayQuanTrac(DataSourceLoadOptions loadOptions, int key)
        {
            var kehoach = EhsKeHoachQuanTracService.GetById(key);
            return DataSourceLoader.Load(kehoach.EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC.ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult InsertNgayQuanTrac(string values, int maKHQuanTrac)
        {
            var khoach = new EhsNgayThucHienChiTietQuanTrac();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKHQuanTrac = maKHQuanTrac;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachQuanTracService.AddNgayQuanTrac(khoach);
            EhsKeHoachQuanTracService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateNgayQuanTrac(int key, string values)
        {
            var kehoach = EhsKeHoachQuanTracService.GetNgayQuanTracById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachQuanTracService.UpdateNgayQuanTrac(kehoach);
            EhsKeHoachQuanTracService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteNgayQuanTrac(int key)
        {
            EhsKeHoachQuanTracService.DeleteNgayQuanTrac(key);
            EhsKeHoachQuanTracService.Save();
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

                string err = EhsKeHoachQuanTracService.ImportExcel(filePath);
                if (err == "")
                    EhsKeHoachQuanTracService.Save();
                else
                {
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
