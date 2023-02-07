using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsDanhMucKeHoachController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IDanhMucKeHoachService _danhMucKeHoachService;
        public EhsDanhMucKeHoachController(IDanhMucKeHoachService danhMucKeHoachService, IWebHostEnvironment hostingEnvironment)
        {
            _danhMucKeHoachService = danhMucKeHoachService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            EhsDanhMucKeHoachPageViewModel model = _danhMucKeHoachService.GetDataDanhMucKeHoachPage(null);
            return View(model);
        }

        [HttpPost]
        public IActionResult GetNoiDungLuatKeHoach(string kehoachID)
        {
            var lst = _danhMucKeHoachService.GetLuatDinhKeHoach(Guid.Parse(kehoachID));
            string ldinh = lst.FirstOrDefault()?.NoiDungLuatDinh;
            return new OkObjectResult(ldinh);
        }

        [HttpPost]
        public IActionResult AddKeHoach(string nameVN, string nameKR, string luatDinh)
        {
            Guid idKh = Guid.NewGuid();
            int maxOrder = 0;
            if (_danhMucKeHoachService.GetDataDanhMucKeHoachPage(null).EhsDMKeHoachViewModels.OrderByDescending(x => x.OrderDM).FirstOrDefault() != null)
            {
                maxOrder = _danhMucKeHoachService.GetDataDanhMucKeHoachPage(null).EhsDMKeHoachViewModels.OrderByDescending(x => x.OrderDM).FirstOrDefault().OrderDM + 1;
            }

            EhsDMKeHoachViewModel ehsDM = new EhsDMKeHoachViewModel()
            {
                Id = idKh,
                TenKeHoach_VN = nameVN,
                TenKeHoach_KR = nameKR,
                UserCreated = UserName,
                UserModified = UserName,
                OrderDM = maxOrder
            };
            EhsLuatDinhKeHoachViewModel luatDinhKH = new EhsLuatDinhKeHoachViewModel()
            {
                MaKeHoach = idKh,
                NoiDungLuatDinh = luatDinh,
                UserCreated = UserName,
                UserModified = UserName
            };
            ehsDM.EHS_LUATDINH_KEHOACH.Add(luatDinhKH);

            _danhMucKeHoachService.UpdateDMKeHoach(ehsDM);
            _danhMucKeHoachService.Save();

            return new OkObjectResult(idKh);
        }

        [HttpPost]
        public IActionResult DeleteKeHoach(string maKeHoach)
        {
            _danhMucKeHoachService.DeleteDMKeHoach(Guid.Parse(maKeHoach));
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maKeHoach);
        }
    }
}
