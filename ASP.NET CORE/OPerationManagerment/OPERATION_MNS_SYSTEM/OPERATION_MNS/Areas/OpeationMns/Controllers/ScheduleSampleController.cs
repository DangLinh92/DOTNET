using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Implementation;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Sameple;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class ScheduleSampleController : AdminBaseController
    {
        private IScheduleSampleService _scheduleSampleService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ScheduleSampleController(IScheduleSampleService scheduleSampleService, IWebHostEnvironment hostingEnvironment)
        {
            _scheduleSampleService = scheduleSampleService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions, string fromTime, string toTime, bool isOpen)
        {
            List<TinhHinhSanXuatSampleViewModel> data = null;
            if (isOpen)
            {
                data = _scheduleSampleService.GetOpens().OrderBy(x => x.LeadTimeMax).ToList();
                var data2 = data.Where(x => x.LeadTimeMax <= 7 && x.Code.NullString().ToUpper() == "R").OrderBy(x => x.LeadTimeMax).ToList();

                int index = 0;
                for (int i = 0; i < data2.Count; i++)
                {
                    if (i == 0)
                    {
                        data2[i].IsHightLight = true;
                        data2[i].LevelHightLight = 1;
                        data2[i].MucDoKhanCap = 1;
                        index = 1;
                    }
                    else
                    if (index <= 5)
                    {
                        if (data2[i].LeadTimeMax > data2[i - 1].LeadTimeMax)
                        {
                            data2[i].IsHightLight = true;
                            data2[i].LevelHightLight = ++index;
                            data2[i].MucDoKhanCap = index;
                        }
                        else
                        if (data2[i].LeadTimeMax == data2[i - 1].LeadTimeMax)
                        {
                            data2[i].IsHightLight = true;
                            data2[i].LevelHightLight = index;
                            data2[i].MucDoKhanCap = index;
                        }
                    }
                    else
                    {
                        data2[i].MucDoKhanCap = ++index;
                    }
                }

                List<TinhHinhSanXuatSampleViewModel> lst2 = new List<TinhHinhSanXuatSampleViewModel>();
                lst2.AddRange(data2);
                lst2.AddRange(data.FindAll(x => x.LeadTimeMax > 7 || x.Code.NullString().ToUpper() != "R"));

                return DataSourceLoader.Load(lst2, loadOptions);
            }

            List<TinhHinhSanXuatSampleViewModel> data1 = _scheduleSampleService.GetByTime(fromTime, toTime).OrderBy(x => x.LeadTimeMax).ToList();
            data = data1.Where(x => x.DeleteFlg != "Y" && x.LeadTimeMax <= 7 && x.Code.NullString().ToUpper() == "R").OrderBy(x => x.LeadTimeMax).ToList();

            int index2 = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    data[i].IsHightLight = true;
                    data[i].LevelHightLight = 1;

                    if (data[i].MucDoKhanCap == 0)
                    {
                        data[i].MucDoKhanCap = 1;
                    }

                    index2 = 1;
                }
                else
                    if (index2 <= 5)
                {
                    if (data[i].LeadTimeMax > data[i - 1].LeadTimeMax)
                    {
                        data[i].IsHightLight = true;
                        data[i].LevelHightLight = ++index2;
                        data[i].MucDoKhanCap = index2;
                    }
                    else
                    if (data[i].LeadTimeMax == data[i - 1].LeadTimeMax)
                    {
                        data[i].IsHightLight = true;
                        data[i].LevelHightLight = index2;
                        data[i].MucDoKhanCap = index2;
                    }
                }
                else
                {
                    data[i].LevelHightLight = ++index2;
                    data[i].MucDoKhanCap = index2;
                }
            }

            List<TinhHinhSanXuatSampleViewModel> lst = new List<TinhHinhSanXuatSampleViewModel>();
            lst.AddRange(data.OrderBy(x=>x.MucDoKhanCap));
            lst.AddRange(data1.FindAll(x => x.DeleteFlg == "Y" || x.LeadTimeMax > 7 || x.Code.NullString().ToUpper() != "R"));

            return DataSourceLoader.Load(lst, loadOptions);
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            TinhHinhSanXuatSampleViewModel model = _scheduleSampleService.FindById(key);
            JsonConvert.PopulateObject(values, model);

            _scheduleSampleService.Update(model);
            return Ok();
        }

        // update plan in operation by lotno

        public IActionResult InputPlanByOperation()
        {
            return View();
        }

        [HttpPut]
        public IActionResult PutPlan(int key, string values)
        {
            TinhHinhSanXuatSampleViewModel model = _scheduleSampleService.FindById(key);
            JsonConvert.PopulateObject(values, model);

            //model.PlanInputDate = model.PlanInputDate_1 != null ? model.PlanInputDate_1?.ToString("yyyyMMdd") : "";
            //model.Wall_Plan_Date = model.Wall_Plan_Date_1 != null ? model.Wall_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Roof_Plan_Date = model.Roof_Plan_Date_1 != null ? model.Roof_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Seed_Plan_Date = model.Seed_Plan_Date_1 != null ? model.Seed_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.PlatePR_Plan_Date = model.PlatePR_Plan_Date_1 != null ? model.PlatePR_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Plate_Plan_Date = model.Plate_Plan_Date_1 != null ? model.Plate_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.PreProbe_Plan_Date = model.PreProbe_Plan_Date_1 != null ? model.PreProbe_Plan_Date_1?.ToString("yyyyMMdd") : "";
            model.PreProbe_Actual_Date = model.PreProbe_Actual_Date_1 != null ? model.PreProbe_Actual_Date_1?.ToString("yyyyMMdd") : "";
            //model.PreDicing_Plan_Date = model.PreDicing_Plan_Date_1 != null ? model.PreDicing_Plan_Date_1?.ToString("yyyyMMdd") : "";
            model.PreDicing_Actual_Date = model.PreDicing_Actual_Date_1 != null ? model.PreDicing_Actual_Date_1?.ToString("yyyyMMdd") : "";
            //model.AllProbe_Plan_Date = model.AllProbe_Plan_Date_1 != null ? model.AllProbe_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.BG_Plan_Date = model.BG_Plan_Date_1 != null ? model.BG_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Dicing_Plan_Date = model.Dicing_Plan_Date_1 != null ? model.Dicing_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.ChipIns_Plan_Date = model.ChipIns_Plan_Date_1 != null ? model.ChipIns_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Packing_Plan_Date = model.Packing_Plan_Date_1 != null ? model.Packing_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.OQC_Plan_Date = model.OQC_Plan_Date_1 != null ? model.OQC_Plan_Date_1?.ToString("yyyyMMdd") : "";
            //model.Shipping_Plan_Date = model.Shipping_Plan_Date_1 != null ? model.Shipping_Plan_Date_1?.ToString("yyyyMMdd") : "";

            _scheduleSampleService.Update(model);
            return Ok();
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, string param)
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
                string fName = CorrelationIdGenerator.GetNextId() + filename;
                string filePath = Path.Combine(folder, fName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                ResultDB rs = _scheduleSampleService.ImportExcel(filePath, param);

                if (rs.ReturnInt == 0)
                {
                    _scheduleSampleService.Save();
                }
                else
                {
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                return new OkObjectResult(filePath);
            }

            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        #region Import comment
        public IActionResult DelayComment()
        {
            return View();
        }

        [HttpGet]
        public object GetDelayComment(DataSourceLoadOptions loadOptions, string fromTime, string toTime, bool isOpen)
        {
            List<DELAY_COMMENT_SAMPLE> data = new List<DELAY_COMMENT_SAMPLE>();
            if (isOpen)
            {
                data = _scheduleSampleService.GetDelayOpen();
                return DataSourceLoader.Load(data, loadOptions);
            }

            data = _scheduleSampleService.GetDelayByTime(fromTime, toTime);
            return DataSourceLoader.Load(data, loadOptions);
        }

        [HttpPut]
        public IActionResult PutComment(int key, string values)
        {
            DELAY_COMMENT_SAMPLE model = _scheduleSampleService.FindCommentById(key);
            JsonConvert.PopulateObject(values, model);
            _scheduleSampleService.UpdateComment(model);
            return Ok();
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportCommentExcel(IList<IFormFile> files, string param)
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
                string fName = CorrelationIdGenerator.GetNextId() + filename;
                string filePath = Path.Combine(folder, fName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                ResultDB rs = _scheduleSampleService.ImportDelayExcel(filePath, param);

                if (rs.ReturnInt == 0)
                {
                    _scheduleSampleService.Save();
                }
                else
                {
                    return new NotFoundObjectResult(rs.ReturnString);
                }

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                return new OkObjectResult(filePath);
            }

            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
        #endregion
    }
}
