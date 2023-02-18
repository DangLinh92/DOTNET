using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Areas.OpeationMns.Models;
using OPERATION_MNS.Data.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class PostOperationShippingController : AdminBaseController
    {
        private readonly IPostOprationShippingService PostOprationShippingService;
        private readonly IMemoryCache _memoryCache;
        private List<XuatHangViewModel> DataXH;
        public PostOperationShippingController(IPostOprationShippingService postOprationShippingService, IMemoryCache memoryCache)
        {
            PostOprationShippingService = postOprationShippingService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object PostOperationShipping(DataSourceLoadOptions loadOptions, string fromTime, string toTime)
        {
            var lstModel = PostOprationShippingService.GetXuatHangViewModel(fromTime, toTime);
            _memoryCache.Remove("DataXH");
            _memoryCache.Set("DataXH", lstModel);

            _memoryCache.Remove("FromTime");
            _memoryCache.Set("FromTime", fromTime);

            _memoryCache.Remove("ToTime");
            _memoryCache.Set("ToTime", toTime);

            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpGet]
        public object SaveData(DataSourceLoadOptions loadOptions,string nguoiNhan,string nguoiXuat)
        {
            var lstModel = ResetData();
            if(lstModel.Count > 0)
            {
                PostOpeationShippingViewModel model;
                foreach (var item in lstModel[0].DataXH)
                {
                    if(item.KetQuaFAKiemTra.NullString().ToUpper() == "OK" && item.InDB == "Y")
                    {
                        model = PostOprationShippingService.FindItemXH1(item.MoveOutTime + item.LotID);

                        model.NguoiNhan = nguoiNhan;
                        model.NguoiXuat = nguoiXuat;

                        item.NguoiNhan = nguoiNhan;
                        item.NguoiXuat = nguoiXuat;
                        PostOprationShippingService.Update(model);
                    }
                }

                PostOprationShippingService.Save();
            }

            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public object Batch_XH3([FromBody] List<DataChange> changes)
        {
            string key = "";
            DataXH = new List<XuatHangViewModel>();
            List<PostOpeationShippingViewModel> model;

            _memoryCache.TryGetValue("DataXH", out DataXH);

            foreach (var change in changes)
            {
                key = change.Key.NullString();

                XuatHang3ViewModel xh1 = new XuatHang3ViewModel();
                if (change.Type == "update" || change.Type == "remove")
                {
                    model = PostOprationShippingService.FindItemXH3(key);

                    if (model == null || model.Count == 0)
                    {
                        if (DataXH != null && DataXH.Count > 0)
                        {
                            model = DataXH[0].DataXH.FindAll(x => x.Module + x.Model == key); // Module + Model
                        }
                        else
                        {
                            model = new List<PostOpeationShippingViewModel>() {};
                        }
                    }
                    else
                    {
                        foreach (var item in model)
                        {
                            item.InDB = "Y";
                        }
                    }

                    if (DataXH != null && DataXH.Count > 0)
                    {
                        xh1 = DataXH[0].XuatHang3ViewModels.FindAll(x => x.Key == key).FirstOrDefault();
                    }
                }
                else
                {
                    model = new List<PostOpeationShippingViewModel>();
                }

                if (change.Type == "insert" || change.Type == "update")
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), xh1);

                    foreach (var item in model)
                    {
                        item.GhiChu_XH3 = xh1.GhiChu.NullString();

                        if (change.Type == "insert" || item.InDB == "N")
                        {
                            PostOprationShippingService.Add(item);
                        }
                        else if (change.Type == "update" || item.InDB == "Y")
                        {
                            PostOprationShippingService.Update(item);
                        }
                    }

                    change.Data = xh1;
                }
                else if (change.Type == "remove")
                {
                    //PostOprationShippingService.Delete(model);
                }
            }

            PostOprationShippingService.Save();

            return Ok(changes);
        }


        [HttpPost]
        public object Batch_XH1([FromBody] List<DataChange> changes)
        {
            string key = "";
            DataXH = new List<XuatHangViewModel>();
            PostOpeationShippingViewModel model;

            _memoryCache.TryGetValue("DataXH", out DataXH);

            foreach (var change in changes)
            {
                key = change.Key.NullString();

                XuatHang1ViewModel xh1 = new XuatHang1ViewModel();
                if (change.Type == "update" || change.Type == "remove")
                {
                    model = PostOprationShippingService.FindItemXH1(key);

                    if (model == null)
                    {
                        if (DataXH != null && DataXH.Count > 0)
                            model = DataXH[0].DataXH.Find(x => x.MoveOutTime + x.LotID == key); //NgayXuat + LotID
                        else
                        {
                            model = new PostOpeationShippingViewModel() { InDB = "N"};
                        }
                    }
                    else
                    {
                        model.InDB = "Y";
                    }

                    if (DataXH != null && DataXH.Count > 0)
                    {
                        xh1 = DataXH[0].XuatHang1ViewModels.FindAll(x => x.Key == key).FirstOrDefault();
                    }
                }
                else
                {
                    model = new PostOpeationShippingViewModel() { InDB = "N" };
                }

                if (change.Type == "insert" || change.Type == "update")
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), xh1);

                    model.ChipMapQty = xh1.ChipMapQty;
                    model.VanDeDacBiet = xh1.VanDeDacBiet.NullString();
                    model.DiffMapMes = model.ChipMapQty - model.ChipMesQty;

                    xh1.DiffMapMes = model.DiffMapMes;

                    if (change.Type == "insert" || model.InDB == "N")
                    {
                        PostOprationShippingService.Add(model);
                    }
                    else if (change.Type == "update" || model.InDB == "Y")
                    {
                        PostOprationShippingService.Update(model);
                    }

                    change.Data = xh1;
                }
                else if (change.Type == "remove")
                {
                    PostOprationShippingService.Delete(model);
                }
            }

            PostOprationShippingService.Save();

            return Ok(changes);
        }

        [HttpPost]
        public object Batch_XH2([FromBody] List<DataChange> changes)
        {
            string key = "";
            DataXH = new List<XuatHangViewModel>();
            List<PostOpeationShippingViewModel> model;

            _memoryCache.TryGetValue("DataXH", out DataXH);

            foreach (var change in changes)
            {
                key = change.Key.NullString();

                XuatHang2ViewModel xh1 = new XuatHang2ViewModel();
                if (change.Type == "update" || change.Type == "remove")
                {
                    model = PostOprationShippingService.FindItemXH2(key);

                    if (model == null || model.Count == 0)
                    {
                        if (DataXH != null && DataXH.Count > 0)
                        {
                            model = DataXH[0].DataXH.FindAll(x => x.MoveOutTime + x.Module + x.Model + x.CassetteID == key); // Ngay + Module + Model + CasstteID
                        }
                        else
                        {
                            model = new List<PostOpeationShippingViewModel>();
                        }
                    }
                    else
                    {
                        foreach (var item in model)
                        {
                            item.InDB = "Y";
                        }
                    }

                    if (DataXH != null && DataXH.Count > 0)
                    {
                        xh1 = DataXH[0].XuatHang2ViewModels.FindAll(x => x.Key == key).FirstOrDefault();
                    }
                }
                else
                {
                    model = new List<PostOpeationShippingViewModel>();
                }

                if (change.Type == "insert" || change.Type == "update")
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), xh1);

                    foreach (var item in model)
                    {
                        item.GhiChu_XH2 = xh1.GhiChu.NullString();
                        item.KetQuaFAKiemTra = xh1.KetQuaFAKiemTra.NullString();
                        item.NguoiKiemTra = xh1.NguoiKiemTra.NullString();

                        if (change.Type == "insert" || item.InDB == "N")
                        {
                            PostOprationShippingService.Add(item);
                        }
                        else if (change.Type == "update" || item.InDB == "Y")
                        {
                            PostOprationShippingService.Update(item);
                        }
                    }

                    change.Data = xh1;
                }
                else if (change.Type == "remove")
                {
                    //PostOprationShippingService.Delete(model);
                }
            }

            PostOprationShippingService.Save();

            return Ok(changes);
        }

        private List<XuatHangViewModel> ResetData()
        {
            _ = _memoryCache.TryGetValue("FromTime", out string fromTime);
            _ = _memoryCache.TryGetValue("ToTime", out string toTime);

            var lstModel = PostOprationShippingService.GetXuatHangViewModel(fromTime, toTime);
            _memoryCache.Remove("DataXH");
            _memoryCache.Set("DataXH", lstModel);
            return lstModel;
        }
    }
}
