using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Lfem;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class LfemMonitoringController : AdminBaseController
    {
        IInventoryService _InventoryService;
        public LfemMonitoringController(IInventoryService service)
        {
            _InventoryService = service;
        }

        public IActionResult TotalAndFinishView()
        {
            LotMonitoringLfemViewModel model = _InventoryService.GetMonitoringLfemData("","",-1,"1");
            return View(model.TotalProduct);
        }

        [HttpPost]
        public IActionResult SearchMonitoringLfem(string operation,string model,decimal stayday)
        {
            TotalOperation data = _InventoryService.GetMonitoringLfemData(operation, model, stayday,"1").TotalProduct;
            return View("TotalAndFinishView", data);
        }

        public IActionResult FinishProductView()
        {
            LotMonitoringLfemViewModel model = _InventoryService.GetMonitoringLfemData("", "", -1, "3");
            return View(model.FinishProduct);
        }

        [HttpPost]
        public IActionResult SearchMonitoringFinishProcLfem(string operation, string model, decimal stayday)
        {
            TotalOperation data = _InventoryService.GetMonitoringLfemData(operation, model, stayday, "3").FinishProduct;
            return View("FinishProductView", data);
        }
        public IActionResult ProcessProductView()
        {
            LotMonitoringLfemViewModel model = _InventoryService.GetMonitoringLfemData("", "", -1, "2");
            return View(model);
        }
    }
}
