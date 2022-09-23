using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
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
        private IBoPhanService _boPhanService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private ChamCongDataModel ChamCongData;
        private readonly IMemoryCache _memoryCache;
        private INhanVienService _nhanVienService;

        public BangCongController(INhanVienService nhanVienService, IBangCongService bangCongService, IBoPhanService boPhanService, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger, IMemoryCache memoryCache)
        {
            _bangCongService = bangCongService;
            _hostingEnvironment = hostEnvironment;
            _boPhanService = boPhanService;
            _nhanVienService = nhanVienService;
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
        public IActionResult Search(string timeEndUser, string status,string department, string timeTo)
        {
            if (status.NullString() == "")
            {
                status = Status.Active.ToString();
            }
            else
            {
                status = Status.InActive.ToString();
            }

            List<DeNghiLamThemGioModel> lstLamthemgio = new List<DeNghiLamThemGioModel>();
            var lst = _bangCongService.GetDataReport(timeEndUser, status, timeTo, department, ref lstLamthemgio);

            if (department.NullString() == "")
            {
                lst = lst.Where(x => x.BoPhan != "KOREA").ToList();
            }

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
            List<string> bophans = new List<string>();
            List<List<TongHopNhanSuDailyViewModel>> lstNhansutonghop = new List<List<TongHopNhanSuDailyViewModel>>();

            if (dept.NullString() == "")
            {
                var lstbp = _boPhanService.GetAll("").Where(x => x.Id != "KOREA");
                foreach (var item in lstbp)
                {
                    bophans.Add(item.TenBoPhan);
                }

                foreach (var item in bophans)
                {
                    var data = _bangCongService.TongHopNhanSuReport(time, item).OrderBy(x => x.NgayBaoCao).ToList();
                    if (data.Count > 0)
                    {
                        lstNhansutonghop.Add(data);
                    }
                }
            }
            else
            {
                bophans.Add(dept);
                var data = _bangCongService.TongHopNhanSuReport(time, dept).OrderBy(x => x.NgayBaoCao).ToList();
                if (data.Count == 0)
                {
                    return new BadRequestObjectResult("Not found data!");
                }

                lstNhansutonghop.Add(data);
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
                foreach (var item in bophans)
                {
                    package.Workbook.Worksheets.Copy("Data", item);
                }

                package.Workbook.Worksheets.Delete(package.Workbook.Worksheets["Data"]);

                string sheetName = "";
                foreach (var data in lstNhansutonghop)
                {
                    sheetName = data.FirstOrDefault().BoPhan;

                    if (sheetName.NullString() == "") continue;

                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

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
                                    else if (tt.TrucTiepGianTiep == "GianTiepSX" && tt.ChucVu == "STAFF")
                                    {
                                        worksheet.Cells["H" + (beginIndex + 2)].Value = "STAFF";
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
                                    else if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "STAFF PM")
                                    {
                                        worksheet.Cells["H" + (beginIndex + 1)].Value = "STAFF PM";
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
                                    else if (tt.TrucTiepGianTiep == "GianTiepSX" && tt.ChucVu == "STAFF PM")
                                    {
                                        for (int k = 0; k < 16; k++)
                                        {
                                            colName = GetExcelColumnName(k + 9);

                                            if (colName == "J" || colName == "W" || colName == "U")
                                            {
                                                continue;
                                            }

                                            // fill working status
                                            newColName = colName + (beginIndex + 5);
                                            UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                        }
                                    }
                                    else if (tt.TrucTiepGianTiep == "GianTiepSX" && tt.ChucVu == "STAFF")
                                    {
                                        worksheet.Cells["H" + (beginIndex + 5)].Value = "STAFF";
                                        for (int k = 0; k < 16; k++)
                                        {
                                            colName = GetExcelColumnName(k + 9);

                                            if (colName == "J" || colName == "W" || colName == "U")
                                            {
                                                continue;
                                            }

                                            // fill working status
                                            newColName = colName + (beginIndex + 5);
                                            UpdateDataNsu(colName, newColName, tt, ref worksheet);
                                        }
                                    }
                                    else
                                    if (tt.TrucTiepGianTiep == "TrucTiepSX" && tt.ChucVu == "STAFF PM")
                                    {
                                        worksheet.Cells["H" + (beginIndex + 4)].Value = "STAFF PM";
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
                            cellfrom = "A" + (beginIndex + 6) + ":X" + (beginIndex + 11);
                            cellTo = "A" + (beginIndex + 12) + ":X" + (beginIndex + 17);
                            worksheet.Cells[cellfrom].Copy(worksheet.Cells[cellTo]);
                        }

                        beginIndex += 6;
                        if (data.Count == 1)
                        {
                            worksheet.DeleteRow(11, 6);
                        }
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
        public IActionResult OutPutExcel(string timeEndUser, string status,string department, string timeTo)
        {
            _memoryCache.TryGetValue("ChamCongData", out ChamCongData);
            if (!_memoryCache.TryGetValue("ChamCongData", out ChamCongData) || ChamCongData?.ChamCongData?.Count == 0 || ChamCongData?.ChamCongData?.FirstOrDefault()?.BoPhan != department)
            {
                List<DeNghiLamThemGioModel> lstLamthemgio = new List<DeNghiLamThemGioModel>();
                var lst = _bangCongService.GetDataReport(timeEndUser, status, timeTo, department, ref lstLamthemgio);

                
                if (lst.Count == 0)
                {
                    return new BadRequestObjectResult("Not found data!");
                }
                else
                {
                    string time = timeTo + "-01";
                    ChamCongData = new ChamCongDataModel(lst, time);
                    _memoryCache.Remove("ChamCongData");
                    _memoryCache.Set("ChamCongData", ChamCongData);
                }
            }

            List<ChamCongDataViewModel> data = ChamCongData.ChamCongData;

            if (department.NullString() == "")
            {
                data = data.Where(x => x.BoPhan != "KOREA").ToList();
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = "";
            string fileTemp = "BangCongTmp.xlsx";
            if (department.NullString() == "KOREA")
            {
                sFileName = $"chamCong_KOREA_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
                fileTemp = "BangCongKoreaTemplate.xlsx";
            }
            else
            {
                sFileName = $"chamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            }

            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), fileTemp));

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
                if (department.NullString() == "KOREA")
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["BangCong"];
                    worksheet.Name = timeTo;
                    worksheet.Cells["A1"].Value = timeTo + "-01";

                    DateTime fromTime = DateTime.Parse(timeTo + "-01");
                    DateTime endTime = fromTime.AddMonths(1).AddDays(-1);

                    int beginColIndex = 3;
                    string headerColName = "";
                    foreach (var day in EachDay.EachDays(fromTime, endTime))
                    {
                        headerColName = GetExcelColumnName(beginColIndex);
                        worksheet.Cells[headerColName + "2"].Value = day.ToString("yyyy-MM-dd");
                        // worksheet.Cells[headerColName + "1"].Value = day.DayOfWeek.ToString();
                        beginColIndex += 3;
                    }

                    int beginRowIndex = 4;
                    string colName = "";
                    string colName1 = "";
                    string colName2 = "";
                    int k = 0;
                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cells["A" + beginRowIndex].Value = data[i].TenNV;

                        k = 3;
                        for (int j = 1; j <= 31; j++)
                        {
                            colName = GetExcelColumnName(k);
                            colName1 = GetExcelColumnName(k+1);
                            colName2 = GetExcelColumnName(k+2);

                            foreach (var inout in data[i].TimeInOutModels.OrderBy(x => x.DayCheck))
                            {
                                if (int.Parse(inout.DayCheck.Substring(8, 2)) == j)
                                {
                                    worksheet.Cells[colName + beginRowIndex].Value = inout.InTime.TimeHHMM();
                                    worksheet.Cells[colName1 + beginRowIndex].Value = inout.OutTime.TimeHHMM();
                                    worksheet.Cells[colName2 + beginRowIndex].Value = inout.HangMuc.NullString();
                                }
                            }

                            k += 3;
                        }

                        if (i < data.Count - 2)
                        {
                            worksheet.Cells["A" + (beginRowIndex + 1) + ":CW" + (beginRowIndex + 1)].Copy(worksheet.Cells["A" + (beginRowIndex + 2) + ":CW" + (beginRowIndex + 2)]);
                        }

                        beginRowIndex += 1;
                    }

                    if (data.Count == 1)
                    {
                        worksheet.DeleteRow(5, 1);
                    }

                    package.Save(); //Save the workbook.
                }
                else
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
        public IActionResult ExportDenghiOT(string timeEndUser, string bophan, string fromTime, string endTime)
        {
            List<DeNghiLamThemGioModel> lstlamthem = new List<DeNghiLamThemGioModel>();
            string timeTo = fromTime.Substring(0, 7);
            var lst = _bangCongService.GetDataReport("", "", timeTo, bophan.NullString(), ref lstlamthem);

            List<DeNghiLamThemGioModel> data = new List<DeNghiLamThemGioModel>();
            if (bophan.NullString() != "")
            {
                data = lstlamthem.Where(x => x.BoPhan == bophan && x.NgayDangKy.CompareTo(fromTime) >= 0 && x.NgayDangKy.CompareTo(endTime) <= 0).OrderBy(x => x.MaNV).ToList();
            }
            else
            {
                data = lstlamthem.Where(x => x.NgayDangKy.CompareTo(fromTime) >= 0 && x.NgayDangKy.CompareTo(endTime) <= 0).OrderBy(x => x.MaNV).ToList();
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

                    var lstOfDay = data.Where(x => x.NgayDangKy == dayCheck).OrderBy(x => x.From).ToList();

                    if (lstOfDay.Count() == 0)
                    {
                        package.Workbook.Worksheets.Delete(worksheet);
                    }
                    else
                    {
                        if (lstOfDay.Count > 50)
                        {
                            worksheet.InsertRow(11, lstOfDay.Count - 50);

                            for (int c = 11; c < 11 + lstOfDay.Count - 50; c++)
                            {
                                worksheet.Cells["A10:I10"].Copy(worksheet.Cells["A" + c + ":I" + c]);
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

        [HttpPost]
        public IActionResult ExportBaoCaoOT(string timeEndUser, string bophan, string fromTime, string endTime)
        {
            List<DeNghiLamThemGioModel> lstlamthem = new List<DeNghiLamThemGioModel>();
            string timeTo = fromTime.Substring(0, 7);
            var lst = _bangCongService.GetDataReport("", "", timeTo, bophan.NullString(), ref lstlamthem);

            List<DeNghiLamThemGioModel> data = new List<DeNghiLamThemGioModel>();
            if (bophan.NullString() != "")
            {
                data = lstlamthem.Where(x => x.BoPhan == bophan && x.NgayDangKy.CompareTo(fromTime) >= 0 && x.NgayDangKy.CompareTo(endTime) <= 0).OrderBy(x => x.MaNV).ToList();
            }
            else
            {
                data = lstlamthem.Where(x => x.NgayDangKy.CompareTo(fromTime) >= 0 && x.NgayDangKy.CompareTo(endTime) <= 0).OrderBy(x => x.MaNV).ToList();
            }

            string status = Status.InActive.NullString();
            int dayFrom = DateTime.Parse(fromTime).Day;
            int dayTo = DateTime.Parse(endTime).Day;

            if (bophan.NullString() != "")
            {
                var lstNvien = _nhanVienService.GetAll().Where(x => x.Status != status && x.MaBoPhan == bophan).ToList();
                string daycheck = timeTo;
                foreach (var item in lstNvien)
                {
                    for (int i = dayFrom; i <= dayTo; i++)
                    {
                        daycheck = timeTo;

                        if (i < 10)
                        {
                            daycheck += "-0" + i;
                        }
                        else
                        {
                            daycheck += "-" + i;
                        }

                        if (!data.Any(x => x.MaNV == item.Id && x.NgayDangKy == daycheck))
                        {
                            data.Add(new DeNghiLamThemGioModel()
                            {
                                MaNV = item.Id,
                                BoPhan = item.MaBoPhan,
                                Duration = "0",
                                NgayDangKy = daycheck,
                                TenNV = item.TenNV
                            });
                        }
                    }
                }
            }
            else
            {
                var lstNvien = _nhanVienService.GetAll().Where(x => x.Status != status).ToList();
                string daycheck = timeTo;
                foreach (var item in lstNvien)
                {
                    for (int i = dayFrom; i <= dayTo; i++)
                    {
                        daycheck = timeTo;

                        if (i < 10)
                        {
                            daycheck += "-0" + i;
                        }
                        else
                        {
                            daycheck += "-" + i;
                        }

                        if (!data.Any(x => x.MaNV == item.Id && x.NgayDangKy == daycheck))
                        {
                            data.Add(new DeNghiLamThemGioModel()
                            {
                                MaNV = item.Id,
                                BoPhan = item.MaBoPhan,
                                Duration = "0",
                                NgayDangKy = daycheck,
                                TenNV = item.TenNV
                            });
                        }
                    }
                }
            }

            List<DeNghiLamThemGioModel> data2 = new List<DeNghiLamThemGioModel>();
            DeNghiLamThemGioModel model;
            double duration = 0;
            foreach (var item in data.ToList().GroupBy(x => new { x.MaNV, x.NgayDangKy }).Select(y => y))
            {
                model = new DeNghiLamThemGioModel()
                {
                    MaNV = item.Key.MaNV,
                    NgayDangKy = item.Key.NgayDangKy
                };
                duration = 0;
                foreach (var sub in item)
                {
                    model.TenNV = sub.TenNV;
                    model.BoPhan = sub.BoPhan;

                    if (!string.IsNullOrEmpty(sub.Duration.NullString()) && double.TryParse(sub.Duration, out _))
                    {
                        duration += double.Parse(sub.Duration);
                    }
                }
                model.Duration = duration.IfNullIsZero();
                data2.Add(model);
            };

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"BaoCaoLamThemGio_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "BaoCaoLamThemGio.xlsx"));
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

                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                string month = from.ToString("yyyy-MM");

                worksheet.Cells["L1"].Value = "DỮ LIỆU OT THÁNG " + month + "_" + bophan;

                int no = 0;
                string colName = "";
                var lstData = data2.GroupBy(x => new { x.MaNV, x.BoPhan, x.TenNV }).Select(y => y).OrderBy(x => x.Key.BoPhan).ThenBy(x => x.Key.MaNV);

                if (lstData.Count() > 10)
                {
                    worksheet.InsertRow(11, lstData.Count());

                    for (int c = 11; c < 11 + lstData.Count(); c++)
                    {
                        worksheet.Cells["A10:AI10"].Copy(worksheet.Cells["A" + c + ":AI" + c]);
                    }
                }

                foreach (var item in lstData)
                {
                    worksheet.Cells["A" + (no + 5)].Value = no + 1;
                    worksheet.Cells["B" + (no + 5)].Value = item.Key.BoPhan;
                    worksheet.Cells["C" + (no + 5)].Value = item.Key.MaNV;
                    worksheet.Cells["D" + (no + 5)].Value = item.Key.TenNV;

                    foreach (var sub in item)
                    {
                        for (int i = 0; i < 31; i++)
                        {
                            if (int.Parse(sub.NgayDangKy.Substring(8, 2)) == i + 1)
                            {
                                colName = GetExcelColumnName(i + 5);
                                worksheet.Cells[colName + (no + 5)].Value = double.Parse(sub.Duration.IfNullIsZero());
                            } //yyyy-MM-dd
                        }
                    }

                    no += 1;
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
