using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Export;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    [IgnoreAntiforgeryToken]
    public class BangLuongChiTietController : AdminBaseController
    {
        private string DocumentName = "BangLuongChiTiet.xlsx";
        const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IDetailSalaryService _detailSalaryService;
        private INgayChotCongService _ngayChotCongService;
        private readonly IMemoryCache _memoryCache;
        public BangLuongChiTietController(IWebHostEnvironment hostEnvironment, IDetailSalaryService detailSalaryService, INgayChotCongService ngayChotCongService, IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostEnvironment;
            _detailSalaryService = detailSalaryService;
            _ngayChotCongService = ngayChotCongService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);

            string thangView = HttpContext.Session.Get<string>("BangLuongThang");
            string chotCongChoThang = _ngayChotCongService.FinLastItem().ChotCongChoThang;
            if (!string.IsNullOrEmpty(thangView))
            {
                chotCongChoThang = thangView;
            }
            else
            {
                chotCongChoThang = DateTime.Parse(chotCongChoThang).AddMonths(1).ToString("yyyy-MM-dd");
            }
            ViewBag.ThangChotCong = chotCongChoThang;

            string isInBL = HttpContext.Session.Get<string>("IsInBangLuongChiTiet");
            if (isInBL == CommonConstants.IN)
            {
                string fileName = CreatBangCongChiTiet(chotCongChoThang);
                HttpContext.Session.Set("TblLuongChiTiet", fileName);
                var sWebRootFolder = _hostingEnvironment.WebRootPath;
                string directory = Path.Combine(sWebRootFolder, "export-files/sr");
                ViewData["DocumentPath"] = Path.Combine(directory, fileName);
            }
            else
            {
                ViewData["DocumentPath"] = "";
                HttpContext.Session.Set("IsInBangLuongChiTiet", CommonConstants.IN);
            }
            SetSessionInpage(CommonConstants.IN);
            return View();
        }

        [HttpPost]
        public IActionResult ViewBangCongByMonth(string time)
        {
            HttpContext.Session.Set("IsInBangLuongChiTiet", CommonConstants.IN);
            //string fileName = CreatBangCongChiTiet(time);
            //HttpContext.Session.Set("TblLuongChiTiet", fileName);
            HttpContext.Session.Set("BangLuongThang", time);
            //var sWebRootFolder = _hostingEnvironment.WebRootPath;
            //string directory = Path.Combine(sWebRootFolder, "export-files/sr");
            //ViewData["DocumentPath"] = Path.Combine(directory, fileName);
            //ViewBag.ThangChotCong = time;
            //return RedirectToAction("Index");
            return new OkObjectResult(time);
        }

        [HttpPost]
        [HttpGet]
        public IActionResult DxDocumentRequest()
        {
            return SpreadsheetRequestProcessor.GetResponse(HttpContext);
        }

        public ActionResult Export(string fileName)
        {
            HttpContext.Session.Set("TblLuongChiTiet", fileName);
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files/sr");
            ViewData["DocumentPath"] = Path.Combine(directory, fileName);

            return View();
        }

        public IActionResult DownloadXlsx(SpreadsheetClientState spreadsheetState)
        {
            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);

            MemoryStream stream = new MemoryStream();
            spreadsheet.SaveCopy(stream, DocumentFormat.Xlsx);
            stream.Position = 0;
            DocumentName = HttpContext.Session.Get<string>("TblLuongChiTiet");
            return File(stream, XlsxContentType, DocumentName);
        }

        public IActionResult DownloadHtml(SpreadsheetClientState spreadsheetState)
        {
            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);

            HtmlDocumentExporterOptions options = new HtmlDocumentExporterOptions();
            options.CssPropertiesExportType = DevExpress.XtraSpreadsheet.Export.Html.CssPropertiesExportType.Style;
            options.Encoding = Encoding.UTF8;
            options.EmbedImages = true;
            options.SheetIndex = spreadsheet.Document.Worksheets.ActiveWorksheet.Index;

            MemoryStream stream = new MemoryStream();
            spreadsheet.Document.ExportToHtml(stream, options);
            stream.Position = 0;
            DocumentName = HttpContext.Session.Get<string>("TblLuongChiTiet");
            string fileHtml = DocumentName.Substring(0, DocumentName.LastIndexOf('.')) + ".html";
            return File(stream, "text/html", fileHtml);
        }

        [HttpPost]
        public IActionResult ChotBangLuong(string thang)
        {
            string time = thang + "-01";

            if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM") + "-01").AddMonths(-1).ToString("yyyy-MM-dd") == time)
            {
                //List<HR_SALARY> lstluongcoban = new List<HR_SALARY>();
                //_memoryCache.TryGetValue("BasicSalaryData", out lstluongcoban);
                List<BangLuongChiTietViewModel> data = _detailSalaryService.GetBangLuongChiTiet(time);

                if (data.Count > 0)
                {
                    _detailSalaryService.ChotBangLuong(time, data);
                }

                HR_NgayChotCongViewModel ngayChot = new HR_NgayChotCongViewModel()
                {
                    NgayChotCong = DateTime.Now.ToString("yyyy-MM-dd"),
                    ChotCongChoThang = time
                };

                _ngayChotCongService.Update(ngayChot);

                return new OkObjectResult(thang);
            }

            return new BadRequestObjectResult("Tháng được chốt công không phù hợp!");
        }

        public string CreatBangCongChiTiet(string time)
        {
            //List<HR_SALARY> lstluongcoban = new List<HR_SALARY>();
            //_memoryCache.TryGetValue("BasicSalaryData", out lstluongcoban);
            List<BangLuongChiTietViewModel> data = new List<BangLuongChiTietViewModel>();
            string chotCongChoThang = _ngayChotCongService.FinLastItem().ChotCongChoThang;
            if (chotCongChoThang.CompareTo(time) < 0)
            {
                data = _detailSalaryService.GetBangLuongChiTiet(time);
            }
            else
            {
                data = _detailSalaryService.GetHistoryBangLuongChiTiet(time);
            }

            if (data == null)
            {
                data = new List<BangLuongChiTietViewModel>();
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files/sr");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"BangLuongChiTiet_T" + time + $"_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/sr/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "Salary_Detail.xlsx"));
            if (file.Exists)
            {
                file.Delete();
            }

            if (fileSrc.Exists)
            {
                fileSrc.CopyTo(file.FullName, true);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                ExcelWorksheet worksheetTH = package.Workbook.Worksheets[2];
                ExcelWorksheet worksheetPivot = package.Workbook.Worksheets[3];

                if (time.Contains("-"))
                {
                    worksheetTH.Cells["B2"].Value = "BẢNG LƯƠNG THÁNG " + time.Split("-")[1] + "." + time.Split("-")[0] + " (Người lao động Việt Nam)";
                    worksheetTH.Cells["B3"].Value = time.Split("-")[1] + "." + time.Split("-")[0] + " 월 급여표 (현지 근로자) _ 지급";
                    worksheetPivot.Cells["B1"].Value = "BẢNG TỔNG HỢP TIỀN LƯƠNG THÁNG " + time.Split("-")[1] + " NĂM " + time.Split("-")[0];
                }

                int beginIndex = 3;
                string cellFrom = "";
                string cellTo = "";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells["A" + beginIndex].Value = i + 1;
                    worksheet.Cells["B" + beginIndex].Value = data[i].MaNV;

                    worksheet.Cells["C" + beginIndex].Value = data[i].DoiTuongPhuCapDocHai;
                    worksheet.Cells["D" + beginIndex].Value = data[i].MaBoPhan;
                    worksheet.Cells["E" + beginIndex].Value = data[i].Grade;
                    worksheet.Cells["F" + beginIndex].Value = data[i].TenNV;
                    worksheet.Cells["G" + beginIndex].Value = data[i].BoPhan;
                    worksheet.Cells["H" + beginIndex].Value = data[i].ChucVu;
                    worksheet.Cells["I" + beginIndex].Value = data[i].NgayVao;
                    worksheet.Cells["J" + beginIndex].Value = data[i].BasicSalary;
                    worksheet.Cells["K" + beginIndex].Value = data[i].LivingAllowance;
                    worksheet.Cells["L" + beginIndex].Value = data[i].PositionAllowance;
                    worksheet.Cells["M" + beginIndex].Value = data[i].AbilityAllowance;
                    worksheet.Cells["O" + beginIndex].Value = data[i].HarmfulAllowance;
                    worksheet.Cells["R" + beginIndex].Value = data[i].TongNgayCongThucTe;
                    worksheet.Cells["S" + beginIndex].Value = data[i].NgayCongThuViecBanNgay;
                    worksheet.Cells["T" + beginIndex].Value = data[i].NgayCongThuViecBanDem;
                    worksheet.Cells["W" + beginIndex].Value = data[i].NgayCongChinhThucBanNgay;
                    worksheet.Cells["X" + beginIndex].Value = data[i].NgayCongChinhThucBanDem;
                    worksheet.Cells["Y" + beginIndex].Value = data[i].NghiViecCoLuong;

                    worksheet.Cells["AC" + beginIndex].Value = data[i].GioLamThemTrongTV_150;
                    worksheet.Cells["AD" + beginIndex].Value = data[i].GioLamThemTrongTV_200;
                    worksheet.Cells["AE" + beginIndex].Value = data[i].GioLamThemTrongTV_210;
                    worksheet.Cells["AF" + beginIndex].Value = data[i].GioLamThemTrongTV_270;
                    worksheet.Cells["AG" + beginIndex].Value = data[i].GioLamThemTrongTV_300;
                    worksheet.Cells["AH" + beginIndex].Value = data[i].GioLamThemTrongTV_390;

                    worksheet.Cells["AI" + beginIndex].Value = data[i].GioLamThemTrongCT_150;
                    worksheet.Cells["AJ" + beginIndex].Value = data[i].GioLamThemTrongCT_200;
                    worksheet.Cells["AK" + beginIndex].Value = data[i].GioLamThemTrongCT_210;
                    worksheet.Cells["AL" + beginIndex].Value = data[i].GioLamThemTrongCT_270;
                    worksheet.Cells["AM" + beginIndex].Value = data[i].GioLamThemTrongCT_300;
                    worksheet.Cells["AN" + beginIndex].Value = data[i].GioLamThemTrongCT_390;

                    worksheet.Cells["AQ" + beginIndex].Value = data[i].SoNgayLamCaDemTruocLe_TV;
                    worksheet.Cells["AR" + beginIndex].Value = data[i].SoNgayLamCaDemTruocLe_CT;
                    worksheet.Cells["AT" + beginIndex].Value = data[i].SoNgayLamCaDem_TV;
                    worksheet.Cells["AU" + beginIndex].Value = data[i].SoNgayLamCaDem_CT;

                    worksheet.Cells["AW" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_150;
                    worksheet.Cells["AX" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_200_NT;
                    worksheet.Cells["AY" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_200_CN;
                    worksheet.Cells["AZ" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_270;
                    worksheet.Cells["BA" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_300;
                    worksheet.Cells["BB" + beginIndex].Value = data[i].HoTroThoiGianLamViecTV_390;

                    worksheet.Cells["BC" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_150;
                    worksheet.Cells["BD" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_200_NT;
                    worksheet.Cells["BE" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_200_CN;
                    worksheet.Cells["BF" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_270;
                    worksheet.Cells["BG" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_300;
                    worksheet.Cells["BH" + beginIndex].Value = data[i].HoTroThoiGianLamViecCT_390;

                    worksheet.Cells["BK" + beginIndex].Value = data[i].HoTroNgayThanhLapCty_CaNgayTV;
                    worksheet.Cells["BL" + beginIndex].Value = data[i].HoTroNgayThanhLapCty_CaNgayCT;
                    worksheet.Cells["BM" + beginIndex].Value = data[i].HoTroNgayThanhLapCty_CaDemTV_TruocLe;
                    worksheet.Cells["BN" + beginIndex].Value = data[i].HoTroNgayThanhLapCty_CaDemCT_TruocLe;

                    worksheet.Cells["BP" + beginIndex].Value = data[i].NghiKhamThai;
                    worksheet.Cells["BQ" + beginIndex].Value = data[i].NghiViecKhongThongBao;
                    worksheet.Cells["BR" + beginIndex].Value = data[i].SoNgayNghiBu_AL30;
                    worksheet.Cells["BS" + beginIndex].Value = data[i].SoNgayNghiBu_NB;

                    worksheet.Cells["CA" + beginIndex].Value = data[i].HoTroPCCC_CoSo;
                    worksheet.Cells["CB" + beginIndex].Value = data[i].HoTroAT_SinhVien;

                    worksheet.Cells["CD" + beginIndex].Value = data[i].TV_NghiKhongLuong;
                    worksheet.Cells["CE" + beginIndex].Value = data[i].NghiKhongLuong;
                    worksheet.Cells["CF" + beginIndex].Value = data[i].Probation_Late_Come_Early_Leave_Time;
                    worksheet.Cells["CG" + beginIndex].Value = data[i].Official_Late_Come_Early_Leave_Time;

                    worksheet.Cells["CK" + beginIndex].Value = data[i].ThuocDoiTuong_BHXH;
                    worksheet.Cells["CN" + beginIndex].Value = data[i].TruQuyPhongChongThienTai;
                    worksheet.Cells["CP" + beginIndex].Value = data[i].Thuong;
                    worksheet.Cells["CS" + beginIndex].Value = data[i].SoNguoiPhuThuoc;
                    worksheet.Cells["CX" + beginIndex].Value = data[i].NgayVao;
                    worksheet.Cells["CY" + beginIndex].Value = data[i].Note;
                    worksheet.Cells["CZ" + beginIndex].Value = data[i].InsentiveStandard;
                    worksheet.Cells["DA" + beginIndex].Value = data[i].DanhGia;
                    worksheet.Cells["DD" + beginIndex].Value = data[i].HoTroCongDoan;
                    worksheet.Cells["DE" + beginIndex].Value = data[i].SoTK;
                    worksheet.Cells["DF" + beginIndex].Value = data[i].DoiTuongThamGiaCD;
                    worksheet.Cells["DR" + beginIndex].Value = data[i].DoiTuongTruyThuBHYT;
                    worksheet.Cells["DT" + beginIndex].Value = data[i].SoConNho;
                    worksheet.Cells["DU" + beginIndex].Value = data[i].SoNgayNghi70;
                    worksheet.Cells["DW" + beginIndex].Value = data[i].NgayNghiViec;
                    worksheet.Cells["DX" + beginIndex].Value = data[i].DieuChinhCong_Total;

                    worksheet.Cells["EA" + beginIndex].Value = data[i].TraTienPhepNam_Total;
                    worksheet.Cells["ED" + beginIndex].Value = data[i].TT_Tien_GioiThieu;

                    if (i < data.Count - 1)
                    {
                        cellFrom = "A" + beginIndex + ":ED" + beginIndex;
                        cellTo = "A" + (beginIndex + 1) + ":ED" + (beginIndex + 1);
                        worksheet.Cells[cellFrom].Copy(worksheet.Cells[cellTo]);
                    }
                    beginIndex += 1;
                }

                beginIndex = 6;
                worksheetTH.InsertRow(beginIndex + 1, data.Count, beginIndex);
                for (int i = 0; i < data.Count; i++)
                {
                    // SHEET TH
                    worksheetTH.Cells["B" + beginIndex].Value = data[i].MaNV;

                    if (i < data.Count - 1)
                    {
                        worksheetTH.Cells["A" + beginIndex + ":AO" + beginIndex].Copy(worksheetTH.Cells["A" + (beginIndex + 1) + ":AO" + (beginIndex + 1)]);
                    }
                    beginIndex += 1;
                }

                package.Save(); // Save the workbook.
            }
            return sFileName;
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

                List<HR_SALARY> lstUpdate = new List<HR_SALARY>();
                ResultDB rs = _detailSalaryService.ImportExcel(filePath, out lstUpdate);

                if (rs.ReturnInt != 0)
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(filePath);
                    }
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                _memoryCache.Remove("BasicSalaryData");
                _memoryCache.Set("BasicSalaryData", lstUpdate);

                return new OkObjectResult(filePath);
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
