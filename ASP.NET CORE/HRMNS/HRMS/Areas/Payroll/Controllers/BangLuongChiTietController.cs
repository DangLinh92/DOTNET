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
using DevExtreme.AspNet.Data;
using System.Xml.Linq;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMS.Services;
using Syncfusion.EJ2;
using HRMNS.Data.Entities.Payroll;

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
        private IDCChamCongService _dcChamCongService;
        private IPhepNamService _phepNamService;
        private INhanVienService _nhanvienService;
        private IConNhoMnsService _conNhoService;
        private readonly IEmailSender _emailSender;

        private readonly IMemoryCache _memoryCache;
        public BangLuongChiTietController(IWebHostEnvironment hostEnvironment,
            IDetailSalaryService detailSalaryService,
            INgayChotCongService ngayChotCongService,
            IDCChamCongService dcChamCongService,
            ISalaryService salaryService,
            IPhepNamService phepNamService,
            INhanVienService nhanvienService,
            IConNhoMnsService conNhoService,
             IEmailSender emailSender,
            IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostEnvironment;
            _detailSalaryService = detailSalaryService;
            _ngayChotCongService = ngayChotCongService;
            _dcChamCongService = dcChamCongService;
            _phepNamService = phepNamService;
            _nhanvienService = nhanvienService;
            _memoryCache = memoryCache;
            _salaryService = salaryService;
            _emailSender = emailSender;
            _conNhoService = conNhoService;
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
                string fileName = CreatBangCongChiTiet(chotCongChoThang, chedo);
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
            spreadsheet.SaveCopy(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
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

            data = data.OrderBy(x => x.NgayVao).ToList();

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

                // Điều chỉnh công
                ExcelWorksheet worksheetDC_Cong = package.Workbook.Worksheets[5];

                // Phép Năm
                ExcelWorksheet worksheetPhepNam = package.Workbook.Worksheets[6];

                // Salary Detail
                ExcelWorksheet worksheetSalaryDetail = package.Workbook.Worksheets[7];

                // Nghi TS
                ExcelWorksheet worksheetConNhoDetail = package.Workbook.Worksheets[9];

                if (time.Contains("-"))
                {
                    worksheetTH.Cells["B2"].Value = "BẢNG LƯƠNG THÁNG " + time.Split("-")[1] + "." + time.Split("-")[0] + " (Người lao động Việt Nam)";
                    worksheetTH.Cells["B3"].Value = time.Split("-")[1] + "." + time.Split("-")[0] + " 월 급여표 (현지 근로자) _ 지급";
                    worksheetPivot.Cells["B1"].Value = "BẢNG TỔNG HỢP TIỀN LƯƠNG THÁNG " + time.Split("-")[1] + " NĂM " + time.Split("-")[0];
                }

                int beginIndex = 3;
                int inluongBeginIndex = 2;
                string cellFrom = "";
                string cellTo = "";

                string cellFromInluong = "";
                string cellToInLuong = "";

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
                    // worksheet.Cells["J" + beginIndex].Value = data[i].BasicSalary;
                    worksheet.Cells["K" + beginIndex].Value = data[i].LivingAllowance;
                    //worksheet.Cells["L" + beginIndex].Value = data[i].PositionAllowance;
                    //worksheet.Cells["M" + beginIndex].Value = data[i].AbilityAllowance;
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
                    // worksheet.Cells["CS" + beginIndex].Value = data[i].SoNguoiPhuThuoc;
                    worksheet.Cells["CX" + beginIndex].Value = data[i].NgayVao;
                    worksheet.Cells["CY" + beginIndex].Value = data[i].Note;
                    // worksheet.Cells["CZ" + beginIndex].Value = data[i].InsentiveStandard;
                    worksheet.Cells["DA" + beginIndex].Value = data[i].DanhGia;
                    worksheet.Cells["DD" + beginIndex].Value = data[i].HoTroCongDoan;
                    worksheet.Cells["DE" + beginIndex].Value = data[i].SoTK;
                    worksheet.Cells["DF" + beginIndex].Value = data[i].DoiTuongThamGiaCD;
                    worksheet.Cells["DR" + beginIndex].Value = data[i].DoiTuongTruyThuBHYT;
                    // worksheet.Cells["DT" + beginIndex].Value = data[i].SoConNho;
                    worksheet.Cells["DU" + beginIndex].Value = data[i].SoNgayNghi70;// L160
                    worksheet.Cells["DW" + beginIndex].Value = data[i].NgayNghiViec;
                    // worksheet.Cells["DX" + beginIndex].Value = data[i].DieuChinhCong_Total;

                    // worksheet.Cells["EA" + beginIndex].Value = data[i].TraTienPhepNam_Total;
                    worksheet.Cells["ED" + beginIndex].Value = data[i].TT_Tien_GioiThieu;
                    worksheet.Cells["EE" + beginIndex].Value = data[i].GioiTinh;
                    worksheet.Cells["EF" + beginIndex].Value = data[i].BauThaiSan;
                    worksheet.Cells["EG" + beginIndex].Value = data[i].ThoiGianChuaNghi;

                    if (i < data.Count - 1)
                    {
                        cellFrom = "A" + beginIndex + ":EH" + beginIndex;
                        cellTo = "A" + (beginIndex + 1) + ":EH" + (beginIndex + 1);

                        worksheet.Cells[cellFrom].Copy(worksheet.Cells[cellTo]);

                        cellFromInluong = "A" + inluongBeginIndex + ":EM" + inluongBeginIndex;
                        cellToInLuong = "A" + (inluongBeginIndex + 1) + ":EM" + (inluongBeginIndex + 1);
                    }

                    beginIndex += 1;
                    inluongBeginIndex += 1;
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

                // Sheet Điều Chỉnh Công
                string _month = time.Substring(0, 7);
                List<DCChamCongViewModel> lstDCChamCong = _dcChamCongService.GetAll("", y => y.HR_NHANVIEN, z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL).Where(x => x.ChiTraVaoLuongThang2.NullString().Contains(_month)).ToList();
                worksheetDC_Cong.Cells["A2"].Value = "BẢNG ĐIỀU CHỈNH BỔ SUNG DỮ LIỆU CÔNG THÁNG " + DateTime.Parse(time).AddMonths(-1).ToString("MM-yyyy");

                int beginIndex_dccong = 6;

                for (int i = 0; i < lstDCChamCong.Count; i++)
                {
                    worksheetDC_Cong.Cells["A" + beginIndex_dccong].Value = i + 1;
                    worksheetDC_Cong.Cells["B" + beginIndex_dccong].Value = lstDCChamCong[i].MaNV;
                    worksheetDC_Cong.Cells["C" + beginIndex_dccong].Value = lstDCChamCong[i].HR_NHANVIEN.TenNV;
                    worksheetDC_Cong.Cells["D" + beginIndex_dccong].Value = lstDCChamCong[i].HR_NHANVIEN.MaBoPhan;
                    worksheetDC_Cong.Cells["E" + beginIndex_dccong].Value = lstDCChamCong[i].NgayDieuChinh;
                    worksheetDC_Cong.Cells["AF" + beginIndex_dccong].Value = lstDCChamCong[i].NoiDungDC;
                    worksheetDC_Cong.Cells["AI" + beginIndex_dccong].Value = 0;
                    worksheetDC_Cong.Cells["AJ" + beginIndex_dccong].Value = 0;
                    worksheetDC_Cong.Cells["AK" + beginIndex_dccong].Value = lstDCChamCong[i].TongSoTien;

                    cellFrom = "";
                    cellTo = "";
                    if (i < lstDCChamCong.Count - 1)
                    {
                        cellFrom = "A" + beginIndex_dccong + ":AK" + beginIndex_dccong;
                        cellTo = "A" + (beginIndex_dccong + 1) + ":AK" + (beginIndex_dccong + 1);
                        worksheetDC_Cong.Cells[cellFrom].Copy(worksheetDC_Cong.Cells[cellTo]);
                    }
                    beginIndex_dccong += 1;
                }

                // Sheet Phép Năm
                List<PhepNamViewModel> lstPhepNam = _phepNamService.GetAll("").Where(x => x.ThoiGianChiTra != null && x.ThoiGianChiTra.NullString().Contains(_month)).ToList();
                List<NhanVienViewModel> lstNhanVien = _nhanvienService.GetAll(x => x.HR_BO_PHAN_DETAIL);
                int beginIndex_phepNam = 3;
                NhanVienViewModel nv;
                PhepNamViewModel pn;
                for (int i = 0; i < lstPhepNam.Count; i++)
                {
                    pn = lstPhepNam[i];
                    nv = lstNhanVien.FirstOrDefault(x => x.Id == pn.MaNhanVien);
                    if (nv.NgayNghiViec.NullString() != "")
                    {
                        worksheetPhepNam.Cells["A" + beginIndex_phepNam].Value = i + 1;
                        worksheetPhepNam.Cells["B" + beginIndex_phepNam].Value = pn.MaNhanVien;
                        worksheetPhepNam.Cells["C" + beginIndex_phepNam].Value = nv.HR_BO_PHAN_DETAIL.TenBoPhanChiTiet;
                        worksheetPhepNam.Cells["D" + beginIndex_phepNam].Value = nv.TenNV;

                        worksheetPhepNam.Cells["E" + beginIndex_phepNam].Value = pn.ThangBatDauDocHai?.ToString("yyyy-MM-dd") + " ~ " + pn.ThangKetThucDocHai?.ToString("yyyy-MM-dd");
                        worksheetPhepNam.Cells["F" + beginIndex_phepNam].Value = nv.NgayVao;
                        worksheetPhepNam.Cells["G" + beginIndex_phepNam].Value = nv.NgayNghiViec;
                        worksheetPhepNam.Cells["H" + beginIndex_phepNam].Value = pn.SoPhepDaUng;
                        worksheetPhepNam.Cells["I" + beginIndex_phepNam].Value = pn.SoPhepDocHai;
                        worksheetPhepNam.Cells["K" + beginIndex_phepNam].Value = pn.SoPhepNam;
                        worksheetPhepNam.Cells["N" + beginIndex_phepNam].Value = pn.NghiThang_1;
                        worksheetPhepNam.Cells["O" + beginIndex_phepNam].Value = pn.NghiThang_2;
                        worksheetPhepNam.Cells["P" + beginIndex_phepNam].Value = pn.NghiThang_3;
                        worksheetPhepNam.Cells["Q" + beginIndex_phepNam].Value = pn.NghiThang_4;
                        worksheetPhepNam.Cells["R" + beginIndex_phepNam].Value = pn.NghiThang_5;
                        worksheetPhepNam.Cells["S" + beginIndex_phepNam].Value = pn.NghiThang_6;
                        worksheetPhepNam.Cells["T" + beginIndex_phepNam].Value = pn.NghiThang_7;
                        worksheetPhepNam.Cells["U" + beginIndex_phepNam].Value = pn.NghiThang_8;
                        worksheetPhepNam.Cells["V" + beginIndex_phepNam].Value = pn.NghiThang_9;
                        worksheetPhepNam.Cells["W" + beginIndex_phepNam].Value = pn.NghiThang_10;
                        worksheetPhepNam.Cells["X" + beginIndex_phepNam].Value = pn.NghiThang_11;
                        worksheetPhepNam.Cells["Y" + beginIndex_phepNam].Value = pn.NghiThang_12;
                        worksheetPhepNam.Cells["AI" + beginIndex_phepNam].Value = pn.MucThanhToan;
                        worksheetPhepNam.Cells["AJ" + beginIndex_phepNam].Value = pn.SoTienChiTra;

                        cellFrom = "";
                        cellTo = "";
                        if (i < lstPhepNam.Count - 1)
                        {
                            cellFrom = "A" + beginIndex_phepNam + ":AJ" + beginIndex_phepNam;
                            cellTo = "A" + (beginIndex_phepNam + 1) + ":AJ" + (beginIndex_phepNam + 1);
                            worksheetPhepNam.Cells[cellFrom].Copy(worksheetPhepNam.Cells[cellTo]);
                        }
                        beginIndex_phepNam += 1;
                    }
                }

                // Salary Detail

                List<HR_SALARY> lstSalary = _salaryService.GetAllSalary(int.Parse(time.Split("-")[0]));
                HR_SALARY salary;
                int beginIndex_salary = 3;
                worksheetSalaryDetail.Cells["P2"].Value = time.Split("-")[0];

                for (int i = 0; i < lstSalary.Count; i++)
                {
                    salary = lstSalary[i];
                    worksheetSalaryDetail.Cells["A" + beginIndex_salary].Value = i + 1;
                    worksheetSalaryDetail.Cells["B" + beginIndex_salary].Value = salary.MaNV;
                    worksheetSalaryDetail.Cells["C" + beginIndex_salary].Value = salary.HR_NHANVIEN.HR_CHUCDANH.TenChucDanh;
                    worksheetSalaryDetail.Cells["D" + beginIndex_salary].Value = salary.HR_NHANVIEN.HR_BO_PHAN_DETAIL.MaBoPhan_TOP2;
                    worksheetSalaryDetail.Cells["E" + beginIndex_salary].Value = salary.HR_NHANVIEN.TenNV;
                    worksheetSalaryDetail.Cells["F" + beginIndex_salary].Value = salary.HR_NHANVIEN.GioiTinh;
                    worksheetSalaryDetail.Cells["P" + beginIndex_salary].Value = salary.Grade;
                    worksheetSalaryDetail.Cells["AK" + beginIndex_salary].Value = salary.LivingAllowance;
                    worksheetSalaryDetail.Cells["AM" + beginIndex_salary].Value = salary.AbilityAllowance;
                    worksheetSalaryDetail.Cells["AN" + beginIndex_salary].Value = salary.SeniorityAllowance;
                    worksheetSalaryDetail.Cells["AO" + beginIndex_salary].Value = salary.HarmfulAllowance;
                    worksheetSalaryDetail.Cells["AR" + beginIndex_salary].Value = salary.IncentiveLanguage;
                    worksheetSalaryDetail.Cells["AT" + beginIndex_salary].Value = salary.IncentiveTechnical;

                    cellFrom = "";
                    cellTo = "";
                    if (i < lstSalary.Count - 1)
                    {
                        cellFrom = "A" + beginIndex_salary + ":BB" + beginIndex_salary;
                        cellTo = "A" + (beginIndex_salary + 1) + ":BB" + (beginIndex_salary + 1);
                        worksheetSalaryDetail.Cells[cellFrom].Copy(worksheetSalaryDetail.Cells[cellTo]);
                    }
                    beginIndex_salary += 1;
                }

                // Con nho 
                BangLuongChiTietViewModel bl;
                List<HR_CON_NHO> lstConNho = _conNhoService.GetConNhos();

                for (int i = 0; i < lstConNho.Count; i++)
                {
                    bl = data.FirstOrDefault(x => x.MaNV == lstConNho[i].MaNV);

                    worksheetConNhoDetail.Cells["A" + (i + 3)].Value = i + 1;
                    worksheetConNhoDetail.Cells["B" + (i + 3)].Value = lstConNho[i].MaNV;
                    worksheetConNhoDetail.Cells["C" + (i + 3)].Value = lstConNho[i].TenNV;
                    worksheetConNhoDetail.Cells["D" + (i + 3)].Value = bl != null ? bl.BoPhan : "";
                    worksheetConNhoDetail.Cells["E" + (i + 3)].Value = lstConNho[i].NgaySinh;
                    worksheetConNhoDetail.Cells["K" + (i + 3)].Value = lstConNho[i].ThangTinhHuong;

                    if(bl != null)
                    {
                        worksheetConNhoDetail.Cells["L" + (i + 3)].Value = bl.TongNgayCongThucTe > 0 ? "" : bl.NghiTS.NullString();
                        worksheetConNhoDetail.Cells["M" + (i + 3)].Value = bl.NgayNghiViec;
                    }

                    worksheetConNhoDetail.Cells["F" + (i + 3)].Formula = string.Format("=+YEAR($E$1)-YEAR(E{0})", i + 3);
                    worksheetConNhoDetail.Cells["G" + (i + 3)].Formula = string.Format("=+DATEDIF(E{0},$E$1,\"y\")&\" năm\"&\" \"&DATEDIF(E{1},$E$1,\"ym\")&\" tháng\"", i + 3, i + 3);
                    worksheetConNhoDetail.Cells["H" + (i + 3)].Formula = string.Format("=+COUNTIFS($B$3:$B$1048576,B{0},$I$3:$I$1048576,\"Có\")", i + 3);
                    worksheetConNhoDetail.Cells["I" + (i + 3)].Formula = string.Format("=+IF((YEAR($E$1)-YEAR(E{0}))>6,\"Không\",IF(AND((YEAR($E$1)-YEAR(E{1}))=6,MONTH($E$1)<MONTH($C$1)),\"có\",IF(AND(YEAR($E$1)-YEAR(E{2})=6,MONTH($E$1)>=MONTH($C$1)),\"Không\",\"có\")))", i + 3, i + 3, i + 3);
                    worksheetConNhoDetail.Cells["J" + (i + 3)].Formula = string.Format("=+IF(I{0}=\"Không\",0,IF(AND(I{1}=\"có\",L{2}=\"nghỉ thai sản\"),0,30000))", i + 3, i + 3, i + 3);
                    worksheetConNhoDetail.Cells["N" + (i + 3)].Formula = string.Format("=+IF(COUNTIFS($B$3:$B$1048576,B{0},$E$3:$E$1048576,E{1})>1,\"Có\",\"Không\")", i + 3, i + 3);
                    worksheetConNhoDetail.Cells["R" + (i + 3)].Formula = string.Format("=+COUNTIFS($B$3:$B$1048576,B{0},$J$3:$J$1048576,\">0\")", i + 3);
                }

                //string maNV;
                //for (int i = worksheetConNhoDetail.Dimension.Start.Row + 2; i <= worksheetConNhoDetail.Dimension.End.Row; i++)
                //{
                //    maNV = worksheetConNhoDetail.Cells[i, 2].Text.NullString().ToUpper();

                //    if (maNV.NullString() == "")
                //    {
                //        break;
                //    }

                //    bl = data.FirstOrDefault(x => x.MaNV == maNV);
                //    worksheetConNhoDetail.Cells["L" + i].Value = bl != null ? bl.NghiTS : "";
                //}

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

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult SendMailLuong(IList<IFormFile> files, [FromQuery] string param)
        {
            try
            {
                if (files != null && files.Count > 0)
                {
                    string month = param;
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

                    string folderPayrollDocuments = _hostingEnvironment.WebRootPath + $@"\uploaded\PayrollDocuments";
                    if (!Directory.Exists(folderPayrollDocuments))
                    {
                        Directory.CreateDirectory(folderPayrollDocuments);
                    }
                    else
                    {
                        Directory.Delete(folderPayrollDocuments, true);
                        Directory.CreateDirectory(folderPayrollDocuments);
                    }

                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet worksheetInLuong = packet.Workbook.Worksheets[0];
                        var sWebRootFolder = _hostingEnvironment.WebRootPath;
                        string email = "";
                        string path = Path.Combine(sWebRootFolder, "templates/Templatebody.html");
                        string content = "";
                        List<NhanVienViewModel> lstNhanVienInfo = _nhanvienService.GetAll().ToList();
                        PayslipItem payslipItem = new PayslipItem();
                        for (int i = worksheetInLuong.Dimension.Start.Row + 1; i <= worksheetInLuong.Dimension.End.Row; i++)
                        {
                            string Bi = worksheetInLuong.Cells["B" + i].Text.NullString();
                            string Ci = worksheetInLuong.Cells["C" + i].Text.NullString();
                            string Di = worksheetInLuong.Cells["D" + i].Text.NullString();
                            string Ei = worksheetInLuong.Cells["E" + i].Text.NullString();
                            string Fi = worksheetInLuong.Cells["F" + i].Text.IfNullIsZero();
                            string Gi = worksheetInLuong.Cells["G" + i].Text.IfNullIsZero();
                            string Hi = worksheetInLuong.Cells["H" + i].Text.IfNullIsZero();
                            string Ii = worksheetInLuong.Cells["I" + i].Text.IfNullIsZero();
                            string Ji = worksheetInLuong.Cells["J" + i].Text.IfNullIsZero();
                            string Ki = worksheetInLuong.Cells["K" + i].Text.IfNullIsZero();
                            string Li = worksheetInLuong.Cells["L" + i].Text.IfNullIsZero();
                            string Mi = worksheetInLuong.Cells["M" + i].Text.IfNullIsZero();
                            string Ni = worksheetInLuong.Cells["N" + i].Text.IfNullIsZero();

                            string Oi = worksheetInLuong.Cells["O" + i].Text.IfNullIsZero();
                            string Pi = worksheetInLuong.Cells["P" + i].Text.IfNullIsZero();
                            string Qi = worksheetInLuong.Cells["Q" + i].Text.IfNullIsZero();
                            string Ri = worksheetInLuong.Cells["R" + i].Text.IfNullIsZero();
                            string Si = worksheetInLuong.Cells["S" + i].Text.IfNullIsZero();
                            string Ti = worksheetInLuong.Cells["T" + i].Text.IfNullIsZero();

                            string Ui = worksheetInLuong.Cells["U" + i].Text.IfNullIsZero();
                            string Vi = worksheetInLuong.Cells["V" + i].Text.IfNullIsZero();
                            string Wi = worksheetInLuong.Cells["W" + i].Text.IfNullIsZero();
                            string Xi = worksheetInLuong.Cells["X" + i].Text.IfNullIsZero();
                            string Yi = worksheetInLuong.Cells["Y" + i].Text.IfNullIsZero();
                            string Zi = worksheetInLuong.Cells["Z" + i].Text.IfNullIsZero();
                            string AAi = worksheetInLuong.Cells["AA" + i].Text.IfNullIsZero();
                            string ABi = worksheetInLuong.Cells["AB" + i].Text.IfNullIsZero();

                            string ACi = worksheetInLuong.Cells["AC" + i].Text.IfNullIsZero();
                            string ADi = worksheetInLuong.Cells["AD" + i].Text.IfNullIsZero();
                            double AEi = 0;
                            double AFi = 0;
                            double AGi = 0;

                            string AHi = worksheetInLuong.Cells["AH" + i].Text.IfNullIsZero();
                            string AIi = worksheetInLuong.Cells["AI" + i].Text.IfNullIsZero();
                            string AJi = worksheetInLuong.Cells["AJ" + i].Text.IfNullIsZero();
                            string AKi = worksheetInLuong.Cells["AK" + i].Text.IfNullIsZero();
                            string ALi = worksheetInLuong.Cells["AL" + i].Text.IfNullIsZero();
                            string AMi = worksheetInLuong.Cells["AM" + i].Text.IfNullIsZero();

                            string ANi = worksheetInLuong.Cells["AN" + i].Text.IfNullIsZero();
                            string AOi = worksheetInLuong.Cells["AO" + i].Text.IfNullIsZero();
                            string APi = worksheetInLuong.Cells["AP" + i].Text.IfNullIsZero();

                            string AQi = worksheetInLuong.Cells["AQ" + i].Text.IfNullIsZero();
                            string ARi = worksheetInLuong.Cells["AR" + i].Text.IfNullIsZero();

                            string ASi = worksheetInLuong.Cells["AS" + i].Text.IfNullIsZero();
                            string ATi = worksheetInLuong.Cells["AT" + i].Text.IfNullIsZero();

                            string AUi = worksheetInLuong.Cells["AU" + i].Text.IfNullIsZero();
                            string AVi = worksheetInLuong.Cells["AV" + i].Text.IfNullIsZero();

                            string AWi = worksheetInLuong.Cells["AW" + i].Text.IfNullIsZero();
                            string AXi = worksheetInLuong.Cells["AX" + i].Text.IfNullIsZero();

                            string AYi = worksheetInLuong.Cells["AY" + i].Text.IfNullIsZero();
                            string AZi = worksheetInLuong.Cells["AZ" + i].Text.IfNullIsZero();

                            string BAi = worksheetInLuong.Cells["BA" + i].Text.IfNullIsZero();
                            string BBi = worksheetInLuong.Cells["BB" + i].Text.IfNullIsZero();

                            string BCi = worksheetInLuong.Cells["BC" + i].Text.IfNullIsZero();
                            string BDi = worksheetInLuong.Cells["BD" + i].Text.IfNullIsZero();

                            string BEi = worksheetInLuong.Cells["BE" + i].Text.IfNullIsZero();
                            string BFi = worksheetInLuong.Cells["BF" + i].Text.IfNullIsZero();

                            string BGi = worksheetInLuong.Cells["BG" + i].Text.IfNullIsZero();
                            string BHi = worksheetInLuong.Cells["BH" + i].Text.IfNullIsZero();

                            string BIi = worksheetInLuong.Cells["BI" + i].Text.IfNullIsZero();
                            string BJi = worksheetInLuong.Cells["BJ" + i].Text.IfNullIsZero();

                            string BKi = worksheetInLuong.Cells["BK" + i].Text.IfNullIsZero();
                            string BLi = worksheetInLuong.Cells["BL" + i].Text.IfNullIsZero();

                            string BMi = worksheetInLuong.Cells["BM" + i].Text.IfNullIsZero();

                            string BNi = worksheetInLuong.Cells["BN" + i].Text.IfNullIsZero();
                            string BOi = worksheetInLuong.Cells["BO" + i].Text.IfNullIsZero();
                            string BPi = worksheetInLuong.Cells["BP" + i].Text.IfNullIsZero();
                            string BQi = worksheetInLuong.Cells["BQ" + i].Text.IfNullIsZero();
                            string BRi = worksheetInLuong.Cells["BR" + i].Text.IfNullIsZero();
                            string BSi = worksheetInLuong.Cells["BS" + i].Text.IfNullIsZero();
                            string BTi = worksheetInLuong.Cells["BT" + i].Text.IfNullIsZero();
                            string BUi = worksheetInLuong.Cells["BU" + i].Text.IfNullIsZero();

                            string BVi = worksheetInLuong.Cells["BV" + i].Text.IfNullIsZero();
                            string BWi = worksheetInLuong.Cells["BW" + i].Text.IfNullIsZero();
                            string BXi = worksheetInLuong.Cells["BX" + i].Text.IfNullIsZero();
                            string BYi = worksheetInLuong.Cells["BY" + i].Text.IfNullIsZero();
                            string BZi = worksheetInLuong.Cells["BZ" + i].Text.IfNullIsZero();
                            string CAi = worksheetInLuong.Cells["CA" + i].Text.IfNullIsZero();
                            string CBi = worksheetInLuong.Cells["CB" + i].Text.IfNullIsZero();
                            string CCi = worksheetInLuong.Cells["CC" + i].Text.IfNullIsZero();

                            string CDi = worksheetInLuong.Cells["CD" + i].Text.IfNullIsZero();
                            string CEi = worksheetInLuong.Cells["CE" + i].Text.IfNullIsZero();
                            string CFi = worksheetInLuong.Cells["CF" + i].Text.IfNullIsZero();
                            string CGi = worksheetInLuong.Cells["CG" + i].Text.IfNullIsZero();
                            string CHi = worksheetInLuong.Cells["CH" + i].Text.IfNullIsZero();
                            string CIi = worksheetInLuong.Cells["CI" + i].Text.IfNullIsZero();
                            string CJi = worksheetInLuong.Cells["CJ" + i].Text.IfNullIsZero();
                            string CKi = worksheetInLuong.Cells["CK" + i].Text.IfNullIsZero();
                            string CLi = worksheetInLuong.Cells["CL" + i].Text.IfNullIsZero();
                            string CMi = worksheetInLuong.Cells["CM" + i].Text.IfNullIsZero();

                            string CNi = worksheetInLuong.Cells["CN" + i].Text.IfNullIsZero();
                            string COi = worksheetInLuong.Cells["CO" + i].Text.IfNullIsZero();
                            string CPi = worksheetInLuong.Cells["CP" + i].Text.IfNullIsZero();
                            string CQi = worksheetInLuong.Cells["CQ" + i].Text.IfNullIsZero();
                            string CRi = worksheetInLuong.Cells["CR" + i].Text.IfNullIsZero();

                            string CSi = worksheetInLuong.Cells["CS" + i].Text.IfNullIsZero();
                            string CTi = worksheetInLuong.Cells["CT" + i].Text.IfNullIsZero();
                            string CUi = worksheetInLuong.Cells["CU" + i].Text.IfNullIsZero();
                            string CVi = worksheetInLuong.Cells["CV" + i].Text.IfNullIsZero();
                            string CWi = worksheetInLuong.Cells["CW" + i].Text.IfNullIsZero();
                            string CXi = worksheetInLuong.Cells["CX" + i].Text.IfNullIsZero();

                            string CYi = worksheetInLuong.Cells["CY" + i].Text.IfNullIsZero();
                            string CZi = worksheetInLuong.Cells["CZ" + i].Text.IfNullIsZero();
                            string DAi = worksheetInLuong.Cells["DA" + i].Text.IfNullIsZero();
                            string DBi = worksheetInLuong.Cells["DB" + i].Text.IfNullIsZero();
                            string DCi = worksheetInLuong.Cells["DC" + i].Text.IfNullIsZero();

                            string DDi = worksheetInLuong.Cells["DD" + i].Text.IfNullIsZero();
                            string DEi = worksheetInLuong.Cells["DE" + i].Text.IfNullIsZero();

                            string DFi = worksheetInLuong.Cells["DF" + i].Text.IfNullIsZero();
                            string DGi = worksheetInLuong.Cells["DG" + i].Text.IfNullIsZero();

                            string DHi = worksheetInLuong.Cells["DH" + i].Text.IfNullIsZero();
                            string DIi = worksheetInLuong.Cells["DI" + i].Text.IfNullIsZero();
                            string DJi = worksheetInLuong.Cells["DJ" + i].Text.IfNullIsZero();
                            string DKi = worksheetInLuong.Cells["DK" + i].Text.IfNullIsZero();
                            string DLi = worksheetInLuong.Cells["DL" + i].Text.IfNullIsZero();
                            string DMi = worksheetInLuong.Cells["DM" + i].Text.IfNullIsZero();
                            string DNi = worksheetInLuong.Cells["DN" + i].Text.IfNullIsZero();
                            string DOi = worksheetInLuong.Cells["DO" + i].Text.IfNullIsZero();
                            string DPi = worksheetInLuong.Cells["DP" + i].Text.IfNullIsZero();
                            string DQi = worksheetInLuong.Cells["DQ" + i].Text.IfNullIsZero();
                            string DRi = worksheetInLuong.Cells["DR" + i].Text.IfNullIsZero();

                            string DSi = worksheetInLuong.Cells["DS" + i].Text.IfNullIsZero();
                            string DTi = worksheetInLuong.Cells["DT" + i].Text.IfNullIsZero();
                            string DUi = worksheetInLuong.Cells["DU" + i].Text.IfNullIsZero();
                            string DVi = worksheetInLuong.Cells["DV" + i].Text.IfNullIsZero();
                            string DWi = worksheetInLuong.Cells["DW" + i].Text.IfNullIsZero();

                            string DXi = worksheetInLuong.Cells["DX" + i].Text.IfNullIsZero();
                            string DYi = worksheetInLuong.Cells["DY" + i].Text.IfNullIsZero();
                            string DZi = worksheetInLuong.Cells["DZ" + i].Text.IfNullIsZero();
                            string EAi = worksheetInLuong.Cells["EA" + i].Text.IfNullIsZero();

                            string EBi = worksheetInLuong.Cells["EB" + i].Text.IfNullIsZero();
                            string ECi = worksheetInLuong.Cells["EC" + i].Text.IfNullIsZero();
                            string EFi = worksheetInLuong.Cells["EF" + i].Text.IfNullIsZero();

                            payslipItem = new PayslipItem();
                            payslipItem.Month = month;
                            payslipItem.Month2 = DateTime.Parse(month + "-01").ToString("Y");

                            payslipItem.Ten = Ci;
                            payslipItem.Code = Bi;
                            payslipItem.BP = Di;
                            payslipItem.Ngayvao = Ei;

                            payslipItem.LCB = Fi.StringFormatNumber("N0");
                            payslipItem.PCDS = Gi.StringFormatNumber("N0");
                            payslipItem.PCCV = Hi.StringFormatNumber("N0");
                            payslipItem.PC_NL = Ii.StringFormatNumber("N0");
                            payslipItem.Luong_D = Li.StringFormatNumber("N0");

                            payslipItem.PhepNamTon = EFi.StringFormatNumber("N1");
                            payslipItem.PCTN = Ji.StringFormatNumber("N0");
                            payslipItem.PCDH = Ki.StringFormatNumber("N0");
                            payslipItem.Luong_H = Mi.StringFormatNumber("N0");

                            payslipItem.TV_ngay = Ni.StringFormatNumber("N1");
                            payslipItem.TV_dem = Pi.StringFormatNumber("N1");
                            payslipItem.CT_ngay = Ri.StringFormatNumber("N1");
                            payslipItem.CT_dem = Ti.StringFormatNumber("N1");
                            payslipItem.Nghi_co_luong = Vi.StringFormatNumber("N1");
                            payslipItem.Cong_ngay = Zi.StringFormatNumber("N0");
                            payslipItem.Cong_dem = AAi.StringFormatNumber("N0");
                            payslipItem.TTien_nghi_co_luong = Wi.StringFormatNumber("N0");
                            payslipItem.nghi_KL = Xi.StringFormatNumber("N0");
                            payslipItem.TT_lviec = Yi.StringFormatNumber("N0");
                            payslipItem.Luong_theo_ngay_cong = ABi.StringFormatNumber("N0");

                            payslipItem.OT_time_150 = BOi.StringFormatNumber("N1");
                            payslipItem.OT_time_200 = BPi.StringFormatNumber("N1");
                            payslipItem.OT_time_210 = BQi.StringFormatNumber("N1");
                            payslipItem.OT_time_270 = BRi.StringFormatNumber("N1");
                            payslipItem.OT_time_300 = BSi.StringFormatNumber("N1");
                            payslipItem.OT_time_390 = BTi.StringFormatNumber("N1");
                            payslipItem.OT_time_260 = BUi.StringFormatNumber("N1");

                            payslipItem.Cong15 = BVi.StringFormatNumber("N1");
                            payslipItem.Cong20 = BWi.StringFormatNumber("N1");
                            payslipItem.Cong21 = BXi.StringFormatNumber("N1");
                            payslipItem.Cong27 = BYi.StringFormatNumber("N1");
                            payslipItem.Cong30 = BZi.StringFormatNumber("N1");
                            payslipItem.COng39 = CAi.StringFormatNumber("N1");
                            payslipItem.COng26 = CBi.StringFormatNumber("N1");

                            payslipItem.HT_15_Total = CNi.StringFormatNumber("N1");
                            payslipItem.HT_200_Total = COi.StringFormatNumber("N1");
                            payslipItem.HT_270_Total = CPi.StringFormatNumber("N1");
                            payslipItem.HT_300_Total = CQi.StringFormatNumber("N1");
                            payslipItem.HT_390_Total = CRi.StringFormatNumber("N1");

                            payslipItem.Cong151 = CSi.StringFormatNumber("N1");
                            payslipItem.Cong201 = CTi.StringFormatNumber("N1");
                            payslipItem.Cong271 = CUi.StringFormatNumber("N1");
                            payslipItem.Cong301 = CVi.StringFormatNumber("N1");
                            payslipItem.COng391 = CWi.StringFormatNumber("N1");

                            payslipItem.Tong_OT = CCi.StringFormatNumber("N1");
                            payslipItem.Tong_HTLV = CXi.StringFormatNumber("N1");

                            payslipItem.Ca_ngay_TV = AHi.StringFormatNumber("N0");
                            payslipItem.Ca_ngay_CT = AIi.StringFormatNumber("N0");
                            payslipItem.ca_dem_TV_ky_niem_truoc_le = AJi.StringFormatNumber("N0");
                            payslipItem.ca_dem_CT_ky_niem_truoc_le = AKi.StringFormatNumber("N0");
                            payslipItem.Thanh_tien = ALi.StringFormatNumber("N0");

                            payslipItem.Nghi_Bu_AL30 = CYi.StringFormatNumber("N1");
                            payslipItem.Nghi_Bu_NB = CZi.StringFormatNumber("N1");
                            payslipItem.Ho_tro_PC_NB = DAi.StringFormatNumber("N1");
                            payslipItem.Ho_tro_luong_NB = DBi.StringFormatNumber("N1");
                            payslipItem.Tong_ho_tro_NB = DCi.StringFormatNumber("N1");

                            payslipItem.so_ngay_nghi_70 = ACi.StringFormatNumber("N1");
                            payslipItem.thanh_tien_nghi_70 = ADi.StringFormatNumber("N1");

                            payslipItem.Cong_them = DRi.StringFormatNumber("N1");
                            payslipItem.Chuyencan = DHi.StringFormatNumber("N0");
                            payslipItem.Incentive = DIi.StringFormatNumber("N0");
                            payslipItem.Thanh_toan_PN = DQi.StringFormatNumber("N0");
                            payslipItem.HT_gui_tre = DLi.StringFormatNumber("N0");
                            payslipItem.HT_PCCC_co_so = DMi.StringFormatNumber("N0");
                            payslipItem.HT_ATNVSV = DNi.StringFormatNumber("N0");
                            payslipItem.HT_CD = "-";

                            payslipItem.HT_Sinh_ly = DOi.StringFormatNumber("N0");
                            payslipItem.TN_khac = DPi.StringFormatNumber("N0");
                            payslipItem.Dem_TV = DDi.StringFormatNumber("N1");
                            payslipItem.Dem_CT = DFi.StringFormatNumber("N1");

                            payslipItem.Ttien = DEi.StringFormatNumber("N0");
                            payslipItem.Ttien1 = DGi.StringFormatNumber("N0");

                            payslipItem.Cong_tru = EBi.StringFormatNumber("N0");
                            payslipItem.BHXH = DSi.StringFormatNumber("N0");
                            payslipItem.Truy_thu_BHYT = DTi.StringFormatNumber("N0");
                            payslipItem.Cong_doan = DUi.StringFormatNumber("N0");
                            payslipItem.thue_TNCN = DWi.StringFormatNumber("N0");
                            payslipItem.hmuon = DXi.StringFormatNumber("N0");
                            payslipItem.Di_muon = DYi.StringFormatNumber("N1");
                            payslipItem.tru_khac = DZi.StringFormatNumber("N0");
                            payslipItem.Quy_PCTT = DVi.StringFormatNumber("N0");

                            // THUC NHAN
                            payslipItem.Thuc_nhan = ECi.StringFormatNumber("N0");

                            #region Html doc
                            //HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                            //document.Load(path);

                            //document.GetElementbyId("title_bangluong").InnerHtml += month;
                            //document.GetElementbyId("title_payslip").InnerHtml += DateTime.Parse(month + "-01").ToString("Y");

                            //// NAME AND DEPT
                            //document.GetElementbyId("Name_NV").InnerHtml = Ci;
                            //document.GetElementbyId("Group_NV").InnerHtml = Di;
                            //document.GetElementbyId("Code").InnerHtml = Bi;
                            //document.GetElementbyId("Date_join").InnerHtml = Ei;

                            //// LUONG CO BAN
                            //document.GetElementbyId("LCB_id").InnerHtml = Fi.StringFormatNumber("N0");
                            //document.GetElementbyId("PCDS_id").InnerHtml = Gi.StringFormatNumber("N0");
                            //document.GetElementbyId("PCTNhiem_id").InnerHtml = Hi.StringFormatNumber("N0");
                            //document.GetElementbyId("PCNangLuc_id").InnerHtml = Ii.StringFormatNumber("N0");
                            //document.GetElementbyId("Luong_Day_id").InnerHtml = Li.StringFormatNumber("N0");

                            //document.GetElementbyId("phep_nam").InnerHtml = EFi.StringFormatNumber("N1");
                            //document.GetElementbyId("PCTN_id").InnerHtml = Ji.StringFormatNumber("N0");
                            //document.GetElementbyId("PCDH_id").InnerHtml = Ki.StringFormatNumber("N0");
                            //document.GetElementbyId("Luong_H_id").InnerHtml = Mi.StringFormatNumber("N0");

                            //// THOI GIAN LAM VIEC
                            //document.GetElementbyId("Thu_viec_ngay_id").InnerHtml = Ni.StringFormatNumber("N1");
                            //document.GetElementbyId("thu_viec_dem130_id").InnerHtml = Pi.StringFormatNumber("N1");

                            //document.GetElementbyId("Chinh_thuc_ngay_id").InnerHtml = Ri.StringFormatNumber("N1");
                            //document.GetElementbyId("chinh_thuc_dem130_id").InnerHtml = Ti.StringFormatNumber("N1");
                            //document.GetElementbyId("nghi_co_luong_id").InnerHtml = Vi.StringFormatNumber("N1");

                            //document.GetElementbyId("Tong_cong_BN_id").InnerHtml = Zi.StringFormatNumber("N0");
                            //document.GetElementbyId("ngay_cong_dem_id").InnerHtml = AAi.StringFormatNumber("N1");
                            //document.GetElementbyId("tt_nghi_luong_id").InnerHtml = Wi.StringFormatNumber("N1");
                            //document.GetElementbyId("nghi_khong_luong_id").InnerHtml = Xi.StringFormatNumber("N1");
                            //document.GetElementbyId("tong_lam_viec_id").InnerHtml = Yi.StringFormatNumber("N1");
                            //document.GetElementbyId("luong_theo_ngay_cong_id").InnerHtml = ABi.StringFormatNumber("N1");

                            //// TANG CA - HO TRO LAM VIEC
                            //document.GetElementbyId("OT_time_150_id").InnerHtml = BOi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_200_id").InnerHtml = BPi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_210_id").InnerHtml = BQi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_270_id").InnerHtml = BRi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_300_id").InnerHtml = BSi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_390_id").InnerHtml = BTi.StringFormatNumber("N1");
                            //document.GetElementbyId("OT_time_260").InnerHtml = BUi.StringFormatNumber("N1");

                            //document.GetElementbyId("Cong15_OT_id").InnerHtml = BVi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong20_OT_id").InnerHtml = BWi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong21_OT_id").InnerHtml = BXi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong27_OT_id").InnerHtml = BYi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong30_OT_id").InnerHtml = BZi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong39_OT_id").InnerHtml = CAi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong26_OT_id").InnerHtml = CBi.StringFormatNumber("N1");

                            //document.GetElementbyId("HT_15_Total_id").InnerHtml = CNi.StringFormatNumber("N1");
                            //document.GetElementbyId("HT_200_Total_id").InnerHtml = COi.StringFormatNumber("N1");
                            //document.GetElementbyId("HT_270_Total_id").InnerHtml = CPi.StringFormatNumber("N1");
                            //document.GetElementbyId("HT_300_Total_id").InnerHtml = CQi.StringFormatNumber("N1");
                            //document.GetElementbyId("HT_390_Total_id").InnerHtml = CRi.StringFormatNumber("N1");

                            //document.GetElementbyId("Cong15_HT_id").InnerHtml = CSi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong20_HT_id").InnerHtml = CTi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong27_HT_id").InnerHtml = CUi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong30_HT_id").InnerHtml = CVi.StringFormatNumber("N1");
                            //document.GetElementbyId("Cong39_HT_id").InnerHtml = CWi.StringFormatNumber("N1");

                            //document.GetElementbyId("Tong_OT_id").InnerHtml = CCi.StringFormatNumber("N1");
                            //document.GetElementbyId("Tong_HTLV_id").InnerHtml = CXi.StringFormatNumber("N1");

                            //// HO TRO THANH LAP CONG TY
                            //document.GetElementbyId("Ca_ngay_TV_id").InnerHtml = AHi.StringFormatNumber("N0");
                            //document.GetElementbyId("Ca_ngay_CT_id").InnerHtml = AIi.StringFormatNumber("N0");
                            //document.GetElementbyId("Ca_dem_TV_truoc_le_id").InnerHtml = AJi.StringFormatNumber("N0");
                            //document.GetElementbyId("Ca_dem_CT_truoc_le_id").InnerHtml = AKi.StringFormatNumber("N0");
                            //document.GetElementbyId("Thanh_tien_truoc_le_id").InnerHtml = ALi.StringFormatNumber("N0");

                            //// NGHI BU
                            //document.GetElementbyId("Nghi_Bu_AL_id").InnerHtml = CYi.StringFormatNumber("N1");
                            //document.GetElementbyId("Nghi_Bu_NB_id").InnerHtml = CZi.StringFormatNumber("N1");
                            //document.GetElementbyId("Ho_tro_PC_NB_id").InnerHtml = DAi.StringFormatNumber("N1");
                            //document.GetElementbyId("Ho_tro_luong30_id").InnerHtml = DBi.StringFormatNumber("N1");
                            //document.GetElementbyId("Tong_ho_tro_NB_id").InnerHtml = DCi.StringFormatNumber("N1");

                            //// LUU TRU CONG TY
                            //document.GetElementbyId("so_ngay_nghi_TTV").InnerHtml = ACi.StringFormatNumber("N1");
                            //document.GetElementbyId("Thanh_tien_nghi_TTV").InnerHtml = ADi.StringFormatNumber("N1");

                            //// KHOAN CONG KHAC

                            //document.GetElementbyId("Cong_them_id").InnerHtml = DRi.StringFormatNumber("N1");
                            //document.GetElementbyId("Chuyen_can_id").InnerHtml = DHi.StringFormatNumber("N0");
                            //document.GetElementbyId("Incentive_id").InnerHtml = DIi.StringFormatNumber("N0");
                            //document.GetElementbyId("TTPhepNam_id").InnerHtml = DQi.StringFormatNumber("N0");
                            //document.GetElementbyId("HT_gui_tre_id").InnerHtml = DLi.StringFormatNumber("N0");
                            //document.GetElementbyId("HT_PCCC_co_so_id").InnerHtml = DMi.StringFormatNumber("N0");
                            //document.GetElementbyId("HT_ATNVSV_id").InnerHtml = DNi.StringFormatNumber("N0");
                            //document.GetElementbyId("HT_SinhLy_Id").InnerHtml = DOi.StringFormatNumber("N0");
                            //document.GetElementbyId("TN_Khac_id").InnerHtml = DPi.StringFormatNumber("N0");
                            //document.GetElementbyId("TT_TV_dem_id").InnerHtml = DEi.StringFormatNumber("N0");
                            //document.GetElementbyId("TT_CT_dem_id").InnerHtml = DGi.StringFormatNumber("N0");
                            //document.GetElementbyId("Dem_TV_id").InnerHtml = DDi.StringFormatNumber("N1");
                            //document.GetElementbyId("Dem_CT_id").InnerHtml = DFi.StringFormatNumber("N1");

                            //// CAC KHOAN KHAU TRU
                            //document.GetElementbyId("Cong_tru_id").InnerHtml = EBi.StringFormatNumber("N0");
                            //document.GetElementbyId("BH_XH_id").InnerHtml = DSi.StringFormatNumber("N0");
                            //document.GetElementbyId("TRUY_THU_BHYT_id").InnerHtml = DTi.StringFormatNumber("N0");
                            //document.GetElementbyId("Cong_doan_id").InnerHtml = DUi.StringFormatNumber("N0");
                            //document.GetElementbyId("Thue_TNCN_id").InnerHtml = DWi.StringFormatNumber("N0");
                            //document.GetElementbyId("hmuon_id").InnerHtml = DXi.StringFormatNumber("N0");
                            //document.GetElementbyId("Di_muon_id").InnerHtml = DYi.StringFormatNumber("N1");
                            //document.GetElementbyId("tru_khac_id").InnerHtml = DZi.StringFormatNumber("N0");
                            //document.GetElementbyId("QuyPCTT_id").InnerHtml = DVi.StringFormatNumber("N0");

                            //// THUC NHAN
                            //document.GetElementbyId("Thuc_nhan_id").InnerHtml = ECi.StringFormatNumber("N0");

                            //using (var stream = new MemoryStream())
                            //{
                            //    document.Save(stream);
                            //    stream.Position = 0;
                            //    content = new StreamReader(stream).ReadToEnd();
                            //}
                            #endregion

                            if (lstNhanVienInfo.FirstOrDefault(x => x.Id == payslipItem.Code).NgaySinh.NullString() != "")
                            {
                                payslipItem.NamSinh = DateTime.Parse(lstNhanVienInfo.FirstOrDefault(x => x.Id == payslipItem.Code).NgaySinh).Year.ToString();
                            }
                            else
                            {
                                payslipItem.NamSinh = month.Substring(0, 4);
                            }

                            string attackPath = CreatePayrollDocument(payslipItem, folderPayrollDocuments);
                            email = lstNhanVienInfo.FirstOrDefault(x => x.Id == payslipItem.Code).Email.NullString();

                            if (email != "")
                            {
                                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                                document.Load(path);
                                document.GetElementbyId("id_dear").InnerHtml = "Gửi Anh/Chị " + payslipItem.Ten + "!";
                                using (var stream = new MemoryStream())
                                {
                                    document.Save(stream);
                                    stream.Position = 0;
                                    content = new StreamReader(stream).ReadToEnd();
                                }

                                var message = new Message(new string[] { email }, "WHC - THÔNG TIN LƯƠNG THÁNG " + month, content);
                                _emailSender.SendEmailHtmlWithAttack(message, attackPath);

                                // await Task.Delay(1000);
                            }
                        }

                        if (System.IO.File.Exists(filePath))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(filePath);
                        }
                    }
                    return new OkObjectResult(true);
                }

                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreatePayrollDocument(PayslipItem item, string folderPayrollDocuments)
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;

            string templatePath = Path.Combine(sWebRootFolder, "templates/Payroll_slip_for_merge -1023.docx");
            string filePath = Path.Combine(folderPayrollDocuments, $"{item.Code}_{item.Ten}_{item.Month}.docx");

            // Sao chép file template .dotx thành file .docx
            System.IO.File.Copy(templatePath, filePath, true);

            using (DocumentFormat.OpenXml.Packaging.WordprocessingDocument wordDocument = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(filePath, true))
            {
                ReplacePlaceholders(wordDocument, item);
                wordDocument.MainDocumentPart.Document.Save();
            }

            // code + tháng + năm vào : 2105001 06 92
            string password = item.Code.ToUpper().Replace("H", "") + item.Month.Split("-")[1] + item.NamSinh.Substring(item.NamSinh.Length - 2);
            SetPassword(filePath, password);

            return filePath;
        }

        private void ReplacePlaceholders(DocumentFormat.OpenXml.Packaging.WordprocessingDocument document, PayslipItem item)
        {
            var body = document.MainDocumentPart.Document.Body;

            foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (text.Text.Contains("«Time»"))
                {
                    text.Text = text.Text.Replace("«Time»", item.Month);
                }
                else if (text.Text.Contains("«Time»"))
                {
                    text.Text = text.Text.Replace("«Time»", item.Month2);
                }
                else if (text.Text.Contains("«Ten»"))
                {
                    text.Text = text.Text.Replace("«Ten»", item.Ten);
                }
                else if (text.Text.Contains("«BP»"))
                {
                    text.Text = text.Text.Replace("«BP»", item.BP);
                }
                else if (text.Text.Contains("«Code»"))
                {
                    text.Text = text.Text.Replace("«Code»", item.Code);
                }
                else if (text.Text.Contains("«Ngayvao»"))
                {
                    text.Text = text.Text.Replace("«Ngayvao»", item.Ngayvao);
                }
                else if (text.Text.Contains("«LCB»"))
                {
                    text.Text = text.Text.Replace("«LCB»", item.LCB);
                }
                else if (text.Text.Contains("«PCDS»"))
                {
                    text.Text = text.Text.Replace("«PCDS»", item.PCDS);
                }
                else if (text.Text.Contains("«PCCV»"))
                {
                    text.Text = text.Text.Replace("«PCCV»", item.PCCV);
                }
                else if (text.Text.Contains("«PC_NL»"))
                {
                    text.Text = text.Text.Replace("«PC_NL»", item.PC_NL);
                }
                else if (text.Text.Contains("«Luong_D»"))
                {
                    text.Text = text.Text.Replace("«Luong_D»", item.Luong_D);
                }
                else if (text.Text.Contains("«Phép_năm_tồn»"))
                {
                    text.Text = text.Text.Replace("«Phép_năm_tồn»", item.PhepNamTon);
                }
                else if (text.Text.Contains("«PCTN»"))
                {
                    text.Text = text.Text.Replace("«PCTN»", item.PCTN);
                }
                else if (text.Text.Contains("«PCDH»"))
                {
                    text.Text = text.Text.Replace("«PCDH»", item.PCDH);
                }
                else if (text.Text.Contains("«Luong_H»"))
                {
                    text.Text = text.Text.Replace("«Luong_H»", item.Luong_H);
                }
                else if (text.Text.Contains("«TV_ngay»"))
                {
                    text.Text = text.Text.Replace("«TV_ngay»", item.TV_ngay);
                }
                else if (text.Text.Contains("«TV_dem»"))
                {
                    text.Text = text.Text.Replace("«TV_dem»", item.TV_dem);
                }
                else if (text.Text.Contains("«CT_ngay»"))
                {
                    text.Text = text.Text.Replace("«CT_ngay»", item.CT_ngay);
                }
                else if (text.Text.Contains("«CT_dem»"))
                {
                    text.Text = text.Text.Replace("«CT_dem»", item.CT_dem);
                }
                else if (text.Text.Contains("«Nghi_co_luong»"))
                {
                    text.Text = text.Text.Replace("«Nghi_co_luong»", item.Nghi_co_luong);
                }
                else if (text.Text.Contains("«Cong_ngay»"))
                {
                    text.Text = text.Text.Replace("«Cong_ngay»", item.Cong_ngay);
                }
                else if (text.Text.Contains("«Cong_dem»"))
                {
                    text.Text = text.Text.Replace("«Cong_dem»", item.Cong_dem);
                }
                else if (text.Text.Contains("«TTien_nghi_co_luong»"))
                {
                    text.Text = text.Text.Replace("«TTien_nghi_co_luong»", item.TTien_nghi_co_luong);
                }
                else if (text.Text.Contains("«nghi_KL»"))
                {
                    text.Text = text.Text.Replace("«nghi_KL»", item.nghi_KL);
                }
                else if (text.Text.Contains("«TT_lviec»"))
                {
                    text.Text = text.Text.Replace("«TT_lviec»", item.TT_lviec);
                }
                else if (text.Text.Contains("«Luong_theo_ngay_cong»"))
                {
                    text.Text = text.Text.Replace("«Luong_theo_ngay_cong»", item.Luong_theo_ngay_cong);
                }
                else if (text.Text.Contains("«OT_time_150»"))
                {
                    text.Text = text.Text.Replace("«OT_time_150»", item.OT_time_150);
                }
                else if (text.Text.Contains("«OT_time_200»"))
                {
                    text.Text = text.Text.Replace("«OT_time_200»", item.OT_time_200);
                }
                else if (text.Text.Contains("«OT_time_210»"))
                {
                    text.Text = text.Text.Replace("«OT_time_210»", item.OT_time_210);
                }
                else if (text.Text.Contains("«OT_time_270»"))
                {
                    text.Text = text.Text.Replace("«OT_time_270»", item.OT_time_270);
                }
                else if (text.Text.Contains("«OT_time_300»"))
                {
                    text.Text = text.Text.Replace("«OT_time_300»", item.OT_time_300);
                }
                else if (text.Text.Contains("«OT_time_390»"))
                {
                    text.Text = text.Text.Replace("«OT_time_390»", item.OT_time_390);
                }
                else if (text.Text.Contains("«OT_time_260»"))
                {
                    text.Text = text.Text.Replace("«OT_time_260»", item.OT_time_260);
                }
                else if (text.Text.Contains("«Cong15»"))
                {
                    text.Text = text.Text.Replace("«Cong15»", item.Cong15);
                }
                else if (text.Text.Contains("«Cong20»"))
                {
                    text.Text = text.Text.Replace("«Cong20»", item.Cong20);
                }
                else if (text.Text.Contains("«Cong21»"))
                {
                    text.Text = text.Text.Replace("«Cong21»", item.Cong21);
                }
                else if (text.Text.Contains("«Cong27»"))
                {
                    text.Text = text.Text.Replace("«Cong27»", item.Cong27);
                }
                else if (text.Text.Contains("«Cong30»"))
                {
                    text.Text = text.Text.Replace("«Cong30»", item.Cong30);
                }
                else if (text.Text.Contains("«COng39»"))
                {
                    text.Text = text.Text.Replace("«COng39»", item.COng39);
                }
                else if (text.Text.Contains("«COng26»"))
                {
                    text.Text = text.Text.Replace("«COng26»", item.COng26);
                }
                else if (text.Text.Contains("«HT_15_Total»"))
                {
                    text.Text = text.Text.Replace("«HT_15_Total»", item.HT_15_Total);
                }
                else if (text.Text.Contains("«HT_200_Total»"))
                {
                    text.Text = text.Text.Replace("«HT_200_Total»", item.HT_200_Total);
                }
                else if (text.Text.Contains("«HT_270_Total»"))
                {
                    text.Text = text.Text.Replace("«HT_270_Total»", item.HT_270_Total);
                }
                else if (text.Text.Contains("«HT_300_Total»"))
                {
                    text.Text = text.Text.Replace("«HT_300_Total»", item.HT_300_Total);
                }
                else if (text.Text.Contains("«HT_390_Total»"))
                {
                    text.Text = text.Text.Replace("«HT_390_Total»", item.HT_390_Total);
                }
                else if (text.Text.Contains("«Cong151»"))
                {
                    text.Text = text.Text.Replace("«Cong151»", item.Cong151);
                }
                else if (text.Text.Contains("«Cong201»"))
                {
                    text.Text = text.Text.Replace("«Cong201»", item.Cong201);
                }
                else if (text.Text.Contains("«Cong271»"))
                {
                    text.Text = text.Text.Replace("«Cong271»", item.Cong271);
                }
                else if (text.Text.Contains("«Cong301»"))
                {
                    text.Text = text.Text.Replace("«Cong301»", item.Cong301);
                }
                else if (text.Text.Contains("«COng391»"))
                {
                    text.Text = text.Text.Replace("«COng391»", item.COng391);
                }
                else if (text.Text.Contains("«Tong_OT»"))
                {
                    text.Text = text.Text.Replace("«Tong_OT»", item.Tong_OT);
                }
                else if (text.Text.Contains("«Tổng_HTLV»"))
                {
                    text.Text = text.Text.Replace("«Tổng_HTLV»", item.Tong_HTLV);
                }
                else if (text.Text.Contains("«Tổng_HTLV»"))
                {
                    text.Text = text.Text.Replace("«Tổng_HTLV»", item.Tong_HTLV);
                }
                else if (text.Text.Contains("«Ca_ngày_TV»"))
                {
                    text.Text = text.Text.Replace("«Ca_ngày_TV»", item.Ca_ngay_TV);
                }
                else if (text.Text.Contains("«Ca_ngày_CT»"))
                {
                    text.Text = text.Text.Replace("«Ca_ngày_CT»", item.Ca_ngay_CT);
                }
                else if (text.Text.Contains("«Ca_ngày_CT»"))
                {
                    text.Text = text.Text.Replace("«Ca_ngày_CT»", item.Ca_ngay_CT);
                }
                else if (text.Text.Contains("«ca_đêm_TV_kỷ_niệm_trước_lễ»"))
                {
                    text.Text = text.Text.Replace("«ca_đêm_TV_kỷ_niệm_trước_lễ»", item.ca_dem_TV_ky_niem_truoc_le);
                }
                else if (text.Text.Contains("«ca_đêm_kỷ_niệm_CT_trước_lễ»"))
                {
                    text.Text = text.Text.Replace("«ca_đêm_kỷ_niệm_CT_trước_lễ»", item.ca_dem_CT_ky_niem_truoc_le);
                }
                else if (text.Text.Contains("«Thành_tiền»"))
                {
                    text.Text = text.Text.Replace("«Thành_tiền»", item.Thanh_tien);
                }
                else if (text.Text.Contains("«Nghi_Bu_AL30»"))
                {
                    text.Text = text.Text.Replace("«Nghi_Bu_AL30»", item.Nghi_Bu_AL30);
                }
                else if (text.Text.Contains("«Nghi_Bu_NB»"))
                {
                    text.Text = text.Text.Replace("«Nghi_Bu_NB»", item.Nghi_Bu_NB);
                }
                else if (text.Text.Contains("«Hỗ_trợ_PC_NB»"))
                {
                    text.Text = text.Text.Replace("«Hỗ_trợ_PC_NB»", item.Ho_tro_PC_NB);
                }
                else if (text.Text.Contains("«Hỗ_trợ_lương_NB»"))
                {
                    text.Text = text.Text.Replace("«Hỗ_trợ_lương_NB»", item.Ho_tro_luong_NB);
                }
                else if (text.Text.Contains("«Tổng_hỗ_trợ_NB»"))
                {
                    text.Text = text.Text.Replace("«Tổng_hỗ_trợ_NB»", item.Tong_ho_tro_NB);
                }
                else if (text.Text.Contains("«số_ngày_nghỉ_70»"))
                {
                    text.Text = text.Text.Replace("«số_ngày_nghỉ_70»", item.so_ngay_nghi_70);
                }
                else if (text.Text.Contains("«Thành_tiền_nghỉ_70»"))
                {
                    text.Text = text.Text.Replace("«Thành_tiền_nghỉ_70»", item.thanh_tien_nghi_70);
                }
                else if (text.Text.Contains("«Cong_them»"))
                {
                    text.Text = text.Text.Replace("«Cong_them»", item.Cong_them);
                }
                else if (text.Text.Contains("«Chuyencan»"))
                {
                    text.Text = text.Text.Replace("«Chuyencan»", item.Chuyencan);
                }
                else if (text.Text.Contains("«Incentive»"))
                {
                    text.Text = text.Text.Replace("«Incentive»", item.Incentive);
                }
                else if (text.Text.Contains("«Thanh_toán_PN»"))
                {
                    text.Text = text.Text.Replace("«Thanh_toán_PN»", item.Thanh_toan_PN);
                }
                else if (text.Text.Contains("«HT_gui_tre»"))
                {
                    text.Text = text.Text.Replace("«HT_gui_tre»", item.HT_gui_tre);
                }
                else if (text.Text.Contains("«HT_PCCC_co_so»"))
                {
                    text.Text = text.Text.Replace("«HT_PCCC_co_so»", item.HT_PCCC_co_so);
                }
                else if (text.Text.Contains("«HT_ATNVSV»"))
                {
                    text.Text = text.Text.Replace("«HT_ATNVSV»", item.HT_ATNVSV);
                }
                else if (text.Text.Contains("«HT_CĐ»"))
                {
                    text.Text = text.Text.Replace("«HT_CĐ»", item.HT_CD);
                }
                else if (text.Text.Contains("«TN_khac»"))
                {
                    text.Text = text.Text.Replace("«TN_khac»", item.TN_khac);
                }
                else if (text.Text.Contains("«HT_Sinh_ly»"))
                {
                    text.Text = text.Text.Replace("«HT_Sinh_ly»", item.HT_Sinh_ly);
                }
                else if (text.Text.Contains("«Dem_TV»"))
                {
                    text.Text = text.Text.Replace("«Dem_TV»", item.Dem_TV);
                }
                else if (text.Text.Contains("«Ttien»"))
                {
                    text.Text = text.Text.Replace("«Ttien»", item.Ttien);
                }
                else if (text.Text.Contains("«Dem_CT»"))
                {
                    text.Text = text.Text.Replace("«Dem_CT»", item.Dem_CT);
                }
                else if (text.Text.Contains("«Ttien1»"))
                {
                    text.Text = text.Text.Replace("«Ttien1»", item.Ttien1);
                }
                else if (text.Text.Contains("«Cong_tru»"))
                {
                    text.Text = text.Text.Replace("«Cong_tru»", item.Cong_tru);
                }
                else if (text.Text.Contains("«BHXH»"))
                {
                    text.Text = text.Text.Replace("«BHXH»", item.BHXH);
                }
                else if (text.Text.Contains("«Truy_thu_BHYT»"))
                {
                    text.Text = text.Text.Replace("«Truy_thu_BHYT»", item.Truy_thu_BHYT);
                }
                else if (text.Text.Contains("«Cong_doan»"))
                {
                    text.Text = text.Text.Replace("«Cong_doan»", item.Cong_doan);
                }
                else if (text.Text.Contains("«thue_TNCN»"))
                {
                    text.Text = text.Text.Replace("«thue_TNCN»", item.thue_TNCN);
                }
                else if (text.Text.Contains("«Di_muon»"))
                {
                    text.Text = text.Text.Replace("«Di_muon»", item.Di_muon);
                }
                else if (text.Text.Contains("«hmuon»"))
                {
                    text.Text = text.Text.Replace("«hmuon»", item.hmuon);
                }
                else if (text.Text.Contains("«tru_khac»"))
                {
                    text.Text = text.Text.Replace("«tru_khac»", item.tru_khac);
                }
                else if (text.Text.Contains("«Quy_PCTT»"))
                {
                    text.Text = text.Text.Replace("«Quy_PCTT»", item.Quy_PCTT);
                }
                else if (text.Text.Contains("«Thuc_nhan»"))
                {
                    text.Text = text.Text.Replace("«Thuc_nhan»", item.Thuc_nhan);
                }
            }
        }

        private void SetPassword(string filePath, string password)
        {
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(filePath);
            document.Encrypt(password);
            document.Protect(Spire.Doc.ProtectionType.AllowOnlyReading);
            document.SaveToFile(filePath, Spire.Doc.FileFormat.Docx);
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
