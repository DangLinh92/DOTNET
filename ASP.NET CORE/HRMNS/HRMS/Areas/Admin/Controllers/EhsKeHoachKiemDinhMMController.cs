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
    public class EhsKeHoachKiemDinhMMController : AdminBaseController
    {
        private readonly IEhsKeHoachKiemDinhMayMocService EhsKeHoachKiemDinhMayMocService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EhsKeHoachKiemDinhMMController(IEhsKeHoachKiemDinhMayMocService ehsKeHoachKiemDinhMayMocService, IWebHostEnvironment hostingEnvironment)
        {
            EhsKeHoachKiemDinhMayMocService = ehsKeHoachKiemDinhMayMocService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object KeHoachKiemDinhMayMoc(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = EhsKeHoachKiemDinhMayMocService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new EhsKeHoachKiemDinhMayMocViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach = EhsKeHoachKiemDinhMayMocService.Add(khoach);
            EhsKeHoachKiemDinhMayMocService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var kehoach = EhsKeHoachKiemDinhMayMocService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsKeHoachKiemDinhMayMocService.Update(kehoach);
            EhsKeHoachKiemDinhMayMocService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(Guid key)
        {
            EhsKeHoachKiemDinhMayMocService.Delete(key);
            EhsKeHoachKiemDinhMayMocService.Save();
        }

        [HttpGet]
        public object GetThucHienKiemDinhMayMoc(DataSourceLoadOptions loadOptions, Guid key)
        {
            var kehoach = EhsKeHoachKiemDinhMayMocService.GetById(key);

            if(kehoach == null)
            {
                return DataSourceLoader.Load(new List<EhsThoiGianKiemDinhMayMocViewModel>(), loadOptions);
            }

            return DataSourceLoader.Load(kehoach.EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM.OrderByDescending(x => x.NgayBatDau).ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult AddThoiGianKiemDinhMayMoc(string values, Guid maKH)
        {
            var khoach = new EhsThoiGianKiemDinhMayMocViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKH_KDMM = maKH;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            khoach = EhsKeHoachKiemDinhMayMocService.AddThoiGianKiemDinhMayMoc(khoach);
            EhsKeHoachKiemDinhMayMocService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateThoiGianKiemDinhMayMoc(int key, string values)
        {
            var kehoach = EhsKeHoachKiemDinhMayMocService.GetThoiGianKiemDinhMayMocById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachKiemDinhMayMocService.UpdateThoiGianKiemDinhMayMoc(kehoach);
            EhsKeHoachKiemDinhMayMocService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteThoiGianDinhMayMoc(int key)
        {
            EhsKeHoachKiemDinhMayMocService.DeleteThoiKiemDinhMayMoc(key);
            EhsKeHoachKiemDinhMayMocService.Save();
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

                string err = EhsKeHoachKiemDinhMayMocService.ImportExcel(filePath);

                if (err == "")
                    EhsKeHoachKiemDinhMayMocService.Save();
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
