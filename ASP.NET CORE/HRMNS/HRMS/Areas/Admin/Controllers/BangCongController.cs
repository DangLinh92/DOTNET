using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class BangCongController : AdminBaseController
    {
        private IBangCongService _bangCongService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private ChamCongDataModel ChamCongData;
        private readonly IMemoryCache _memoryCache;

        public BangCongController(IBangCongService bangCongService, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger, IMemoryCache memoryCache)
        {
            _bangCongService = bangCongService;
            _hostingEnvironment = hostEnvironment;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            ChamCongData = new ChamCongDataModel(new List<ChamCongDataViewModel>(), "");
            ViewBag.DayOfMonths = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            _memoryCache.Remove("ChamCongData");
            _memoryCache.Set("ChamCongData", ChamCongData);
            return View(new List<ChamCongDataViewModel>());
        }

        [HttpPost]
        public IActionResult Search(string department, string timeTo)
        {
            List<DeNghiLamThemGioModel> lstLamthemgio = new List<DeNghiLamThemGioModel>();
            var lst = _bangCongService.GetDataReport(timeTo, department, ref lstLamthemgio);
            string time = timeTo + "-01";
            ViewBag.DayOfMonths = DateTime.DaysInMonth(DateTime.Parse(time).Year, DateTime.Parse(time).Month);

            ChamCongData = new ChamCongDataModel(lst, time);

            _memoryCache.Remove("ChamCongData");
            _memoryCache.Set("ChamCongData", ChamCongData);

            return PartialView("_gridBangCongPartialView", lst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TongHopNhanSuDaily(string time, string dept)
        {
            var data = _bangCongService.TongHopNhanSuReport(time, dept).OrderBy(x => x.NgayBaoCao).ToList();
            if (data.Count == 0)
            {
                return new BadRequestObjectResult("Not found data!");
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"Tonghopnhansu_" + dept + $"_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "TongHopNhanSuTemplate.xlsx"));
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
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                int beginIndex = 5;
                string cellfrom = "";
                string cellTo = "";
                string colName = "";
                string newColName = "";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells["A" + beginIndex].Value = dept;
                    worksheet.Cells["B" + beginIndex].Value = data[i].TongNV;
                    worksheet.Cells["C" + beginIndex].Value = data[i].NghiTS;
                    worksheet.Cells["E" + beginIndex].Value = data[i].NgayBaoCao;

                    foreach (var calv in data[i].CaLamViec_Value)
                    {
                        if (calv.CalamViec == "CN_WHC")
                        {
                            foreach (var tt in calv.ThongTins)
                            {
                                if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "OP")
                                {
                                    for (int k = 0; k < 16; k++)
                                    {
                                        colName = GetExcelColumnName(k + 9);

                                        if (colName == "J" || colName == "W" || colName == "U")
                                        {
                                            continue;
                                        }

                                        // fill working status
                                        newColName = colName + (beginIndex + 0);
                                        UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                    }
                                }
                                else
                                 if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "STAFF")
                                {
                                    for (int k = 0; k < 16; k++)
                                    {
                                        colName = GetExcelColumnName(k + 9);

                                        if (colName == "J" || colName == "W" || colName == "U")
                                        {
                                            continue;
                                        }

                                        // fill working status
                                        newColName = colName + (beginIndex + 1);
                                        UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                    }
                                }
                                else
                                if (tt.TrucTiepGianTiep == "GianTiepSX" && tt.ChucVu == "STAFF PM")
                                {
                                    for (int k = 0; k < 16; k++)
                                    {
                                        colName = GetExcelColumnName(k + 9);

                                        if (colName == "J" || colName == "W" || colName == "U")
                                        {
                                            continue;
                                        }

                                        // fill working status
                                        newColName = colName + (beginIndex + 2);
                                        UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                    }
                                }
                            }
                        }
                        else if (calv.CalamViec == "CD_WHC")
                        {
                            foreach (var tt in calv.ThongTins)
                            {
                                if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "OP")
                                {
                                    for (int k = 0; k < 16; k++)
                                    {
                                        colName = GetExcelColumnName(k + 9);

                                        if (colName == "J" || colName == "W" || colName == "U")
                                        {
                                            continue;
                                        }

                                        // fill working status
                                        newColName = colName + (beginIndex + 3);
                                        UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                    }
                                }
                                else
                                if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "STAFF")
                                {
                                    for (int k = 0; k < 16; k++)
                                    {
                                        colName = GetExcelColumnName(k + 9);

                                        if (colName == "J" || colName == "W" || colName == "U")
                                        {
                                            continue;
                                        }

                                        // fill working status
                                        newColName = colName + (beginIndex + 4);
                                        UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                    }
                                }
                            }
                        }
                    }

                    if (i < data.Count - 2)
                    {
                        // copy range cell
                        cellfrom = "A" + (beginIndex + 5) + ":X" + (beginIndex + 9);
                        cellTo = "A" + (beginIndex + 10) + ":X" + (beginIndex + 14);
                        worksheet.Cells[cellfrom].Copy(worksheet.Cells[cellTo]);
                    }

                    beginIndex += 5;
                    if (data.Count == 1)
                    {
                        worksheet.DeleteRow(10, 5);
                    }
                }

                package.Save(); // Save the workbook.
            }

            //return RedirectToAction("Export", "Spreadsheet", new { area = "Admin", DocumentPath = sFileName });
            return new OkObjectResult(sFileName);
        }

        private void UpdateDataNsu(string colName, string newColName, TrucTiepGianTiepSL tt, ref ExcelWorksheet worksheet)
        {
            if (colName == "I" && tt.TongSoNguoi > 0)
            {
                worksheet.Cells[newColName].Value = tt.TongSoNguoi;
            }
            else
            if (colName == "K" && tt.DiMuon > 0)
            {
                worksheet.Cells[newColName].Value = tt.DiMuon;
            }
            else
            if (colName == "L" && tt.VeSom > 0)
            {
                worksheet.Cells[newColName].Value = tt.VeSom;
            }
            else
            if (colName == "M" && tt.NghiPhep > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiPhep;
            }
            else
            if (colName == "N" && tt.NghiKhongLuong > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiKhongLuong;
            }
            else
             if (colName == "O" && tt.NghiKhongThongBao > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiKhongThongBao;
            }
            else
            if (colName == "P" && tt.NghiHuongLuong70 > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiHuongLuong70;
            }
            else
            if (colName == "Q" && tt.NghiDacBiet > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiDacBiet;
            }
            else
             if (colName == "R" && tt.DiCongTac > 0)
            {
                worksheet.Cells[newColName].Value = tt.DiCongTac;
            }
            else
             if (colName == "S" && tt.NghiOm > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiOm;
            }
            else
              if (colName == "T" && tt.NghiViec > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiViec;
            }
            else
            if (colName == "V" && tt.NghiLe > 0)
            {
                worksheet.Cells[newColName].Value = tt.NghiLe;
            }
            else
            if (colName == "X")
            {
                worksheet.Cells[newColName].Value = tt.Note.NullString();
            }
        }

        /// <summary>
        /// Xuat file cham cong - can cấp quyền cho IIS trên folder root để xử lý file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OutPutExcel()
        {
            _memoryCache.TryGetValue("ChamCongData", out ChamCongData);
            if (!_memoryCache.TryGetValue("ChamCongData", out ChamCongData) || ChamCongData?.ChamCongData?.Count == 0)
            {
                return new BadRequestObjectResult("Not found data!");
            }

            List<ChamCongDataViewModel> data = ChamCongData.ChamCongData;

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"chamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "BangCongTmp.xlsx"));
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
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                // TIEU ĐỀ
                worksheet.Cells["R1"].Value = "MONTHLY ATTENDANCE RECORD OF " + GetMonthYearEng(ChamCongData.TimeChamCong);
                worksheet.Cells["R2"].Value = "BẢNG CHẤM CÔNG THÁNG " + ChamCongData.TimeChamCong.Split("-")[1] + " NĂM " + ChamCongData.TimeChamCong.Split("-")[0];

                worksheet.Cells["A1"].Value = ChamCongData.TimeChamCong.Replace("-", "");

                int beginIndex = 17;
                string cellfrom = "";
                string cellTo = "";
                string colName = "";
                string newColName = "";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells["A" + beginIndex].Value = (i + 1);
                    worksheet.Cells["B" + beginIndex].Value = data[i].MaNV;
                    worksheet.Cells["C" + beginIndex].Value = data[i].TenNV;
                    worksheet.Cells["D" + beginIndex].Value = data[i].NgayVao;
                    worksheet.Cells["E" + beginIndex].Value = data[i].BoPhanDetail;
                    worksheet.Cells["CN" + beginIndex].Value = data[i].VP_SX;

                    for (int j = 1; j <= 31; j++)
                    {
                        colName = GetExcelColumnName(j + 7);
                        // fill working status
                        newColName = colName + beginIndex;
                        foreach (var ws in data[i].WorkingStatuses)
                        {
                            if (int.Parse(ws.DayCheck.Substring(8, 2)) == j)  // 2022-12-03
                            {
                                if (ws.Value != "-")
                                {
                                    worksheet.Cells[newColName].Value = ws.Value;
                                }

                                if (ws.Value != "DS" && ws.Value != "NS" && ws.Value != "-")
                                {
                                    worksheet.Cells[newColName].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[newColName].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 204, 153));
                                }
                                break;
                            }
                        }

                        // fill OT
                        for (int k = 1; k <= 6; k++) // 150,200,210,270,300,390
                        {
                            newColName = colName + (beginIndex + k);

                            foreach (var ot in data[i].OvertimeValues)
                            {
                                if (int.Parse(ot.DayCheckOT.Substring(8, 2)) == j)  // 2022-12-03
                                {
                                    if (k == 1 && ot.DMOvertime == "150") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                    else
                                    if (k == 2 && ot.DMOvertime == "200") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                    else
                                    if (k == 3 && ot.DMOvertime == "210") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                    else
                                    if (k == 4 && ot.DMOvertime == "270") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                    else
                                    if (k == 5 && ot.DMOvertime == "300") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                    else
                                    if (k == 6 && ot.DMOvertime == "390") // 150
                                    {
                                        worksheet.Cells[newColName].Value = ot.ValueOT;
                                    }
                                }
                            }
                        }

                        // fill EL/LC
                        newColName = colName + (beginIndex + 7);
                        foreach (var st in data[i].EL_LC_Statuses)
                        {
                            if (int.Parse(st.DayCheck_EL.Substring(8, 2)) == j)  // 2022-12-03
                            {
                                if (st.Value_EL > 0)
                                {
                                    worksheet.Cells[newColName].Value = st.Value_EL;
                                    worksheet.Cells[newColName].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[newColName].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 217, 179));
                                }
                            }
                        }
                    }

                    if (i < data.Count - 2)
                    {
                        // copy range cell
                        cellfrom = "A" + (beginIndex + 8) + ":CO" + (beginIndex + 16);
                        cellTo = "A" + (beginIndex + 16) + ":CO" + (beginIndex + 24);
                        worksheet.Cells[cellfrom].Copy(worksheet.Cells[cellTo]);

                        // format sum total
                        string hAM = "";
                        for (int h = 39; h <= 91; h++) // AM ->
                        {
                            hAM = GetExcelColumnName(h);
                            worksheet.Cells[hAM + (beginIndex + 24)].Formula = "SUM(" + hAM + "17:" + hAM + (beginIndex + 16) + ")";
                        }
                    }

                    if (i < data.Count - 1)
                    {
                        // format ngay chu nhat mau xanh
                        for (int d = 1; d <= 31; d++)
                        {
                            colName = GetExcelColumnName(d + 7);

                            if (DateTime.DaysInMonth(DateTime.Parse(ChamCongData.TimeChamCong).Year, DateTime.Parse(ChamCongData.TimeChamCong).Month) >= d)
                            {
                                var ex = worksheet.Cells[colName + (beginIndex + 8) + ":" + colName + (beginIndex + 15)].ConditionalFormatting.AddExpression();
                                ex.Formula = string.Format("IF(" + colName + "$16=\"{0}\",1,0)", "Sun");
                                ex.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ex.Style.Fill.BackgroundColor.Color = Color.FromArgb(155, 194, 230);
                            }
                        }
                    }

                    beginIndex += 8;
                    if (data.Count == 1)
                    {
                        worksheet.DeleteRow(25, 8);
                    }
                }

                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private string GetMonthYearEng(string time)
        {
            int month = int.Parse(time.Split("-")[1]);
            string year = time.Split("-")[0];
            string result = "";

            switch (month)
            {
                case 1:
                    result += "JAN, " + year;
                    break;
                case 2:
                    result += "FEB, " + year;
                    break;
                case 3:
                    result += "MAR, " + year;
                    break;
                case 4:
                    result += "APR, " + year;
                    break;
                case 5:
                    result += "MAY, " + year;
                    break;
                case 6:
                    result += "JUN, " + year;
                    break;
                case 7:
                    result += "JUL, " + year;
                    break;
                case 8:
                    result += "AUG, " + year;
                    break;
                case 9:
                    result += "SEP, " + year;
                    break;
                case 10:
                    result += "OCT, " + year;
                    break;
                case 11:
                    result += "NOV, " + year;
                    break;
                case 12:
                    result += "DEC, " + year;
                    break;
                default:
                    break;
            }

            return result;
        }

        [HttpPost]
        public IActionResult ExportDenghiOT(string bophan, string fromTime, string endTime)
        {
            List<DeNghiLamThemGioModel> lstlamthem = new List<DeNghiLamThemGioModel>();
            string timeTo = fromTime.Substring(0, 7);
            var lst = _bangCongService.GetDataReport(timeTo, bophan, ref lstlamthem);

            List<DeNghiLamThemGioModel> data = new List<DeNghiLamThemGioModel>();
            if (bophan != "")
            {
                data = lstlamthem.Where(x => x.BoPhan == bophan && x.NgayDangKy.CompareTo(fromTime) >= 0 && x.NgayDangKy.CompareTo(endTime) <= 0).OrderBy(x => x.MaNV).ToList();
            }
            else
            {
                data = lstlamthem.OrderBy(x => x.MaNV).ToList();
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"DeNghiLamThemGio_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "DeNghiLamThemGioTemp.xlsx"));
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
                DateTime from = DateTime.Parse(fromTime);
                DateTime to = DateTime.Parse(endTime);
                for (int i = from.Day; i <= to.Day; i++)
                {
                    if (i < 10)
                    {
                        package.Workbook.Worksheets.Copy("Data", "0" + i);
                    }
                    else
                    {
                        package.Workbook.Worksheets.Copy("Data", i.NullString());
                    }
                }

                package.Workbook.Worksheets.Delete(package.Workbook.Worksheets["Data"]);

                ExcelWorksheet worksheet;
                string month = from.ToString("yyyy-MM");
                string dayCheck = "";
                for (int i = from.Day; i <= to.Day; i++)
                {
                    if (i < 10)
                    {
                        worksheet = package.Workbook.Worksheets["0" + i];
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets[i.NullString()];
                    }

                    worksheet.Cells["A5"].Value = "Tên bộ phận/ Section: " + bophan + "_Ca  A +B";

                    if (i < 10)
                    {
                        dayCheck = month + "-0" + i;
                        worksheet.Cells["F5"].Value = month + "-0" + i;
                    }
                    else
                    {
                        dayCheck = month + "-" + i;
                        worksheet.Cells["F5"].Value = month + "-" + i;
                    }

                    var lstOfDay = data.Where(x => x.NgayDangKy == dayCheck).OrderBy(x=>x.From).ToList();

                    if (lstOfDay.Count() == 0)
                    {
                        package.Workbook.Worksheets.Delete(worksheet);
                    }
                    else
                    {
                        if (lstOfDay.Count > 50)
                        {
                            worksheet.InsertRow(11, lstOfDay.Count - 50);

                            for (int c = 11; c < 11+ lstOfDay.Count - 50; c++)
                            {
                                worksheet.Cells["A10:I10"].Copy(worksheet.Cells["A"+c+":I"+c]);
                            }
                        }

                        for (int k = 9; k < lstOfDay.Count + 9; k++)
                        {
                            worksheet.Cells["A" + k].Value = (k - 8);
                            worksheet.Cells["B" + k].Value = lstOfDay[k - 9].MaNV;
                            worksheet.Cells["C" + k].Value = lstOfDay[k - 9].TenNV;
                            worksheet.Cells["D" + k].Value = lstOfDay[k - 9].From;
                            worksheet.Cells["E" + k].Value = lstOfDay[k - 9].To;
                            worksheet.Cells["F" + k].Value = double.Parse(lstOfDay[k - 9].Duration);
                            worksheet.Cells["I" + k].Value = lstOfDay[k - 9].Note;
                        }
                    }
                }

                package.Save();
            }

            return new OkObjectResult(fileUrl);
        }
    }

    public class ChamCongDataModel
    {
        public List<ChamCongDataViewModel> ChamCongData { get; set; }
        public string TimeChamCong { get; set; }
        public ChamCongDataModel(List<ChamCongDataViewModel> data, string time)
        {
            ChamCongData = data;
            TimeChamCong = time;
        }
    }
}
