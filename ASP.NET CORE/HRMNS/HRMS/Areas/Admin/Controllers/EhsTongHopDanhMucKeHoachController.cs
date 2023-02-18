using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
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
            return View();
        }

        [HttpGet]
        public object TongHopKeHoachByYear(DataSourceLoadOptions loadOptions, string year)
        {
            if (year.NullString() == "")
                year = DateTime.Now.Year.ToString();

            var model = _danhMucKeHoachService.TongHopKeHoachByYear(year);
            return DataSourceLoader.Load(model, loadOptions);
        }

        public IActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Get kế hoạch trong khoảng thời gian
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>
        [HttpGet]
        public object ListKeHoach(DataSourceLoadOptions loadOptions, string fromTime, string toTime)
        {
           var data =  _danhMucKeHoachService.DanhSachKeHoachByTime(fromTime, toTime);
            return DataSourceLoader.Load(data, loadOptions);
        }
    }
}
