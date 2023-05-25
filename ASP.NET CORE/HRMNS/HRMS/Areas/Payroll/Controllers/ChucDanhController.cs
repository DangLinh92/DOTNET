using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Areas.Payroll.Controllers
{
    public class ChucDanhController : AdminBaseController
    {
        IChucDanhService _chucDanhService;
        public ChucDanhController(IChucDanhService chucDanhService)
        {
            _chucDanhService = chucDanhService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var chucDanhs = _chucDanhService.GetAll(null);
            return new OkObjectResult(chucDanhs);
        }

        [HttpGet]
        public IActionResult GetAll2()
        {
            var chucDanhs = _chucDanhService.GetAll(null).Where(x=>x.PhuCap > 0);
            return new OkObjectResult(chucDanhs);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            ChucDanhViewModel model = new ChucDanhViewModel();
            JsonConvert.PopulateObject(values, model);

            _chucDanhService.Add(model);
            _chucDanhService.Save();
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            var entity = _chucDanhService.GetById(key);

            JsonConvert.PopulateObject(values, entity);
            HR_CHUCDANH chucdanh = new HR_CHUCDANH()
            {
                Id = key,
                TenChucDanh = entity.TenChucDanh,
                PhuCap = entity.PhuCap
            };
            _chucDanhService.Update(chucdanh);
            _chucDanhService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(string key)
        {
            _chucDanhService.Delete(key);
            _chucDanhService.Save();
        }
    }
}
