using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class MaterialToSapCodeController : AdminBaseController
    {
        private IMaterialToSapCodeService _materialToSapCodeService;
        public MaterialToSapCodeController(IMaterialToSapCodeService materialToSapCodeService)
        {
            _materialToSapCodeService = materialToSapCodeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_materialToSapCodeService.GetAllData(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            MaterialToSapViewModel model = new MaterialToSapViewModel();
            JsonConvert.PopulateObject(values, model);

            _materialToSapCodeService.Add(model);
            _materialToSapCodeService.Save();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _materialToSapCodeService.GetById(key);

            JsonConvert.PopulateObject(values, model);
            _materialToSapCodeService.Update(model);
            _materialToSapCodeService.Save();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _materialToSapCodeService.Delete(key);
        }
    }
}
