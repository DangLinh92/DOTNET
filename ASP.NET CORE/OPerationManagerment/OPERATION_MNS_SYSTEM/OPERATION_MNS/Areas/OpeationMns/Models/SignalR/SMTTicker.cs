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
using OPERATION_MNS.Application.ViewModels.SMT;
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
    public class SMTTicker : ISMTTicker
    {

        private IEnumerable<DailyPlanSMTViewModel> _dailyPlans;

        private IHubContext<LiveUpdateSignalR_SMT_Hub> _hubContext { get; set; }

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(90000); // 60s

        private readonly Timer _timer;

        private readonly object _dailyPlanSMTLock = new object();

        public static string BeginDateOfDailyPlanSMT;

        public SMTTicker(IHubContext<LiveUpdateSignalR_SMT_Hub> hubContext)
        {
            _hubContext = hubContext;

            _dailyPlans = GetDailyPlan();
            _timer = new Timer(GetDailyPlanSMT, null, _updateInterval, _updateInterval);

        }

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

        // dailyPlan
        public IEnumerable<DailyPlanSMTViewModel> SmtDailyPlan()
        {
            return _dailyPlans;
        }

        private void GetDailyPlanSMT(object state)
        {
            lock (_dailyPlanSMTLock)
            {
                IEnumerable<DailyPlanSMTViewModel> lst = GetDailyPlan();
                foreach (var plan in lst)
                {
                    BroadcastDailyPlanSMT(plan);
                }
            }
        }

        private void BroadcastDailyPlanSMT(DailyPlanSMTViewModel daily)
        {
            // client method getDailyWLP2
            _hubContext.Clients.All.SendAsync("getDailyPlanSMT", daily);
        }

        public IEnumerable<DailyPlanSMTViewModel> GetDailyPlan()
        {
            List<DailyPlanSMTViewModel> SmtDailys = new List<DailyPlanSMTViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            string datePlan = GetBeginDateOfSMT().ToString("yyyy-MM-dd");
            dic.Add("A_DAY_INPUT", datePlan);
            ResultDB resultDB = ExecProceduce2("GET_DAILY_PLAN_SMT", dic);

            if (resultDB.ReturnInt == 0)
            {
                // LUY KẾ KẾ HOACH T0->T+5
                DataTable tblSmtPlan = resultDB.ReturnDataSet.Tables[0];
                DataTable tblSmtPlan1 = resultDB.ReturnDataSet.Tables[1];
                DataTable tblSmtPlan2 = resultDB.ReturnDataSet.Tables[2];
                DataTable tblSmtPlan3 = resultDB.ReturnDataSet.Tables[3];
                DataTable tblSmtPlan4 = resultDB.ReturnDataSet.Tables[4];
                DataTable tblSmtPlan5 = resultDB.ReturnDataSet.Tables[5];

                // luy ke sxtt
                DataTable tblSmtActual = resultDB.ReturnDataSet.Tables[6];

                // Ton thuc te hien tai
                DataTable tblSmtInventory = resultDB.ReturnDataSet.Tables[7];

                DailyPlanSMTViewModel daily;
                string filterEx = "";
                foreach (DataRow row in tblSmtPlan.Rows)
                {
                    daily = new DailyPlanSMTViewModel();
                    daily.MesCode = row["MesItemId"].NullString();
                    daily.Model = row["MesItemId"].NullString().Substring(3, 4);
                    daily.GocPlanToday = double.Parse(row["QTY"].IfNullIsZero());

                    filterEx = "MesItemId = '" + daily.MesCode + "'";

                    daily.Inventory = 0;
                    if (tblSmtInventory.Select(filterEx).Length > 0)
                    {
                        daily.Inventory = double.Parse(tblSmtInventory.Select(filterEx)[0]["ChipQty"].IfNullIsZero());
                    }

                    daily.GocPlanToday_1 = 0;
                    daily.GocPlanToday_2 = 0;
                    daily.GocPlanToday_3 = 0;
                    daily.GocPlanToday_4 = 0;
                    daily.GocPlanToday_5 = 0;
                    daily.ActualToday = 0;

                    // thuc tế 
                    if (tblSmtActual.Select(filterEx).Length > 0)
                    {
                        daily.ActualToday = double.Parse(tblSmtActual.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    // Luy kế goc plan

                    if (tblSmtPlan1.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_1 = double.Parse(tblSmtPlan1.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan2.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_2 = double.Parse(tblSmtPlan2.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan3.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_3 = double.Parse(tblSmtPlan3.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan4.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_4 = double.Parse(tblSmtPlan4.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan5.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_5 = double.Parse(tblSmtPlan5.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    // ke hoach

                    daily.PlanToday = daily.ActualToday - daily.GocPlanToday;
                    daily.PlanToday_1 = daily.ActualToday - daily.GocPlanToday_1;
                    daily.PlanToday_2 = daily.ActualToday - daily.GocPlanToday_2;
                    daily.PlanToday_3 = daily.ActualToday - daily.GocPlanToday_3;
                    daily.PlanToday_4 = daily.ActualToday - daily.GocPlanToday_4;
                    daily.PlanToday_5 = daily.ActualToday - daily.GocPlanToday_5;
                    daily.LastUpdate = DateTime.Now.ToString("HH:mm:ss");

                    SmtDailys.Add(daily);
                }
            }

            return SmtDailys;
        }

        public static NextDay GetNexDaySMT()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string datePlan = GetBeginDateOfSMT().ToString("yyyy-MM-dd");
            dic.Add("A_DATE_PLAN", datePlan);
            ResultDB resultDB = ExecProceduce2("GET_NEXT_5_DAY_SMT", dic);
            NextDay nextDay = new NextDay();
            if (resultDB.ReturnInt == 0)
            {
                DataTable dt = resultDB.ReturnDataSet.Tables[0];
                nextDay.Day1 = dt.Rows[0]["NEXT1"].NullString();
                nextDay.Day2 = dt.Rows[0]["NEXT2"].NullString();
                nextDay.Day3 = dt.Rows[0]["NEXT3"].NullString();
                nextDay.Day4 = dt.Rows[0]["NEXT4"].NullString();
                nextDay.Day5 = dt.Rows[0]["NEXT5"].NullString();
            }

            return nextDay;
        }

        public static DateTime GetBeginDateOfSMT()
        {
            string dateResult = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(BeginDateOfDailyPlanSMT))
            {
                dateResult = DateTime.Parse(BeginDateOfDailyPlanSMT).ToString("yyyy-MM-dd");
            }

            return DateTime.Parse(dateResult);
        }
    }
}
