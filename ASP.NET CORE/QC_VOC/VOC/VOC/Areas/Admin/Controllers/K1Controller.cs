using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Areas.Admin.Models;
using VOC.Data.EF.Extensions;
using VOC.Utilities.Common;
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Areas.Admin.Controllers
{
    public class K1Controller : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocMstService _vocMstService;

        public K1Controller(IVocMstService vocMstService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocMstService = vocMstService;
            _logger = logger;
        }

        public IActionResult Index(int year)
        {
            VocInfomationsModel model = GetData(year, CommonConstants.ALL);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="site">CSP/LFEM</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Search(int year, string site)
        {
            VocInfomationsModel model = GetData(year, site.NullString(), true);
            return View("Index", model);
            //return new OkObjectResult(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="site">CSP/LFEM/ALL</param>
        /// <param name="isSearch"></param>
        /// <returns></returns>
        private VocInfomationsModel GetData(int year, string site, bool isSearch = false)
        {
            VocInfomationsModel model = new VocInfomationsModel();

            // Ve Bieu Do PPM
            List<VOCPPM_Ex> pMDataCharts;
            if (!isSearch && (year <= 0 || year == DateTime.Now.Year))
            {
                model.VocPPMView = _vocMstService.ReportPPMByYear(DateTime.Now.Year.ToString(), site, out pMDataCharts);
            }
            else
            {
                model.VocPPMView = _vocMstService.ReportPPMByYear(year.ToString(), site, out pMDataCharts);
            }

            model.vOCPPMs = pMDataCharts;
            ViewBag.UpdateDayK1 = GetUpdateDay();
            return model;
        }

        [HttpGet]
        public IActionResult UploadList()
        {
            GmesDataViewModel gmes = _vocMstService.GetGmesData(0,0);
            return View(gmes);
        }

        [HttpPost]
        public IActionResult searchUpload(string yearMonth)
        {
            if(yearMonth.NullString() == "")
            {
                GmesDataViewModel gmes = _vocMstService.GetGmesData(0, 0);
                return View("UploadList", gmes);
            }
            else
            {
                int Year = int.Parse(yearMonth.Split('-')[0]);
                int month = int.Parse(yearMonth.Split('-')[1]);
                GmesDataViewModel gmes = _vocMstService.GetGmesData(Year, month);
                gmes.Year = yearMonth;
                return View("UploadList", gmes);
            }
        }

        [HttpPost]
        public IActionResult UpdatePPMByYear(VocPPMYearViewModel model, [FromQuery] string action)
        {
            bool isAdd = action == "Add";
            _vocMstService.UpdatePPMByYear(isAdd, model);
            _vocMstService.Save();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetPPMByYear(int id)
        {
            VocPPMYearViewModel model = _vocMstService.GetPPMByYear(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetTargetByYear()
        {
            int year = DateTime.Now.Year;
            double target = _vocMstService.GetTargetPPMByYear(year);
            return new OkObjectResult(target);
        }

        [HttpGet]
        public IActionResult GetPPMByYearMonth(int id)
        {
            VocPPMViewModel model = _vocMstService.GetPPMByYearMonth(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdatePPM(VocPPMViewModel model, [FromQuery] string action)
        {
            bool isAdd = action == "Add";
            _vocMstService.UpdatePPMByYearMonth(isAdd, model);
            _vocMstService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult DeletePPMByYear(int Id)
        {
            _vocMstService.DeletePPMByYear(Id);
            _vocMstService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult DeletePPMByYearMonth(int Id)
        {
            _vocMstService.DeletePPMByYearMonth(Id);
            _vocMstService.Save();
            return new OkObjectResult(Id);
        }

        [HttpPost]
        public IActionResult ExportExcelK1(int year)
        {
            if (year <= 0)
            {
                return new BadRequestObjectResult("Year :" + year + " invalid!");
            }

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"K1고객 불량율.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));

            if (file.Exists)
            {
                file.Delete();
            }
            System.IO.File.Copy(Path.Combine(Path.Combine(sWebRootFolder, "templates"), sFileName), Path.Combine(Path.Combine(sWebRootFolder, "export-files"), sFileName));
            FileInfo newfile = new FileInfo(Path.Combine(directory, sFileName));

            List<VOCPPM_Ex> pMDataCharts;
            var vocs = _vocMstService.ReportPPMByYear(year.ToString(), "ALL", out pMDataCharts);

            List<DataK1> targetAlls_csp = new List<DataK1>();
            List<DataK1> dataActual_csp = new List<DataK1>();

            List<DataK1> targetAlls_lfem = new List<DataK1>();
            List<DataK1> dataActual_lfem = new List<DataK1>();

            DataK1 target;
            DataK1 actual;
            foreach (var item in vocs.dataChartsAll)
            {

                target = new DataK1();
                actual = new DataK1();

                target.x01 = item.dataTargetAll[0];
                target.x02 = item.dataTargetAll[1];
                target.x03 = item.dataTargetAll[2];
                target.x04 = item.dataTargetAll[3];
                target.x05 = item.dataTargetAll[4];
                target.x06 = item.dataTargetAll[5];
                target.x07 = item.dataTargetAll[6];
                target.x08 = item.dataTargetAll[7];
                target.x09 = item.dataTargetAll[8];
                target.x10 = item.dataTargetAll[9];
                target.x11 = item.dataTargetAll[10];
                target.x12 = item.dataTargetAll[11];
                target.x13 = item.dataTargetAll[12];
                target.x14 = item.dataTargetAll[13];
                target.x15 = item.dataTargetAll[14];
                target.x16 = item.dataTargetAll[15];

                if (item.Module == "CSP")
                {
                    targetAlls_csp.Add(target);
                }
                else
                {
                    targetAlls_lfem.Add(target);
                }
                // actual data
                actual.x01 = item.lstData[0];
                actual.x02 = item.lstData[1];
                actual.x03 = item.lstData[2];
                actual.x04 = item.lstData[3];
                actual.x05 = item.lstData[4];
                actual.x06 = item.lstData[5];
                actual.x07 = item.lstData[6];
                actual.x08 = item.lstData[7];
                actual.x09 = item.lstData[8];
                actual.x10 = item.lstData[9];
                actual.x11 = item.lstData[10];
                actual.x12 = item.lstData[11];
                actual.x13 = item.lstData[12];
                actual.x14 = item.lstData[13];
                actual.x15 = item.lstData[14];
                actual.x16 = item.lstData[15];

                if (item.Module == "CSP")
                {
                    dataActual_csp.Insert(0, actual);
                }
                else
                {
                    dataActual_lfem.Add(actual);
                }
            }

            List<DataK1> dataSEVT_lfem = new List<DataK1>();
            List<DataK1> dataSEV_lfem = new List<DataK1>();

            List<DataK1> dataSEVT_csp = new List<DataK1>();
            List<DataK1> dataSEV_csp = new List<DataK1>();
            foreach (var item in pMDataCharts)
            {
                foreach (var cus in item.vOCPPM_Customers)
                {
                    if (cus.Customer != "SEVT" && cus.Customer != "SEV")
                    {
                        continue;
                    }

                    DataK1 defect = new DataK1();
                    DataK1 input = new DataK1();

                    foreach (var voc in cus.vocPPMModels.OrderBy(x => x.Month))
                    {
                        if (voc.Type == "Input")
                        {
                            switch (voc.Month)
                            {
                                case 1:
                                    input.x01 = voc.Value;
                                    continue;
                                case 2:
                                    input.x02 = voc.Value;
                                    continue;
                                case 3:
                                    input.x03 = voc.Value;
                                    continue;
                                case 4:
                                    input.x04 = voc.Value;
                                    continue;
                                case 5:
                                    input.x05 = voc.Value;
                                    continue;
                                case 6:
                                    input.x06 = voc.Value;
                                    continue;
                                case 7:
                                    input.x07 = voc.Value;
                                    continue;
                                case 8:
                                    input.x08 = voc.Value;
                                    continue;
                                case 9:
                                    input.x09 = voc.Value;
                                    continue;
                                case 10:
                                    input.x10 = voc.Value;
                                    continue;
                                case 11:
                                    input.x11 = voc.Value;
                                    continue;
                                case 12:
                                    input.x12 = voc.Value;
                                    continue;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (voc.Month)
                            {
                                case 1:
                                    defect.x01 = voc.Value;
                                    continue;
                                case 2:
                                    defect.x02 = voc.Value;
                                    continue;
                                case 3:
                                    defect.x03 = voc.Value;
                                    continue;
                                case 4:
                                    defect.x04 = voc.Value;
                                    continue;
                                case 5:
                                    defect.x05 = voc.Value;
                                    continue;
                                case 6:
                                    defect.x06 = voc.Value;
                                    continue;
                                case 7:
                                    defect.x07 = voc.Value;
                                    continue;
                                case 8:
                                    defect.x08 = voc.Value;
                                    continue;
                                case 9:
                                    defect.x09 = voc.Value;
                                    continue;
                                case 10:
                                    defect.x10 = voc.Value;
                                    continue;
                                case 11:
                                    defect.x11 = voc.Value;
                                    continue;
                                case 12:
                                    defect.x12 = voc.Value;
                                    continue;
                                default:
                                    break;
                            }
                        }
                    }

                    DataK1 p = new DataK1();
                    DataK1 t = new DataK1();
                    foreach (var ppm in cus.pPMByMonths.OrderBy(x => x.Month))
                    {
                        switch (ppm.Month)
                        {
                            case 1:
                                p.x01 = ppm.PPM;
                                t.x01 = ppm.Target;
                                continue;
                            case 2:
                                p.x02 = ppm.PPM;
                                t.x02 = ppm.Target;
                                continue;
                            case 3:
                                p.x03 = ppm.PPM;
                                t.x03 = ppm.Target;
                                continue;
                            case 4:
                                p.x04 = ppm.PPM;
                                t.x04 = ppm.Target;
                                continue;
                            case 5:
                                p.x05 = ppm.PPM;
                                t.x05 = ppm.Target;
                                continue;
                            case 6:
                                p.x06 = ppm.PPM;
                                t.x06 = ppm.Target;
                                continue;
                            case 7:
                                p.x07 = ppm.PPM;
                                t.x07 = ppm.Target;
                                continue;
                            case 8:
                                p.x08 = ppm.PPM;
                                t.x08 = ppm.Target;
                                continue;
                            case 9:
                                p.x09 = ppm.PPM;
                                t.x09 = ppm.Target;
                                continue;
                            case 10:
                                p.x10 = ppm.PPM;
                                t.x10 = ppm.Target;
                                continue;
                            case 11:
                                p.x11 = ppm.PPM;
                                t.x11 = ppm.Target;
                                continue;
                            case 12:
                                p.x12 = ppm.PPM;
                                t.x12 = ppm.Target;
                                continue;
                            default:
                                break;
                        }
                    }

                    if (item.Module == "LFEM")
                    {
                        if (cus.Customer == "SEVT")
                        {
                            dataSEVT_lfem.Add(input);
                            dataSEVT_lfem.Add(defect);
                            dataSEVT_lfem.Add(p);
                            dataSEVT_lfem.Add(t);
                        }

                        if (cus.Customer == "SEV")
                        {
                            dataSEV_lfem.Add(input);
                            dataSEV_lfem.Add(defect);
                            dataSEV_lfem.Add(p);
                            dataSEV_lfem.Add(t);
                        }
                    }
                    else
                    {
                        if (cus.Customer == "SEVT")
                        {
                            dataSEVT_csp.Add(input);
                            dataSEVT_csp.Add(defect);
                            dataSEVT_csp.Add(p);
                            dataSEVT_csp.Add(t);
                        }

                        if (cus.Customer == "SEV")
                        {
                            dataSEV_csp.Add(input);
                            dataSEV_csp.Add(defect);
                            dataSEV_csp.Add(p);
                            dataSEV_csp.Add(t);
                        }
                    }
                }
            }

            using (ExcelPackage package = new ExcelPackage(newfile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["K1 불량율"];
                worksheet.Cells["D6"].LoadFromCollection(targetAlls_csp);
                worksheet.Cells["D10"].LoadFromCollection(targetAlls_lfem);

                if (dataActual_csp.Count > 0)
                {
                    worksheet.Cells["D7"].Value = dataActual_csp[0].x01;
                    worksheet.Cells["E7"].Value = dataActual_csp[0].x02;
                    worksheet.Cells["F7"].Value = dataActual_csp[0].x03;
                }

                if (dataActual_lfem.Count > 0)
                {
                    worksheet.Cells["D11"].Value = dataActual_lfem[0].x01;
                    worksheet.Cells["E11"].Value = dataActual_lfem[0].x02;
                    worksheet.Cells["F11"].Value = dataActual_lfem[0].x03;
                }
                
                worksheet.Cells["H4"].Value = year.ToString().Substring(2, 2) + "年";
                worksheet.Cells["G4"].Value = year.ToString().Substring(2, 2) + "年";
                worksheet.Cells["F4"].Value = (year - 1).ToString().Substring(2, 2) + "年";
                worksheet.Cells["E4"].Value = (year - 2).ToString().Substring(2, 2) + "年";
                worksheet.Cells["D4"].Value = (year - 3).ToString().Substring(2, 2) + "年";

                worksheet.Cells.AutoFitColumns();

                // LFEM
                ExcelWorksheet worksheet_LFEM = package.Workbook.Worksheets["LFEM"];

                if (dataSEVT_lfem.Count > 0)
                {
                    worksheet_LFEM.Cells["E63"].LoadFromCollection(dataSEVT_lfem);
                }

                if (dataSEV_lfem.Count > 0)
                {
                    worksheet_LFEM.Cells["E41"].LoadFromCollection(dataSEV_lfem);
                }

                worksheet_LFEM.Cells["B66"].Value = year.ToString().Substring(2, 2) + "년 target";
                worksheet_LFEM.Cells["B44"].Value = year.ToString().Substring(2, 2) + "년 target";
                worksheet_LFEM.Cells["B22"].Value = year.ToString().Substring(2, 2) + "년 target";

                worksheet_LFEM.Cells["B63"].Value = "Wisol " + year.ToString().Substring(2, 2) + "년 GMES";
                worksheet_LFEM.Cells["B41"].Value = "Wisol " + year.ToString().Substring(2, 2) + "년 GMES";
                worksheet_LFEM.Cells["B19"].Value = "Wisol " + year.ToString().Substring(2, 2) + "년 GMES";

                foreach (var item in pMDataCharts)
                {
                    foreach (var cus in item.vOCPPM_Customers)
                    {
                        if(cus.Customer == "SEVT" && item.Module == "LFEM")
                        {
                            worksheet_LFEM.Cells["D66"].Value = cus.pPMByMonths.FirstOrDefault(x => x.Month == 0).Target;
                        }
                        else if(cus.Customer == "SEV" && item.Module == "LFEM")
                        {
                            worksheet_LFEM.Cells["D44"].Value = cus.pPMByMonths.FirstOrDefault(x => x.Month == 0).Target;
                        }
                    }
                }

                // CSP
                ExcelWorksheet worksheet_CSP = package.Workbook.Worksheets["CSP"];

                if (dataSEVT_csp.Count > 0)
                {
                    worksheet_CSP.Cells["E63"].LoadFromCollection(dataSEVT_csp);
                }

                if (dataSEV_csp.Count > 0)
                {
                    worksheet_CSP.Cells["E41"].LoadFromCollection(dataSEV_csp);
                }

                foreach (var item in pMDataCharts)
                {
                    foreach (var cus in item.vOCPPM_Customers)
                    {
                        if (cus.Customer == "SEVT" && item.Module == "CSP")
                        {
                            worksheet_CSP.Cells["D66"].Value = cus.pPMByMonths.FirstOrDefault(x => x.Month == 0).Target;
                        }
                        else if (cus.Customer == "SEV" && item.Module == "CSP")
                        {
                            worksheet_CSP.Cells["D44"].Value = cus.pPMByMonths.FirstOrDefault(x => x.Month == 0).Target;
                        }
                    }
                }

                worksheet_CSP.Cells["Q63:T66"].Clear();
                worksheet_CSP.Cells["Q41:T44"].Clear();

                worksheet_LFEM.Cells["Q63:T66"].Clear();
                worksheet_LFEM.Cells["Q41:T44"].Clear();

                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

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
                ResultDB result = _vocMstService.ImportExcel(filePath, param);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                if (result.ReturnInt == 0)
                {
                    return new OkObjectResult(filePath);
                }
                else
                {
                    _logger.LogError(result.ReturnString);
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }

            _logger.LogError("Upload file: " + CommonConstants.NotFoundObjectResult_Msg);
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        [HttpPost]
        public IActionResult UpdateDay(string date)
        {
            string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\updateDay";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, "updateLastDayK1.txt");
            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                file.Delete();
            }

            using (StreamWriter sw = file.CreateText())
            {
                sw.WriteLine(date);
            }

            return new OkObjectResult(date);
        }

        public string GetUpdateDay()
        {
            string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\updateDay";
            string filePath = Path.Combine(folder, "updateLastDayK1.txt");
            FileInfo file = new FileInfo(filePath);

            string date = "";
            if (file.Exists)
            {
                using (StreamReader sr = file.OpenText())
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        date += s;
                    }
                }
            }

            return date.Trim();
        }
    }
}
