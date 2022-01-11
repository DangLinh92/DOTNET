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
            
        public NhanVienController(INhanVienService nhanVienService)
        {
            _nhanvienService = nhanVienService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _nhanvienService.GetAll();
            return new OkObjectResult(new GenericResult(true, model));
        }
        #endregion
    }
}
