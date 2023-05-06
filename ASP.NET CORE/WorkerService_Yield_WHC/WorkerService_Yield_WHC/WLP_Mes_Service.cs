using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WorkerService_Yield_WHC.Models;

namespace WorkerService_Yield_WHC
{
    public class WLP_Mes_Service : IWlpMesService
    {
        private readonly ILogger<WLP_Mes_Service> _logger;
        const string IN = "I";
        const string OUT = "O";
        public WLP_Mes_Service(ILogger<WLP_Mes_Service> logger)
        {
            _logger = logger;
        }

        public void UpdateOuputDaiLyTrend()
        {
            try
            {
                SQLService sql = new SQLService(Commons.ConnectionString);

                Dictionary<string, string> dic = new Dictionary<string, string>();

                // Wlp
                ResultDB resultDB = sql.ExecProceduce2("PKG_BUSINESS@TREND_DAILY_WLP", dic);

                if (resultDB.ReturnInt == 0)
                {
                    DataTable wlpData = resultDB.ReturnDataSet.Tables[0];
                    DataTable tcWlpData = resultDB.ReturnDataSet.Tables[1];
                    DataTable bdmpData = resultDB.ReturnDataSet.Tables[2];
                    DataTable allData = resultDB.ReturnDataSet.Tables[3];

                    List<OutPutTrendDailyWlp> OutputTrendDaily = new List<OutPutTrendDailyWlp>();
                    OutPutTrendDailyWlp outPut;

                    foreach (DataRow drWork in wlpData.Rows)
                    {
                        outPut = new OutPutTrendDailyWlp();
                        outPut.WorkDate = drWork["Work Date"].NullString();

                        if (outPut.WorkDate.Length == 8)
                        {
                            outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                            outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                            outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                            outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                        }

                        double cumYieldSummary = 0;
                        for (int i = 0; i < wlpData.Columns.Count; i++)
                        {
                            if (wlpData.Columns[i].ColumnName.StartsWith("YIELD"))
                            {

                            }
                            else
                            if (wlpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                            {
                                cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                    (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                            }
                            else
                            {
                                continue;
                            }
                        }

                        outPut.N_WLP_CumYield = cumYieldSummary;
                        OutputTrendDaily.Add(outPut);
                    }

                    foreach (DataRow drWork in tcWlpData.Rows)
                    {
                        if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                        {
                            outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                        }
                        else
                        {
                            outPut = new OutPutTrendDailyWlp();
                            outPut.WorkDate = drWork["Work Date"].NullString();

                            if (outPut.WorkDate.Length == 8)
                            {
                                outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                            }

                            OutputTrendDaily.Add(outPut);
                        }

                        double cumYieldSummary = 0;
                        for (int i = 0; i < tcWlpData.Columns.Count; i++)
                        {
                            if (tcWlpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                            {
                                cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                    (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                            }
                            else
                            {
                                continue;
                            }
                        }

                        outPut.TC_WLP_CumYield = cumYieldSummary;
                    }


                    foreach (DataRow drWork in bdmpData.Rows)
                    {
                        if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                        {
                            outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                        }
                        else
                        {
                            outPut = new OutPutTrendDailyWlp();
                            outPut.WorkDate = drWork["Work Date"].NullString();

                            if (outPut.WorkDate.Length == 8)
                            {
                                outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                            }

                            OutputTrendDaily.Add(outPut);
                        }

                        double cumYieldSummary = 0;
                        for (int i = 0; i < bdmpData.Columns.Count; i++)
                        {
                            if (bdmpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                            {
                                cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                    (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                            }
                            else
                            {
                                continue;
                            }
                        }

                        outPut.BDMP_CumYield = cumYieldSummary;
                    }

                    foreach (DataRow drWork in allData.Rows)
                    {
                        if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                        {
                            outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                        }
                        else
                        {
                            outPut = new OutPutTrendDailyWlp();
                            outPut.WorkDate = drWork["Work Date"].NullString();

                            if (outPut.WorkDate.Length == 8)
                            {
                                outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                            }

                            OutputTrendDaily.Add(outPut);
                        }

                        double cumYieldSummary = 0;
                        for (int i = 0; i < allData.Columns.Count; i++)
                        {
                            if (allData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                            {
                                cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                    (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                            }
                            else
                            {
                                continue;
                            }
                        }

                        outPut.Total_CumYield = cumYieldSummary;
                    }

                    // UPDATE DATA 
                    Dictionary<string, string> dic2 = new Dictionary<string, string>();
                    DataTable data = OutputTrendDaily.ToDataTable();
                    ResultDB _resultDB = sql.ExecuteDB("PKG_BUSINESS@UPDATE_TREND_DAILY_WLP", dic2, "A_DATA", data);

                    if (_resultDB.ReturnInt == 0)
                    {
                        _logger.LogInformation("UpdateOuputDaiLyTrend success at: {time}", DateTimeOffset.Now);
                    }
                    else
                    {
                        _logger.LogInformation("UpdateOuputDaiLyTrend error:" + _resultDB.ReturnString);
                        _logger.LogInformation("UpdateOuputDaiLyTrend error at: {time}", DateTimeOffset.Now);
                    }
                }

                foreach (var day in EachDay.EachDayForWeeks(DateTime.Now, DateTime.Now))
                {
                    // wlp week
                    Dictionary<string, string> dicweek = new Dictionary<string, string>();
                    dicweek.Add("A_BEGIN_DATE", day.ToString("yyyyMMdd"));
                    ResultDB resultDB_week = sql.ExecProceduce2("PKG_BUSINESS@TREND_DAILY_WEEK_WLP", dicweek);

                    if (resultDB_week.ReturnInt == 0)
                    {
                        DataTable wlpData = resultDB_week.ReturnDataSet.Tables[0];
                        DataTable tcWlpData = resultDB_week.ReturnDataSet.Tables[1];
                        DataTable bdmpData = resultDB_week.ReturnDataSet.Tables[2];
                        DataTable allData = resultDB_week.ReturnDataSet.Tables[3];

                        List<OutPutTrendDailyWlp> OutputTrendDaily = new List<OutPutTrendDailyWlp>();
                        OutPutTrendDailyWlp outPut = null;

                        foreach (DataRow drWork in wlpData.Rows)
                        {
                            outPut = new OutPutTrendDailyWlp();
                            outPut.WorkDate = drWork["Work Date"].NullString();
                            outPut.Week = int.Parse(drWork["_Week"].NullString());

                            if (outPut.WorkDate.Length == 8)
                            {
                                outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                            }

                            double cumYieldSummary = 0;
                            for (int i = 0; i < wlpData.Columns.Count; i++)
                            {
                                if (wlpData.Columns[i].ColumnName.StartsWith("YIELD"))
                                {

                                }
                                else
                                if (wlpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                                {
                                    cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                        (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            outPut.N_WLP_CumYield = cumYieldSummary;
                            OutputTrendDaily.Add(outPut);
                        }

                        foreach (DataRow drWork in tcWlpData.Rows)
                        {
                            if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                            {
                                outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                            }
                            else
                            {
                                outPut = new OutPutTrendDailyWlp();
                                outPut.WorkDate = drWork["Work Date"].NullString();
                                outPut.Week = int.Parse(drWork["_Week"].NullString());

                                if (outPut.WorkDate.Length == 8)
                                {
                                    outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                    outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                    outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                }

                                OutputTrendDaily.Add(outPut);
                            }

                            double cumYieldSummary = 0;
                            for (int i = 0; i < tcWlpData.Columns.Count; i++)
                            {
                                if (tcWlpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                                {
                                    cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                        (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            outPut.TC_WLP_CumYield = cumYieldSummary;
                        }

                        foreach (DataRow drWork in bdmpData.Rows)
                        {
                            if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                            {
                                outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                            }
                            else
                            {
                                outPut = new OutPutTrendDailyWlp();
                                outPut.WorkDate = drWork["Work Date"].NullString();
                                outPut.Week = int.Parse(drWork["_Week"].NullString());

                                if (outPut.WorkDate.Length == 8)
                                {
                                    outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                    outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                    outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                }

                                OutputTrendDaily.Add(outPut);
                            }

                            double cumYieldSummary = 0;
                            for (int i = 0; i < bdmpData.Columns.Count; i++)
                            {
                                if (bdmpData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                                {
                                    cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                        (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            outPut.BDMP_CumYield = cumYieldSummary;
                        }

                        foreach (DataRow drWork in allData.Rows)
                        {
                            if (OutputTrendDaily.Exists(x => x.WorkDate == drWork["Work Date"].NullString()))
                            {
                                outPut = OutputTrendDaily.FirstOrDefault(x => x.WorkDate == drWork["Work Date"].NullString());
                            }
                            else
                            {
                                outPut = new OutPutTrendDailyWlp();
                                outPut.WorkDate = drWork["Work Date"].NullString();
                                outPut.Week = int.Parse(drWork["_Week"].NullString());

                                if (outPut.WorkDate.Length == 8)
                                {
                                    outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                    outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                    outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                }

                                OutputTrendDaily.Add(outPut);
                            }

                            double cumYieldSummary = 0;
                            for (int i = 0; i < allData.Columns.Count; i++)
                            {
                                if (allData.Columns[i].ColumnName.StartsWith("CUM_YIELD"))
                                {
                                    cumYieldSummary = (cumYieldSummary == 0 ? 1 : cumYieldSummary) *
                                        (double.Parse(drWork[i].IfNullIsZero()) == 0 ? 1 : double.Parse(drWork[i].IfNullIsZero()));
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            outPut.Total_CumYield = cumYieldSummary;
                        }

                        // UPDATE DATA 
                        Dictionary<string, string> dic2 = new Dictionary<string, string>();
                        DataTable data = OutputTrendDaily.ToDataTable();
                        ResultDB _resultDBweek = sql.ExecuteDB("PKG_BUSINESS@UPDATE_TREND_DAILY_WEEK_WLP", dic2, "A_DATA", data);

                        if (_resultDBweek.ReturnInt == 0)
                        {
                            _logger.LogInformation("UpdateOuputDaiLyTrend week success at: {time}", DateTimeOffset.Now);
                        }
                        else
                        {
                            _logger.LogInformation("UpdateOuputDaiLyTrend week error:" + _resultDBweek.ReturnString);
                            _logger.LogInformation("UpdateOuputDaiLyTrend week error at: {time}", DateTimeOffset.Now);
                        }
                    }
                }

                // Module
                ResultDB resultDB1 = sql.ExecProceduce2("PKG_BUSINESS@TREND_DAILY_MODULE", dic);

                if (resultDB1.ReturnInt == 0)
                {
                    DataTable dtOperation = resultDB1.ReturnDataSet.Tables[0];
                    DataTable allData = resultDB1.ReturnDataSet.Tables[1];
                    DataTable groupItemData = resultDB1.ReturnDataSet.Tables[2];

                    List<OutPutTrendDailyModule> OutputTrendModuleDaily = new List<OutPutTrendDailyModule>();
                    OutPutTrendDailyModule outPut;

                    double operInQty = 0, operOutQty = 0;
                    double operRatio = 0, dayRatio = 0, sumRatio = 0;
                    int i = 0;

                    foreach (DataRow drWork in allData.Rows)
                    {
                        if (OutputTrendModuleDaily.Exists(x => x.WorkDate == drWork["Date"].NullString()))
                        {
                            outPut = OutputTrendModuleDaily.FirstOrDefault(x => x.WorkDate == drWork["Date"].NullString());
                        }
                        else
                        {
                            outPut = new OutPutTrendDailyModule();
                            outPut.WorkDate = drWork["Date"].NullString();

                            if (outPut.WorkDate.Length == 8)
                            {
                                outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                            }

                            OutputTrendModuleDaily.Add(outPut);
                        }

                        dayRatio = -1;
                        sumRatio = -1;
                        for (int j = 0; j < dtOperation.Rows.Count; j++)
                        {
                            string operCode = dtOperation.Rows[j]["OPERATION_ID"].ToString();

                            // 공정별 투입/생산 수량을 기준으로 직행율을 계산
                            operInQty = double.Parse(drWork[IN + operCode].IfNullIsZero());
                            operOutQty = double.Parse(drWork[OUT + operCode].IfNullIsZero());

                            //Debug.WriteLine("operInQty:" + operInQty);
                            //Debug.WriteLine("operOutQty:" + operOutQty);

                            if (operInQty == 0)
                            {
                                operRatio = 0D;
                            }
                            else
                            {
                                operRatio = operOutQty / operInQty;
                            }

                            // 당일 직행율을 계산
                            if (operRatio > 0)
                            {
                                if (dayRatio < 0)
                                {
                                    dayRatio = operRatio;
                                }
                                else
                                {
                                    dayRatio *= operRatio;
                                }
                            }

                            // 누적 직행율을 계산
                            if (i == 0)
                            {
                                // 최초는 Row는 공정 직행율과 동일
                                sumRatio = dayRatio;
                            }
                            else
                            {
                                //Debug.WriteLine("dtData.Rows[0]_1_IN:" + operCode + ": " + allData.Rows[0][IN + operCode].IfNullIsZero());
                                //Debug.WriteLine("dtData.Rows[0]_1_OUT:" + operCode + ": " + allData.Rows[0][OUT + operCode].IfNullIsZero());

                                // 합산된 투입/생산 수량을 기준으로 직행율을 계산

                                allData.Rows[0][IN + operCode] = double.Parse(allData.Rows[0][IN + operCode].IfNullIsZero()) + operInQty;
                                allData.Rows[0][OUT + operCode] = double.Parse(allData.Rows[0][OUT + operCode].IfNullIsZero()) + operOutQty;

                                operInQty = double.Parse(allData.Rows[0][IN + operCode].IfNullIsZero());
                                operOutQty = double.Parse(allData.Rows[0][OUT + operCode].IfNullIsZero());

                                //Debug.WriteLine("operInQty2:" + operInQty);
                                //Debug.WriteLine("operOutQty2:" + operOutQty);

                                if (operInQty == 0)
                                {
                                    operRatio = 0D;
                                }
                                else
                                {
                                    operRatio = operOutQty / operInQty;
                                }

                                // 합산 직행율을 계산
                                if (operRatio > 0)
                                {
                                    if (sumRatio < 0)
                                    {
                                        sumRatio = operRatio;
                                    }
                                    else
                                    {
                                        sumRatio *= operRatio;
                                    }

                                    //Debug.WriteLine("operRatio:" + operRatio);
                                    //Debug.WriteLine("sumRatio:" + sumRatio);
                                }
                            }
                        }

                        outPut.Total_CumYield = Math.Round(sumRatio * 100D, 2);

                        i++;
                    }

                    List<string> lstMaterialGroup = new List<string>()
                    {
                        "MG010","MG030","MG220","MG200","MG210","MG190","MG180","MG170","MG160", "MG150","MG140","MG130","MG120","MG110","MG100","MG090",
                        "MG080","MG070","MG060","MG050","MG235","MG230","MG020","MG045","MG040","MG035"
                    };

                    foreach (var group in lstMaterialGroup)
                    {
                        operInQty = 0; operOutQty = 0;
                        operRatio = 0; dayRatio = 0; sumRatio = 0;
                        i = 0;

                        DataRow[] rslt = groupItemData.Select("MATERIAL_GROUP1 = '" + group + "'").OrderBy(x => x["Date"]).ToArray();

                        foreach (DataRow drWork in rslt)
                        {
                            if (OutputTrendModuleDaily.Exists(x => x.WorkDate == drWork["Date"].NullString()))
                            {
                                outPut = OutputTrendModuleDaily.FirstOrDefault(x => x.WorkDate == drWork["Date"].NullString());
                            }
                            else
                            {
                                outPut = new OutPutTrendDailyModule();
                                outPut.WorkDate = drWork["Date"].NullString();

                                if (outPut.WorkDate.Length == 8)
                                {
                                    outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                    outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                    outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                    outPut.Week = DateTime.Parse(outPut.Year + "-" + outPut.Month + "-" + outPut.Day).GetWeekOfYear() + 1;
                                }

                                OutputTrendModuleDaily.Add(outPut);
                            }

                            dayRatio = -1;
                            sumRatio = -1;
                            for (int j = 0; j < dtOperation.Rows.Count; j++)
                            {
                                string operCode = dtOperation.Rows[j]["OPERATION_ID"].ToString();

                                // 공정별 투입/생산 수량을 기준으로 직행율을 계산
                                operInQty = double.Parse(drWork[IN + operCode].IfNullIsZero());
                                operOutQty = double.Parse(drWork[OUT + operCode].IfNullIsZero());
                                if (operInQty == 0)
                                {
                                    operRatio = 0D;
                                }
                                else
                                {
                                    operRatio = operOutQty / operInQty;
                                }

                                // 당일 직행율을 계산
                                if (operRatio > 0)
                                {
                                    if (dayRatio < 0)
                                    {
                                        dayRatio = operRatio;
                                    }
                                    else
                                    {
                                        dayRatio *= operRatio;
                                    }
                                }

                                // 누적 직행율을 계산
                                if (i == 0)
                                {
                                    // 최초는 Row는 공정 직행율과 동일
                                    sumRatio = dayRatio;
                                }
                                else
                                {
                                    // 합산된 투입/생산 수량을 기준으로 직행율을 계산
                                    operInQty = double.Parse(rslt[0][IN + operCode].IfNullIsZero()) + operInQty; ;
                                    operOutQty = double.Parse(rslt[0][OUT + operCode].IfNullIsZero()) + operOutQty;
                                    if (operInQty == 0)
                                    {
                                        operRatio = 0D;
                                    }
                                    else
                                    {
                                        operRatio = operOutQty / operInQty;
                                    }

                                    // 합산 직행율을 계산
                                    if (operRatio > 0)
                                    {
                                        if (sumRatio < 0)
                                        {
                                            sumRatio = operRatio;
                                        }
                                        else
                                        {
                                            sumRatio *= operRatio;
                                        }
                                    }
                                }
                            }


                            switch (group)
                            {
                                case "MG010":
                                    outPut.MG010_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG030":
                                    outPut.MG030_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG220":
                                    outPut.MG220_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG200":
                                    outPut.MG200_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG210":
                                    outPut.MG210_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG190":
                                    outPut.MG190_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG180":
                                    outPut.MG180_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG170":
                                    outPut.MG170_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG160":
                                    outPut.MG160_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG150":
                                    outPut.MG150_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG140":
                                    outPut.MG140_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG130":
                                    outPut.MG130_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG120":
                                    outPut.MG120_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG110":
                                    outPut.MG110_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG100":
                                    outPut.MG100_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG090":
                                    outPut.MG090_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG080":
                                    outPut.MG080_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG070":
                                    outPut.MG070_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG060":
                                    outPut.MG060_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG050":
                                    outPut.MG050_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG235":
                                    outPut.MG235_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG230":
                                    outPut.MG230_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG020":
                                    outPut.MG020_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG045":
                                    outPut.MG045_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG040":
                                    outPut.MG040_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                                case "MG035":
                                    outPut.MG035_CumYield = Math.Round(sumRatio * 100D, 2);
                                    break;
                            }

                            i++;
                        }
                    }

                    // UPDATE DATA 
                    Dictionary<string, string> dic2 = new Dictionary<string, string>();
                    DataTable data = OutputTrendModuleDaily.ToDataTable();
                    ResultDB _resultDB1 = sql.ExecuteDB("PKG_BUSINESS@UPDATE_TREND_DAILY_MODULE", dic2, "A_DATA", data);

                    if (_resultDB1.ReturnInt == 0)
                    {
                        _logger.LogInformation("UpdateOuputDaiLyTrend Module success at: {time}", DateTimeOffset.Now);
                    }
                    else
                    {
                        _logger.LogInformation("UpdateOuputDaiLyTrend Module error:" + _resultDB1.ReturnString);
                        _logger.LogInformation("UpdateOuputDaiLyTrend Module error at: {time}", DateTimeOffset.Now);
                    }
                }

                // Module Week
                foreach (var day in EachDay.EachDayForWeeks(DateTime.Now, DateTime.Now))
                {
                    Dictionary<string, string> dicweek = new Dictionary<string, string>();
                    dicweek.Add("A_BEGIN_DATE", day.ToString("yyyyMMdd"));
                    ResultDB resultDB1_week = sql.ExecProceduce2("PKG_BUSINESS@TREND_DAILY_WEEK_MODULE", dicweek);

                    if (resultDB1_week.ReturnInt == 0)
                    {
                        DataTable dtOperation = resultDB1_week.ReturnDataSet.Tables[0];
                        DataTable allData = resultDB1_week.ReturnDataSet.Tables[1];
                        DataTable groupItemData = resultDB1_week.ReturnDataSet.Tables[2];

                        List<OutPutTrendDailyModule> OutputTrendModuleDaily = new List<OutPutTrendDailyModule>();
                        OutPutTrendDailyModule outPut;

                        double operInQty = 0, operOutQty = 0;
                        double operRatio = 0, dayRatio = 0, sumRatio = 0;
                        int i = 0;

                        foreach (DataRow drWork in allData.Rows)
                        {
                            if (OutputTrendModuleDaily.Exists(x => x.WorkDate == drWork["Date"].NullString()))
                            {
                                outPut = OutputTrendModuleDaily.FirstOrDefault(x => x.WorkDate == drWork["Date"].NullString());
                            }
                            else
                            {
                                outPut = new OutPutTrendDailyModule();
                                outPut.WorkDate = drWork["Date"].NullString();
                                outPut.Week = int.Parse(drWork["_Week"].NullString());

                                if (outPut.WorkDate.Length == 8)
                                {
                                    outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                    outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                    outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                }

                                OutputTrendModuleDaily.Add(outPut);
                            }

                            dayRatio = -1;
                            sumRatio = -1;
                            for (int j = 0; j < dtOperation.Rows.Count; j++)
                            {
                                string operCode = dtOperation.Rows[j]["OPERATION_ID"].ToString();

                                // 공정별 투입/생산 수량을 기준으로 직행율을 계산
                                operInQty = double.Parse(drWork[IN + operCode].IfNullIsZero());
                                operOutQty = double.Parse(drWork[OUT + operCode].IfNullIsZero());

                                //Debug.WriteLine("operInQty:" + operInQty);
                                //Debug.WriteLine("operOutQty:" + operOutQty);

                                if (operInQty == 0)
                                {
                                    operRatio = 0D;
                                }
                                else
                                {
                                    operRatio = operOutQty / operInQty;
                                }

                                // 당일 직행율을 계산
                                if (operRatio > 0)
                                {
                                    if (dayRatio < 0)
                                    {
                                        dayRatio = operRatio;
                                    }
                                    else
                                    {
                                        dayRatio *= operRatio;
                                    }
                                }

                                // 누적 직행율을 계산
                                if (i == 0)
                                {
                                    // 최초는 Row는 공정 직행율과 동일
                                    sumRatio = dayRatio;
                                }
                                else
                                {
                                    //Debug.WriteLine("dtData.Rows[0]_1_IN:" + operCode + ": " + allData.Rows[0][IN + operCode].IfNullIsZero());
                                    //Debug.WriteLine("dtData.Rows[0]_1_OUT:" + operCode + ": " + allData.Rows[0][OUT + operCode].IfNullIsZero());

                                    // 합산된 투입/생산 수량을 기준으로 직행율을 계산

                                    allData.Rows[0][IN + operCode] = double.Parse(allData.Rows[0][IN + operCode].IfNullIsZero()) + operInQty;
                                    allData.Rows[0][OUT + operCode] = double.Parse(allData.Rows[0][OUT + operCode].IfNullIsZero()) + operOutQty;

                                    operInQty = double.Parse(allData.Rows[0][IN + operCode].IfNullIsZero());
                                    operOutQty = double.Parse(allData.Rows[0][OUT + operCode].IfNullIsZero());

                                    //Debug.WriteLine("operInQty2:" + operInQty);
                                    //Debug.WriteLine("operOutQty2:" + operOutQty);

                                    if (operInQty == 0)
                                    {
                                        operRatio = 0D;
                                    }
                                    else
                                    {
                                        operRatio = operOutQty / operInQty;
                                    }

                                    // 합산 직행율을 계산
                                    if (operRatio > 0)
                                    {
                                        if (sumRatio < 0)
                                        {
                                            sumRatio = operRatio;
                                        }
                                        else
                                        {
                                            sumRatio *= operRatio;
                                        }

                                        //Debug.WriteLine("operRatio:" + operRatio);
                                        //Debug.WriteLine("sumRatio:" + sumRatio);
                                    }
                                }
                            }

                            outPut.Total_CumYield = Math.Round(sumRatio * 100D, 2);

                            i++;
                        }

                        List<string> lstMaterialGroup = new List<string>()
                    {
                        "MG010","MG030","MG220","MG200","MG210","MG190","MG180","MG170","MG160", "MG150","MG140","MG130","MG120","MG110","MG100","MG090",
                        "MG080","MG070","MG060","MG050","MG235","MG230","MG020","MG045","MG040","MG035"
                    };

                        foreach (var group in lstMaterialGroup)
                        {
                            operInQty = 0; operOutQty = 0;
                            operRatio = 0; dayRatio = 0; sumRatio = 0;
                            i = 0;

                            DataRow[] rslt = groupItemData.Select("MATERIAL_GROUP1 = '" + group + "'").OrderBy(x => x["Date"]).ToArray();

                            foreach (DataRow drWork in rslt)
                            {
                                if (OutputTrendModuleDaily.Exists(x => x.WorkDate == drWork["Date"].NullString()))
                                {
                                    outPut = OutputTrendModuleDaily.FirstOrDefault(x => x.WorkDate == drWork["Date"].NullString());
                                }
                                else
                                {
                                    outPut = new OutPutTrendDailyModule();
                                    outPut.WorkDate = drWork["Date"].NullString();
                                    outPut.Week = int.Parse(drWork["_Week"].NullString());

                                    if (outPut.WorkDate.Length == 8)
                                    {
                                        outPut.Year = int.Parse(outPut.WorkDate.Substring(0, 4));
                                        outPut.Month = int.Parse(outPut.WorkDate.Substring(4, 2));
                                        outPut.Day = int.Parse(outPut.WorkDate.Substring(6, 2));
                                    }

                                    OutputTrendModuleDaily.Add(outPut);
                                }

                                dayRatio = -1;
                                sumRatio = -1;
                                for (int j = 0; j < dtOperation.Rows.Count; j++)
                                {
                                    string operCode = dtOperation.Rows[j]["OPERATION_ID"].ToString();

                                    // 공정별 투입/생산 수량을 기준으로 직행율을 계산
                                    operInQty = double.Parse(drWork[IN + operCode].IfNullIsZero());
                                    operOutQty = double.Parse(drWork[OUT + operCode].IfNullIsZero());
                                    if (operInQty == 0)
                                    {
                                        operRatio = 0D;
                                    }
                                    else
                                    {
                                        operRatio = operOutQty / operInQty;
                                    }

                                    // 당일 직행율을 계산
                                    if (operRatio > 0)
                                    {
                                        if (dayRatio < 0)
                                        {
                                            dayRatio = operRatio;
                                        }
                                        else
                                        {
                                            dayRatio *= operRatio;
                                        }
                                    }

                                    // 누적 직행율을 계산
                                    if (i == 0)
                                    {
                                        // 최초는 Row는 공정 직행율과 동일
                                        sumRatio = dayRatio;
                                    }
                                    else
                                    {
                                        // 합산된 투입/생산 수량을 기준으로 직행율을 계산
                                        operInQty = double.Parse(rslt[0][IN + operCode].IfNullIsZero()) + operInQty; ;
                                        operOutQty = double.Parse(rslt[0][OUT + operCode].IfNullIsZero()) + operOutQty;
                                        if (operInQty == 0)
                                        {
                                            operRatio = 0D;
                                        }
                                        else
                                        {
                                            operRatio = operOutQty / operInQty;
                                        }

                                        // 합산 직행율을 계산
                                        if (operRatio > 0)
                                        {
                                            if (sumRatio < 0)
                                            {
                                                sumRatio = operRatio;
                                            }
                                            else
                                            {
                                                sumRatio *= operRatio;
                                            }
                                        }
                                    }
                                }


                                switch (group)
                                {
                                    case "MG010":
                                        outPut.MG010_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG030":
                                        outPut.MG030_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG220":
                                        outPut.MG220_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG200":
                                        outPut.MG200_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG210":
                                        outPut.MG210_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG190":
                                        outPut.MG190_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG180":
                                        outPut.MG180_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG170":
                                        outPut.MG170_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG160":
                                        outPut.MG160_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG150":
                                        outPut.MG150_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG140":
                                        outPut.MG140_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG130":
                                        outPut.MG130_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG120":
                                        outPut.MG120_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG110":
                                        outPut.MG110_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG100":
                                        outPut.MG100_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG090":
                                        outPut.MG090_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG080":
                                        outPut.MG080_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG070":
                                        outPut.MG070_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG060":
                                        outPut.MG060_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG050":
                                        outPut.MG050_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG235":
                                        outPut.MG235_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG230":
                                        outPut.MG230_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG020":
                                        outPut.MG020_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG045":
                                        outPut.MG045_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG040":
                                        outPut.MG040_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                    case "MG035":
                                        outPut.MG035_CumYield = Math.Round(sumRatio * 100D, 2);
                                        break;
                                }

                                i++;
                            }
                        }

                        // UPDATE DATA 
                        Dictionary<string, string> dic2 = new Dictionary<string, string>();
                        DataTable data = OutputTrendModuleDaily.ToDataTable();
                        ResultDB _resultDB1 = sql.ExecuteDB("PKG_BUSINESS@UPDATE_TREND_DAILY_WEEK_MODULE", dic2, "A_DATA", data);

                        if (_resultDB1.ReturnInt == 0)
                        {
                            _logger.LogInformation("UpdateOuputDaiLyTrend Module week success at: {time}", DateTimeOffset.Now);
                        }
                        else
                        {
                            _logger.LogInformation("UpdateOuputDaiLyTrend Module week error:" + _resultDB1.ReturnString);
                            _logger.LogInformation("UpdateOuputDaiLyTrend Module week error at: {time}", DateTimeOffset.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UpdateOuputDaiLyTrend error:" + ex.Message);
            }

        }
    }
}
