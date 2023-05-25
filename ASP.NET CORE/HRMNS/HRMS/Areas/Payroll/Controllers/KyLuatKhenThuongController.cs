using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRMS.Areas.Payroll.Controllers
{
    public class KyLuatKhenThuongController : AdminBaseController
    {
        private IKyLuatKhenThuongService _kyLuatKhenThuongService;
        public KyLuatKhenThuongController(IKyLuatKhenThuongService kyLuatKhenThuongService)
        {
            _kyLuatKhenThuongService = kyLuatKhenThuongService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_kyLuatKhenThuongService.GetAll(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            HR_KY_LUAT_KHENTHUONG model = new HR_KY_LUAT_KHENTHUONG();
            JsonConvert.PopulateObject(values, model);

            _kyLuatKhenThuongService.Add(model);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var entity = _kyLuatKhenThuongService.FindById(key);
            JsonConvert.PopulateObject(values, entity);
            _kyLuatKhenThuongService.Update(entity);
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _kyLuatKhenThuongService.Delete(key);
        }
    }
}
