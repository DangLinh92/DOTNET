using DevExpress.ClipboardSource.SpreadsheetML;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OPERATION_MNS.Application.Implementation;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.LotTracking;
using OPERATION_MNS.Data.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class LotTrackingController : AdminBaseController
    {
        private ILotTrackingService _lotTrackingService;
        private readonly IMemoryCache _memoryCache;
        public LotTrackingController(ILotTrackingService lotTrackingService, IMemoryCache memoryCache)
        {
            _lotTrackingService = lotTrackingService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetLotTraking(DataSourceLoadOptions loadOptions, string lotModule, string affectLot)
        {
            List<WaferInfo> data;

            if (lotModule.NullString() != "")
            {
                data = _lotTrackingService.GetTrackingVIEWs(lotModule);
            }
            else if (affectLot.NullString() != "")
            {
                data = _lotTrackingService.GetAffectlotInfo(affectLot);
            }
            else
            {
                data = new List<WaferInfo>();
            }

            _memoryCache.Remove("LotTrakingData");
            _memoryCache.Set("LotTrakingData", data);

            return DataSourceLoader.Load(data, loadOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cassetId"></param>
        /// <param name="lotId">WLP1 Lot Number</param>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetLotHistory(string lotId, DataSourceLoadOptions loadOptions)
        {
            _memoryCache.TryGetValue("LotTrakingData", out List<WaferInfo> data);
            if (data != null)
            {
                return DataSourceLoader.Load(data.Where(x => x.LotID == lotId).Select(x => x.LotHistories).FirstOrDefault(), loadOptions);
            }
            return DataSourceLoader.Load(new List<LotHistory>(), loadOptions);
        }

        //[HttpPost]
        //public IActionResult GetLotTraking(string lotModule)
        //{
        //    if (lotModule.NullString() != "")
        //    {
        //        var data = _lotTrackingService.GetPackingInfo(lotModule);
        //        LotTrackingWithLotId models = new LotTrackingWithLotId()
        //        {
        //            lotModule = lotModule,
        //            LotTrackingViewModels = data
        //        };
        //        return View("Index", models);
        //    }
        //    else
        //    {
        //        return View("Index", new LotTrackingWithLotId());
        //    }
        //}
    }
}
