using FirstEmptyWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstEmptyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new IndexModel();
            model.Message = "Hello from model";
            return View(model);
        }

        public string Detail(string id)
        {
            return "Detail"+id;
        }
    }
}
