using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Charts.Native;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Hubs;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace OPERATION_MNS.Areas.OpeationMns.Models.SignalR
{
    public class WLP2Ticker : IWLP2Ticker
    {
        private IEnumerable<StockHoldPositionViewModel> _dailyStockWlp2;
        private IEnumerable<DailyPlanWlp2ViewModel> _dailyPlanWLP2;

        private IHubContext<LiveUpdateSignalR_WLP2_Hub> _hubContext { get; set; }

        private readonly TimeSpan _updateInterval_Plan = TimeSpan.FromMilliseconds(70000); // 65s 70000
        private readonly TimeSpan _updateInterval_DailyPlan = TimeSpan.FromMilliseconds(90000); // 120s

        private readonly Timer _timer_StockWlp2;
        private readonly Timer _timer_planWlp2;
   
        private readonly object _updateStockWlp2Lock = new object();
        private readonly object _updateDailyWlp2Lock = new object();

        public static string BeginDateOfWLP2;
      
        public static bool WLP2_Stock;
        public static bool WLP2_DailyPlan;

        public WLP2Ticker(IHubContext<LiveUpdateSignalR_WLP2_Hub> hubContext)
        {
            _hubContext = hubContext;

            // WLP2 
            if (WLP2_Stock)
            {
                _dailyStockWlp2 = GetStockHoldPosition();
                _timer_StockWlp2 = new Timer(GetStockWLP2, null, _updateInterval_Plan, _updateInterval_Plan);
            }

            if (WLP2_DailyPlan)
            {
                _dailyPlanWLP2 = GetDailyPlanWlp2();

                // update yield
                GetYieldWlp2ByDay(ref _dailyPlanWLP2);

                _timer_planWlp2 = new Timer(GetDailyWLP2s, null, _updateInterval_DailyPlan, _updateInterval_DailyPlan);
            }

        }

        public static void SetStatus(bool wLP2_Stock,bool wLP2_DailyPlan)
        {
            WLP2_Stock = wLP2_Stock;
            WLP2_DailyPlan = wLP2_DailyPlan;
        }

        #region WLP2 Stock
        private void GetStockWLP2(object state)
        {
            lock (_updateStockWlp2Lock)
            {
                _dailyStockWlp2 = GetStockHoldPosition();

                foreach (var st in _dailyStockWlp2)
                {
                    BroadcastStockWlp2(st);
                }
            }
        }

        private void BroadcastStockWlp2(StockHoldPositionViewModel stock)
        {
            _hubContext.Clients.All.SendAsync("GetStockWLP2", stock);
        }


        private IEnumerable<VIEW_WIP_POST_WLP> GetDailyStockWlp2Chip()
        {
            IEnumerable<VIEW_WIP_POST_WLP> result = new List<VIEW_WIP_POST_WLP>();

            ResultDB resultDB = ExecProceduce2("VIEW_WIP_POST_NOMAL_CHIP_WLP2", new Dictionary<string, string>());
            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<VIEW_WIP_POST_WLP>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        private IEnumerable<SMT_RETURN_WLP2> GetSMTReturnWlp2()
        {
            IEnumerable<SMT_RETURN_WLP2> result = new List<SMT_RETURN_WLP2>();

            ResultDB resultDB = ExecProceduce2("GET_SMT_RETURN_WLP2", new Dictionary<string, string>());
            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<SMT_RETURN_WLP2>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        private IEnumerable<StockHoldPositionViewModel> GetStockHoldPosition()
        {
            List<StockHoldPositionViewModel> rs = new List<StockHoldPositionViewModel>();
            IEnumerable<VIEW_WIP_POST_WLP> wip = GetDailyStockWlp2Chip();
            IEnumerable<SMT_RETURN_WLP2> smtReturns = GetSMTReturnWlp2();
            StockHoldPositionViewModel stModel;

            foreach (var item in wip)
            {
                if (rs.Exists(x => x.SapCode == item.Material_SAP_CODE))//&& x.Material == item.Material
                {
                    stModel = rs.Find(x => x.SapCode == item.Material_SAP_CODE);
                    stModel.SmtReturn = smtReturns.Where(x => x.SapCode == stModel.SapCode).Sum(x => x.SmtReturn);
                }
                else
                {
                    stModel = new StockHoldPositionViewModel()
                    {
                        SapCode = item.Material_SAP_CODE,
                        Module = item.Model
                        // Material = item.Material
                    };

                    stModel.SmtReturn = smtReturns.Where(x => x.SapCode == stModel.SapCode).Sum(x => x.SmtReturn);
                }

                if (item.Hold_Flag == "Y")
                {
                    // Hold 수량

                    stModel.HoldWafer +=
                        item.CassetteInputStock + item.Wait + item.PostOperationInputWait + item.OQC_WaferInspection +
                        item.WaferCarrierPacking + item.InputCheck + item.WaferShipping + item.Marking + item.NgoaiQuanSauMarking +
                        item.TapeLamination + item.NgoaiQuanSauLamination + item.BackGrinding + item.NgoaiQuanSauBackGrinding +
                        item.TapeDelamination + item.NgoaiQuanSauMDS + item.WaferOven + item.WaferDicing +
                        item.DeTaping + item.ChipVisualInspection + item.UVInspection + item.ReelPacking;

                    stModel.HoldReel += item.ReelOperationInput + item.ReelVisualInspection + item.ReelCounter + item.ReelOven + item.OneStPackingLabel;
                    stModel.HoldOQC += item.OQC + item.OneSTPacking + item.Shipping;
                }
                else
                {
                    // TỒN VỊ TRÍ THIẾT BỊ        

                    stModel.AtmosphericProcess += item.CassetteInputStock + item.Wait;
                    stModel.WatingProcess += item.PostOperationInputWait + item.OQC_WaferInspection + item.WaferCarrierPacking + item.InputCheck;

                    stModel.BG +=
                        item.WaferShipping +
                        item.Marking + item.NgoaiQuanSauMarking +
                        item.TapeLamination + item.NgoaiQuanSauLamination +
                        item.BackGrinding + item.NgoaiQuanSauBackGrinding +
                        item.TapeDelamination + item.NgoaiQuanSauMDS;

                    stModel.WaferOven += item.WaferOven;
                    stModel.Dicing += item.WaferDicing;
                    stModel.ChipInspection += item.DeTaping + item.ChipVisualInspection + item.UVInspection;
                    stModel.ReelPacking += item.ReelPacking;

                    stModel.ReelInspection += item.ReelOperationInput + item.ReelVisualInspection;
                    stModel.ReelCounter += item.ReelCounter;
                    stModel.ReelOven += item.ReelOven;
                    stModel.OQC += item.OneStPackingLabel + item.OQC + item.OneSTPacking;
                    stModel.WaitingforShipment += item.Shipping;

                    stModel.OneStPackingLabel_OQC += item.OneStPackingLabel + item.OQC;
                    // TỒN CÔNG ĐOẠN WLP2 

                    stModel.WaferNguyenLieu =
                        stModel.AtmosphericProcess +
                        stModel.WatingProcess +
                        stModel.BG +
                        stModel.WaferOven +
                        stModel.Dicing +
                        stModel.ChipInspection +
                        stModel.ReelPacking;

                    stModel.BanThanhPham =
                        stModel.ReelInspection +
                        stModel.ReelCounter +
                        stModel.ReelOven +
                        stModel.OneStPackingLabel_OQC;

                    stModel.ChoXuatHang += item.OneSTPacking + item.Shipping;

                    stModel.Sum = stModel.WaferNguyenLieu + stModel.BanThanhPham + stModel.ChoXuatHang;
                }

                stModel.LastUpdate = DateTime.Now.ToString("HH:mm:ss");

                if (!rs.Exists(x => x.SapCode == item.Material_SAP_CODE)) //&& x.Material == item.Material
                {
                    rs.Add(stModel);
                }
            }

            return rs.OrderBy(x => x.SapCode);
        }

        public IEnumerable<StockHoldPositionViewModel> GetWlp2StockHold()
        {
            return _dailyStockWlp2;
        }
        #endregion

        #region WLP2 Daily plan
        public IEnumerable<DailyPlanWlp2ViewModel> GetWlp2DailyPlans()
        {
            return _dailyPlanWLP2;
        }
        private void GetDailyWLP2s(object state)
        {
            lock (_updateDailyWlp2Lock)
            {
                IEnumerable<DailyPlanWlp2ViewModel> lst = GetDailyPlanWlp2();
                DailyPlanWlp2ViewModel obj;
                foreach (var plan in lst)
                {
                    obj = _dailyPlanWLP2.FirstOrDefault(x => x.SapCode == plan.SapCode);

                    if (obj != null)
                    {
                        plan.CumYield = obj.CumYield;
                    }

                    if (obj != null && !obj.DeepEquals(plan, new List<string>() { "LastUpdate" }))
                    {
                        BroadcastDailyWlp2(plan);
                    }
                }
                _dailyPlanWLP2 = lst;
            }
        }

        private void BroadcastDailyWlp2(DailyPlanWlp2ViewModel daily)
        {
            // client method getDailyWLP2
            _hubContext.Clients.All.SendAsync("getDailyWLP2", daily);
        }

        private IEnumerable<DailyPlanWlp2ViewModel> GetDailyPlanWlp2()
        {
            List<DailyPlanWlp2ViewModel> rs = new List<DailyPlanWlp2ViewModel>();
            IEnumerable<VIEW_WIP_POST_WLP> wip = GetDailyStockWlp2Chip();

            IEnumerable<GOC_PLAN_WLP2> gocPlans = GetDailyPlanWlp2ByDay();
            //IEnumerable<GOC_PLAN_WLP2> gocBeforePlans = GetBeforePlanWlp2ByDayAndModel();
            IEnumerable<DAILY_PLAN_WLP2> dailyPlansUpdated = GetDailyPlanByModelUpdated(GetBeginDateOfWlp2().ToString("yyyy-MM-dd"));

            DailyPlanWlp2ViewModel stModel;
            float standarQty = 0;
            GOC_PLAN_WLP2 goc;
            string sapcodes = "";
            float backGrindingUpdated = 0;

            List<GOC_STANDAR_QTY> lstStandarQty = GetStandarQty(CommonConstants.WLP2).ToList();
            GOC_STANDAR_QTY stQty;
            float beforeGap = 0;
            foreach (var item in wip)
            {
                if (dailyPlansUpdated.FirstOrDefault(x => x.Model == item.Material_SAP_CODE) != null)
                {
                    backGrindingUpdated = dailyPlansUpdated.FirstOrDefault(x => x.Model == item.Material_SAP_CODE).BackGrinding;
                }

                if (rs.Exists(x => x.SapCode == item.Material_SAP_CODE))
                {
                    stModel = rs.Find(x => x.SapCode == item.Material_SAP_CODE);
                }
                else
                {
                    stQty = lstStandarQty.FirstOrDefault(x => x.Model == item.Material_SAP_CODE);
                    goc = gocPlans.FirstOrDefault(x => x.Model == item.Material_SAP_CODE);

                    standarQty = stQty != null ? stQty.StandardQtyForMonth : 1;

                    stModel = new DailyPlanWlp2ViewModel()
                    {
                        SapCode = item.Material_SAP_CODE,
                        Module = item.Model,
                        LastUpdate = DateTime.Now.ToString("HH:mm:ss"),
                        QtyChipBase = standarQty,
                        Type = goc?.Type
                    };

                    //beforeGap = gocBeforePlans.Count() > 0 ? gocBeforePlans.FirstOrDefault(x => x.Model == item.Material_SAP_CODE).QuantityGap : 0;
                    stModel.KHSXTheoNgay = goc != null ? goc.QuantityGap : 0;
                    stModel.WaferOven = stModel.KHSXTheoNgay;
                    stModel.Dicing = stModel.KHSXTheoNgay;
                    stModel.ChipInspection = stModel.KHSXTheoNgay;
                    stModel.Packing = stModel.KHSXTheoNgay;
                    stModel.ReelInspection = stModel.KHSXTheoNgay;
                    stModel.QC_Pass = stModel.KHSXTheoNgay > 0 ? 0 : stModel.KHSXTheoNgay;
                }

                if (item.Hold_Flag == "Y")
                {
                    stModel.HoldNVL += item.Total;
                }
                else
                {
                    stModel.ToTalWip += item.Total;
                    stModel.ChoXuatHang += item.OneSTPacking + item.Shipping;
                    stModel.NguyenLieuOKSX = stModel.ToTalWip - stModel.ChoXuatHang;

                    if (stModel.KHSXTheoNgay <= 0)
                    {
                        stModel.SoLuongConLaiSauKHSX = stModel.NguyenLieuOKSX + stModel.KHSXTheoNgay;
                    }
                    else
                    {
                        stModel.SoLuongConLaiSauKHSX = stModel.NguyenLieuOKSX;
                    }

                    stModel.WaferOven +=
                        item.WaferOven + item.WaferDicing + item.DeTaping + item.ChipVisualInspection + item.UVInspection +
                        item.ReelPacking + item.ReelOperationInput + item.ReelVisualInspection + item.ReelCounter +
                        item.ReelOven + item.OneStPackingLabel + item.OQC + item.OneSTPacking;

                    if (stModel.WaferOven >= 0)
                    {
                        stModel.WaferOven = 0;
                    }

                    stModel.BackGrinding = backGrindingUpdated > 0 ? backGrindingUpdated : (-1) * stModel.WaferOven / standarQty;

                    stModel.Dicing += item.DeTaping + item.ChipVisualInspection + item.UVInspection +
                        item.ReelPacking +
                        item.ReelOperationInput + item.ReelVisualInspection +
                        item.ReelCounter +
                        item.ReelOven +
                        item.OneStPackingLabel + item.OQC + item.OneSTPacking;

                    if (stModel.Dicing >= 0)
                    {
                        stModel.Dicing = 0;
                    }

                    stModel.ChipInspection +=
                        item.ReelPacking +
                        item.ReelOperationInput + item.ReelVisualInspection +
                        item.ReelCounter +
                        item.ReelOven +
                        item.OneStPackingLabel + item.OQC + item.OneSTPacking;

                    if (stModel.ChipInspection >= 0)
                    {
                        stModel.ChipInspection = 0;
                    }

                    stModel.Packing +=
                        item.ReelOperationInput + item.ReelVisualInspection +
                        item.ReelCounter +
                        item.ReelOven +
                        item.OneStPackingLabel + item.OQC + item.OneSTPacking;

                    if (stModel.Packing >= 0)
                    {
                        stModel.Packing = 0;
                    }

                    stModel.ReelInspection +=
                        item.ReelCounter +
                        item.ReelOven +
                        item.OneStPackingLabel + item.OQC + item.OneSTPacking;

                    if (stModel.ReelInspection >= 0)
                    {
                        stModel.ReelInspection = 0;
                    }
                }

                if (!rs.Exists(x => x.SapCode == item.Material_SAP_CODE))
                {
                    if (!sapcodes.Contains(stModel.SapCode))
                    {
                        sapcodes += stModel.SapCode + "-";
                    }

                    rs.Add(stModel);
                }
            }

            // "OPT2000", "OP01100", "OP02000", "OP05000-OP04500-OP04000", "OP06000", "OP06500-OP07000","OP10000-OP09000-OP11000"
            if (sapcodes.Length > 0)
            {
                Dictionary<string, string> dicPriory = GetPrioryForSapcode();

                string IsNeedReloadPage = GetRloadValue(CommonConstants.WLP2);

                if (dicPriory.Count > 0)
                {
                    foreach (var item in rs)
                    {
                        item.IsLoadPage = IsNeedReloadPage;

                        if (dicPriory.ContainsKey(item.SapCode))
                        {
                            item.PrioryInOperation = dicPriory[item.SapCode];
                            item.NumberPriory = 1;
                        }
                    }
                }
                else
                {
                    foreach (var item in rs)
                    {
                        item.IsLoadPage = IsNeedReloadPage;
                        item.PrioryInOperation = null;
                        item.NumberPriory = 0;
                    }
                }
            }

            return rs.OrderByDescending(x => x.NumberPriory);
        }

        private IEnumerable<GOC_PLAN_WLP2> GetDailyPlanWlp2ByDay()
        {
            IEnumerable<GOC_PLAN_WLP2> result = new List<GOC_PLAN_WLP2>();

            string date = GetBeginDateOfWlp2().ToString("yyyy-MM-dd");

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_DATE", date);
            param.Add("A_DANHMUC", "SAN_XUAT");

            ResultDB resultDB = ExecProceduce2("GET_GOC_PLAN_WLP2_BY_DAY", param);

            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<GOC_PLAN_WLP2>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        private IEnumerable<GOC_PLAN_WLP2> GetBeforePlanWlp2ByDayAndModel()
        {
            string date = GetBeginDateOfWlp2().AddDays(-1).ToString("yyyy-MM-dd");
            IEnumerable<GOC_PLAN_WLP2> result = new List<GOC_PLAN_WLP2>();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_DATE", date);
            param.Add("A_DANHMUC", "SAN_XUAT");

            ResultDB resultDB = ExecProceduce2("GET_GOC_PLAN_WLP2_BY_MODEL_DAY", param);

            if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
            {
                result = DataTableToJson.ConvertDataTable<GOC_PLAN_WLP2>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        private string GetRloadValue(string dept)
        {
            string procName = "GET_RELOAD_AFTER_UPDATE_STAYLOT_DAILY";

            if (dept == CommonConstants.LFEM)
            {
                procName = "GET_RELOAD_AFTER_UPDATE_STAYLOT_DAILY_LFEM";
            }
            else
            if (dept == CommonConstants.WLP2)
            {
                procName = "GET_RELOAD_AFTER_UPDATE_STAYLOT_DAILY";
            }

            string result = "";

            Dictionary<string, string> param = new Dictionary<string, string>();
            ResultDB resultDB = ExecProceduce2(procName, param);
            DataTable tb = resultDB.ReturnDataSet.Tables[0];
            if (resultDB.ReturnInt == 0 && tb.Rows.Count > 0)
            {
                result = tb.Rows[0]["RLOAD_VALUE"].NullString();
            }
            return result;
        }

        /// <summary>
        /// check xem model có hàng ưu tiên không.
        /// </summary>
        /// <param name="sapcodes"></param>
        /// <param name="operations"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetPrioryForSapcode()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Dictionary<string, string> param = new Dictionary<string, string>();

            ResultDB resultDB = ExecProceduce2("GET_STAY_LOT_BY_MODEL_CHECK_WLP2", param);

            if (resultDB.ReturnInt == 0)
            {
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    if (!result.ContainsKey(row["SAP_Code"].NullString()))
                    {
                        result.Add(row["SAP_Code"].NullString(), string.Join("-", row["PrioryInOperation"].NullString().Split("-").Distinct()));
                    }
                }
            }
            return result;
        }

        private void GetYieldWlp2ByDay(ref IEnumerable<DailyPlanWlp2ViewModel> data)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            ResultDB resultDB = ExecProceduce2("GET_YIELD_WLP2_BY_DAY", param);

            if (resultDB.ReturnInt == 0)
            {
                DataTable dtOutputTrendMaterial = resultDB.ReturnDataSet.Tables[0];

                if (dtOutputTrendMaterial == null || dtOutputTrendMaterial.Rows.Count == 0)
                {
                    return;
                }

                DataColumn dc = null;
                dc = new DataColumn("SUM_YIELD");
                dc.DataType = typeof(System.Double);
                dc.DefaultValue = 0;
                dtOutputTrendMaterial.Columns.Add(dc);
                dc = new DataColumn("SUM_CUM_YIELD");
                dc.DataType = typeof(System.Double);
                dc.DefaultValue = 0;
                dtOutputTrendMaterial.Columns.Add(dc);

                foreach (DataRow drWork in dtOutputTrendMaterial.Rows)
                {
                    double yieldSummary = 0;
                    double cumYieldSummary = 0;
                    for (int i = 0; i < dtOutputTrendMaterial.Columns.Count; i++)
                    {
                        if (i == dtOutputTrendMaterial.Columns.Count - 1)
                        {

                        }
                        if (dtOutputTrendMaterial.Columns[i].ColumnName.StartsWith("YIELD"))
                        {
                            yieldSummary = (yieldSummary == 0 ? 1 : yieldSummary) *
                                (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                        }
                        else if (dtOutputTrendMaterial.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                        {
                            cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                        }
                        else
                        {
                            continue;
                        }

                    }

                    drWork["SUM_YIELD"] = yieldSummary;
                    drWork["SUM_CUM_YIELD"] = cumYieldSummary;
                }

                //Sorting the Table
                dtOutputTrendMaterial.DefaultView.Sort = "WORK_DATE desc";
                dtOutputTrendMaterial = dtOutputTrendMaterial.DefaultView.ToTable(true);

                List<YieldItem> yieldItems = new List<YieldItem>();
                YieldItem item;
                foreach (DataRow yield in dtOutputTrendMaterial.Rows)
                {
                    if (yieldItems.Any(x => x.Sapcode == yield["SAP_Code"].NullString() && x.Day == yield["WORK_DATE"].NullString()))
                    {
                        item = yieldItems.FirstOrDefault(x => x.Sapcode == yield["SAP_Code"].NullString() && x.Day == yield["WORK_DATE"].NullString());
                        item.Yield += double.Parse(yield["SUM_CUM_YIELD"].IfNullIsZero());

                        if (double.Parse(yield["SUM_CUM_YIELD"].IfNullIsZero()) > 0)
                        {
                            item.Qty += 1;
                        }
                    }
                    else
                    {
                        item = new YieldItem()
                        {
                            Day = yield["WORK_DATE"].NullString(),
                            Sapcode = yield["SAP_Code"].NullString(),
                            Qty = 1
                        };
                        item.Yield += double.Parse(yield["SUM_CUM_YIELD"].IfNullIsZero());

                        yieldItems.Add(item);
                    }
                }

                foreach (DailyPlanWlp2ViewModel vm in data)
                {
                    foreach (YieldItem yield in yieldItems.OrderBy(x => x.Sapcode).ThenByDescending(x => x.Day))
                    {
                        if (vm.SapCode == yield.Sapcode && yield.Yield > 0 && yield.Qty > 0)
                        {
                            vm.CumYield = yield.Yield / yield.Qty;
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<GOC_STANDAR_QTY> GetStandarQty(string dept)
        {
            List<GOC_STANDAR_QTY> result = new List<GOC_STANDAR_QTY>();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_DEPT", dept);

            ResultDB resultDB = ExecProceduce2("GET_GOC_STANDAR_QTY", param);

            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<GOC_STANDAR_QTY>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        private IEnumerable<DAILY_PLAN_WLP2> GetDailyPlanByModelUpdated(string datePlan)
        {
            List<DAILY_PLAN_WLP2> result = new List<DAILY_PLAN_WLP2>();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_DATE_PLAN", datePlan);

            ResultDB resultDB = ExecProceduce2("GET_DAILY_PLAN_WLP2_UPDATED", param);

            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<DAILY_PLAN_WLP2>(resultDB.ReturnDataSet.Tables[0]);
            }
            return result;
        }

        public static DateTime GetBeginDateOfWlp2()
        {
            string dateResult = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(BeginDateOfWLP2))
            {
                dateResult = DateTime.Parse(BeginDateOfWLP2).ToString("yyyy-MM-dd");
            }

            return DateTime.Parse(dateResult);
        }


        #endregion


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private static ResultDB ExecProceduce2(string ProcName, Dictionary<string, string> Dictionary)
        {
            try
            {
                string connString = "Persist Security Info=True;Data Source = 10.70.10.97;Initial Catalog = OPERATION_MNSDB;User Id = sa;Password = Wisol@123;Connect Timeout=3";

                using var con = new SqlConnection(connString);
                ResultDB resultDb = new ResultDB();
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                cmd = new SqlCommand(ProcName.Replace('.', '@'), con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in Dictionary)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                SqlParameter N_RETURN = new SqlParameter("@N_RETURN", SqlDbType.Int);
                N_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(N_RETURN);

                SqlParameter V_RETURN = new SqlParameter("@V_RETURN", SqlDbType.NVarChar, 4000);
                V_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(V_RETURN);

                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                resultDb.ReturnDataSet = ds;
                resultDb.ReturnInt = (int)N_RETURN.Value;
                resultDb.ReturnString = (string)V_RETURN.Value;
                return resultDb;
            }
            catch (Exception ex)
            {
                ResultDB result = new ResultDB();
                result.ReturnInt = -1;
                result.ReturnString = "서버 연결이 불가능합니다. Không kết nối được đến máy chủ.";
                return result;
            }
        }
    } 
}
