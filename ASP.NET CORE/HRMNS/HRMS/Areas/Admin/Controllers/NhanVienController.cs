using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class NhanVienController : AdminBaseController
    {
        INhanVienService _nhanvienService;
        IBoPhanService _boPhanService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NhanVienController(INhanVienService nhanVienService, IBoPhanService boPhanService, IWebHostEnvironment hostingEnvironment)
        {
            _nhanvienService = nhanVienService;
            _boPhanService = boPhanService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return View(nhanviens);
        }

        [HttpGet]
        public IActionResult OnGetPartialData()
        {
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return PartialView("_NhanVienGridPartial", nhanviens);
        }

        [HttpPost]
        public IActionResult SaveEmployee(NhanVienCustomizeViewModel nhanvienVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                bool isAdd = nhanvienVm.Action == CommonConstants.Add_Action;
                bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == nhanvienVm.NhanVien.Id);
                //bool notExist = nv == null;

                if (isAdd && notExist)
                {
                    _nhanvienService.Add(nhanvienVm.NhanVien);
                }
                else if (isAdd && !notExist)
                {
                    return new ConflictObjectResult(CommonConstants.ConflictObjectResult_Msg);
                }
                else if (!isAdd && notExist)
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }
                else if (!isAdd && !notExist)
                {
                    NhanVienViewModel nhanVien = _nhanvienService.GetById(nhanvienVm.NhanVien.Id);
                    nhanVien.TenNV = nhanvienVm.NhanVien.TenNV;
                    nhanVien.GioiTinh = nhanvienVm.NhanVien.GioiTinh;
                    nhanVien.Email = nhanvienVm.NhanVien.Email;
                    nhanVien.SoDienThoai = nhanvienVm.NhanVien.SoDienThoai;
                    nhanVien.NgayVao = nhanvienVm.NhanVien.NgayVao;
                    nhanVien.MaBoPhan = nhanvienVm.NhanVien.MaBoPhan;
                    nhanVien.MaChucDanh = nhanvienVm.NhanVien.MaChucDanh;

                    _nhanvienService.UpdateSingle(nhanVien);
                }

                _nhanvienService.Save();

                return new OkObjectResult(nhanvienVm.NhanVien);
            }
        }

        [HttpPost]
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
                _nhanvienService.ImportExcel(filePath);
                _nhanvienService.Save();

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
        public IActionResult ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Nhanvien_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var nhanviens = _nhanvienService.GetAll().Where(x => x.Status.NullString() != Status.InActive.ToString()).Select(x => new
            {
                x.Id,
                x.TenNV,
                x.MaBoPhan,
                x.MaChucDanh,
                x.GioiTinh,
                x.NgaySinh,
                x.NoiSinh,
                x.TinhTrangHonNhan,
                x.DanToc,
                x.TonGiao,
                x.DiaChiThuongTru,
                x.SoDienThoai,
                x.SoDienThoaiNguoiThan,
                x.Email,
                x.CMTND,
                x.SoTaiKhoanNH,
                x.TenNganHang,
                x.TruongDaoTao,
                x.NgayVao,
                x.NguyenQuan,
                x.DChiHienTai,
                x.MaBHXH,
                x.MaSoThue,
                x.Status
            });
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                worksheet.Cells["A1"].LoadFromCollection(nhanviens, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == Id);
            if (notExist)
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
            _nhanvienService.Delete(Id);
            _nhanvienService.Save();
            return new OkObjectResult(null);
        }

        [HttpGet]
        public IActionResult GetById(string Id)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            return new OkObjectResult(nhanVien);
        }

        [HttpGet]
        public IActionResult Profile(string Id)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            if (nhanVien != null)
            {
                NhanVienProfileModel profileModel = new NhanVienProfileModel();
                profileModel.MaNhanVien = nhanVien.Id;
                profileModel.Avartar = nhanVien.Image;
                return View(profileModel);
            }
            else
            {
                return RedirectToAction("Index", "Error", new { id = CommonConstants.NotFound, Area = "Admin" });
            }
        }

        [HttpPost]
        public IActionResult SaveAvatar(string Id, string url)
        {
            NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            nhanVien.Image = url;
            _nhanvienService.UpdateSingle(nhanVien);
            _nhanvienService.Save();

            return new OkObjectResult(null);
        }
    }
}
