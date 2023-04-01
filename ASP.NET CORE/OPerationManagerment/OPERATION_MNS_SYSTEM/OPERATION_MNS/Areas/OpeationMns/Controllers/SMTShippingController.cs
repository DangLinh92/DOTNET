using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class SMTShippingController : AdminBaseController
    {
        public IActionResult Index()
        {
            if(DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 23)
            {
                ViewBag.DateWip = DateTime.Now;
            }
            else
            {
                ViewBag.DateWip = DateTime.Now.AddDays(-1);
            }
            
            return View();
        }
    }
}
