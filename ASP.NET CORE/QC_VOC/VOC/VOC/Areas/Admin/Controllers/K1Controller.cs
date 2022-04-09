using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VOC.Areas.Admin.Controllers
{
    public class K1Controller : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
