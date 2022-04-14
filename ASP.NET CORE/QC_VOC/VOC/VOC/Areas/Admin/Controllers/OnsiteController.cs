using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Utilities.Constants;

namespace VOC.Areas.Admin.Controllers
{
    public class OnsiteController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IVocOnsiteSevice _vocOnsiteService;

        public OnsiteController(IVocOnsiteSevice vocOnsiteService, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _vocOnsiteService = vocOnsiteService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lst = GetData(DateTime.Now.Year, CommonConstants.SEV, CommonConstants.ALL);
            return View(lst);
        }

        [HttpPost]
        public IActionResult Search(int year, string customer,string part)
        {
            var lst = GetData(year, customer, part);
            return View("Index", lst);
        }

        private VocOnsiteList GetData(int year, string customer, string part)
        {
            VocOnsiteList vocOnsiteList = new VocOnsiteList()
            {
                Customer = customer,
                Year = year,
                Part = part
            };

            vocOnsiteList.vocOnsiteModels = _vocOnsiteService.SumDataOnsite(year, customer, part);

            var groups = vocOnsiteList.vocOnsiteModels.GroupBy(x => (x.Week, x.Customer)).Select(gr => (gr, Total: gr.Count()));
            VocOnsiteSumWeek sumWeek;
            foreach (var group in groups)
            {
                sumWeek = new VocOnsiteSumWeek()
                {
                    Time = group.gr.Key.Week,
                    QTY = group.Total,
                    Customer = group.gr.Key.Customer,
                    OK = 0,
                    NG = 0
                };

                foreach (var item in group.gr)
                {
                    sumWeek.OK += item.OK;
                    sumWeek.NG += item.NG;
                }

                vocOnsiteList.vocOnsiteSumWeeks.Add(sumWeek);
            }
            vocOnsiteList.vocOnsiteSumWeeks = vocOnsiteList.vocOnsiteSumWeeks.OrderBy(x => x.Time).ToList();
            return vocOnsiteList;
        }
    }
}
