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
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsTongHopDanhMucKeHoachController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IDanhMucKeHoachService _danhMucKeHoachService;
        private readonly IMemoryCache _memoryCache;

        List<TotalAllItemByYear> TotalAllItemByYear = new List<TotalAllItemByYear>();

        public EhsTongHopDanhMucKeHoachController(IDanhMucKeHoachService danhMucKeHoachService, IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _danhMucKeHoachService = danhMucKeHoachService;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object TongHopKeHoachByYear(DataSourceLoadOptions loadOptions, string year)
        {
            if (year.NullString() == "")
                year = DateTime.Now.Year.ToString();

            var model = _danhMucKeHoachService.TongHopKeHoachByYear(year);

            _memoryCache.Remove("TongHopKeHoachByYear");
            _memoryCache.Set("TongHopKeHoachByYear", model);

            return DataSourceLoader.Load(model, loadOptions);
        }

        /// <summary>
        /// lấy file báo cáo theo nội dung kế hoạch
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="makehoach">Mã kế hoạch</param>
        /// <returns></returns>
        [HttpGet]
        public object GetFileByKeHoach(DataSourceLoadOptions loadOptions, string makehoach)
        {
            _memoryCache.TryGetValue("TongHopKeHoachByYear", out TotalAllItemByYear);
            EhsFileKetQuaViewModel kq;
            List<EhsFileKetQuaViewModel> lstKetQua = new List<EhsFileKetQuaViewModel>();
            string url = "/";
            foreach (var item in TotalAllItemByYear.Where(x=>x.MaKeHoach == makehoach))
            {
                foreach (var tg in _danhMucKeHoachService.GetThoiGianThucHien(item.MaKeHoach))
                {
                    url = _danhMucKeHoachService.GetFolderKetQua(tg.MaNgayChiTiet);

                    if(url != "")
                    {
                        kq = new EhsFileKetQuaViewModel()
                        {
                            NguoiPhuTrach = item.NguoiPhuTrach,
                            NoiDung = item.TenNoiDung,
                            TenDeMuc = item.TenDeMuc,
                            ThoiGianBatDau = tg.ThoiGianBatDau,
                            ThoiGianKetThuc = tg.ThoiGianKetThuc,
                            MaNgayChiTiet = tg.MaNgayChiTiet,
                            Status = tg.Status,
                            UrlFile = url,
                            KetQua = tg.KetQua
                        };
                        lstKetQua.Add(kq);
                    }
                };
            }

            return DataSourceLoader.Load(lstKetQua, loadOptions);
        }


        public IActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Get kế hoạch trong khoảng thời gian
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>
        [HttpGet]
        public object ListKeHoach(DataSourceLoadOptions loadOptions, string fromTime, string toTime)
        {
            var data = _danhMucKeHoachService.DanhSachKeHoachByTime(fromTime, toTime);
            return DataSourceLoader.Load(data, loadOptions);
        }
    }
}
