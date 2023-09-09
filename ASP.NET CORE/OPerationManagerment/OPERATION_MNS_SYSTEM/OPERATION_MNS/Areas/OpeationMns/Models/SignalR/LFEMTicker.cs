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
    public class LFEMTicker : ILFEMTicker
    {

        private IEnumerable<DailyPlanLfemViewModel> _dailyPlanLfem;
        private IEnumerable<WARNING_LOT_RUNTIME_LFEM> _runtimeLfem;
        private IHubContext<LiveUpdateSignalR_LFEM_Hub> _hubContext { get; set; }

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(90000); // 60s

        private readonly TimeSpan _runtimeInterval = TimeSpan.FromMilliseconds(60000); // 10s
      
        private readonly Timer _timer_plan_Daily_Lfem;
        private readonly Timer _timer_runtime_Lfem;

        private readonly object _updateDailyLFEMLock = new object();
        private readonly object _runtimeLFEMLock = new object();

        public static string BeginDateOfLFEM;

        public static bool LFEM_DailyPlan;
        public static bool LFEM_Runtime;

        public LFEMTicker(IHubContext<LiveUpdateSignalR_LFEM_Hub> hubContext)
        {
            _hubContext = hubContext;

            if (LFEM_DailyPlan)
            {
                _dailyPlanLfem = GetDataDailyPlanLfem();
                _timer_plan_Daily_Lfem = new Timer(GetDailyLFEM, null, _updateInterval, _updateInterval);
            }

            if (LFEM_Runtime)
            {
                _runtimeLfem = GetRunTimeLfem();
                _timer_runtime_Lfem = new Timer(GetLotRuntimeLFEM, null, _runtimeInterval, _runtimeInterval);
            }
        }

        public static void SetStatus(bool lfemDailyPlan, bool lfemRuntime)
        {
            LFEM_DailyPlan = lfemDailyPlan;
            LFEM_Runtime = lfemRuntime;
        }

        #region LFEM Daily plan
        public IEnumerable<DailyPlanLfemViewModel> DailyPlanLfem()
        {
            return _dailyPlanLfem;
        }

        private void GetDailyLFEM(object state)
        {
            lock (_updateDailyLFEMLock)
            {
                IEnumerable<DailyPlanLfemViewModel> lst = GetDataDailyPlanLfem();
                DailyPlanLfemViewModel obj;
                foreach (var plan in lst)
                {
                    obj = _dailyPlanLfem.FirstOrDefault(x => x.MesCode == plan.MesCode);

                    if (obj != null && !obj.DeepEquals(plan, new List<string>() { "STT", "LastUpdate" }))
                    {
                        BroadcastDailyLFEM(plan);
                    }
                }
                _dailyPlanLfem = lst;
            }
        }

        private void BroadcastDailyLFEM(DailyPlanLfemViewModel daily)
        {
            // client method getDailyWLP2
            _hubContext.Clients.All.SendAsync("getDailyLFEM", daily);
        }

        public IEnumerable<DailyPlanLfemViewModel> GetDataDailyPlanLfem()
        {
            List<DailyPlanLfemViewModel> DailyPlanLfemViewModels = new List<DailyPlanLfemViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            string datePlan = GetBeginDateOfLFEM().ToString("yyyy-MM-dd");
            dic.Add("A_DATE_PLAN", datePlan);
            ResultDB resultDB = ExecProceduce2("GET_DAILY_PLAN_LFEM_DATA", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable dt = resultDB.ReturnDataSet.Tables[0];
                DataTable dtT0 = resultDB.ReturnDataSet.Tables[1];
                DataTable dtT1 = resultDB.ReturnDataSet.Tables[2];
                DataRow[] resultRow;
                DataRow[] resultRow1;
                string filter = "";
                DailyPlanLfemViewModel vm;
                int number = 0;
                foreach (DataRow row in dt.Rows)
                {
                    vm = new DailyPlanLfemViewModel();
                    vm.Model = row["Model"].NullString();
                    vm.MesCode = row["MesItemId"].NullString();

                    // DAM
                    vm.Dam.Wip = double.Parse(row["Dam_WIP"].IfNullIsZero());
                    vm.Dam.Plan = double.Parse(row["Dam_KHSX"].IfNullIsZero());
                    vm.Dam.Actual = double.Parse(row["Dam_PROD"].IfNullIsZero());
                    vm.Dam.TotalStr = vm.Dam.Plan + "-" + vm.Dam.Actual + "-" + vm.Dam.Wip;

                    vm.Dam.QtyT0 = vm.Dam.Actual - vm.Dam.Plan;

                    filter = "MesItemId = '" + vm.MesCode + "'";
                    resultRow = dtT0.Select(filter);
                    resultRow1 = dtT1.Select(filter);

                    if (resultRow.Count() > 0)
                    {
                        vm.Dam.QtyT1 = vm.Dam.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Dam.QtyT2 = vm.Dam.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // MOLD
                    vm.Mold.Wip = double.Parse(row["Mold_WIP"].IfNullIsZero());
                    vm.Mold.Plan = double.Parse(row["Mold_KHSX"].IfNullIsZero());
                    vm.Mold.Actual = double.Parse(row["Mold_PROD"].IfNullIsZero());
                    vm.Mold.TotalStr = vm.Mold.Plan + "-" + vm.Mold.Actual + "-" + vm.Mold.Wip;

                    vm.Mold.QtyT0 = vm.Mold.Actual - vm.Mold.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.Mold.QtyT1 = vm.Mold.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Mold.QtyT2 = vm.Mold.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // Grinding
                    vm.Grinding.Wip = double.Parse(row["Grinding_WIP"].IfNullIsZero());
                    vm.Grinding.Plan = double.Parse(row["Grinding_KHSX"].IfNullIsZero());
                    vm.Grinding.Actual = double.Parse(row["Grinding_PROD"].IfNullIsZero());
                    vm.Grinding.TotalStr = vm.Grinding.Plan + "-" + vm.Grinding.Actual + "-" + vm.Grinding.Wip;

                    vm.Grinding.QtyT0 = vm.Grinding.Actual - vm.Grinding.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.Grinding.QtyT1 = vm.Grinding.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Grinding.QtyT2 = vm.Grinding.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // Marking
                    vm.Marking.Wip = double.Parse(row["Marking_WIP"].IfNullIsZero());
                    vm.Marking.Plan = double.Parse(row["Marking_KHSX"].IfNullIsZero());
                    vm.Marking.Actual = double.Parse(row["Marking_PROD"].IfNullIsZero());
                    vm.Marking.TotalStr = vm.Marking.Plan + "-" + vm.Marking.Actual + "-" + vm.Marking.Wip;

                    vm.Marking.QtyT0 = vm.Marking.Actual - vm.Marking.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.Marking.QtyT1 = vm.Marking.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Marking.QtyT2 = vm.Marking.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // Dicing
                    vm.Dicing.Wip = double.Parse(row["Dicing_WIP"].IfNullIsZero());
                    vm.Dicing.Plan = double.Parse(row["Dicing_KHSX"].IfNullIsZero());
                    vm.Dicing.Actual = double.Parse(row["Dicing_PROD"].IfNullIsZero());
                    vm.Dicing.TotalStr = vm.Dicing.Plan + "-" + vm.Dicing.Actual + "-" + vm.Dicing.Wip;

                    vm.Dicing.QtyT0 = vm.Dicing.Actual - vm.Dicing.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.Dicing.QtyT1 = vm.Dicing.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Dicing.QtyT2 = vm.Dicing.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // Test
                    vm.Test.Wip = double.Parse(row["Test_WIP"].IfNullIsZero());
                    vm.Test.Plan = double.Parse(row["Test_KHSX"].IfNullIsZero());
                    vm.Test.Actual = double.Parse(row["Test_PROD"].IfNullIsZero());
                    vm.Test.TotalStr = vm.Test.Plan + "-" + vm.Test.Actual + "-" + vm.Test.Wip;

                    vm.Test.QtyT0 = vm.Test.Actual - vm.Test.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.Test.QtyT1 = vm.Test.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.Test.QtyT2 = vm.Test.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // Visual Inspection
                    vm.VisualInspection.Wip = double.Parse(row["VisualInspection_WIP"].IfNullIsZero());
                    vm.VisualInspection.Plan = double.Parse(row["VisualInspection_KHSX"].IfNullIsZero());
                    vm.VisualInspection.Actual = double.Parse(row["VisualInspection_PROD"].IfNullIsZero());
                    vm.VisualInspection.TotalStr = vm.VisualInspection.Plan + "-" + vm.VisualInspection.Actual + "-" + vm.VisualInspection.Wip;

                    vm.VisualInspection.QtyT0 = vm.VisualInspection.Actual - vm.VisualInspection.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.VisualInspection.QtyT1 = vm.VisualInspection.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.VisualInspection.QtyT2 = vm.VisualInspection.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    // OQC
                    vm.OQC.Wip = double.Parse(row["OQC_WIP"].IfNullIsZero());
                    vm.OQC.Plan = double.Parse(row["OQC_KHSX"].IfNullIsZero());
                    vm.OQC.Actual = double.Parse(row["OQC_PROD"].IfNullIsZero());
                    vm.OQC.TotalStr = vm.OQC.Plan + "-" + vm.OQC.Actual + "-" + vm.OQC.Wip;

                    vm.OQC.QtyT0 = vm.OQC.Actual - vm.OQC.Plan;

                    if (resultRow.Count() > 0)
                    {
                        vm.OQC.QtyT1 = vm.OQC.Actual - double.Parse(resultRow[0]["QTY"].IfNullIsZero());
                    }

                    if (resultRow1.Count() > 0)
                    {
                        vm.OQC.QtyT2 = vm.OQC.Actual - double.Parse(resultRow1[0]["QTY"].IfNullIsZero());
                    }

                    vm.LastUpdate = DateTime.Now.ToString("HH:mm:ss");
                    number += 1;
                    vm.STT = number;
                    vm.KHSX = "KH/SX";
                    DailyPlanLfemViewModels.Add(vm);
                }

                Dictionary<string, string> dicPriory = GetPrioryForMesItemLFem();

                string IsNeedReloadPage = GetRloadValue(CommonConstants.LFEM);

                if (dicPriory.Count > 0)
                {
                    foreach (var item in DailyPlanLfemViewModels)
                    {
                        item.IsLoadPage = IsNeedReloadPage;

                        if (dicPriory.ContainsKey(item.MesCode))
                        {
                            item.PrioryInOperation = dicPriory[item.MesCode].Split("^")[0];
                            item.NumberPriory = int.Parse(dicPriory[item.MesCode].Split("^")[1]);
                        }
                        else
                        {
                            item.NumberPriory = 9999;
                        }
                    }
                }
                else
                {
                    foreach (var item in DailyPlanLfemViewModels)
                    {
                        item.IsLoadPage = IsNeedReloadPage;
                        item.PrioryInOperation = null;
                        item.NumberPriory = 0;
                    }
                }
            }

            return DailyPlanLfemViewModels.OrderBy(x => x.NumberPriory).ThenBy(x => x.MesCode);
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

        private Dictionary<string, string> GetPrioryForMesItemLFem()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Dictionary<string, string> param = new Dictionary<string, string>();

            ResultDB resultDB = ExecProceduce2("GET_STAY_LOT_BY_MODEL_LFEM_CHECK", param);

            if (resultDB.ReturnInt == 0)
            {
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    if (!result.ContainsKey(row["MATERIAL_ID"].NullString()))
                    {
                        result.Add(row["MATERIAL_ID"].NullString(), string.Join("-", row["PrioryInOperation"].NullString().Split("-").Distinct()) + "^" + row["Number_Priory"].IfNullIsZero());
                    }
                }
            }
            return result;
        }

        public static DateTime GetBeginDateOfLFEM()
        {
            string dateResult = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(BeginDateOfLFEM))
            {
                dateResult = DateTime.Parse(BeginDateOfLFEM).ToString("yyyy-MM-dd");
            }

            return DateTime.Parse(dateResult);
        }

        public static NextDay GetNexDayLFEM()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string datePlan = GetBeginDateOfLFEM().ToString("yyyy-MM-dd");
            dic.Add("A_DATE_PLAN", datePlan);
            ResultDB resultDB = ExecProceduce2("GET_NEXT_2_DAY_LFEM", dic);
            NextDay nextDay = new NextDay();
            if (resultDB.ReturnInt == 0)
            {
                DataTable dt = resultDB.ReturnDataSet.Tables[0];
                nextDay.Day1 = dt.Rows[0]["NEXT1"].NullString();
                nextDay.Day2 = dt.Rows[0]["NEXT2"].NullString();
            }

            return nextDay;
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

        // Runtime
        public IEnumerable<WARNING_LOT_RUNTIME_LFEM> RunTimeLfem()
        {
            return _runtimeLfem;
        }

        private void GetLotRuntimeLFEM(object state)
        {
            lock (_runtimeLFEMLock)
            {
                IEnumerable<WARNING_LOT_RUNTIME_LFEM> lst = GetRunTimeLfem();
                foreach (var plan in lst)
                {
                    BroadcastRuntimrLFEM(plan);
                }
            }
        }

        private void BroadcastRuntimrLFEM(WARNING_LOT_RUNTIME_LFEM runtime)
        {
            // client method getDailyWLP2
            _hubContext.Clients.All.SendAsync("getLotRuntimeLFEM", runtime);
        }

        public IEnumerable<WARNING_LOT_RUNTIME_LFEM> GetRunTimeLfem()
        {
            List<WARNING_LOT_RUNTIME_LFEM> lstRuntime = new List<WARNING_LOT_RUNTIME_LFEM>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            ResultDB resultDB = ExecProceduce2("GET_RUNTIME_LFEM_DATA", dic);

            if (resultDB.ReturnInt == 0)
            {
                lstRuntime = DataTableToJson.ConvertDataTable<WARNING_LOT_RUNTIME_LFEM>(resultDB.ReturnDataSet.Tables[0]);
            }
            return lstRuntime.OrderByDescending(x=> x.Date.Substring(0,10)).ThenByDescending(x => float.Parse(x.RunTime_m));
        }
    }
}
