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
    public class EhsKeHoachPCCCController : AdminBaseController
    {
        private readonly IEhsKeHoachPCCCService EhsKeHoachPCCCService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EhsKeHoachPCCCController(IEhsKeHoachPCCCService ehsKeHoachPCCCService, IWebHostEnvironment hostingEnvironment)
        {
            EhsKeHoachPCCCService = ehsKeHoachPCCCService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object KeHoachPCCC(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = EhsKeHoachPCCCService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new Ehs_KeHoach_PCCCViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach = EhsKeHoachPCCCService.Add(khoach);
            EhsKeHoachPCCCService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var kehoach = EhsKeHoachPCCCService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsKeHoachPCCCService.Update(kehoach);
            EhsKeHoachPCCCService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(Guid key)
        {
            EhsKeHoachPCCCService.Delete(key);
            EhsKeHoachPCCCService.Save();
        }

        [HttpGet]
        public object GetThucHienPCCC(DataSourceLoadOptions loadOptions, Guid key)
        {
            var kehoach = EhsKeHoachPCCCService.GetById(key);

            if(kehoach == null)
            {
                return DataSourceLoader.Load(new List<EhsThoiGianThucHienPCCCViewModel>(), loadOptions);
            }

            return DataSourceLoader.Load(kehoach.EHS_THOIGIAN_THUC_HIEN_PCCC.ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult AddThoiGianPCCC(string values, Guid maKH)
        {
            var khoach = new EhsThoiGianThucHienPCCCViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKH_PCCC = maKH;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            khoach = EhsKeHoachPCCCService.AddThoiGianPCCC(khoach);
            EhsKeHoachPCCCService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateThoiGianPCCC(int key, string values)
        {
            var kehoach = EhsKeHoachPCCCService.GetThoiGianPCCCById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachPCCCService.UpdateThoiGianPCCC(kehoach);
            EhsKeHoachPCCCService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteThoiGianPCCC(int key)
        {
            EhsKeHoachPCCCService.DeleteThoiGianPCCC(key);
            EhsKeHoachPCCCService.Save();
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

                string err = EhsKeHoachPCCCService.ImportExcel(filePath);
                if (err == "")
                    EhsKeHoachPCCCService.Save();
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
