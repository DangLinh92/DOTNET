using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Admin.Controllers
{
    public class TrainingListController : AdminBaseController
    {
        private ITrainingTypeService _trainingTypeService;
        private ITrainingListService _trainningListService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TrainingListController(ITrainingTypeService trainingTypeService, ITrainingListService trainningListService, IWebHostEnvironment hostingEnvironment)
        {
            _trainningListService = trainningListService;
            _trainingTypeService = trainingTypeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var lst = _trainningListService.GetAll();
            return View(lst);
        }

        [HttpPost]
        public IActionResult GetTrainingType()
        {
            var lst = _trainingTypeService.GetAll();
            return new OkObjectResult(lst);
        }

        [HttpGet]
        public IActionResult GetTrainingById(string Id)
        {
           var item = _trainningListService.GetById(Guid.Parse(Id));
            return new OkObjectResult(item);
        }

        [HttpGet]
        public IActionResult GetNhanVienTraining(string Id)
        {
            var lstNV = _trainningListService.GetNhanVienTraining(Guid.Parse(Id));
            return new OkObjectResult(lstNV);
        }

        [HttpPost]
        public IActionResult PutTraining(string Id, string TrainnigType, string Trainer, string FromDate, string ToDate, string Description, [FromQuery] string action)
        {
            Guid guidId = Guid.NewGuid();
            if (Id.NullString() != "")
            {
                guidId = Guid.Parse(Id);
            }

            int type = TrainnigType.NullString() != "" ? int.Parse(TrainnigType) : 0;
            Hr_TrainingViewModel model = new Hr_TrainingViewModel(guidId, type, Trainer, FromDate, ToDate, Description, 0);
            if (action == "Add")
            {
                _trainningListService.AddTraining(model);
            }
            else
            {
                _trainningListService.UpdateTraining(model);
            }

            _trainningListService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult DeleteTraining(string Id)
        {
            _trainningListService.Delete(Guid.Parse(Id));
            _trainningListService.Save();
            return new OkObjectResult(null);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportNhanVienToTraining(IList<IFormFile> files, [FromQuery] string param)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                if (param.NullString() == "")
                    return new BadRequestObjectResult("Chọn Lịch Đào Tạo"); ;

                ResultDB rs = _trainningListService.ImportNhanVienDaoTao(filePath,Guid.Parse(param));
                if (rs.ReturnInt == 0)
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(filePath);
                    }

                    return new OkObjectResult(filePath);
                }
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }
    }
}
