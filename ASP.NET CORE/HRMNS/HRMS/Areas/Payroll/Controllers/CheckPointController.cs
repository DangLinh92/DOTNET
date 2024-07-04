using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Payroll.Controllers
{
    public class CheckPointController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
