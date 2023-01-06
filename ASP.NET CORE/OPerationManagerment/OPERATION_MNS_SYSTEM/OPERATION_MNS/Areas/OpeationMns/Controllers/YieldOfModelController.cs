using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class YieldOfModelController : AdminBaseController
    {
        private IYieldOfModelService _yieldOfModelService;
        public YieldOfModelController(IYieldOfModelService yieldOfModelService)
        {
            _yieldOfModelService = yieldOfModelService;
        }

        public IActionResult Index()
        {
            var lst = _yieldOfModelService.GetAllYeildOfModel();
            return View(lst);
        }

        [HttpPost]
        public IActionResult PutYield(YieldOfModelViewModel mode, [FromQuery] string action)
        {
            if (action == "Add")
            {
                _yieldOfModelService.AddYeildOfModel(mode);
            }
            else
            {
                _yieldOfModelService.UpdateYeildOfModel(mode);
            }

            _yieldOfModelService.Save();
            return new OkObjectResult(mode);
        }

        [HttpPost]
        public IActionResult DeleteYield(int id)
        {
            _yieldOfModelService.Delete(id);
            _yieldOfModelService.Save();
            return new OkObjectResult(id);
        }

        [HttpGet]
        public IActionResult GetYieldById(int id)
        {
            var model = _yieldOfModelService.GetYeildOfModelById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllModel()
        {
            var lst = _yieldOfModelService.GetAllModel();
            return new OkObjectResult(lst);
        }
    }
}
