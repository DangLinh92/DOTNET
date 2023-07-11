using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
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

namespace HRMS.Areas.Admin.Controllers
{
    public class DCChamCongController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        IDM_DCChamCongService _dmDieuChinhService;
        INhanVienService _nhanVienService;
        IDCChamCongService _dcChamCongService;

        public DCChamCongController(INhanVienService nhanVienService, IDM_DCChamCongService dmDCChamCongservice, IDCChamCongService dcChamCongService, IWebHostEnvironment webHost)
        {
            _nhanVienService = nhanVienService;
            _hostingEnvironment = webHost;
            _dmDieuChinhService = dmDCChamCongservice;
            _dcChamCongService = dcChamCongService;
        }

        [HttpGet]
        public IActionResult GetDanhMucDieuChinh()
        {
            var lst = _dmDieuChinhService.GetAll("");
            return new OkObjectResult(lst);
        }

        [HttpPost]
        public IActionResult SaveDanhMucDieuChinh(DMDieuChinhChamCongViewModel vm)
        {
            var entity = _dmDieuChinhService.GetById(vm.Id);
            if (entity == null)
            {
                _dmDieuChinhService.Add(vm);
            }
            else
            {
                entity.TieuDe = vm.TieuDe;
                _dmDieuChinhService.Update(entity);
            }
            _dmDieuChinhService.Save();
            return new OkObjectResult(vm);
        }

        [HttpPost]
        public IActionResult SaveDieuChinhChamCong(DCChamCongViewModel vm)
        {
            var entity = _dcChamCongService.GetById(vm.Id);

            if (entity == null)
            {
                _dcChamCongService.Add(vm);
            }
            else
            {
                entity.CopyPropertiesFrom(vm,
                    new List<string>()
                    {
                        nameof(vm.Id),nameof(vm.MaNV),
                        nameof(vm.DateCreated), nameof(vm.DateModified),
                        nameof(vm.UserCreated), nameof(vm.UserModified),
                    });
                _dcChamCongService.Update(entity);
            }
            _dcChamCongService.Save();
            return new OkObjectResult(vm);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var entity = _dcChamCongService.GetById(id);
            return new OkObjectResult(entity);
        }

        [HttpPost]
        public IActionResult Search(string department, string status, string timeFrom, string timeTo)
        {
            var lst = _dcChamCongService.Search(status, department, timeFrom, timeTo, x => x.HR_NHANVIEN);
            return PartialView("_gridDCChamCongPartialView", lst);
        }

        [HttpGet]
        public object Employees(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_nhanVienService.GetAll().Where(x => x.Status == "Active" && x.MaBoPhan != "KOREA").Select(x => new { Id = x.Id, TenNV = x.Id + "-" + x.TenNV }), loadOptions);
        }

        public IActionResult Index()
        {
            var lst = _dcChamCongService.GetAll("", y => y.HR_NHANVIEN);
            return View(lst);
        }

        #region POS PUT DELETE
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions, string month)
        {
            string _month = DateTime.Parse(month).ToString("yyyy-MM");
            return DataSourceLoader.Load(_dcChamCongService.GetAll("", y => y.HR_NHANVIEN,x =>x.HR_NHANVIEN.HR_BO_PHAN_DETAIL).Where(x => x.ChiTraVaoLuongThang2.NullString().Contains(_month)), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            DCChamCongViewModel model = new DCChamCongViewModel();
            JsonConvert.PopulateObject(values, model);

            _dcChamCongService.Add(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var entity = _dcChamCongService.GetById(key);

            JsonConvert.PopulateObject(values, entity);
            _dcChamCongService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _dcChamCongService.Delete(key);
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

                ResultDB rs = _dcChamCongService.ImportExcel(filePath);
                if (rs.ReturnInt != 0)
                    return new NotFoundObjectResult(rs.ReturnString);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }
                _dcChamCongService.Save();
                return new OkObjectResult(filePath);
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
        #endregion
    }
}
