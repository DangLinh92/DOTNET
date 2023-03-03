using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class QuanLyGiayPhepController : AdminBaseController
    {
        private readonly IEhsQuanLyGiayPhepService ehsQuanLyGiayPhepService;
        public QuanLyGiayPhepController(IEhsQuanLyGiayPhepService service)
        {
            ehsQuanLyGiayPhepService = service;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            var data = ehsQuanLyGiayPhepService.GetList();
            return DataSourceLoader.Load(data, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var newEmployee = new EhsQuanLyGiayPhepViewModel();
            JsonConvert.PopulateObject(values, newEmployee);
            //newEmployee.ThoiGianThucHien = newEmployee.ThoiGian?.ToString("yyyy-MM-dd");
            ehsQuanLyGiayPhepService.Add(newEmployee);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = ehsQuanLyGiayPhepService.GetById(key);
            JsonConvert.PopulateObject(values, model);
            //model.ThoiGianThucHien = model.ThoiGian?.ToString("yyyy-MM-dd");
            ehsQuanLyGiayPhepService.Update(model);
            ehsQuanLyGiayPhepService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            ehsQuanLyGiayPhepService.Delete(key);
            ehsQuanLyGiayPhepService.Save();
        }
    }
}
