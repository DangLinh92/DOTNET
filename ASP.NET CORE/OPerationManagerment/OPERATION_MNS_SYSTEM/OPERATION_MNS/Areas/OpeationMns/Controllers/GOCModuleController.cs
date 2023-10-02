using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    /// <summary>
    /// Bộ Phận GOC
    /// </summary>
    public class GOCModuleController : AdminBaseController
    {
        private IGOCModuleService moduleService;
        public GOCModuleController(IGOCModuleService gocService)
        {
            moduleService = gocService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SalesView()
        {
            return View();
        }
    }
}
