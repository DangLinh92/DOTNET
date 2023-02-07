using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsKeHoachKhamSucKhoeController : AdminBaseController
    {
        private readonly IEhsKeHoachKhamSKService EhsKeHoachKhamSKService;
        private readonly IWebHostEnvironment HostingEnvironment;

        public EhsKeHoachKhamSucKhoeController(IEhsKeHoachKhamSKService ehsKeHoachKhamSKService, IWebHostEnvironment hostingEnvironment)
        {
            EhsKeHoachKhamSKService = ehsKeHoachKhamSKService;
            HostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object KhamSucKhoe(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = EhsKeHoachKhamSKService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var khoach = new EhsKeHoachKhamSKViewModel();
            JsonConvert.PopulateObject(values, khoach);

            EhsKeHoachKhamSKService.Add(khoach);
            EhsKeHoachKhamSKService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var kehoach = EhsKeHoachKhamSKService.GetById(key);
            JsonConvert.PopulateObject(values, kehoach);

            EhsKeHoachKhamSKService.Update(kehoach);
            EhsKeHoachKhamSKService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void Delete(Guid key)
        {
            EhsKeHoachKhamSKService.Delete(key);
            EhsKeHoachKhamSKService.Save();
        }

        [HttpGet]
        public object GetNgayKhamSK(DataSourceLoadOptions loadOptions, Guid key)
        {
            var kehoach = EhsKeHoachKhamSKService.GetById(key);
            return DataSourceLoader.Load(kehoach.EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK.ToList(), loadOptions);
        }

        // Ngày khám sức khỏe
        [HttpPost]
        public IActionResult InsertNgayKhamSK(string values, Guid maKHKhamSK)
        {
            var khoach = new EhsNgayThucHienChiTietKhamSKViewModel();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKHKhamSK = maKHKhamSK;
            khoach.NgayBatDau = khoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            khoach.NgayKetThuc = khoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachKhamSKService.AddNgayKhamSK(khoach);
            EhsKeHoachKhamSKService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateNgayKhamSK(int key, string values)
        {
            var kehoach = EhsKeHoachKhamSKService.GetNgayKhamSKById(key);
            JsonConvert.PopulateObject(values, kehoach);

            kehoach.NgayBatDau = kehoach.NgayBatDauEx.ToString("yyyy-MM-dd");
            kehoach.NgayKetThuc = kehoach.NgayKetThucEx.ToString("yyyy-MM-dd");

            EhsKeHoachKhamSKService.UpdateNgayKhamSK(kehoach);
            EhsKeHoachKhamSKService.Save();
            return Ok(kehoach);
        }

        [HttpDelete]
        public void DeleteNgayKhamSK(int key)
        {
            EhsKeHoachKhamSKService.DeleteNgayKhamSK(key);
            EhsKeHoachKhamSKService.Save();
        }

        // Nhan viên khám SK
        [HttpPost]
        public IActionResult InsertNhanVienKhamSK(string values, Guid maKHKhamSK)
        {
            var khoach = new EhsNhanVienKhamSucKhoe();
            JsonConvert.PopulateObject(values, khoach);

            khoach.MaKHKhamSK = maKHKhamSK;
            EhsKeHoachKhamSKService.AddNhanVienKhamSK(khoach);
            EhsKeHoachKhamSKService.Save();

            return Ok(khoach);
        }

        [HttpPut]
        public IActionResult UpdateNhanVienKhamSK(int key, string values)
        {
            var nv = EhsKeHoachKhamSKService.GetNhanVienKhamSKById(key);
            JsonConvert.PopulateObject(values, nv);

            EhsKeHoachKhamSKService.UpdateNhanVienKhamSK(nv);
            EhsKeHoachKhamSKService.Save();
            return Ok(nv);
        }

        [HttpDelete]
        public void DeleteNhanVienKhamSK(int key)
        {
            EhsKeHoachKhamSKService.DeleteNhanVienKhamSK(key);
            EhsKeHoachKhamSKService.Save();
        }

        [HttpPost]
        public IActionResult GetDSNhanVienByKeHoach(string maKHKhamSK)
        {
           var data = EhsKeHoachKhamSKService.GetNhanVienKhamSKByKeHoach(Guid.Parse(maKHKhamSK));
            return new OkObjectResult(data);
        }

        /// <summary>
        /// Import nhân viên khám sức khỏe theo nội dung khám sk
        /// </summary>
        /// <param name="files"></param>
        /// <param name="Id">Mã Kế hoach khám SK</param>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files,string Id)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = HostingEnvironment.WebRootPath + $@"\uploaded\excels";
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

                EhsKeHoachKhamSKService.ImportExcel(filePath,Id);
                EhsKeHoachKhamSKService.Save();

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
