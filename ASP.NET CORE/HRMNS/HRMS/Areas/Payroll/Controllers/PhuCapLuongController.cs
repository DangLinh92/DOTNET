using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRMS.Areas.Payroll.Controllers
{
    public class PhuCapLuongController : AdminBaseController
    {
        private IPhuCapLuongService _phuCapLuongService;
        public PhuCapLuongController(IPhuCapLuongService phuCapLuongService)
        {
            _phuCapLuongService = phuCapLuongService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetPCDH(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _phuCapLuongService.GetAll();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpGet]
        public object GetBoPhanAll(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _phuCapLuongService.GetBoPhanAll();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult InsertPCDH(string values)
        {
            var phucap = new PHUCAP_DOC_HAI();
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.AddDH(phucap);

            return Ok(phucap);
        }

        [HttpPut]
        public IActionResult UpdatePCDH(int key, string values)
        {
            var phucap = _phuCapLuongService.GetAllById(key);
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.UpdateDH(phucap);

            return Ok(phucap);
        }

        [HttpDelete]
        public void DeleteDH(int key)
        {
            _phuCapLuongService.DeleteDH(key);
        }

        // thong tin cấp bậc
        [HttpGet]
        public object GetPCGrade(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _phuCapLuongService.GetAllGrade();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult InsertGrade(string values)
        {
            var phucap = new HR_SALARY_GRADE();
            JsonConvert.PopulateObject(values, phucap);
            phucap = _phuCapLuongService.AddGrade(phucap);

            return Ok(phucap);
        }

        [HttpPut]
        public IActionResult UpdateGrade(string key, string values)
        {
            var phucap = _phuCapLuongService.GetGradeById(key);

            if(phucap != null)
            {
                JsonConvert.PopulateObject(values, phucap);
                phucap = _phuCapLuongService.UpdateGrade(phucap);
                return Ok(phucap);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteGrade(string key)
        {
            _phuCapLuongService.DeleteGrade(key);
        }
    }
}
