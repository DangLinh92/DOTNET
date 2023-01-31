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
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Hubs;
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
        private IHubContext<liveUpdateSignalRHub> _hubContext { get; set; }

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(30000); // 30s

        private readonly TimeSpan _updateInterval_Plan = TimeSpan.FromMilliseconds(60000); // 60s

        private readonly Random _updateOrNotRandom = new Random();

        private readonly Timer _timer;
        private readonly Timer _timer_plan;

        private readonly object _updateStockPricesLock = new object();
        private readonly object _updateDailysLock = new object();

        static readonly Random random = new Random();

        public static string BeginDate;
        public static string ViewOption; // wafer , chip
        public static string ViewOption_Actual; // wafer , chip

        public InventoryTicker(IHubContext<liveUpdateSignalRHub> hubContext)
        {
            _hubContext = hubContext;

            _stocks = GenerateStocks();

            _dailyPlans = GetDailyPlanChip();

            _timer = new Timer(UpdateInventory, null, _updateInterval, _updateInterval);

            _timer_plan = new Timer(UpdateDaiLyPlan, null, _updateInterval_Plan, _updateInterval_Plan);
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

            foreach (var item in gocPlan.GocPlanViewModels)
            {
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
                            daily.Shipping_Wait = (decimal)(pl.QuantityGap);
                            daily.Plate_Inspection = daily.Shipping_Wait + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.GapActualPlan_EA = (decimal)pl.QuantityGap;
                            daily.GapActualPlan_SH = (decimal)gocPlan.GocPlanWaferViewModels.FirstOrDefault(x => x.DatePlan == GetBeginDate().ToString("yyyy-MM-dd") && x.Model == pl.Model).QuantityGap;
                        }

                        // kế hoach từ ngày 2-> 5
                        // kế hoach ngày thứ 2,...
                        if (pl.DatePlan == daysPlan[0])
                        {
                            daily.Plate_BST = (decimal)(pl.QuantityGap) + qtyEndOfMonthTemp + iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;
                            daily.Cu_Sn_Plating = iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST + daily.Plate_BST;
                        }

                        // kế hoach ngày thứ 3,...
                        if (pl.DatePlan == daysPlan[1])
                        {
                            daily.Plate_Patterning_Inspection = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST
                               + iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            daily.Plate_Patterning_PR =
                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                daily.Plate_Patterning_Inspection;

                            daily.Seed_Deposition = daily.Plate_Patterning_PR + iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR;
                            daily.Roof_Inspection = daily.Seed_Deposition + iv.Seed_Deposition;
                        }

                        // kế hoach ngày thứ 4,...
                        if (pl.DatePlan == daysPlan[2])
                        {
                            daily.Roof_Measure = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
                                iv.Roof_BST +
                                iv.Roof_Inspection +
                                iv.Seed_Deposition +
                                iv.Before_Plate_PR_Wafer_Inspection + iv.Plate_Patterning_PR +
                                iv.Plate_Patterning_Photo + iv.Plate_Patterning_Develop + iv.After_Plate_Develop_Visual_Inspection + iv.Plate_Patterning_Measure + iv.Plate_Patterning_PR_Ashing + iv.Plate_Patterning_Inspection +
                                iv.Plating_Input_Waiting + iv.Cu_Sn_Plating + iv.St_Plate_Visual_Inspection + iv.SN_Plate_Measure + iv.PR_Strip_Cu_Etching + iv.Nd_Plate_Visual_Inspection + iv.Ti_ething + iv.Plate_Measure + iv.Plate_BST +
                                iv.Plate_Inspection_Wait + iv.Plate_Inspection + iv.Probe_Waite + iv.Wafer_Probe_RF + iv.Wafer_Probe_IR + iv.Shipping_Wait;

                            //daily.Roof_Photo = iv.Roof_Remover + iv.Roof_Oven_PEB +
                            //                    iv.Roof_Develop + iv.After_Roof_Develop_Visual_Inspection +
                            //                    iv.Roof_Ashing + iv.Roof_QDR + iv.Roof_Oven + iv.Wafer_Sorting +
                            //                    iv.Roof_Measure + daily.Roof_Measure;
                        }

                        // kế hoach ngày thứ 5,...
                        if (pl.DatePlan == daysPlan[3])
                        {
                            daily.Roof_Photo = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
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

                            daily.Roof_Laminating = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
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

                            daily.Wall_Inspection = daily.Roof_Laminating + iv.Before_Roof_Lami_Wafer_Inspection + iv.Roof_Laminating;
                        }

                        // kế hoach ngày thứ 6,...
                        if (pl.DatePlan == daysPlan[4])
                        {
                            daily.Wall_PR = (decimal)pl.QuantityGap + qtyEndOfMonthTemp +
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
                        }
                    }
                }
                dailyPlanViewModels.Add(daily);
            }


            return SetYieldForModel(dailyPlanViewModels);
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
                                item.Wall_PR = Math.Floor((100 * item.Wall_PR) / (item.YieldPlan * item.StandardQty));
                                item.Wall_Oven = Math.Floor((100 * item.Wall_Oven) / (item.YieldPlan * item.StandardQty));
                                item.Wall_Inspection = Math.Floor((100 * item.Wall_Inspection) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Laminating = Math.Floor((100 * item.Roof_Laminating) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Ashing = Math.Floor((100 * item.Roof_Ashing) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Measure = Math.Floor((100 * item.Roof_Measure) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Photo = Math.Floor((100 * item.Roof_Photo) / (item.YieldPlan * item.StandardQty));
                                item.Roof_Inspection = Math.Floor((100 * item.Roof_Inspection) / (item.YieldPlan * item.StandardQty));
                                item.Seed_Deposition = Math.Floor((100 * item.Seed_Deposition) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Patterning_PR = Math.Floor((100 * item.Plate_Patterning_PR) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Patterning_Inspection = Math.Floor((100 * item.Plate_Patterning_Inspection) / (item.YieldPlan * item.StandardQty));
                                item.Cu_Sn_Plating = Math.Floor((100 * item.Cu_Sn_Plating) / (item.YieldPlan * item.StandardQty));
                                item.PR_Strip_Cu_Etching = Math.Floor((100 * item.PR_Strip_Cu_Etching) / (item.YieldPlan * item.StandardQty));
                                item.Plate_BST = Math.Floor((100 * item.Plate_BST) / (item.YieldPlan * item.StandardQty));
                                item.Plate_Inspection = Math.Floor((100 * item.Plate_Inspection) / (item.YieldPlan * item.StandardQty));
                                item.Shipping_Wait = Math.Floor((100 * item.Shipping_Wait) / (item.YieldPlan * item.StandardQty));
                            }
                            else
                            {
                                item.Wall_PR = 0;
                                item.Wall_Oven = 0;
                                item.Wall_Inspection = 0;
                                item.Roof_Laminating = 0;
                                item.Roof_Ashing = 0;
                                item.Roof_Measure = 0;
                                item.Roof_Photo = 0;
                                item.Roof_Inspection = 0;
                                item.Seed_Deposition = 0;
                                item.Plate_Patterning_PR = 0;
                                item.Plate_Patterning_Inspection = 0;
                                item.Cu_Sn_Plating = 0;
                                item.PR_Strip_Cu_Etching = 0;
                                item.Plate_BST = 0;
                                item.Plate_Inspection = 0;
                                item.Shipping_Wait = 0;
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

        public IEnumerable<InventoryActualModel> GetAllStocks()
        {
            return _stocks;
        }

        public IEnumerable<DailyPlanViewModel> GetDailyPlan()
        {
            return _dailyPlans;
        }

        private IEnumerable<InventoryActualModel> GetNewAllStocks()
        {
            _stocksNew = GenerateStocks();
            _stocks = _stocksNew;
            return _stocksNew;
        }

        private void UpdateInventory(object state)
        {
            lock (_updateStockPricesLock)
            {
                GetNewAllStocks();

                foreach (var stock in _stocks)
                {
                    BroadcastStock(stock);
                    //if (TryUpdateStockPrice(stock))
                    //{
                    //    BroadcastStock(stock);
                    //}
                }
            }
        }

        private void UpdateDaiLyPlan(object state)
        {
            lock (_updateDailysLock)
            {
                _dailyPlans = GetDailyPlanChip();

                foreach (var plan in _dailyPlans)
                {
                    BroadcastDaily(plan);
                }
            }
        }

        private bool TryUpdateStockPrice(InventoryActualModel stock)
        {
            var r = _updateOrNotRandom.NextDouble();
            if (r > .1)
            {
                return false;
            }

            bool isTrue = false;
            foreach (var item in _stocksNew)
            {
                if (item.InventoryId == stock.InventoryId && stock.IsUpdateData(item, new List<string>() { "LastUpdate" }))
                {
                    stock.Update(item);
                    isTrue = true;
                    break;
                }
            }

            return isTrue;
        }

        private void BroadcastStock(InventoryActualModel stock)
        {
            _hubContext.Clients.All.SendAsync("UpdateInventory", stock);
        }

        private void BroadcastDaily(DailyPlanViewModel plan)
        {
            _hubContext.Clients.All.SendAsync("UpdateDaiLyPlan", plan);
        }

        private void BroadcastWaitime(WaittimeViewModel w)
        {
            _hubContext.Clients.All.SendAsync("UpdateWaittime", w);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
