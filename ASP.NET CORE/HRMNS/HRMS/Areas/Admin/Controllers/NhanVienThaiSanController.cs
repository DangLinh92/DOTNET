using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var lst = _nhanVienThaiSanService.GetAll().FindAll(x=>x.HR_NHANVIEN.MaBoPhan == Department).OrderByDescending(x => x.DateModified).ToList();
                return View(lst);
            }
        }

        [HttpPost]
        public IActionResult ExportExcel(string maNV, string timeFrom, string timeTo)
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
            var data = _nhanVienThaiSanService.Search(maNV, timeFrom, timeTo);
            List<NhanVienThaiSan_ExportViewModel> lstTSan = new List<NhanVienThaiSan_ExportViewModel>();
            NhanVienThaiSan_ExportViewModel model;
            foreach (var item in data)
            {
                model = new NhanVienThaiSan_ExportViewModel()
                {
                    MaNV = item.MaNV,
                    NameNV = item.HR_NHANVIEN.TenNV,
                    CheDoThaiSan = item.CheDoThaiSan == "ConNho1H" ? "Con nhỏ - Về sớm 1H(Không trừ về sớm,Tính OT)" : (item.CheDoThaiSan == "ConNho" ? "Con nhỏ - Về đúng giờ(Không tính OT)" : "Thai sản"),
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
        public IActionResult Search(string maNV,string fromTime, string toTime)
        {
            var lst = _nhanVienThaiSanService.Search(maNV, fromTime, toTime);
            return PartialView("_gridThaiSanPartialView", lst);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ts = _nhanVienThaiSanService.GetById(id,x=>x.HR_NHANVIEN);
            return new OkObjectResult(ts);
        }

        [HttpPost]
        public IActionResult SaveThaiSan(NhanVienThaiSanViewModel model,[FromQuery]string action)
        {
            if(action == "Add")
            {
                var nv = _nhanVienThaiSanService.GetById(model.Id);
                if(nv != null)
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
    }
}
