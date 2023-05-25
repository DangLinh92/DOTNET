using HRMNS.Data.EF.Extensions;
using HRMS.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    [Authorize]
    [Area("Payroll")]
    public class AdminBaseController : Controller
    {
        public ILogger _logger;
        public string Department { get => User.Claims.FirstOrDefault(x => x.Type == "Deparment").Value.NullString(); }
        public string UserRole 
        { 
            get => User.Claims.FirstOrDefault(x => x.Type == "Roles").Value.NullString().Split(';')[0]; 
        }

        public string UserName
        {
            get => User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.NullString().Split(';')[0];
        }

        public void DeleteFileSr(IWebHostEnvironment _hostingEnvironment)
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files/sr");
            if (Directory.Exists(directory))
            {
                foreach (var file in Directory.GetFiles(directory))
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        public void SetSessionInpage(string inOut)
        {
            HttpContext.Session.Set("IsInBangLuongChiTiet", inOut);
        }
    }
}
