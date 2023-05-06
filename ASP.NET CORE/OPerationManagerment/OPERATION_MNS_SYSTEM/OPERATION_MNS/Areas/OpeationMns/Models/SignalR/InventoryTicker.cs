using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Hubs;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;

namespace OPERATION_MNS.Areas.OpeationMns.Models.SignalR
{
    public class InventoryTicker : IInventoryTicker
    {
        private IEnumerable<InventoryActualModel> _stocks;
        private IEnumerable<InventoryActualModel> _stocksNew;
        private IEnumerable<DailyPlanViewModel> _dailyPlans;
        private IEnumerable<StockHoldPositionViewModel> _dailyStockWlp2;
        private IEnumerable<DailyPlanWlp2ViewModel> _dailyPlanWLP2;

        private IHubContext<liveUpdateSignalRHub> _hubContext { get; set; }

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(60000); // 60s

        private readonly TimeSpan _updateInterval_Plan = TimeSpan.FromMilliseconds(70000); // 65s

        private readonly TimeSpan _updateInterval_DailyPlan = TimeSpan.FromMilliseconds(90000); // 120s

        private readonly Timer _timer;
        private readonly Timer _timer_plan;
        private readonly Timer _timer_StockWlp2;
        private readonly Timer _timer_planWlp2;

        private readonly object _updateStockPricesLock = new object();
        private readonly object _updateDailysLock = new object();
        private readonly object _updateStockWlp2Lock = new object();
        private readonly object _updateDailyWlp2Lock = new object();

        public static string BeginDate;
        public static string ViewOption; // wafer , chip
        public static string ViewOption_Actual; // wafer , chip

        public static string BeginDateOfWLP2;

        public static bool WLP1_Stock;
        public static bool WLP1_DailyPlan;
        public static bool WLP2_Stock;
        public static bool WLP2_DailyPlan;

