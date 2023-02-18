using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsKanBanController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDanhMucKeHoachService _danhMucKeHoachService;
        private readonly IMemoryCache _memoryCache;

        public EhsKanBanController(IWebHostEnvironment hostingEnvironment, IDanhMucKeHoachService danhMucKeHoachService, IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _danhMucKeHoachService = danhMucKeHoachService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
           var data = _danhMucKeHoachService.GetKanBanBoard();
            _memoryCache.Remove("Task");
            _memoryCache.Set("Task", data);

            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateEventAfterDrag(List<string> pendings, List<string> todos, List<string> inprogress, List<string> complete)
        {
            foreach (var id in pendings)
            {
                 _danhMucKeHoachService.UpdateEvent(id, "Pending", "", "", "","", "DRAG");
            }

            foreach (var id in todos)
            {
                _danhMucKeHoachService.UpdateEvent(id, "TODO", "", "", "", "", "DRAG");
            }

            foreach (var id in inprogress)
            {
                _danhMucKeHoachService.UpdateEvent(id, "Inprogress", "", "", "", "", "DRAG");
            }

            foreach (var id in complete)
            {
                _danhMucKeHoachService.UpdateEvent(id, "Completed", "", "", "", "", "DRAG");
            }

            return Ok(true);
        }

        List<KanbanViewModel> TaskData;

        [HttpPost]
        public IActionResult GetTaskById(string id)
        {
            TaskData = new List<KanbanViewModel>();
            _memoryCache.TryGetValue("Task", out TaskData);

            KanbanViewModel data = TaskData.SingleOrDefault(x => x.Id == id);
            return Ok(data);
        }

        [HttpPost]
        public IActionResult UpdateTask(KanbanViewModel model)
        {
            _danhMucKeHoachService.UpdateEvent(model.Id, model.Status, model.Priority, model.Progress + "", model.ActualFinish,model.BeginTime, "EDIT");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult HideTask(string id)
        {
            _danhMucKeHoachService.UpdateEvent(id, "", "", "", "", "", "DELETE");
            return RedirectToAction("Index");
        }
    }
}
