using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Luong.Controllers
{
    [Area("Luong")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
