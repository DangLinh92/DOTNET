using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsTongHopDanhMucKeHoachController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IDanhMucKeHoachService _danhMucKeHoachService;
        public EhsTongHopDanhMucKeHoachController(IDanhMucKeHoachService danhMucKeHoachService, IWebHostEnvironment hostingEnvironment)
        {
            _danhMucKeHoachService = danhMucKeHoachService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var model = _danhMucKeHoachService.TongHopKeHoachByYear("2022");
            return View(model);
        }

        [HttpPost]
        public IActionResult GetFileTongHopKeHoach(string year)
        {
            var tonghopKeHoachs = _danhMucKeHoachService.TongHopKeHoachByYear(year);

            if (tonghopKeHoachs == null || tonghopKeHoachs.Count == 0)
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"KeHoachEHS_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "KeHoachEHS_Template.xlsx"));
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
                ExcelWorksheets worksheets = package.Workbook.Worksheets;
                ExcelWorksheet KeHoachQtrac = worksheets.First(x=>x.Name.EndsWith("환경 평가"));
                ExcelWorksheet KeHoachKhamSK = worksheets.First(x=>x.Name.EndsWith("건강검진 계획"));
                ExcelWorksheet KeHoachATLaoDong = worksheets.First(x=>x.Name.EndsWith("노동안전 교육"));
                ExcelWorksheet KeHoachKiemDinhMayMoc = worksheets.First(x=>x.Name.EndsWith("기계 검교정"));
                ExcelWorksheet KeHoachPhongChayCC = worksheets.First(x=>x.Name.EndsWith("소방에 관한 계획"));
                ExcelWorksheet KeHoachATBXa = worksheets.First(x=>x.Name.EndsWith("매년 핵 방사 안전 실시 업무"));
                ExcelWorksheet tongHop = worksheets.First(x=>x.Name.EndsWith("TOTAL"));

                // Kế hoạch quan trắc

                if(KeHoachQtrac != null)
                {
                    KeHoachQtrac.Cells["B1"].Value = "KẾ HOẠCH THỰC HIỆN QUAN TRẮC " + year + "\n" + year + " 환경 평가 계획";

                }

                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }
    }
}
