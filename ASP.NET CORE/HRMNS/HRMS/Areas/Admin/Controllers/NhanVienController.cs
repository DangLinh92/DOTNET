using HRMNS.Application.Interfaces;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class NhanVienController : AdminBaseController
    {
        INhanVienService _nhanvienService;
        IBoPhanService _boPhanService;
            
        public NhanVienController(INhanVienService nhanVienService,IBoPhanService boPhanService)
        {
            _nhanvienService = nhanVienService;
            _boPhanService = boPhanService;
        }

        public IActionResult Index()
        {
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            var model = _nhanvienService.GetAll();
            return View(model);
        }

        //#region AJAX API
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var model = _nhanvienService.GetAll();
        //    return View(model);
        //}
        //#endregion
    }
}
