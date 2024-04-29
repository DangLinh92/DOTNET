using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Export;
using GroupDocs.Viewer.Options;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Extensions;
using HRMS.Infrastructure.Interfaces;
using HRMS.ScheduledTasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DevExpress.DataAccess.DataFederation;

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
        private ISalaryService _salaryService;
        private readonly IMemoryCache _memoryCache;
        public BangLuongChiTietController(IWebHostEnvironment hostEnvironment, IDetailSalaryService detailSalaryService, INgayChotCongService ngayChotCongService, ISalaryService salaryService, IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostEnvironment;
            _detailSalaryService = detailSalaryService;
            _ngayChotCongService = ngayChotCongService;
            _memoryCache = memoryCache;
            _salaryService = salaryService;
        }

        public async Task<IActionResult> Index()
        {
            DeleteFileSr(_hostingEnvironment);

            string thangView = HttpContext.Session.Get<string>("BangLuongThang");
            string chedo = HttpContext.Session.Get<string>("CheDoView");
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
            ViewBag.CheDoView = chedo;

            // Phép Năm update
            
            string isInBL = HttpContext.Session.Get<string>("IsInBangLuongChiTiet");
            if (isInBL == CommonConstants.IN)
            {
                string fileName =  CreatBangCongChiTiet(chotCongChoThang, chedo);
                HttpContext.Session.Set("TblLuongChiTiet", fileName);
                var sWebRootFolder = _hostingEnvironment.WebRootPath;
                string directory = Path.Combine(sWebRootFolder, "export-files/sr");
                ViewData["DocumentPath"] = Path.Combine(directory, fileName);
            }
            else
            {
                await ChotCongFinal();
                ViewData["DocumentPath"] = "";
                HttpContext.Session.Set("IsInBangLuongChiTiet", CommonConstants.IN);
            }
            SetSessionInpage(CommonConstants.IN);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewBangCongByMonth(string time, string chedo)
        {
            // wait 30s để update phep nam
            await Task.Delay(TimeSpan.FromSeconds(15));

            HttpContext.Session.Set("IsInBangLuongChiTiet", CommonConstants.IN);
            //string fileName = CreatBangCongChiTiet(time);
            //HttpContext.Session.Set("TblLuongChiTiet", fileName);
            HttpContext.Session.Set("BangLuongThang", time);
            HttpContext.Session.Set("CheDoView", chedo);
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

            List<BangLuongChiTietViewModel> data = _detailSalaryService.GetBangLuongChiTiet(time, "");

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
            HttpContext.Session.Set("BangLuongThang", "");

            return new OkObjectResult(thang);
        }

        [HttpPost]
        public IActionResult XacNhanThanhToanNghiViec(string chedo)
        {
            if (chedo == "NghiViec")
            {
                List<string> lstData = HttpContext.Session.Get<List<string>>("Data_XacNhanChiTra");

                if (lstData.Count > 0)
                {
                    _detailSalaryService.XacNhanChiTra(lstData);
                    return new OkObjectResult(lstData);
                }
                else
                {
                    return new NotFoundObjectResult("Data Not Found");
                }
            }
            else
            {
                return new NotFoundObjectResult("Only : NghiViec");
            }
        }

        public string CreatBangCongChiTiet(string time, string chedo)
        {
            List<BangLuongChiTietViewModel> data = new List<BangLuongChiTietViewModel>();
            string chotCongChoThang = _ngayChotCongService.FinLastItem().ChotCongChoThang;
            if (chotCongChoThang.CompareTo(time) < 0)
            {
                data = _detailSalaryService.GetBangLuongChiTiet(time, chedo);

                if (chedo == "NghiViec")
                {
                    HttpContext.Session.Set("Data_XacNhanChiTra", data.Select(x => x.MaNV));
                }
                else
                {
                    HttpContext.Session.Set("Data_XacNhanChiTra", new List<string>());
                }
            }
            else
            {
                data = _detailSalaryService.GetHistoryBangLuongChiTiet(time, chedo);
            }

            if (data == null)
            {
                data = new List<BangLuongChiTietViewModel>();
            }

            // làm việc < 5 day thì k tính lương
            foreach (var item in data.ToList())
            {
                if (item.NgayNghiViec.NullString() != "")
                {
                    if (EachDay.GetWorkingDay(DateTime.Parse(item.NgayVao), DateTime.Parse(item.NgayNghiViec.NullString()).AddDays(-1)) - item.NghiKhongLuong < 5)
                    {
                        data.Remove(item);
                    }
                }
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                ExcelWorksheet worksheetTH = package.Workbook.Worksheets[1];
                ExcelWorksheet worksheetPivot = package.Workbook.Worksheets[2];

                if (time.Contains("-"))
                {
                    worksheetTH.Cells["B2"].Value = "BẢNG LƯƠNG THÁNG " + time.Split("-")[1] + "." + time.Split("-")[0] + " (Người lao động Việt Nam)";
                    worksheetTH.Cells["B3"].Value = time.Split("-")[1] + "." + time.Split("-")[0] + " 월 급여표 (현지 근로자) _ 지급";
                    worksheetPivot.Cells["B1"].Value = "BẢNG TỔNG HỢP TIỀN LƯƠNG THÁNG " + time.Split("-")[1] + " NĂM " + time.Split("-")[0];
                }

                int beginIndex = 3;
                string cellFrom = "";
                string cellTo = "";
                worksheet.Cells["P1"].Value = DateTime.Parse(time + "-01").ToString("yyyy-MM-dd");
                worksheet.Cells["DG1"].Value = DateTime.Parse(time + "-01").ToString("yyyy-MM-dd");
                worksheet.Cells["Q1"].Value = DateTime.Parse(time + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

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

                    //worksheet.Cells["CD" + beginIndex].Value = data[i].TV_NghiKhongLuong;
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
                    worksheet.Cells["DU" + beginIndex].Value = data[i].SoNgayNghi70;// L160
                    worksheet.Cells["DW" + beginIndex].Value = data[i].NgayNghiViec;
                    worksheet.Cells["DX" + beginIndex].Value = data[i].DieuChinhCong_Total;

                    worksheet.Cells["EA" + beginIndex].Value = data[i].TraTienPhepNam_Total;
                    worksheet.Cells["ED" + beginIndex].Value = data[i].TT_Tien_GioiThieu;
                    worksheet.Cells["EE" + beginIndex].Value = data[i].GioiTinh;
                    worksheet.Cells["EF" + beginIndex].Value = data[i].BauThaiSan;
                    worksheet.Cells["EG" + beginIndex].Value = data[i].ThoiGianChuaNghi;

                    if (i < data.Count - 1)
                    {
                        cellFrom = "A" + beginIndex + ":EH" + beginIndex;
                        cellTo = "A" + (beginIndex + 1) + ":EH" + (beginIndex + 1);
                        worksheet.Cells[cellFrom].Copy(worksheet.Cells[cellTo]);
                    }
                    beginIndex += 1;
                }

                worksheet.Cells["J" + beginIndex].Formula = string.Format("=SUM(J3:J{0})", beginIndex - 1);
                worksheet.Cells["K" + beginIndex].Formula = string.Format("=SUM(K3:K{0})", beginIndex - 1);
                worksheet.Cells["L" + beginIndex].Formula = string.Format("=SUM(L3:L{0})", beginIndex - 1);
                worksheet.Cells["M" + beginIndex].Formula = string.Format("=SUM(M3:M{0})", beginIndex - 1);
                worksheet.Cells["O" + beginIndex].Formula = string.Format("=SUM(O3:O{0})", beginIndex - 1);
                worksheet.Cells["R" + beginIndex].Formula = string.Format("=SUM(R3:R{0})", beginIndex - 1);
                worksheet.Cells["S" + beginIndex].Formula = string.Format("=SUM(S3:S{0})", beginIndex - 1);
                worksheet.Cells["T" + beginIndex].Formula = string.Format("=SUM(T3:T{0})", beginIndex - 1);
                worksheet.Cells["W" + beginIndex].Formula = string.Format("=SUM(W3:W{0})", beginIndex - 1);
                worksheet.Cells["X" + beginIndex].Formula = string.Format("=SUM(X3:X{0})", beginIndex - 1);
                worksheet.Cells["Y" + beginIndex].Formula = string.Format("=SUM(Y3:Y{0})", beginIndex - 1);

                worksheet.Cells["AC" + beginIndex].Formula = string.Format("=SUM(AC3:AC{0})", beginIndex - 1);
                worksheet.Cells["AD" + beginIndex].Formula = string.Format("=SUM(AD3:AD{0})", beginIndex - 1);
                worksheet.Cells["AE" + beginIndex].Formula = string.Format("=SUM(AE3:AE{0})", beginIndex - 1);
                worksheet.Cells["AF" + beginIndex].Formula = string.Format("=SUM(AF3:AF{0})", beginIndex - 1);
                worksheet.Cells["AG" + beginIndex].Formula = string.Format("=SUM(AG3:AG{0})", beginIndex - 1);
                worksheet.Cells["AH" + beginIndex].Formula = string.Format("=SUM(AH3:AH{0})", beginIndex - 1);
                worksheet.Cells["AI" + beginIndex].Formula = string.Format("=SUM(AI3:AI{0})", beginIndex - 1);
                worksheet.Cells["AJ" + beginIndex].Formula = string.Format("=SUM(AJ3:AJ{0})", beginIndex - 1);

                worksheet.Cells["AK" + beginIndex].Formula = string.Format("=SUM(AK3:AK{0})", beginIndex - 1);
                worksheet.Cells["AL" + beginIndex].Formula = string.Format("=SUM(AL3:AL{0})", beginIndex - 1);
                worksheet.Cells["AM" + beginIndex].Formula = string.Format("=SUM(AM3:AM{0})", beginIndex - 1);
                worksheet.Cells["AN" + beginIndex].Formula = string.Format("=SUM(AN3:AN{0})", beginIndex - 1);
                worksheet.Cells["AQ" + beginIndex].Formula = string.Format("=SUM(AQ3:AQ{0})", beginIndex - 1);
                worksheet.Cells["AR" + beginIndex].Formula = string.Format("=SUM(AR3:AR{0})", beginIndex - 1);
                worksheet.Cells["AT" + beginIndex].Formula = string.Format("=SUM(AT3:AT{0})", beginIndex - 1);
                worksheet.Cells["AU" + beginIndex].Formula = string.Format("=SUM(AU3:AU{0})", beginIndex - 1);
                worksheet.Cells["AW" + beginIndex].Formula = string.Format("=SUM(AW3:AW{0})", beginIndex - 1);
                worksheet.Cells["AX" + beginIndex].Formula = string.Format("=SUM(AX3:AX{0})", beginIndex - 1);
                worksheet.Cells["AY" + beginIndex].Formula = string.Format("=SUM(AY3:AY{0})", beginIndex - 1);
                worksheet.Cells["AZ" + beginIndex].Formula = string.Format("=SUM(AZ3:AZ{0})", beginIndex - 1);
                worksheet.Cells["BA" + beginIndex].Formula = string.Format("=SUM(BA3:BA{0})", beginIndex - 1);
                worksheet.Cells["BB" + beginIndex].Formula = string.Format("=SUM(BB3:BB{0})", beginIndex - 1);
                worksheet.Cells["BC" + beginIndex].Formula = string.Format("=SUM(BC3:BC{0})", beginIndex - 1);
                worksheet.Cells["BD" + beginIndex].Formula = string.Format("=SUM(BD3:BD{0})", beginIndex - 1);
                worksheet.Cells["BE" + beginIndex].Formula = string.Format("=SUM(BE3:BE{0})", beginIndex - 1);
                worksheet.Cells["BF" + beginIndex].Formula = string.Format("=SUM(BF3:BF{0})", beginIndex - 1);
                worksheet.Cells["BG" + beginIndex].Formula = string.Format("=SUM(BG3:BG{0})", beginIndex - 1);
                worksheet.Cells["BH" + beginIndex].Formula = string.Format("=SUM(BH3:BH{0})", beginIndex - 1);
                worksheet.Cells["BK" + beginIndex].Formula = string.Format("=SUM(BK3:BK{0})", beginIndex - 1);
                worksheet.Cells["BL" + beginIndex].Formula = string.Format("=SUM(BL3:BL{0})", beginIndex - 1);
                worksheet.Cells["BM" + beginIndex].Formula = string.Format("=SUM(BM3:BM{0})", beginIndex - 1);

                worksheet.Cells["BN" + beginIndex].Formula = string.Format("=SUM(BN3:BN{0})", beginIndex - 1);
                worksheet.Cells["BP" + beginIndex].Formula = string.Format("=SUM(BP3:BP{0})", beginIndex - 1);
                worksheet.Cells["BQ" + beginIndex].Formula = string.Format("=SUM(BQ3:BQ{0})", beginIndex - 1);
                worksheet.Cells["BR" + beginIndex].Formula = string.Format("=SUM(BR3:BR{0})", beginIndex - 1);
                worksheet.Cells["BS" + beginIndex].Formula = string.Format("=SUM(BS3:BS{0})", beginIndex - 1);
                worksheet.Cells["CA" + beginIndex].Formula = string.Format("=SUM(CA3:CA{0})", beginIndex - 1);
                worksheet.Cells["CB" + beginIndex].Formula = string.Format("=SUM(CB3:CB{0})", beginIndex - 1);
                worksheet.Cells["CE" + beginIndex].Formula = string.Format("=SUM(CE3:CE{0})", beginIndex - 1);
                worksheet.Cells["CF" + beginIndex].Formula = string.Format("=SUM(CF3:CF{0})", beginIndex - 1);
                worksheet.Cells["CG" + beginIndex].Formula = string.Format("=SUM(CG3:CG{0})", beginIndex - 1);
                worksheet.Cells["CK" + beginIndex].Formula = string.Format("=SUM(CK3:CK{0})", beginIndex - 1);
                worksheet.Cells["CN" + beginIndex].Formula = string.Format("=SUM(CN3:CN{0})", beginIndex - 1);
                worksheet.Cells["CP" + beginIndex].Formula = string.Format("=SUM(CP3:CP{0})", beginIndex - 1);
                worksheet.Cells["CS" + beginIndex].Formula = string.Format("=SUM(CS3:CS{0})", beginIndex - 1);
                worksheet.Cells["CX" + beginIndex].Formula = string.Format("=SUM(CX3:CX{0})", beginIndex - 1);
                worksheet.Cells["CY" + beginIndex].Formula = string.Format("=SUM(CY3:CY{0})", beginIndex - 1);
                worksheet.Cells["CZ" + beginIndex].Formula = string.Format("=SUM(CZ3:CZ{0})", beginIndex - 1);
                worksheet.Cells["DA" + beginIndex].Formula = string.Format("=SUM(DA3:DA{0})", beginIndex - 1);
                worksheet.Cells["DD" + beginIndex].Formula = string.Format("=SUM(DD3:DD{0})", beginIndex - 1);
                worksheet.Cells["DE" + beginIndex].Formula = string.Format("=SUM(DE3:DE{0})", beginIndex - 1);
                worksheet.Cells["DF" + beginIndex].Formula = string.Format("=SUM(DF3:DF{0})", beginIndex - 1);
                worksheet.Cells["DR" + beginIndex].Formula = string.Format("=SUM(DR3:DR{0})", beginIndex - 1);
                worksheet.Cells["DT" + beginIndex].Formula = string.Format("=SUM(DT3:DT{0})", beginIndex - 1);
                worksheet.Cells["DU" + beginIndex].Formula = string.Format("=SUM(DU3:DU{0})", beginIndex - 1);
                worksheet.Cells["DW" + beginIndex].Formula = string.Format("=SUM(DW3:DW{0})", beginIndex - 1);
                worksheet.Cells["DX" + beginIndex].Formula = string.Format("=SUM(DX3:DX{0})", beginIndex - 1);
                worksheet.Cells["EA" + beginIndex].Formula = string.Format("=SUM(EA3:EA{0})", beginIndex - 1);
                worksheet.Cells["ED" + beginIndex].Formula = string.Format("=SUM(ED3:ED{0})", beginIndex - 1);
                worksheet.Cells["EE" + beginIndex].Formula = string.Format("=SUM(EE3:EE{0})", beginIndex - 1);
                worksheet.Cells["EF" + beginIndex].Formula = string.Format("=SUM(EF3:EF{0})", beginIndex - 1);
                worksheet.Cells["EG" + beginIndex].Formula = string.Format("=SUM(EG3:EG{0})", beginIndex - 1);

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

        public async Task ChotCongFinal()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDBContext>(options =>
               options.UseSqlServer(
                    @"Persist Security Info=True;Data Source = 10.70.21.208;Initial Catalog = HRMSDB2;User Id = sa;Password = sa@21208;Connect Timeout=3", o => o.MigrationsAssembly("HRMNS.Data.EF")));

            serviceCollection.AddSingleton(HRMNS.Application.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper());
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.AddScoped<UserManager<APP_USER>, UserManager<APP_USER>>();
            serviceCollection.AddScoped<RoleManager<APP_ROLE>, RoleManager<APP_ROLE>>();

            serviceCollection.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            serviceCollection.AddScoped(typeof(IRespository<,>), typeof(EFRepository<,>));
            serviceCollection.AddScoped<UpdatePhepNamDaiLyJob>();
            serviceCollection.AddScoped<INgayChotCongService, NgayChotCongService>();
            serviceCollection.AddScoped<INhanVienService, NhanVienService>();
            serviceCollection.AddScoped<IPhepNamService, PhepNamService>();
            serviceCollection.AddScoped<IDangKyChamCongChiTietService, DangKyChamCongChiTietService>();
            serviceCollection.AddScoped<IDangKyChamCongDacBietService, DangKyChamCongDacBietService>();
            serviceCollection.AddScoped<IDangKyChamCongDacBietService, DangKyChamCongDacBietService>();

            serviceCollection.AddLogging();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = await schedFact.GetScheduler();
            sched.JobFactory = new PhepNamHanldeJobFactory(serviceProvider);

            await sched.Start();

            var job = JobBuilder.Create<UpdatePhepNamDaiLyJob>()
                                .WithIdentity("PhepNamDaiLyJob", "group1")
                                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("PhepNamDaiLyJob-trigger", "group1")
                .StartNow()
                //.WithSimpleSchedule(x => x
                //.WithIntervalInSeconds(40)
                //.RepeatForever())
                .Build();

            await sched.ScheduleJob(job, trigger);
        }

        //[HttpPost]
        //[RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        //[RequestSizeLimit(209715200)]
        //public IActionResult ImportExcel(IList<IFormFile> files)
        //{
        //    if (files != null && files.Count > 0)
        //    {
        //        var file = files[0];
        //        var filename = ContentDispositionHeaderValue
        //                           .Parse(file.ContentDisposition)
        //                           .FileName
        //                           .Trim('"');

        //        string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }

        //        string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
        //        using (FileStream fs = System.IO.File.Create(filePath))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }

        //        List<HR_SALARY> lstUpdate = new List<HR_SALARY>();
        //        ResultDB rs = _detailSalaryService.ImportExcel(filePath, out lstUpdate);

        //        if (rs.ReturnInt != 0)
        //        {
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                // If file found, delete it    
        //                System.IO.File.Delete(filePath);
        //            }
        //            return new NotFoundObjectResult(rs.ReturnString);
        //        }

        //        if (System.IO.File.Exists(filePath))
        //        {
        //            // If file found, delete it    
        //            System.IO.File.Delete(filePath);
        //        }

        //        _memoryCache.Remove("BasicSalaryData");
        //        _memoryCache.Set("BasicSalaryData", lstUpdate);

        //        return new OkObjectResult(filePath);
        //    }
        //    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        //}
    }
}
