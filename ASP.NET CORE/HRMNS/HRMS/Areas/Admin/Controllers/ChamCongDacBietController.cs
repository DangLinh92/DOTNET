using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Admin.Controllers
{
    public class ChamCongDacBietController : AdminBaseController
    {
        private IDangKyChamCongDacBietService _chamCongDacBietService;
        private IDangKyChamCongChiTietService _chamCongChiTietService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ChamCongDacBietController(IDangKyChamCongDacBietService chamCongService, IDangKyChamCongChiTietService chamCongChiTietService, IWebHostEnvironment hostingEnvironment)
        {
            _chamCongDacBietService = chamCongService;
            _chamCongChiTietService = chamCongChiTietService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lst = _chamCongDacBietService.GetAll(x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET);
            return View(lst);
        }

        [HttpPost]
        public IActionResult RegisterChamCongDB(DangKyChamCongDacBietViewModel data, [FromQuery] string action)
        {
            if (action == "Add")
            {
                var itemCheck = _chamCongDacBietService.GetSingle(x => x.MaNV == data.MaNV && x.MaChamCong_ChiTiet == data.MaChamCong_ChiTiet && x.NgayBatDau == data.NgayBatDau && x.NgayKetThuc == data.NgayKetThuc);

                if(itemCheck != null)
                {
                    itemCheck.MaChamCong_ChiTiet = data.MaChamCong_ChiTiet;
                    itemCheck.NgayBatDau = data.NgayBatDau;
                    itemCheck.NgayKetThuc = data.NgayKetThuc;
                    itemCheck.NoiDung = data.NoiDung;
                    _chamCongDacBietService.Update(itemCheck);
                }
                else
                {
                    _chamCongDacBietService.Add(data);
                }
            }
            else
            {
                DangKyChamCongDacBietViewModel chamcongVm = _chamCongDacBietService.GetById(data.Id);
                chamcongVm.MaChamCong_ChiTiet = data.MaChamCong_ChiTiet;
                chamcongVm.NgayBatDau = data.NgayBatDau;
                chamcongVm.NgayKetThuc = data.NgayKetThuc;
                chamcongVm.NoiDung = data.NoiDung;

                _chamCongDacBietService.Update(chamcongVm);
            }

            _chamCongDacBietService.Save();
            return new OkObjectResult(data);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
           return new OkObjectResult( _chamCongDacBietService.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _chamCongDacBietService.Delete(id);
            _chamCongDacBietService.Save();
            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult Search(string department, string timeFrom, string timeTo)
        {
           var lst = _chamCongDacBietService.Search(department, timeFrom, timeTo, x => x.HR_NHANVIEN, y => y.DANGKY_CHAMCONG_CHITIET);
            return PartialView("_gridChamCongDacBietPartialView", lst);
        }

        [HttpGet]
        public IActionResult GetDmucChamCongChiTiet()
        {
            var lst = _chamCongChiTietService.GetAll(x => x.DM_DANGKY_CHAMCONG);
            return new OkObjectResult(lst);
        }
    }
}
