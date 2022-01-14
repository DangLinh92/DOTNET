using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
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

        public NhanVienController(INhanVienService nhanVienService, IBoPhanService boPhanService)
        {
            _nhanvienService = nhanVienService;
            _boPhanService = boPhanService;
        }

        //public IActionResult Index()
        //{
        //    ViewBag.BoPhans = _boPhanService.GetAll(null);
        //    nhanviens = _nhanvienService.GetAll();
        //    return View(nhanviens);
        //}

        public IActionResult Index(
            [FromQuery(Name = "id")] string id,
            [FromQuery(Name = "name")] string name,
            [FromQuery(Name = "dept")] string dept)
        {

            ViewBag.BoPhans = _boPhanService.GetAll(null);
            ViewBag.SearchId = id;
            ViewBag.SearchName = name;
            ViewBag.SearchDept = dept;

            if (dept == "ALL")
            {
                dept = "";
            }

            List<NhanVienViewModel> nhanviens = _nhanvienService.Search(id, name, dept);
            return View(nhanviens);
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
