using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
