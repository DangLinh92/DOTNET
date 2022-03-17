using HRMNS.Application.ViewModels.Time_Attendance;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class BangCongController : AdminBaseController
    {
        public IActionResult Index()
        {
            var lst = new List<AttendanceRecordViewModel>()
            {
                new AttendanceRecordViewModel()
                {
                    MaNV = "H2105001",
                    HR_NHANVIEN = new HRMNS.Application.ViewModels.HR.NhanVienViewModel()
                    {
                        Id = "H2105001",
                        TenNV = "LE VAN DANG",
                        NgayVao = "2021-05-20",
                        HR_BO_PHAN_DETAIL = new HRMNS.Application.ViewModels.HR.BoPhanDetailViewModel()
                        {
                            TenBoPhanChiTiet = "Support PI"
                        },
                    },
                    Time_Check = "2022-03-17"
                }
            };
            return View(lst);
        }
    }
}
