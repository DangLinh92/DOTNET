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
    public class NhanVienThaiSanController : AdminBaseController
    {
        INhanVienThaiSanService _nhanVienThaiSanService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NhanVienThaiSanController(INhanVienThaiSanService nhanVienThaiSanService, IWebHostEnvironment hostingEnvironment)
        {
            _nhanVienThaiSanService = nhanVienThaiSanService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(Department))
            {
                var lst = _nhanVienThaiSanService.GetAll().OrderByDescending(x => x.DateModified).ToList();
                return View(lst);
            }
            else
            {
                var lst = _nhanVienThaiSanService.GetAll().FindAll(x=>x.HR_NHANVIEN.MaBoPhan == Department).OrderByDescending(x => x.DateModified).ToList();
                return View(lst);
            }
        }

        [HttpPost]
        public IActionResult Search(string maNV,string fromTime, string toTime)
        {
            var lst = _nhanVienThaiSanService.Search(maNV, fromTime, toTime);
            return PartialView("_gridThaiSanPartialView", lst);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ts = _nhanVienThaiSanService.GetById(id,x=>x.HR_NHANVIEN);
            return new OkObjectResult(ts);
        }

        [HttpPost]
        public IActionResult SaveThaiSan(NhanVienThaiSanViewModel model,[FromQuery]string action)
        {
            if(action == "Add")
            {
                var nv = _nhanVienThaiSanService.GetById(model.Id);
                if(nv != null)
                {
                    nv.MaNV = model.MaNV;
                    nv.CheDoThaiSan = model.CheDoThaiSan;
                    nv.FromDate = model.FromDate;
                    nv.ToDate = model.ToDate;

                    _nhanVienThaiSanService.Update(nv);
                }
                else
                {
                    _nhanVienThaiSanService.Add(model);
                }
            }
            else
            {
                var nv = _nhanVienThaiSanService.GetById(model.Id);
                if (nv != null)
                {
                    nv.MaNV = model.MaNV;
                    nv.CheDoThaiSan = model.CheDoThaiSan;
                    nv.FromDate = model.FromDate;
                    nv.ToDate = model.ToDate;

                    _nhanVienThaiSanService.Update(nv);
                }
            }

            _nhanVienThaiSanService.Save();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _nhanVienThaiSanService.Delete(id);
            _nhanVienThaiSanService.Save();
            return new OkObjectResult(id);
        }
    }
}
