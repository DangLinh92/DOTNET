using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Areas.Admin.Controllers
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
    }
}
