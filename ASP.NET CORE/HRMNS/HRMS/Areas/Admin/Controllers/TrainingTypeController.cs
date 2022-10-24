using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Admin.Controllers
{
    public class TrainingTypeController : AdminBaseController
    {
        private ITrainingTypeService _trainingTypeService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TrainingTypeController(ITrainingTypeService trainingTypeService, IWebHostEnvironment hostingEnvironment)
        {
            _trainingTypeService = trainingTypeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var lst = _trainingTypeService.GetAll();
            return View(lst);
        }

        [HttpPost]
        public IActionResult UpTrainingType(string Id,string TrainName,string Description,string Status, [FromQuery]string action)
        {
            TrainingTypeViewModel model = new TrainingTypeViewModel(TrainName, Description, Status);

            if(Id.NullString() != "")
            {
                model.Id = int.Parse(Id);
            }

            if (action == "Add")
            {
                _trainingTypeService.Add(model);
            }
            else
            {
                _trainingTypeService.Update(model);
            }

            _trainingTypeService.Save();
            return new OkObjectResult(null);
        }

        [HttpGet]
        public IActionResult GetTypeById(int Id)
        {
            var model =_trainingTypeService.GetById(Id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult DeleteType(int Id)
        {
            _trainingTypeService.Delete(Id);
            _trainingTypeService.Save();
            return new OkObjectResult(Id);
        }

    }
}
