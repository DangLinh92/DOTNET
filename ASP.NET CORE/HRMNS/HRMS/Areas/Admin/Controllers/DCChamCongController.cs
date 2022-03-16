using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class DCChamCongController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        IDM_DCChamCongService _dmDieuChinhService;
        IDCChamCongService _dcChamCongService;

        public DCChamCongController(IDM_DCChamCongService dmDCChamCongservice, IDCChamCongService dcChamCongService, IWebHostEnvironment webHost)
        {
            _hostingEnvironment = webHost;
            _dmDieuChinhService = dmDCChamCongservice;
            _dcChamCongService = dcChamCongService;
        }

        [HttpGet]
        public IActionResult GetDanhMucDieuChinh()
        {
            var lst = _dmDieuChinhService.GetAll("");
            return new OkObjectResult(lst);
        }

        [HttpPost]
        public IActionResult SaveDanhMucDieuChinh(DMDieuChinhChamCongViewModel vm)
        {
            var entity = _dmDieuChinhService.GetById(vm.Id);
            if (entity == null)
            {
                _dmDieuChinhService.Add(vm);
            }
            else
            {
                entity.TieuDe = vm.TieuDe;
                _dmDieuChinhService.Update(entity);
            }
            _dmDieuChinhService.Save();
            return new OkObjectResult(vm);
        }

        [HttpPost]
        public IActionResult SaveDieuChinhChamCong(DCChamCongViewModel vm)
        {
            var entity = _dcChamCongService.GetById(vm.Id);

            if (entity == null)
            {
                _dcChamCongService.Add(vm);
            }
            else
            {
                entity.CopyPropertiesFrom(vm,
                    new List<string>()
                    {
                        nameof(vm.Id),nameof(vm.MaNV),
                        nameof(vm.DateCreated), nameof(vm.DateModified),
                        nameof(vm.UserCreated), nameof(vm.UserModified),
                    });
                _dcChamCongService.Update(entity);
            }
            _dcChamCongService.Save();
            return new OkObjectResult(vm);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var entity = _dcChamCongService.GetById(id);
            return new OkObjectResult(entity);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dcChamCongService.Delete(id);
            _dcChamCongService.Save();
            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult Search(string department, string status, string timeFrom, string timeTo)
        {
            var lst = _dcChamCongService.Search(status, department, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DM_DIEUCHINH_CHAMCONG);
            return PartialView("_gridDCChamCongPartialView", lst);
        }

        public IActionResult Index()
        {
            var lst = _dcChamCongService.GetAll("", x => x.DM_DIEUCHINH_CHAMCONG, y => y.HR_NHANVIEN);
            return View(lst);
        }
    }
}
