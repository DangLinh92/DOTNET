using VOC.Utilities.Constants;
using VOC.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            else
            {
                model.Code = "404";
                model.Title = "Oops! Error!";
                model.Message = id;
            }
            return View(model);
        }
    }
}
