using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Areas.OpeationMns.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    [Area("OpeationMns")]
    public class ErrorController : Controller
    {
        public IActionResult Index(string id)
        {
            ErrorInforModel model = new ErrorInforModel();
            if(id == CommonConstants.NotFound)
            {
                model.Code = "404";
                model.Title = "Oops! Page not found!";
                model.Message = "The page you requested was not found.";
            }
            return View(model);
        }
    }
}
