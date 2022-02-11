using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class HRLoaiHDController : AdminBaseController
    {
        IHRLoaiHopDongService _hrLoaiHDService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HRLoaiHDController(IHRLoaiHopDongService loaiHDService, IWebHostEnvironment hostingEnvironment)
        {
            _hrLoaiHDService = loaiHDService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           List<LoaiHopDongViewModel> lstLoaiHD = _hrLoaiHDService.GetAll();
            return new OkObjectResult(lstLoaiHD);
        }
    }
}