        public InventoryTicker(IHubContext<liveUpdateSignalRHub> hubContext)
        {
            _hubContext = hubContext;

            // WLP1

            if (WLP1_Stock)
            {
                _stocks = GenerateStocks();
                _timer = new Timer(UpdateInventory, null, _updateInterval, _updateInterval);
            }

            if (WLP1_DailyPlan)
            {
                _dailyPlans = GetDailyPlanChip();
                _timer_plan = new Timer(UpdateDaiLyPlans, null, _updateInterval_Plan, _updateInterval_Plan);
            }

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

        public static void SetStatus(bool wLP1_Stock, bool wLP2_Stock, bool wLP1_DailyPlan, bool wLP2_DailyPlan)
        {
            InventoryTicker.WLP1_Stock = wLP1_Stock;
            InventoryTicker.WLP2_Stock = wLP2_Stock;
            InventoryTicker.WLP1_DailyPlan = wLP1_DailyPlan;
            InventoryTicker.WLP2_DailyPlan = wLP2_DailyPlan;
        }

        #region WLP1 Daily plan
        public IEnumerable<DailyPlanViewModel> GetDailyPlan()
        {
            return _dailyPlans;
        }

        private void UpdateDaiLyPlans(object state)
        {
            lock (_updateDailysLock)
            {
                IEnumerable<DailyPlanViewModel> lst = GetDailyPlanChip();
                //_dailyPlans = GetDailyPlanChip();
                DailyPlanViewModel obj;
                foreach (var plan in lst)
                {
                    obj = _dailyPlans.FirstOrDefault(x => x.Model == plan.Model);

                    if (obj != null && !obj.DeepEquals(plan, new List<string>() { "STT", "LastUpdate" }))
                    {
                        BroadcastDaily(plan);
                    }
                }

                _dailyPlans = lst;
            }
        }

        private void BroadcastDaily(DailyPlanViewModel plan)
        {
            _hubContext.Clients.All.SendAsync("updateDaiLyPlan", plan);
        }

        // kế hoạch chip theo ngày : 6 ngày kể từ ngày hiện tại
        static IEnumerable<DailyPlanViewModel> GetDailyPlanChip()
        {
            List<DailyPlanViewModel> dailyPlanViewModels = new List<DailyPlanViewModel>();

            GOC_PlanModel gocPlan = GetGOC_Plan_Chip();

            if (gocPlan == null)
                return dailyPlanViewModels;

            int leadTime = int.Parse(gocPlan.SettingItemsViewModels.FirstOrDefault(x => x.Id == "LEAD_TIME_PLAN").ItemValue.IfNullIsZero());

            List<string> daysPlan = GetDaysPlan(gocPlan, leadTime);
            decimal qtyEndOfMonth = 0;
            decimal qtyEndOfMonthTemp = 0;
            DailyPlanViewModel daily;
            int row = 0;
            string allModels = "";

            foreach (var item in gocPlan.GocPlanViewModels)
            {
                allModels += item.Model + ",";

                if (!gocPlan.InventoryActualModels.Exists(x => x.Material_SAP_CODE == item.Model))
                {
                    gocPlan.InventoryActualModels.Add(new InventoryActualModel()
                    {
                        Model = item.Module,
                        Material_SAP_CODE = item.Model,
                        Unit = CommonConstants.CHIP
                    });
                }
            }

            List<StayLotListByModel> QtyStayLot = GetQtyStayLot(allModels);

            foreach (var iv in gocPlan.InventoryActualModels.OrderBy(x => x.Material_SAP_CODE))
            {
                if (string.IsNullOrEmpty(iv.Material_SAP_CODE))
                {
                    continue;
                }

                row += 1;
                daily = new DailyPlanViewModel()
                {
                    Module = iv.Model,
                    Model = iv.Material_SAP_CODE,
                    Model_Short = iv.Material_SAP_CODE.Length >= 8 ? iv.Material_SAP_CODE.Substring(1, 7) : iv.Material_SAP_CODE,
                    Category = iv.Category,
                    LastUpdate = DateTime.Now.ToString("HH:mm:ss"),
                   
                    STT = row
                };

                foreach (var pl in gocPlan.GocPlanViewModels.OrderBy(x => x.Model).ThenBy(x => x.DatePlan))
                {
                    if (pl.Model == daily.Model)
                    {
                        if(ViewOption.NullString() == "")
                        {
                            daily.ChipWafer = CommonConstants.WAFER;
                        }
                        else
                        {
                            daily.ChipWafer = ViewOption;
                        }
                        
                        daily.StandardQty = (decimal)pl.StandardQtyForMonth;

                        // lưu lại lượng GAP cuôi tháng nếu daily plan nằm trên 2 tháng 
                        if (pl.DatePlan == GetEndOfMonth())
                        {
                            qtyEndOfMonth = (decimal)pl.QuantityGap;
                        }
                        else if (pl.DatePlan.CompareTo(GetEndOfMonth()) < 0)
                        {
                            qtyEndOfMonth = 0;
                        }

                        if (pl.DatePlan.CompareTo(GetEndOfMonth()) <= 0)
                        {
                            qtyEndOfMonthTemp = 0;
                        }
                        else
                        {
                            qtyEndOfMonthTemp = qtyEndOfMonth;
                        }

                        // Kế hoạch công đoạn Plate inspection cho ngày 1
                        if (pl.DatePlan == GetBeginDate().ToString("yyyy-MM-dd"))
                        {
                            daily.Shipping_Wait.Plan = (decimal)(pl.QuantityGap);
                            daily.Plate_Inspection.Plan = daily.Shipping_Wait.Plan + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Shipping_Wait.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP72500") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP72500").QtyChip : 0;
                            daily.Plate_Inspection.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP70000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP70000").QtyChip : 0;
                            daily.Shipping_Wait.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP72500") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP72500").QtyWF : 0;
                            daily.Plate_Inspection.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP70000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP70000").QtyWF : 0;

                            daily.GapActualPlan_EA = (decimal)pl.QuantityGap;
                            daily.GapActualPlan_SH = (decimal)gocPlan.GocPlanWaferViewModels.FirstOrDefault(x => x.DatePlan == GetBeginDate().ToString("yyyy-MM-dd") && x.Model == pl.Model).QuantityGap;
                        }

                        // kế hoach từ ngày 2-> 5
                        // kế hoach ngày thứ 2,...
                        if (pl.DatePlan == daysPlan[0])
                        {
                            daily.Plate_BST.Plan = (decimal)(pl.QuantityGap) + qtyEndOfMonthTemp + iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;
                            daily.Cu_Sn_Plating.Plan = iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST + daily.Plate_BST.Plan;

                            daily.Plate_BST.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP69000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP69000").QtyChip : 0;
                            daily.Cu_Sn_Plating.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP60000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP60000").QtyChip : 0;
                            daily.Plate_BST.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP69000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP69000").QtyWF : 0;
                            daily.Cu_Sn_Plating.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP60000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP60000").QtyWF : 0;
                        }

                        // kế hoach ngày thứ 3,...
                        if (pl.DatePlan == daysPlan[1])
                        {
                            daily.Plate_Patterning_Inspection.Plan = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST
                               + iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Plate_Patterning_PR.Plan =
                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                daily.Plate_Patterning_Inspection.Plan;

                            daily.Seed_Deposition.Plan = daily.Plate_Patterning_PR.Plan + iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR;
                            daily.Roof_Inspection.Plan = daily.Seed_Deposition.Plan + iv.Seed_Deposition;

                            daily.Plate_Patterning_Inspection.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP59000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP59000").QtyChip : 0;
                            daily.Plate_Patterning_PR.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP53000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP53000").QtyChip : 0;
                            daily.Seed_Deposition.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP52000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP52000").QtyChip : 0;
                            daily.Roof_Inspection.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP51000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP51000").QtyChip : 0;

                            daily.Plate_Patterning_Inspection.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP59000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP59000").QtyWF : 0;
                            daily.Plate_Patterning_PR.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP53000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP53000").QtyWF : 0;
                            daily.Seed_Deposition.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP52000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP52000").QtyWF : 0;
                            daily.Roof_Inspection.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP51000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP51000").QtyWF : 0;
                        }

                        // kế hoach ngày thứ 4,...
                        if (pl.DatePlan == daysPlan[2])
                        {
                            daily.Roof_Measure.Plan = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                iv.Roof_BST +
                                iv.Roof_Inspection +
                                iv.Seed_Deposition +
                                iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR +
                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST +
                                iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Roof_Measure.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP49000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP49000").QtyChip : 0;
                            daily.Roof_Measure.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP49000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP49000").QtyWF : 0;

                            //daily.Roof_Photo = iv.Roof_Remover + iv.Roof_Oven_PEB +
                            //                    iv.Roof_Develop + iv.After_Roof_Develop_Visual_Inspection +
                            //                    iv.Roof_Ashing + iv.Roof_QDR + iv.Roof_Oven + iv.Wafer_Sorting +
                            //                    iv.Roof_Measure + daily.Roof_Measure;
                        }

                        // kế hoach ngày thứ 5,...
                        if (pl.DatePlan == daysPlan[3])
                        {
                            daily.Roof_Photo.Plan = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                                iv.Roof_Remover + iv.Roof_Oven_PEB +
                                                iv.Roof_Develop + iv.After_Roof_Develop_Visual_Inspection +
                                                iv.Roof_Ashing + iv.Roof_QDR + iv.Roof_Oven + iv.Wafer_Sorting +
                                                iv.Roof_Measure + iv.Roof_BST +
                                                iv.Roof_Inspection +
                                                iv.Seed_Deposition +
                                                iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR +
                                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST +
                                                iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait; ;

                            daily.Roof_Laminating.Plan = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                                iv.After_Roof_Lami_Visual_Inspection + iv.Roof_Hardening + iv.Roof_Mask_Cleaning + iv.Roof_Photo +
                                                iv.Roof_Remover + iv.Roof_Oven_PEB +
                                                iv.Roof_Develop + iv.After_Roof_Develop_Visual_Inspection +
                                                iv.Roof_Ashing + iv.Roof_QDR + iv.Roof_Oven + iv.Wafer_Sorting +
                                                iv.Roof_Measure +
                                                iv.Roof_BST +
                                                iv.Roof_Inspection +
                                                iv.Seed_Deposition +
                                                iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR +
                                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST +
                                                iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Wall_Inspection.Plan = daily.Roof_Laminating.Plan + iv.Before_Roof_Lami_Wafer_Inspection + iv.Roof_Laminating;

                            daily.Roof_Photo.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP43000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP43000").QtyChip : 0;
                            daily.Roof_Laminating.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP40000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP40000").QtyChip : 0;
                            daily.Wall_Inspection.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP38000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP38000").QtyChip : 0;

                            daily.Roof_Photo.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP43000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP43000").QtyWF : 0;
                            daily.Roof_Laminating.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP40000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP40000").QtyWF : 0;
                            daily.Wall_Inspection.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP38000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP38000").QtyWF : 0;
                        }

                        // kế hoach ngày thứ 6,...
                        if (pl.DatePlan == daysPlan[4])
                        {
                            daily.Wall_PR.Plan = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                                iv.Wall_Ashing + iv.Wall_Inspection + iv.Wall_Ashing_Waiting + iv.Wall_PR_Measure + iv.Wall_Oven + iv.Wall_Develop + iv.Wall_Photo +
                                                iv.Before_Roof_Lami_Wafer_Inspection + iv.Roof_Laminating +
                                                iv.After_Roof_Lami_Visual_Inspection + iv.Roof_Hardening + iv.Roof_Mask_Cleaning + iv.Roof_Photo +
                                                iv.Roof_Remover + iv.Roof_Oven_PEB +
                                                iv.Roof_Develop + iv.After_Roof_Develop_Visual_Inspection +
                                                iv.Roof_Ashing + iv.Roof_QDR + iv.Roof_Oven + iv.Wafer_Sorting +
                                                iv.Roof_Measure +
                                                iv.Roof_BST +
                                                iv.Roof_Inspection +
                                                iv.Seed_Deposition +
                                                iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR +
                                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST +
                                                iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Wall_PR.StockChip = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP31000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP31000").QtyChip : 0;
                            daily.Wall_PR.StockWafer = QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP31000") != null ? QtyStayLot.FirstOrDefault(x => x.Model == pl.Model && x.OperationId == "OP31000").QtyWF : 0;
                        }
                    }
                }
                dailyPlanViewModels.Add(daily);
            }


