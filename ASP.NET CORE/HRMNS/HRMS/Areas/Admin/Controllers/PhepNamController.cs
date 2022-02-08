using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class PhepNamController : AdminBaseController
    {
        IPhepNamService _phepNamService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PhepNamController(IPhepNamService phepNamService, IWebHostEnvironment hostingEnvironment)
        {
            _phepNamService = phepNamService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePhepNam(List<PhepNamViewModel> phepNams)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                foreach (var item in phepNams)
                {
                    PhepNamViewModel phepNam = _phepNamService.GetById(item.Id);
                    if (phepNam != null)
                    {
                        phepNam.MaNhanVien = item.MaNhanVien;
                        phepNam.SoPhepNam = item.SoPhepNam;
                        phepNam.SoPhepConLai = item.SoPhepConLai;
                        _phepNamService.Update(phepNam);
                    }
                    else
                    {
                        _phepNamService.Add(item);
                    }
                }
                _phepNamService.Save();
                return PartialView("/Areas/Admin/Views/NhanVien/_profileCompassionateLeavePartial.cshtml", phepNams);
            }
        }
    }
}
