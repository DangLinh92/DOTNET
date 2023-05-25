using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    public class ConDoanController : AdminBaseController
    {
        private readonly ICongDoanNotJoinService _congDoanNotJoinService; 
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ConDoanController(ICongDoanNotJoinService congDoanNotJoinService, IWebHostEnvironment hostingEnvironment)
        {
            _congDoanNotJoinService = congDoanNotJoinService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);
            SetSessionInpage(CommonConstants.OUT);
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            var lstModel = _congDoanNotJoinService.GetAll();
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var congdoan = new CONGDOAN_NOT_JOIN();
            JsonConvert.PopulateObject(values, congdoan);
            congdoan = _congDoanNotJoinService.Add(congdoan);

            return Ok(congdoan);
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _congDoanNotJoinService.Delete(key);
        }
    }
}
