using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Implementation;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Data.Entities;
using System;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class DateOffLineSampleController : AdminBaseController
    {
        private IDateOffLineSampleService _dateOffLineSampleService;

        public DateOffLineSampleController(IDateOffLineSampleService dateOffLineSampleService)
        {
            _dateOffLineSampleService = dateOffLineSampleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_dateOffLineSampleService.GetAllData(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
             DATE_OFF_LINE_SAMPLE model = new DATE_OFF_LINE_SAMPLE();
            JsonConvert.PopulateObject(values, model);

            if(DateTime.TryParse(model.ItemValue,out DateTime _d))
            {
                if(_d.DayOfWeek != DayOfWeek.Sunday)
                {
                    model.ItemValue = _d.ToString("yyyy-MM-dd");
                    _dateOffLineSampleService.Add(model);
                    _dateOffLineSampleService.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _dateOffLineSampleService.GetById(key);
            JsonConvert.PopulateObject(values, model);

            if (DateTime.TryParse(model.ItemValue, out DateTime _d))
            {
                if (_d.DayOfWeek != DayOfWeek.Sunday)
                {
                    model.ItemValue = _d.ToString("yyyy-MM-dd");
                    _dateOffLineSampleService.Update(model);
                    _dateOffLineSampleService.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _dateOffLineSampleService.Delete(key);
        }
    }
}
