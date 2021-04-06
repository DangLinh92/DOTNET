using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFirst.Controllers
{
    public class ProductController : Controller
    {
        [Route("[controller]")]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id?}")]
        public IActionResult Index(string id)
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            return View(id);
        }
    }
}
