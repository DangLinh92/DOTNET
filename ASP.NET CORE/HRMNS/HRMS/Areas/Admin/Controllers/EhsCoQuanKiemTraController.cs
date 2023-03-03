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
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsCoQuanKiemTraController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IEhsCoQuanKiemtraService _coquanService;
        private readonly IMemoryCache _memoryCache;

        public EhsCoQuanKiemTraController(IEhsCoQuanKiemtraService Service, IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _coquanService = Service;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetAll(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _coquanService.GetList();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var vm = new EhsCoQuanKiemTraViewModel();
            JsonConvert.PopulateObject(values, vm);
            //vm.NgayKiemTra = vm.NgayKiemTra2.ToString("yyyy-MM-dd");
            vm = _coquanService.Add(vm);
            return Ok(vm);
        }

        [HttpPut]
        public IActionResult Update(int key, string values)
        {
            var vm = _coquanService.GetById(key);
            JsonConvert.PopulateObject(values, vm);
            //vm.NgayKiemTra = vm.NgayKiemTra2.ToString("yyyy-MM-dd");
            vm = _coquanService.Update(vm);
            _coquanService.Save();
            return Ok(vm);
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _coquanService.Delete(key);
            _coquanService.Save();
        }
    }
}
