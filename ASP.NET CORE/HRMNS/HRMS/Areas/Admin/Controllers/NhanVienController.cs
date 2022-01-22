using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public IActionResult Index()
        {
            ViewBag.BoPhans = _boPhanService.GetAll(null);
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return View(nhanviens);
        }

        [HttpGet]
        public IActionResult OnGetPartialData()
        {
            List<NhanVienViewModel> nhanviens = _nhanvienService.GetAll();
            return PartialView("_NhanVienGridPartial", nhanviens);
        }

        [HttpPost]
        public IActionResult SaveEmployee(NhanVienCustomizeViewModel nhanvienVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                bool isAdd = nhanvienVm.Action == CommonConstants.Add_Action;
                bool notExist = !_nhanvienService.GetAll().Any(x => x.Id == nhanvienVm.NhanVien.Id);
                //bool notExist = nv == null;

                if (isAdd && notExist)
                {
                    _nhanvienService.Add(nhanvienVm.NhanVien);
                }
                else if (isAdd && !notExist)
                {
                    return new ConflictObjectResult(CommonConstants.ConflictObjectResult_Msg);
                }
                else if (!isAdd && notExist)
                {
                    return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
                }
                else if (!isAdd && !notExist)
                {
                    _nhanvienService.Update(nhanvienVm.NhanVien);
                }

                _nhanvienService.Save();

                return new OkObjectResult(nhanvienVm.NhanVien);
            }
        }

        [HttpGet]
        public IActionResult GetById(string Id)
        {
           NhanVienViewModel nhanVien = _nhanvienService.GetById(Id);
            return new OkObjectResult(nhanVien);
        }

        //public IActionResult Index(
        //    [FromQuery(Name = "id")] string id,
        //    [FromQuery(Name = "name")] string name,
        //    [FromQuery(Name = "dept")] string dept)
        //{

        //    ViewBag.BoPhans = _boPhanService.GetAll(null);
        //    ViewBag.SearchId = id;
        //    ViewBag.SearchName = name;
        //    ViewBag.SearchDept = dept;

        //    if (dept == "ALL")
        //    {
        //        dept = "";
        //    }

        //    List<NhanVienViewModel> nhanviens = _nhanvienService.Search(id, name, dept);
        //    return View(nhanviens);
        //}

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
