using Layout_SectionInAspCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Layout_SectionInAspCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Name"] = "danglv";
            ViewData["Info"] = new UserModel()
            {
                Name = "le van dang",
                Age = 19
            };

            ViewBag.UserInfo = new UserModel()
            {
                Name = "dang.levan",
                Age = 19
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            var user = new UserModel() {Name = "danglv", Age = 28};
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductEditModel model)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                message = "product " + model.Name + " Rate " + model.Rate + " With Rating " + model.Rating +
                          " create success";
            }
            else
            {
                message = "Failed to create the product";
            }

            return View();//return Content(message);
        }
    }
}
