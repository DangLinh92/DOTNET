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
    public class EhsHangMucNGController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IHangMucNGService _danhMucNGService;
        private IEhsCoQuanKiemtraService _coquanService;
        private readonly IMemoryCache _memoryCache;

        public EhsHangMucNGController(IEhsCoQuanKiemtraService coquanService, IHangMucNGService danhMucNGService, IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _danhMucNGService = danhMucNGService;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
            _coquanService = coquanService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetNGByYear(DataSourceLoadOptions loadOptions, string year)
        {
            var lstModel = _danhMucNGService.GetByYear(year);
            var lstNG = _coquanService.GetNG(year);
            EhsHangMucNGViewModel vm;
            foreach (var item in lstNG)
            {
                vm = new EhsHangMucNGViewModel()
                {
                    DeMuc = item.Demuc,
                    HangMucNG = item.CoQuanKiemTra,
                    NoiDungVanDeNG = item.NoiDungNG,
                    NguyenNhan = item.NguyenNhan,
                    DoiSachCaiTien = item.DoiSachCaiTien,
                    TinhHinhCaiTien = item.TienDoCaiTien
                };
                lstModel.Add(vm);
            }

            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpGet]
        public object GetByKey(DataSourceLoadOptions loadOptions, string key)
        {
            var lstModel = _danhMucNGService.GetByKey(key);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values, string maNgayCT)
        {
            var vm = new EhsHangMucNGViewModel();
            JsonConvert.PopulateObject(values, vm);
            vm.MaNgayChiTiet = maNgayCT;
            vm = _danhMucNGService.Add(vm);
            _danhMucNGService.Save();

            return Ok(vm);
        }

        [HttpPut]
        public IActionResult Update(int key, string values)
        {
            var vm = _danhMucNGService.GetById(key);
            JsonConvert.PopulateObject(values, vm);

            vm = _danhMucNGService.Update(vm);
            _danhMucNGService.Save();
            return Ok(vm);
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _danhMucNGService.Delete(key);
            _danhMucNGService.Save();
        }
    }
}