            return SetYieldForModel(dailyPlanViewModels);
        }

        static IEnumerable<InventoryActualModel> GenerateStocks_BySapCode()
        {
            IEnumerable<InventoryActualModel> result = new List<InventoryActualModel>();

            ResultDB resultDB = ExecProceduce2("PKG_BUSINESS@GET_INVENTORY_CURRENT_CHIP_BY_SAP_CODE", new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<InventoryActualModel>(resultDB.ReturnDataSet.Tables[0]);
            }

            return result;
        }

        public static DateTime GetBeginDate()
        {
            string dateResult = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(BeginDate))
            {
                dateResult = DateTime.Parse(BeginDate).ToString("yyyy-MM-dd");
            }

            return DateTime.Parse(dateResult);
        }

        private static string GetEndOfMonth()
        {
            return (DateTime.Parse(GetBeginDate().ToString("yyyy-MM") + "-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }

        private static List<string> GetDaysPlan(GOC_PlanModel gocPlan, int leadTime)
        {
            List<string> Days = new List<string>();

            int index = 1;
            while (Days.ToList().Count < leadTime - 1)
            {
                if (!gocPlan.DateOffLineViewModels.Any(x => x.ItemValue == GetBeginDate().AddDays(index).ToString("yyyy-MM-dd")))
                {
                    Days.Add(GetBeginDate().AddDays(index).ToString("yyyy-MM-dd"));
                }
                index += 1;
            }

            Days.Sort();

            return Days;
        }

        // Tinh hieu suat
        static List<DailyPlanViewModel> SetYieldForModel(List<DailyPlanViewModel> DailyPlanViewModels)
        {
            string month = GetBeginDate().ToString("yyyy-MM-dd");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MONTH", month);

            ResultDB resultDB = ExecProceduce2("GET_YIELD_BY_MONTH", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                DailyPlanViewModel item;
                foreach (DataRow row in data.Rows)
                {
                    item = DailyPlanViewModels.FirstOrDefault(x => x.Model == row["Model"].NullString());

                    if (item != null)
                    {
                        item.YieldPlan = decimal.Parse(row["YieldPlan"].NullString());
                        item.YieldActual = decimal.Parse(row["YieldActual"].NullString());
                        item.YieldGap = decimal.Parse(row["YieldGap"].NullString());

                        if (ViewOption == CommonConstants.WAFER || string.IsNullOrEmpty(ViewOption))
                        {
                            if (item.YieldPlan > 0 && item.StandardQty > 0)
                            {
                                // chuyển đổi từ chip -> wafer :số tấm = số chip/ (hieu suat * chip co ban)
                                item.Wall_PR.Plan = Math.Floor((100 * item.Wall_PR.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Wall_Oven.Plan = Math.Floor((100 * item.Wall_Oven.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Wall_Inspection.Plan = Math.Floor((100 * item.Wall_Inspection.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Laminating.Plan = Math.Floor((100 * item.Roof_Laminating.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Ashing.Plan = Math.Floor((100 * item.Roof_Ashing.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Measure.Plan = Math.Floor((100 * item.Roof_Measure.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Photo.Plan = Math.Floor((100 * item.Roof_Photo.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Inspection.Plan = Math.Floor((100 * item.Roof_Inspection.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Seed_Deposition.Plan = Math.Floor((100 * item.Seed_Deposition.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Patterning_PR.Plan = Math.Floor((100 * item.Plate_Patterning_PR.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Patterning_Inspection.Plan = Math.Floor((100 * item.Plate_Patterning_Inspection.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Cu_Sn_Plating.Plan = Math.Floor((100 * item.Cu_Sn_Plating.Plan) / (item.YieldPlan * item.StandardQty));
                                item.PR_Strip_Cu_Etching.Plan = Math.Floor((100 * item.PR_Strip_Cu_Etching.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Plate_BST.Plan = Math.Floor((100 * item.Plate_BST.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Inspection.Plan = Math.Floor((100 * item.Plate_Inspection.Plan) / (item.YieldPlan * item.StandardQty));
                                item.Shipping_Wait.Plan = Math.Floor((100 * item.Shipping_Wait.Plan) / (item.YieldPlan * item.StandardQty));
                            }
                            else
                            {
                                item.Wall_PR.Plan = 0;
                                item.Wall_Oven.Plan = 0;
                                item.Wall_Inspection.Plan = 0;
                                item.Roof_Laminating.Plan = 0;
                                item.Roof_Ashing.Plan = 0;
                                item.Roof_Measure.Plan = 0;
                                item.Roof_Photo.Plan = 0;
                                item.Roof_Inspection.Plan = 0;
                                item.Seed_Deposition.Plan = 0;
                                item.Plate_Patterning_PR.Plan = 0;
                                item.Plate_Patterning_Inspection.Plan = 0;
                                item.Cu_Sn_Plating.Plan = 0;
                                item.PR_Strip_Cu_Etching.Plan = 0;
                                item.Plate_BST.Plan = 0;
                                item.Plate_Inspection.Plan = 0;
                                item.Shipping_Wait.Plan = 0;
                            }
                        }
                    }
                }
            }
            return DailyPlanViewModels;
        }

        static GOC_PlanModel GetGOC_Plan_Chip()
        {
            GOC_PlanModel result = new GOC_PlanModel();
            string fromDate = GetBeginDate().ToString("yyyy-MM-dd");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("BEGIN_TIME", fromDate);
            dic.Add("A_UNIT", "CHIP");
            ResultDB resultDB = ExecProceduce2("PKG_BUSINESS@GET_GOC_PLAN_FROM_CURRENT_DAY_1", dic);

            if (resultDB.ReturnInt == 0)
            {
                result.GocPlanViewModels = DataTableToJson.ConvertDataTable<GocPlanViewModel>(resultDB.ReturnDataSet.Tables[0]); // kế hoạch goc
                result.DateOffLineViewModels = DataTableToJson.ConvertDataTable<DateOffLineViewModel>(resultDB.ReturnDataSet.Tables[1]); // các ngày không sx
                result.SettingItemsViewModels = DataTableToJson.ConvertDataTable<SettingItemsViewModel>(resultDB.ReturnDataSet.Tables[2]); // get leadtime
                result.GocPlanWaferViewModels = DataTableToJson.ConvertDataTable<GocPlanViewModel>(resultDB.ReturnDataSet.Tables[3]); // kế hoạch goc wafer
                result.InventoryActualModels = GenerateStocks_BySapCode().ToList();// tồn đến thời điểm hiện tại theo sap code
            }
            else
            {
                result = null;
            }

            return result;
        }

        static List<StayLotListByModel> GetQtyStayLot(string models)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MODEL", models);
            ResultDB resultDB = ExecProceduce2("GET_STAY_LOT_BY_MODEL_WLP1_QTY", dic);
            List<StayLotListByModel> data = new List<StayLotListByModel>();
            if (resultDB.ReturnInt == 0)
            {
                data = DataTableToJson.ConvertDataTable<StayLotListByModel>(resultDB.ReturnDataSet.Tables[0]);
            }
            return data;
        }
        #endregion

        #region WLP1 Ton Hien Tai
        public IEnumerable<InventoryActualModel> GetAllStocks()
        {
            return _stocks;
        }

        // tồn hiện tại
        static IEnumerable<InventoryActualModel> GenerateStocks()
        {
            IEnumerable<InventoryActualModel> result = new List<InventoryActualModel>();

            string procedureName = ViewOption_Actual == CommonConstants.WAFER ? "PKG_BUSINESS@GET_INVENTORY_CURRENT_WAFER" : "PKG_BUSINESS@GET_INVENTORY_CURRENT_CHIP";

            ResultDB resultDB = ExecProceduce2(procedureName, new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                result = DataTableToJson.ConvertDataTable<InventoryActualModel>(resultDB.ReturnDataSet.Tables[0]);
            }

            return result;
        }

        private void UpdateInventory(object state)
        {
            lock (_updateStockPricesLock)
            {
                _stocksNew = GenerateStocks();
                InventoryActualModel obj;
                foreach (var stock in _stocksNew)
                {
                    obj = _stocks.FirstOrDefault(x => x.InventoryId == stock.InventoryId);

                    if (obj != null && !obj.DeepEquals(stock, new List<string>() { "LastUpdate" }))
                    {
                        BroadcastStock(stock);
                    }
                }
                _stocks = _stocksNew;
            }
        }

        private void BroadcastStock(InventoryActualModel stock)
        {
            _hubContext.Clients.All.SendAsync("updateInventory", stock);
        }
        #endregion

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

                    // TỒN CÔNG ĐOẠN WLP2 

                    stModel.WaferNguyenLieu +=
                        stModel.AtmosphericProcess +
                        stModel.WatingProcess +
                        stModel.BG +
                        stModel.WaferOven +
                        stModel.Dicing +
                        stModel.ChipInspection +
                        stModel.ReelPacking;

                    stModel.BanThanhPham +=
                        stModel.ReelInspection +
                        stModel.ReelCounter +
                        stModel.ReelOven +
                        item.OneStPackingLabel + item.OQC;

                    stModel.ChoXuatHang += item.OneSTPacking + item.Shipping;

                    stModel.Sum += stModel.WaferNguyenLieu + stModel.BanThanhPham + stModel.ChoXuatHang;
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
            _hubContext.Clients.All.SendAsync("getDailyWLP2", daily);
        }

        private IEnumerable<DailyPlanWlp2ViewModel> GetDailyPlanWlp2()
        {
            List<DailyPlanWlp2ViewModel> rs = new List<DailyPlanWlp2ViewModel>();
            IEnumerable<VIEW_WIP_POST_WLP> wip = GetDailyStockWlp2Chip();

            IEnumerable<GOC_PLAN_WLP2> gocPlans = GetDailyPlanWlp2ByDay();
            IEnumerable<DAILY_PLAN_WLP2> dailyPlansUpdated = GetDailyPlanByModelUpdated(GetBeginDate().ToString("yyyy-MM-dd"));

            DailyPlanWlp2ViewModel stModel;
            float standarQty = 0;
            GOC_PLAN_WLP2 goc;
            string sapcodes = "";
            float backGrindingUpdated = 0;

            List<GOC_STANDAR_QTY> lstStandarQty = GetStandarQty(CommonConstants.WLP2).ToList();
            GOC_STANDAR_QTY stQty;
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

                string IsNeedReloadPage = GetRloadValue();

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

        private string GetRloadValue()
        {
            string result = "";


            Dictionary<string, string> param = new Dictionary<string, string>();
            ResultDB resultDB = ExecProceduce2("GET_RELOAD_AFTER_UPDATE_STAYLOT_DAILY", param);
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

    public class YieldItem
    {
        public string Sapcode { get; set; }
        public double Yield { get; set; }
        public string Day { get; set; }
        public int Qty { get; set; }
    }
}
