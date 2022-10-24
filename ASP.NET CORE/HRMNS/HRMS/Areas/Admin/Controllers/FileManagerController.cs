using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class FileManagerController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
