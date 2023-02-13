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
                            model = DataXH[0].DataXH.Find(x => DateTime.Parse(x.MoveOutTime).ToString("yyyyMMddHHmmss") + x.LotID == key); //NgayXuat + LotID
                        else
                        {
                            model = new PostOpeationShippingViewModel();
                        }
                    }

                    if (DataXH != null && DataXH.Count > 0)
                    {
                        xh1 = DataXH[0].XuatHang1ViewModels.FindAll(x => x.Key == key).FirstOrDefault();
                    }
                }
                else
                {
                    model = new PostOpeationShippingViewModel();
                }

                if (change.Type == "insert" || change.Type == "update")
                {
                    JsonConvert.PopulateObject(change.Data.ToString(), xh1);

                    model.ChipMapQty = xh1.ChipMapQty;
                    model.VanDeDacBiet = xh1.VanDeDacBiet.NullString();
                    model.DiffMapMes = model.ChipMapQty - model.ChipMesQty;

                    xh1.DiffMapMes = model.DiffMapMes;

                    if (change.Type == "insert" || model.InDB != "Y")
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

            string nguoiNhan = "";
            string nguoiXuat = "";

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
                            model = DataXH[0].DataXH.FindAll(x => DateTime.Parse(x.MoveOutTime).ToString("yyyyMMddHHmmss") + x.Module + x.Model + x.CassetteID == key); // Ngay + Module + Model + CasstteID
                        }
                        else
                        {
                            model = new List<PostOpeationShippingViewModel>();
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

                    if(nguoiNhan == "")
                    {
                        nguoiNhan = xh1.NguoiNhan;
                    }

                    if (nguoiXuat == "")
                    {
                        nguoiXuat = xh1.NguoiXuat;
                    }

                    foreach (var item in model)
                    {
                        item.GhiChu_XH2 = xh1.GhiChu;
                        item.KetQuaFAKiemTra = xh1.KetQuaFAKiemTra.NullString();
                        item.NguoiKiemTra = xh1.NguoiKiemTra;
                        item.NguoiNhan = xh1.NguoiNhan;
                        item.NguoiXuat = xh1.NguoiXuat;

                        if (change.Type == "insert" || item.InDB != "Y")
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

            // save all

            //string fromTime = "";
            //string toTime = "";
            //_memoryCache.TryGetValue("FromTime", out fromTime);
            //_memoryCache.TryGetValue("ToTime", out toTime);

            //if(fromTime != "" && toTime != "")
            //{
            //    List<PostOpeationShippingViewModel> dataShipping = new List<PostOpeationShippingViewModel>();
            //    dataShipping = PostOprationShippingService.GetPostOpeationShipping(fromTime,toTime);
            //    foreach (var item in dataShipping)
            //    {
            //        item.NguoiNhan = nguoiNhan;
            //        item.NguoiXuat = nguoiXuat;

            //        if (item.InDB != "Y")
            //        {
            //            PostOprationShippingService.Add(item);
            //        }
            //        else if (item.InDB == "Y")
            //        {
            //            PostOprationShippingService.Update(item);
            //        }
            //    }

            //    PostOprationShippingService.Save();
            //}

            return Ok(changes);
        }

        private void ResetData()
        {
            _ = _memoryCache.TryGetValue("FromTime", out string fromTime);
            _ = _memoryCache.TryGetValue("ToTime", out string toTime);

            var lstModel = PostOprationShippingService.GetXuatHangViewModel(fromTime, toTime);
            _memoryCache.Remove("DataXH");
            _memoryCache.Set("DataXH", lstModel);
        }
    }
}
