using DevExpress.Office.Utils;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMS.Infrastructure.Interfaces;
using HRMS.ScheduledTasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Quartz;
using Quartz.Impl;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

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
        private INgayChotCongService _ngayChotCongService;


        public BangCongController(INgayChotCongService ngayChotCongService, INhanVienService nhanVienService, IBangCongService bangCongService, IBoPhanService boPhanService, IWebHostEnvironment hostEnvironment, ILogger<NhanVien_CaLamViecController> logger, IMemoryCache memoryCache)
        {
            _bangCongService = bangCongService;
            _hostingEnvironment = hostEnvironment;
            _boPhanService = boPhanService;
            _nhanVienService = nhanVienService;
            _logger = logger;
            _ngayChotCongService = ngayChotCongService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            ChamCongData = new ChamCongDataModel(new List<ChamCongDataViewModel>(), "");
            ViewBag.DayOfMonths = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            ViewBag.NgayChotCongNextMonth = DateTime.Parse(_ngayChotCongService.FinLastItem().ChotCongChoThang).AddMonths(2).ToString("yyyy-MM-dd");

            _memoryCache.Remove("ChamCongData");
            _memoryCache.Set("ChamCongData", ChamCongData);
            return View(new List<ChamCongDataViewModel>());
        }

        [HttpPost]
        public IActionResult Search(string timeEndUser, string status, string status_all, string department, string timeTo)
        {
            if (status_all.NullString() == "")
            {
                if (status.NullString() == "")
                {
                    status = Status.Active.ToString();
                }
                else
                {
                    status = Status.InActive.ToString();
                }
            }
            else
            {
                status = "";
            }

            List<DeNghiLamThemGioModel> lstLamthemgio = new List<DeNghiLamThemGioModel>();
            var lst = _bangCongService.GetDataReport(timeEndUser, status, timeTo, department, ref lstLamthemgio);

            if (department.NullString() == "")
            {
                lst = lst.Where(x => x.BoPhan != "KOREA").ToList();
            }
            else
            if (department.NullString() == "KOREA")
            {
                lst = lst.OrderBy(x => x.OrderBy).ToList();
            }

            string time = timeTo + "-01";
            ViewBag.DayOfMonths = DateTime.DaysInMonth(DateTime.Parse(time).Year, DateTime.Parse(time).Month);
            ViewBag.NgayChotCongNextMonth = DateTime.Parse(_ngayChotCongService.FinLastItem().ChotCongChoThang).AddMonths(2).ToString("yyyy-MM-dd");

            ChamCongData = new ChamCongDataModel(lst, time);

            _memoryCache.Remove("ChamCongData");
            _memoryCache.Set("ChamCongData", ChamCongData);

            return PartialView("_gridBangCongPartialView", lst);
        }

        [HttpPost]
        public async Task<IActionResult> ChotBangCong(string timeEndUser, string thang)
        {
            string _month = thang + "-01";
            if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM") + "-01").AddMonths(-1).ToString("yyyy-MM-dd") == _month)
            {
                //HR_NgayChotCongViewModel ngayChot = new HR_NgayChotCongViewModel()
                //{
                //    NgayChotCong = DateTime.Now.ToString("yyyy-MM-dd"),
                //    ChotCongChoThang = _month
                //};
                //_ngayChotCongService.Update(ngayChot);

                await ChotCongFinal();
                UpdateBangCongExtention(timeEndUser, thang);
                return new OkObjectResult(thang);
            }

            return new BadRequestObjectResult(thang);
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
                bool isExist = false;
                List<string> lstDelete = new List<string>();
                foreach (var sh in package.Workbook.Worksheets )
                {
                    isExist = false;
                    foreach (var item in bophans)
                    {
                        if(item == sh.Name)
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist)
                    {
                        lstDelete.Add(sh.Name);
                       
                    }
                    // package.Workbook.Worksheets.Copy("Data", item);
                    // package.Workbook.Worksheets.Delete(package.Workbook.Worksheets["Data"]);
                }

                foreach (var item in lstDelete)
                {
                    package.Workbook.Worksheets.Delete(package.Workbook.Worksheets[item]);
                }

                //package.Workbook.Worksheets.Delete(package.Workbook.Worksheets["Data"]);

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
        public IActionResult OutPutExcel(string timeEndUser, string status, string department, string timeTo)
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
            else
            if (department.NullString() == "KOREA")
            {
                data = data.OrderBy(x => x.OrderBy).ToList(); ;
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
                        beginColIndex += 4;
                    }

                    int beginRowIndex = 4;
                    string colName = "";
                    string colName1 = "";
                    string colName2 = "";
                    string colName3 = "";
                    int k = 0;
                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cells["A" + beginRowIndex].Value = data[i].TenNV;

                        k = 3;
                        for (int j = 1; j <= 31; j++)
                        {
                            colName = GetExcelColumnName(k);
                            colName1 = GetExcelColumnName(k + 1);
                            colName2 = GetExcelColumnName(k + 2);
                            colName3 = GetExcelColumnName(k + 3);

                            foreach (var inout in data[i].TimeInOutModels.OrderBy(x => x.DayCheck))
                            {
                                if (int.Parse(inout.DayCheck.Substring(8, 2)) == j)
                                {
                                    worksheet.Cells[colName + beginRowIndex].Value = inout.InTime.TimeHHMM();
                                    worksheet.Cells[colName1 + beginRowIndex].Value = inout.OutTime.TimeHHMM();
                                    worksheet.Cells[colName2 + beginRowIndex].Value = inout.HangMuc.NullString();
                                    worksheet.Cells[colName3 + beginRowIndex].Value = inout.Draf.NullString();
                                }
                            }

                            k += 4;
                        }

                        if (i < data.Count - 2)
                        {
                            worksheet.Cells["A" + (beginRowIndex + 1) + ":EB" + (beginRowIndex + 1)].Copy(worksheet.Cells["A" + (beginRowIndex + 2) + ":EB" + (beginRowIndex + 2)]);
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
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // TIEU ĐỀ
                    worksheet.Cells["R1"].Value = "MONTHLY ATTENDANCE RECORD OF " + GetMonthYearEng(ChamCongData.TimeChamCong);
                    worksheet.Cells["R2"].Value = "BẢNG CHẤM CÔNG THÁNG " + ChamCongData.TimeChamCong.Split("-")[1] + " NĂM " + ChamCongData.TimeChamCong.Split("-")[0];

                    worksheet.Cells["A1"].Value = ChamCongData.TimeChamCong.Replace("-", "");

                    int beginIndex = 17;
                    string cellfrom = "";
                    string cellTo = "";
                    string colName = "";
                    string newColName = "";

                    List<ChamCongDataViewModel> orderData = new List<ChamCongDataViewModel>();
                    if (department.NullString() == "")
                    {
                        orderData = data.OrderBy(x => x.BoPhan).ThenBy(x => x.BoPhanDetail).ToList();
                    }
                    else
                    {
                        orderData = data.OrderBy(x => x.BoPhanDetail).ToList();
                    }

                    for (int i = 0; i < orderData.Count; i++)
                    {
                        worksheet.Cells["A" + beginIndex].Value = (i + 1);
                        worksheet.Cells["B" + beginIndex].Value = orderData[i].MaNV;
                        worksheet.Cells["C" + beginIndex].Value = orderData[i].TenNV;
                        worksheet.Cells["D" + beginIndex].Value = orderData[i].NgayVao;
                        worksheet.Cells["E" + beginIndex].Value = orderData[i].BoPhanDetail;
                        worksheet.Cells["CP" + beginIndex].Value = orderData[i].VP_SX;

                        for (int j = 1; j <= 31; j++)
                        {
                            colName = GetExcelColumnName(j + 7);
                            // fill working status
                            newColName = colName + beginIndex;
                            foreach (var ws in orderData[i].WorkingStatuses)
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

                                foreach (var ot in orderData[i].OvertimeValues)
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
                            foreach (var st in orderData[i].EL_LC_Statuses)
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

                        if (i < orderData.Count - 2)
                        {
                            // copy range cell
                            cellfrom = "A" + (beginIndex + 8) + ":CQ" + (beginIndex + 16);
                            cellTo = "A" + (beginIndex + 16) + ":CQ" + (beginIndex + 24);
                            worksheet.Cells[cellfrom].Copy(worksheet.Cells[cellTo]);

                            // format sum total
                            string hAM = "";
                            for (int h = 39; h <= 91; h++) // AM ->
                            {
                                hAM = GetExcelColumnName(h);
                                worksheet.Cells[hAM + (beginIndex + 24)].Formula = "SUM(" + hAM + "17:" + hAM + (beginIndex + 16) + ")";
                            }
                        }

                        if (i < orderData.Count - 1)
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
                        if (orderData.Count == 1)
                        {
                            worksheet.DeleteRow(25, 8);
                        }
                    }

                    package.Save(); //Save the workbook.
                }
            }
            return new OkObjectResult(fileUrl);
        }

        private string UpdateBangCongExtention(string timeEndUser, string timeTo)
        {
            try
            {
                List<DeNghiLamThemGioModel> lstLamthemgio = new List<DeNghiLamThemGioModel>();
                var data = _bangCongService.GetDataReport(timeEndUser, "", timeTo, "", ref lstLamthemgio).Where(x => x.BoPhan != "KOREA").ToList();

                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string directory = Path.Combine(sWebRootFolder, "export-files");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string sFileName = $"chamCong_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
                string fileTemp = "BangCongTmp.xlsx";

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

                string timeChamCong = timeTo + "-01";
                BANG_CONG_EXTENTION bangcongEx;
                List<BANG_CONG_EXTENTION> lstBangCongEx = new List<BANG_CONG_EXTENTION>();
                int startColumn = 39;
                string kytu = "";
                double valueEx = 0;
                string newColName = "";
                int beginIndex = 17;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    package.Workbook.FullCalcOnLoad = true;
                    worksheet.Workbook.CalcMode = ExcelCalcMode.Automatic;
                    package.Workbook.Calculate();

                    // TIEU ĐỀ
                    worksheet.Cells["R1"].Value = "MONTHLY ATTENDANCE RECORD OF " + GetMonthYearEng(timeChamCong);
                    worksheet.Cells["R2"].Value = "BẢNG CHẤM CÔNG THÁNG " + timeChamCong.Split("-")[1] + " NĂM " + timeChamCong.Split("-")[0];

                    worksheet.Cells["A1"].Value = timeChamCong.Replace("-", "");

                    string cellfrom = "";
                    string cellTo = "";
                    string colName = "";

                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cells["A" + beginIndex].Value = (i + 1);
                        worksheet.Cells["B" + beginIndex].Value = data[i].MaNV;
                        worksheet.Cells["C" + beginIndex].Value = data[i].TenNV;
                        worksheet.Cells["D" + beginIndex].Value = data[i].NgayVao;
                        worksheet.Cells["E" + beginIndex].Value = data[i].BoPhanDetail;
                        worksheet.Cells["CP" + beginIndex].Value = data[i].VP_SX;

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

                        // Đọc bảng công lấy phân giá trị tính theo công thức từ cột AM ->
                        bangcongEx = new BANG_CONG_EXTENTION();

                        bangcongEx.MaNV = data[i].MaNV;
                        bangcongEx.ThangNam = timeChamCong;

                        worksheet.Cells["AM" + beginIndex].Calculate();
                        worksheet.Cells["AN" + beginIndex].Calculate();
                        worksheet.Cells["AO" + beginIndex].Calculate();
                        worksheet.Cells["AP" + beginIndex].Calculate();
                        worksheet.Cells["AQ" + beginIndex].Calculate();
                        worksheet.Cells["AR" + beginIndex].Calculate();
                        worksheet.Cells["AS" + beginIndex].Calculate();
                        worksheet.Cells["AT" + beginIndex].Calculate();
                        worksheet.Cells["AU" + beginIndex].Calculate();
                        worksheet.Cells["AV" + beginIndex].Calculate();
                        worksheet.Cells["AW" + beginIndex].Calculate();
                        worksheet.Cells["AX" + beginIndex].Calculate();
                        worksheet.Cells["AY" + beginIndex].Calculate();
                        worksheet.Cells["AZ" + beginIndex].Calculate();
                        worksheet.Cells["BA" + beginIndex].Calculate();
                        worksheet.Cells["BB" + beginIndex].Calculate();
                        worksheet.Cells["BC" + beginIndex].Calculate();
                        worksheet.Cells["BD" + beginIndex].Calculate();
                        worksheet.Cells["BE" + beginIndex].Calculate();
                        worksheet.Cells["BF" + beginIndex].Calculate();
                        worksheet.Cells["BG" + beginIndex].Calculate();
                        worksheet.Cells["BH" + beginIndex].Calculate();
                        worksheet.Cells["BI" + beginIndex].Calculate();
                        worksheet.Cells["BJ" + beginIndex].Calculate();
                        worksheet.Cells["BK" + beginIndex].Calculate();
                        worksheet.Cells["BL" + beginIndex].Calculate();
                        worksheet.Cells["BM" + beginIndex].Calculate();
                        worksheet.Cells["BN" + beginIndex].Calculate();
                        worksheet.Cells["BO" + beginIndex].Calculate();
                        worksheet.Cells["BP" + beginIndex].Calculate();
                        worksheet.Cells["BQ" + beginIndex].Calculate();
                        worksheet.Cells["BR" + beginIndex].Calculate();
                        worksheet.Cells["BS" + beginIndex].Calculate();
                        worksheet.Cells["BT" + beginIndex].Calculate();
                        worksheet.Cells["BU" + beginIndex].Calculate();
                        worksheet.Cells["BV" + beginIndex].Calculate();
                        worksheet.Cells["BW" + beginIndex].Calculate();
                        worksheet.Cells["BX" + beginIndex].Calculate();
                        worksheet.Cells["BY" + beginIndex].Calculate();
                        worksheet.Cells["BZ" + beginIndex].Calculate();
                        worksheet.Cells["CA" + beginIndex].Calculate();
                        worksheet.Cells["CB" + beginIndex].Calculate();
                        worksheet.Cells["CC" + beginIndex].Calculate();
                        worksheet.Cells["CD" + beginIndex].Calculate();
                        worksheet.Cells["CE" + beginIndex].Calculate();
                        worksheet.Cells["CF" + beginIndex].Calculate();
                        worksheet.Cells["CG" + beginIndex].Calculate();
                        worksheet.Cells["CH" + beginIndex].Calculate();
                        worksheet.Cells["CI" + beginIndex].Calculate();
                        worksheet.Cells["CJ" + beginIndex].Calculate();
                        worksheet.Cells["CK" + beginIndex].Calculate();
                        worksheet.Cells["CL" + beginIndex].Calculate();
                        worksheet.Cells["CM" + beginIndex].Calculate();
                        worksheet.Cells["CO" + beginIndex].Calculate();
                        worksheet.Cells["CP" + beginIndex].Calculate();
                        worksheet.Cells["CQ" + beginIndex].Calculate();

                        // duyệt từ cột AM
                        startColumn = 39;
                        newColName = GetExcelColumnName(startColumn);
                        kytu = worksheet.Cells[newColName + 14].Value.NullString();
                        while (kytu != "")
                        {
                            if (kytu != "VP_SX" && kytu != "Signature")
                            {
                                valueEx = double.Parse(worksheet.Cells[newColName + beginIndex].Value.IfNullIsZero());
                            }

                            switch (kytu)
                            {
                                case "PH":
                                    bangcongEx.PH = valueEx;
                                    break;
                                case "PD":
                                    bangcongEx.PD = valueEx;
                                    break;
                                case "PN":
                                    bangcongEx.PN = valueEx;
                                    break;
                                case "BH":
                                    bangcongEx.BH = valueEx;
                                    break;
                                case "DS":
                                    bangcongEx.DS = valueEx;
                                    break;
                                case "NS":
                                    bangcongEx.NS = valueEx;
                                    break;
                                case "NCL":
                                    bangcongEx.NCL = valueEx;
                                    break;
                                case "TP":
                                    bangcongEx.TP = valueEx;
                                    break;
                                case "TUP":
                                    bangcongEx.TUP = valueEx;
                                    break;
                                case "CD_TV":
                                    bangcongEx.CD_TV = valueEx;
                                    break;
                                case "CD_CT":
                                    bangcongEx.CD_CT = valueEx;
                                    break;
                                case "AL":
                                    bangcongEx.AL = valueEx;
                                    break;
                                case "AL30":
                                    bangcongEx.AL30 = valueEx;
                                    break;
                                case "L160":
                                    bangcongEx.L160 = valueEx;
                                    break;
                                case "SL":
                                    bangcongEx.SL = valueEx;
                                    break;
                                case "NH":
                                    bangcongEx.NH = valueEx;
                                    break;
                                case "HL":
                                    bangcongEx.HL = valueEx;
                                    break;
                                case "UL":
                                    bangcongEx.UL = valueEx;
                                    break;
                                case "NB":
                                    bangcongEx.NB = valueEx;
                                    break;
                                case "NL":
                                    bangcongEx.NL = valueEx;
                                    break;
                                case "IL":
                                    bangcongEx.IL = valueEx;
                                    break;
                                case "KT":
                                    bangcongEx.KT = valueEx;
                                    break;
                                case "L70":
                                    bangcongEx.L70 = valueEx;
                                    break;
                                case "MD":
                                    bangcongEx.MD = valueEx;
                                    break;
                                case "PMD":
                                    bangcongEx.PMD = valueEx;
                                    break;
                                case "PM":
                                    bangcongEx.PM = valueEx;
                                    break;
                                case "BM":
                                    bangcongEx.BM = valueEx;
                                    break;
                                case "OT_TV_15":
                                    bangcongEx.OT_TV_15 = valueEx;
                                    break;
                                case "OT_TV_20":
                                    bangcongEx.OT_TV_20 = valueEx;
                                    break;
                                case "OT_TV_21":
                                    bangcongEx.OT_TV_21 = valueEx;
                                    break;
                                case "OT_TV_27":
                                    bangcongEx.OT_TV_27 = valueEx;
                                    break;
                                case "OT_TV_30":
                                    bangcongEx.OT_TV_30 = valueEx;
                                    break;
                                case "OT_TV_39":
                                    bangcongEx.OT_TV_39 = valueEx;
                                    break;
                                case "OT_CT_15":
                                    bangcongEx.OT_CT_15 = valueEx;
                                    break;
                                case "OT_CT_20":
                                    bangcongEx.OT_CT_20 = valueEx;
                                    break;
                                case "OT_CT_21":
                                    bangcongEx.OT_CT_21 = valueEx;
                                    break;
                                case "OT_CT_27":
                                    bangcongEx.OT_CT_27 = valueEx;
                                    break;
                                case "OT_CT_30":
                                    bangcongEx.OT_CT_30 = valueEx;
                                    break;
                                case "OT_CT_39":
                                    bangcongEx.OT_CT_39 = valueEx;
                                    break;
                                case "P_TV":
                                    bangcongEx.P_TV = valueEx;
                                    break;
                                case "O_CT":
                                    bangcongEx.O_CT = valueEx;
                                    break;
                                case "OT":
                                    bangcongEx.OT = valueEx;
                                    break;
                                case "TV_150":
                                    bangcongEx.TV_150 = valueEx;
                                    break;
                                case "TV_D_NT_200":
                                    bangcongEx.TV_D_NT_200 = valueEx;
                                    break;
                                case "TV_CN_200":
                                    bangcongEx.TV_CN_200 = valueEx;
                                    break;
                                case "TV_D_CN_270":
                                    bangcongEx.TV_D_CN_270 = valueEx;
                                    break;
                                case "TV_NL_300":
                                    bangcongEx.TV_NL_300 = valueEx;
                                    break;
                                case "TV_D_NL_390":
                                    bangcongEx.TV_D_NL_390 = valueEx;
                                    break;
                                case "CT_150":
                                    bangcongEx.CT_150 = valueEx;
                                    break;
                                case "CT_D_NT_200":
                                    bangcongEx.CT_D_NT_200 = valueEx;
                                    break;
                                case "CT_CN_200":
                                    bangcongEx.CT_CN_200 = valueEx;
                                    break;
                                case "CT_D_CN_270":
                                    bangcongEx.CT_D_CN_270 = valueEx;
                                    break;
                                case "CT_NL_300":
                                    bangcongEx.CT_NL_300 = valueEx;
                                    break;
                                case "CT_D_NL_390":
                                    bangcongEx.CT_D_NL_390 = valueEx;
                                    break;
                                case "VP_SX":
                                    bangcongEx.VP_SX = worksheet.Cells[newColName + beginIndex].Value.NullString();
                                    break;
                                case "SUM_OTHER":
                                    bangcongEx.SUM_OTHER = valueEx;
                                    break;
                            }

                            newColName = GetExcelColumnName(++startColumn);
                            kytu = worksheet.Cells[newColName + 14].Value.NullString();
                        }

                        lstBangCongEx.Add(bangcongEx);

                        if (i < data.Count - 2)
                        {
                            // copy range cell
                            cellfrom = "A" + (beginIndex + 8) + ":CQ" + (beginIndex + 16);
                            cellTo = "A" + (beginIndex + 16) + ":CQ" + (beginIndex + 24);
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

                                if (DateTime.DaysInMonth(DateTime.Parse(timeChamCong).Year, DateTime.Parse(timeChamCong).Month) >= d)
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

                    if (lstBangCongEx.Count > 0)
                        _bangCongService.AddBangCongEx(lstBangCongEx);
                }

                if (file.Exists)
                {
                    file.Delete();
                }
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw;
            }
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

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

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
