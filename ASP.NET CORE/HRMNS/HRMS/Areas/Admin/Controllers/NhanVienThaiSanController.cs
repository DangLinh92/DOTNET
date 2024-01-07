using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class NhanVienThaiSanController : AdminBaseController
    {
        INhanVienThaiSanService _nhanVienThaiSanService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NhanVienThaiSanController(INhanVienThaiSanService nhanVienThaiSanService, IWebHostEnvironment hostingEnvironment)
        {
            _nhanVienThaiSanService = nhanVienThaiSanService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(Department))
            {
                var lst = _nhanVienThaiSanService.GetAll().OrderByDescending(x => x.DateModified).ToList();
                return View(lst);
            }
            else
            {
                var lst = _nhanVienThaiSanService.GetAll().FindAll(x => x.HR_NHANVIEN.MaBoPhan == Department).OrderByDescending(x => x.DateModified).ToList();
                return View(lst);
            }
        }

        [HttpPost]
        public IActionResult ExportExcel(string maNV, string timeFrom, string timeTo,string chedo)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"NhanVienThaiSan_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var data = _nhanVienThaiSanService.Search(maNV, timeFrom, timeTo,chedo);
            List<NhanVienThaiSan_ExportViewModel> lstTSan = new List<NhanVienThaiSan_ExportViewModel>();
            NhanVienThaiSan_ExportViewModel model;
            foreach (var item in data)
            {
                model = new NhanVienThaiSan_ExportViewModel()
                {
                    MaNV = item.MaNV,
                    BoPhan = item.HR_NHANVIEN.MaBoPhan,
                    NameNV = item.HR_NHANVIEN.TenNV,
                    CheDoThaiSan = item.CheDoThaiSan == "MangBau" ? "Mang Bầu" : (item.CheDoThaiSan == "ConNho1H" ? "Con nhỏ - Về sớm 1H(Không trừ về sớm,Tính OT)" : (item.CheDoThaiSan == "ConNho" ? "Con nhỏ - Về đúng giờ(Không tính OT)" : "Thai sản")),
                    FromDate = item.FromDate,
                    ToDate = item.ToDate
                };

                lstTSan.Add(model);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ChamCongDB");
                worksheet.Cells["A1"].LoadFromCollection(lstTSan, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public IActionResult Search(string maNV, string fromTime, string toTime,string chedo)
        {
            var lst = _nhanVienThaiSanService.Search(maNV, fromTime, toTime, chedo);
            return PartialView("_gridThaiSanPartialView", lst);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ts = _nhanVienThaiSanService.GetById(id, x => x.HR_NHANVIEN);
            return new OkObjectResult(ts);
        }

        [HttpPost]
        public IActionResult SaveThaiSan(NhanVienThaiSanViewModel model, [FromQuery] string action)
        {
            if (action == "Add")
            {
                var nv = _nhanVienThaiSanService.GetById(model.Id);
                if (nv != null)
                {
                    nv.MaNV = model.MaNV;
                    nv.CheDoThaiSan = model.CheDoThaiSan;
                    nv.FromDate = model.FromDate;
                    nv.ToDate = model.ToDate;

                    _nhanVienThaiSanService.Update(nv);
                }
                else
                {
                    _nhanVienThaiSanService.Add(model);
                }
            }
            else
            {
                var nv = _nhanVienThaiSanService.GetById(model.Id);
                if (nv != null)
                {
                    nv.MaNV = model.MaNV;
                    nv.CheDoThaiSan = model.CheDoThaiSan;
                    nv.FromDate = model.FromDate;
                    nv.ToDate = model.ToDate;

                    _nhanVienThaiSanService.Update(nv);
                }
            }

            _nhanVienThaiSanService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _nhanVienThaiSanService.Delete(id);
            _nhanVienThaiSanService.Save();
            return new OkObjectResult(id);
        }

        #region Hỗ Trợ Sinh Lý
        public IActionResult HoTroSinhLy()
        {
            return View();
        }

        [HttpGet]
        public object GetHoTroSinhLy(DataSourceLoadOptions loadOptions, string month)
        {
            string _month = DateTime.Parse(month).ToString("yyyy-MM-01");
            return DataSourceLoader.Load(_nhanVienThaiSanService.GetHoTroSinhLy(_month, Department), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            HoTroSinhLyViewModel model = new HoTroSinhLyViewModel();
            JsonConvert.PopulateObject(values, model);
            _nhanVienThaiSanService.AddHotrosinhly(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            HoTroSinhLyViewModel entity = new HoTroSinhLyViewModel();
            entity.Id = key;
            JsonConvert.PopulateObject(values, entity);
            _nhanVienThaiSanService.EditHotrosinhly(entity);
            return Ok();
        }

        [HttpDelete]
        public void DeleteHotroSinhLy(int key)
        {
            _nhanVienThaiSanService.DeleteHotrosinhly(key);
        }

        [HttpPost]
        public IActionResult ExportExcelSinhLy([FromQuery] string month)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Nhanvien_HTSinhLy_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            string _month = DateTime.Parse(month).ToString("yyyy-MM-01");
            List<HoTroSinhLyViewModel> HtroSinhLy = _nhanVienThaiSanService.GetHoTroSinhLyImport(_month, Department);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                var data = HtroSinhLy.Select(x => new
                {
                    x.MaNV,
                    x.TenNV,
                    x.BoPhan,
                    x.ThoiGianChuaNghi,
                    x.Month
                });
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("HoTroSinhLy");
                worksheet.Cells["A1"].LoadFromCollection(data, true, TableStyles.Light11);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        /// <summary>
        /// Import ho tro sinh lý
        /// </summary>
        /// <param name="files"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, [FromQuery] string param)
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
                ResultDB resultDB = _nhanVienThaiSanService.ImportExcel(filePath, param);

                if (resultDB.ReturnInt == 0)
                {
                    _nhanVienThaiSanService.Save();
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
        #endregion
    }
}
