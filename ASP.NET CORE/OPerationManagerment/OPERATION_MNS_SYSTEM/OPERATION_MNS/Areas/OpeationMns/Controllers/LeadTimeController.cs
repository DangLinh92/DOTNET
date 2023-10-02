using DevExpress.XtraSpreadsheet.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class LeadTimeController : AdminBaseController
    {
        ILeadTimeService _LeadTimeService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LeadTimeController(ILeadTimeService service, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _LeadTimeService = service;
        }

        public IActionResult Index()
        {
            string year = DateTime.Now.Year.ToString();
            LeadTimeModel models = GetData(year, "", "0", "", "");

            return View(models);
        }

        [HttpPost]
        public ActionResult Search(string year, string month, string week, string day, string holiday)
        {
            LeadTimeModel models = GetData(year, month, week, day, holiday);
            return View("Index", models);
        }

        [HttpPost]
        public ActionResult GetWeekByMonth(string year, string month)
        {
            LeadTimeModel models = new LeadTimeModel(year);
            var weeks = models.GetWeeksByMonth(year, month);
            return new OkObjectResult(weeks);
        }

        private LeadTimeModel GetData(string year, string month, string week, string day, string holiday)
        {
            LeadTimeModel models = new LeadTimeModel(year);
            models.Month = month.NullString();
            models.Week = int.Parse(week);
            models.Day = day;
            models.Ox = holiday;

            models.Weeks = models.GetWeeksByMonth(year, month.NullString());
            models.Weeks_Lable = models.GetWeeks();

            if (week == "0" || !string.IsNullOrEmpty(day))
            {
                week = "";

                if (!string.IsNullOrEmpty(day))
                {
                    if (month.NullString() != "" && year != "")
                    {
                        int w = DateTime.Parse(year + "-" + month + "-" + day).GetWeekOfYear();
                        models.Week = w == 0 ? 1 : w;
                    }
                    else
                    {
                        models.Week = 0;
                    }
                }
            }

            List<LeadTimeViewModel> lst = _LeadTimeService.GetLeadTime(year, month, week, day, holiday);

            double targetTotal = _LeadTimeService.GetTargetWLP(year);

            //if (month != "" || week != "" || day != "")
            // {

            if (month.NullString() == "" && week.NullString() == "" && day.NullString() == "")
            {
                string _month = DateTime.Now.ToString("MM");
                models.WLP1_LeadTimeByDay = (from lt in lst.Where(x => x.WLP == "WLP1" && x.WorkMonth == _month).AsEnumerable()
                                             group lt by new
                                             {
                                                 YEAR = lt.WorkYear,
                                                 DAY = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.DAY,
                                                 Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                                 Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                                 Value_target = Math.Round(g.Average(s => s.Target), 1)
                                             }).OrderBy(x => int.Parse(x.Label_x)).ToList();

                models.WLP2_LeadTimeByDay = (from lt in lst.Where(x => x.WLP == "WLP2" && x.WorkMonth == _month).AsEnumerable()
                                             group lt by new
                                             {
                                                 YEAR = lt.WorkYear,
                                                 DAY = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.DAY,
                                                 Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                                 Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                                 Value_target = Math.Round(g.Average(s => s.Target), 1)
                                             }).OrderBy(x => int.Parse(x.Label_x)).ToList();
            }
            else
            {
                models.WLP1_LeadTimeByDay = (from lt in lst.Where(x => x.WLP == "WLP1").AsEnumerable()
                                             group lt by new
                                             {
                                                 YEAR = lt.WorkYear,
                                                 DAY = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.DAY,
                                                 Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                                 Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                                 Value_target = Math.Round(g.Average(s => s.Target), 1)
                                             }).OrderBy(x => int.Parse(x.Label_x)).ToList();

                models.WLP2_LeadTimeByDay = (from lt in lst.Where(x => x.WLP == "WLP2").AsEnumerable()
                                             group lt by new
                                             {
                                                 YEAR = lt.WorkYear,
                                                 DAY = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.DAY,
                                                 Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                                 Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                                 Value_target = Math.Round(g.Average(s => s.Target), 1)
                                             }).OrderBy(x => int.Parse(x.Label_x)).ToList();
            }

            // label ngày
            models.Days1 = models.WLP1_LeadTimeByDay.Select(x => x.Label_x.Substring(6, 2) + "일").OrderBy(x => int.Parse(x.Replace("일", ""))).ToList();
            models.Days2 = models.WLP2_LeadTimeByDay.Select(x => x.Label_x.Substring(6, 2) + "일").OrderBy(x => int.Parse(x.Replace("일", ""))).ToList();
            //}

            var lstWlp1Month = (from lt in lst.Where(x => x.WLP == "WLP1").AsEnumerable()
                                group lt by new
                                {
                                    YEAR = lt.WorkYear,
                                    MONTH = lt.WorkMonth,
                                } into g
                                select new ChartDataItem
                                {
                                    Label_x = g.Key.MONTH,
                                    Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                    Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                    Value_target = Math.Round(g.Average(s => s.Target), 1)
                                }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            models.WLP1_LeadTimeByMonth = UpdateDataLost(lstWlp1Month, "Month", models.Weeks_Lable);

            var lstWlp2Month = (from lt in lst.Where(x => x.WLP == "WLP2").AsEnumerable()
                                group lt by new
                                {
                                    YEAR = lt.WorkYear,
                                    MONTH = lt.WorkMonth,
                                } into g
                                select new ChartDataItem
                                {
                                    Label_x = g.Key.MONTH,
                                    Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                    Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                    Value_target = Math.Round(g.Average(s => s.Target), 1)
                                }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            models.WLP2_LeadTimeByMonth = UpdateDataLost(lstWlp2Month, "Month", models.Weeks_Lable);


            List<ChartDataItem> lstWlpMonth = new List<ChartDataItem>();
            foreach (var wlp1 in models.WLP1_LeadTimeByMonth)
            {
                foreach (var wlp2 in models.WLP2_LeadTimeByMonth)
                {
                    if (wlp1.Label_x == wlp2.Label_x)
                    {
                        lstWlpMonth.Add(new ChartDataItem()
                        {
                            Label_x = wlp1.Label_x,
                            Value_runtime = Math.Round(wlp1.Value_runtime + wlp2.Value_runtime, 1),
                            Value_waittime = Math.Round(wlp1.Value_waittime + wlp2.Value_waittime, 1),
                            Value_target = Math.Round(wlp1.Value_target + wlp2.Value_target, 1),
                        });
                        break;
                    }
                }
            }

            models.WLP_LeadTimeByMonth = UpdateDataLost(lstWlpMonth, "Month", models.Weeks_Lable);

            // year

            List<LeadTimeViewModel> lstYear = _LeadTimeService.GetLeadTime(year, "", "", "", holiday);

            if (lstYear.Count > 0)
            {

                var wlp1_y = (from lt in lstYear.Where(x => x.WLP == "WLP1").AsEnumerable()
                              group lt by new
                              {
                                  YEAR = lt.WorkYear,
                              } into g
                              select new ChartDataItem
                              {
                                  Label_x = g.Key.YEAR,
                                  Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                  Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                              }).OrderBy(x => int.Parse(x.Label_x)).ToList();

                var wlp2_y = (from lt in lstYear.Where(x => x.WLP == "WLP2").AsEnumerable()
                              group lt by new
                              {
                                  YEAR = lt.WorkYear,
                              } into g
                              select new ChartDataItem
                              {
                                  Label_x = g.Key.YEAR,
                                  Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                  Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                              }).OrderBy(x => int.Parse(x.Label_x)).ToList();

                models.WLP_LeadTimeByYear = new List<ChartDataItem>()
                {
                    new ChartDataItem()
                    {
                         Label_x =  year,
                         Value_runtime = Math.Round(wlp1_y.FirstOrDefault().Value_runtime +  wlp2_y.FirstOrDefault().Value_runtime,1),
                         Value_waittime =Math.Round(wlp1_y.FirstOrDefault().Value_waittime +  wlp2_y.FirstOrDefault().Value_waittime,1),
                    }
                };

                models.WLP_LeadTimeByYear.FirstOrDefault().Value_target = lstYear.FirstOrDefault(x => x.WLP == "WLP1").Target + lstYear.FirstOrDefault(x => x.WLP == "WLP2").Target;
            }
            else
            {
                models.WLP_LeadTimeByYear = new List<ChartDataItem>();
            }

            //if (models.WLP_LeadTimeByMonth.Count > 0)
            //{
            //    models.WLP_LeadTimeByYear = new List<ChartDataItem>()
            //    {
            //        new ChartDataItem()
            //        {
            //            Label_x = year,
            //            Value_runtime = Math.Round(models.WLP_LeadTimeByMonth.Average(x=>x.Value_runtime),1),
            //            Value_waittime = Math.Round(models.WLP_LeadTimeByMonth.Average(x=>x.Value_waittime),1),
            //            Value_target = Math.Round(models.WLP_LeadTimeByMonth.Average(x=>x.Value_target),1)
            //        }
            //    };
            //}
            //else
            //{
            //    models.WLP_LeadTimeByYear = new List<ChartDataItem>();
            //}

            // week
            var lstWlp1Week = (from lt in lst.Where(x => x.WLP == "WLP1").AsEnumerable()
                               group lt by new
                               {
                                   YEAR = lt.WorkYear,
                                   WEEK = lt.WorkWeek,
                               } into g
                               select new ChartDataItem
                               {
                                   Label_x = g.Key.WEEK,
                                   Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                   Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                   Value_target = Math.Round(g.Average(s => s.Target), 1)
                               }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            models.WLP1_LeadTimeByWeek = UpdateDataLost(lstWlp1Week, "Week", models.Weeks_Lable);

            var lstWlp2Week = (from lt in lst.Where(x => x.WLP == "WLP2").AsEnumerable()
                               group lt by new
                               {
                                   YEAR = lt.WorkYear,
                                   WEEK = lt.WorkWeek,
                               } into g
                               select new ChartDataItem
                               {
                                   Label_x = g.Key.WEEK,
                                   Value_runtime = Math.Round(g.Average(s => s.RunTime), 1),
                                   Value_waittime = Math.Round(g.Average(s => s.WaitTime), 1),
                                   Value_target = Math.Round(g.Average(s => s.Target), 1)
                               }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            models.WLP2_LeadTimeByWeek = UpdateDataLost(lstWlp2Week, "Week", models.Weeks_Lable);


            List<ChartDataItem> lstWlpWeek = new List<ChartDataItem>();
            foreach (var wlp1 in models.WLP1_LeadTimeByWeek)
            {
                foreach (var wlp2 in models.WLP2_LeadTimeByWeek)
                {
                    if (wlp1.Label_x == wlp2.Label_x)
                    {
                        lstWlpWeek.Add(new ChartDataItem()
                        {
                            Label_x = wlp1.Label_x,
                            Value_runtime = Math.Round(wlp1.Value_runtime + wlp2.Value_runtime, 1),
                            Value_waittime = Math.Round(wlp1.Value_waittime + wlp2.Value_waittime, 1),
                            Value_target = Math.Round(wlp1.Value_target + wlp2.Value_target, 1),
                        });
                        break;
                    }
                }
            }

            models.WLP_LeadTimeByWeek = UpdateDataLost(lstWlpWeek, "Week", models.Weeks_Lable);

            return models;
        }

        private List<ChartDataItem> UpdateDataLost(List<ChartDataItem> chartDatas, string monthWeek, List<string> Weeks)
        {
            if (chartDatas.Count == 0) return chartDatas;

            double target = chartDatas[0].Value_target;
            if (monthWeek == "Month")
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (chartDatas.Count >= i)
                    {
                        if (int.Parse(chartDatas[i - 1].Label_x) != i)
                        {
                            chartDatas.Insert(i - 1, new ChartDataItem()
                            {
                                Label_x = i < 10 ? "0" + i : i + "",
                                Value_runtime = 0,
                                Value_waittime = 0,
                                Value_target = target,
                                Value_leadtime = 0
                            });
                        }
                    }
                    else
                    {
                        chartDatas.Add(new ChartDataItem()
                        {
                            Label_x = i < 10 ? "0" + i : i + "",
                            Value_runtime = 0,
                            Value_waittime = 0,
                            Value_target = target,
                            Value_leadtime = 0
                        });
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Weeks.Count; i++)
                {
                    if (chartDatas.Count >= i)
                    {
                        if (int.Parse(chartDatas[i - 1].Label_x) != i)
                        {
                            chartDatas.Insert(i - 1, new ChartDataItem()
                            {
                                Label_x = i + "",
                                Value_runtime = 0,
                                Value_waittime = 0,
                                Value_target = target,
                                Value_leadtime = 0
                            });
                        }
                    }
                    else
                    {
                        chartDatas.Add(new ChartDataItem()
                        {
                            Label_x = i + "",
                            Value_runtime = 0,
                            Value_waittime = 0,
                            Value_target = target,
                            Value_leadtime = 0
                        });
                    }
                }
            }
            return chartDatas;
        }

        #region LFEM
        public IActionResult LeadTimeLfem()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.ToString("MM");
            LeadTimeModel models = new LeadTimeModel(year);// GetLFEMLeadTimeData(year, month, "0", "", "", "R8Y0");
            models.Month = month;
            models.MonthFrom = month;

            return View(models);
        }

        [HttpPost]
        public ActionResult SearchLfem(string year, string monthFrom, string month, string week, string day, string holiday, string model)
        {
            LeadTimeModel models = GetLFEMLeadTimeData(year, monthFrom, month, week, day, holiday, model.NullString());
            return View("LeadTimeLfem", models);
        }

        private LeadTimeModel GetLFEMLeadTimeData(string year,string monthFrom, string month, string week, string day, string holiday, string model)
        {
            LeadTimeModel models = new LeadTimeModel(year);
            models.Month = month.NullString();
            models.MonthFrom = monthFrom.NullString();
            models.Week = int.Parse(week);
            models.Day = day;
            models.Ox = holiday;
            models.Category = model.NullString();

            //models.Weeks = models.GetWeeksByMonth(year, month.NullString());

            //if (week == "0" || week.NullString() == "")
            //{
            //    if (month.NullString() == "")
            //    {
            //        models.Weeks_Lable = models.GetWeeks();
            //    }
            //    else
            //    {
            //        models.Weeks_Lable = models.Weeks;
            //    }
            //}
            //else
            //{
            //    models.Weeks_Lable.Add(week);
            //}

            List<string> weeks = new List<string>();
            if(monthFrom.NullString() != "" && month.NullString() != "")
            for (int i = int.Parse(monthFrom); i <= int.Parse(month); i++)
            {
                    if (i < 10)
                        weeks.AddRange(models.GetWeeksByMonth(year, "0" + i.NullString()));
                    else
                    {
                        weeks.AddRange(models.GetWeeksByMonth(year, i.NullString()));
                    }
            }
            models.Weeks = weeks;
            models.Weeks_Lable = weeks;

            if (week == "0" || !string.IsNullOrEmpty(day))
            {
                week = "";

                if (!string.IsNullOrEmpty(day))
                {
                    if (month.NullString() != "" && year != "")
                    {
                        int w = DateTime.Parse(year + "-" + month + "-" + day).GetWeekOfYear();
                        models.Week = w == 0 ? 1 : w;
                    }
                    else
                    {
                        models.Week = 0;
                    }
                }
            }

            List<LeadTimeViewModel> lstAll = _LeadTimeService.GetLeadTimeLFEM(year, "", "", "", holiday, model);

            List<LeadTimeViewModel> lst = new List<LeadTimeViewModel>(); //_LeadTimeService.GetLeadTimeLFEM(year, month, week, day, holiday, model);

            List<LeadTimeViewModel> lstWeek = new List<LeadTimeViewModel>();

            if (day.NullString() != "" && month.NullString() != "" && year.NullString() != "")
            {
                string date = year + "-" + month + "-" + day;
                lst = lstAll.Where(x => x.WorkDate == date).ToList();
            }
            else if (week.NullString() != "" && day.NullString() == "")
            {
                lst = lstAll.Where(x => int.Parse(x.WorkWeek) == int.Parse(week)).ToList();
            }
            else
            if (month.NullString() != "")
            {
                string fromDate = DateTime.Parse(year + "-" + month + "-01").ToString("yyyy-MM-dd");
                string toDate = DateTime.Parse(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                lst = lstAll.Where(x => x.WorkDate.CompareTo(fromDate) >= 0 && x.WorkDate.CompareTo(toDate) <= 0).ToList();
            }

            if(monthFrom.NullString() != "" && month.NullString() != "")
            {
                string fromDate = DateTime.Parse(year + "-" + monthFrom + "-01").ToString("yyyy-MM-dd");
                string toDate = DateTime.Parse(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                lstWeek = lstAll.Where(x => x.WorkDate.CompareTo(fromDate) >= 0 && x.WorkDate.CompareTo(toDate) <= 0).ToList();
            }

            if (month.NullString() == "" && week.NullString() == "" && day.NullString() == "")
            {
                var operationLeadTime =
                    from leadTime in lst
                    group leadTime by new
                    {
                        WORK_DATE = leadTime.WorkDate,
                        WORK_WEEK = leadTime.WorkWeek,
                        OPERATION_ID = leadTime.OperationID,
                        OPERATION_NAME = leadTime.Operation
                    } into g
                    select new
                    {
                        WorkDate = g.Key.WORK_DATE,
                        WorkWeek = g.Key.WORK_WEEK,
                        OperationId = g.Key.OPERATION_ID,
                        OperationName = g.Key.OPERATION_NAME,
                        WaitTimeAVG = Math.Round(g.Average(r => r.WaitTime), 1),
                        RunTimeAVG = Math.Round(g.Average(r => r.RunTime), 1),
                        LeadTimeAVG = Math.Round(g.Average(r => r.LeadTimeStartEnd), 1)
                    };

                string _monthStart = DateTime.Now.ToString("yyyy-MM") + "-01";
                string _monthEnd = DateTime.Parse(_monthStart).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                models.LFEM_LeadTimeByDay = (from lt in operationLeadTime.Where(x => x.WorkDate.CompareTo(_monthStart) >= 0 && x.WorkDate.CompareTo(_monthEnd) <= 0).AsEnumerable()
                                             group lt by new
                                             {
                                                 WORK_WEEK = lt.WorkWeek,
                                                 WORK_DATE = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.WORK_DATE,
                                                 Value_runtime = Math.Round(g.Sum(s => s.RunTimeAVG) / 24, 1),
                                                 Value_waittime = Math.Round(g.Sum(s => s.WaitTimeAVG) / 24, 1),
                                                 Value_leadtime = Math.Round((g.Sum(s => s.RunTimeAVG) + g.Sum(s => s.WaitTimeAVG)) / 24, 1) 
                                             }).OrderBy(x => x.Label_x).ToList();
            }
            else
            {

                var operationLeadTime =
                   from leadTime in lst
                   group leadTime by new
                   {
                       WORK_DATE = leadTime.WorkDate,
                       WORK_WEEK = leadTime.WorkWeek,
                       OPERATION_ID = leadTime.OperationID,
                       OPERATION_NAME = leadTime.Operation
                   } into g
                   select new
                   {
                       WorkDate = g.Key.WORK_DATE,
                       WorkWeek = g.Key.WORK_WEEK,
                       OperationId = g.Key.OPERATION_ID,
                       OperationName = g.Key.OPERATION_NAME,
                       WaitTimeAVG = Math.Round(g.Average(r => r.WaitTime), 1),
                       RunTimeAVG = Math.Round(g.Average(r => r.RunTime), 1),
                       LeadTimeAVG = Math.Round(g.Average(r => r.LeadTimeStartEnd), 1)
                   };

                models.LFEM_LeadTimeByDay = (from lt in operationLeadTime.AsEnumerable()
                                             group lt by new
                                             {
                                                 WORK_WEEK = lt.WorkWeek,
                                                 WORK_DATE = lt.WorkDate,
                                             } into g
                                             select new ChartDataItem
                                             {
                                                 Label_x = g.Key.WORK_DATE,
                                                 Value_runtime = Math.Round(g.Sum(s => s.RunTimeAVG) / 24, 1),
                                                 Value_waittime = Math.Round(g.Sum(s => s.WaitTimeAVG) / 24, 1),
                                                 Value_leadtime = Math.Round((g.Sum(s => s.RunTimeAVG) + g.Sum(s => s.WaitTimeAVG)) / 24, 1)
                                             }).OrderBy(x => x.Label_x).ToList();
            }

            // label ngày
            models.Days1 = models.LFEM_LeadTimeByDay.Select(x => x.Label_x.Substring(8, 2) + "일").OrderBy(x => int.Parse(x.Replace("일", ""))).ToList();

            var operationLeadTimeMonth =
                        from leadTime in lstAll.AsEnumerable()
                        group leadTime by new
                        {
                            WORK_YEAR = leadTime.WorkYear,
                            WORK_MONTH = leadTime.WorkMonth,
                            OPERATION_ID = leadTime.OperationID,
                            OPERATION_NAME = leadTime.Operation
                        } into g
                        select new
                        {
                            WorkYear = g.Key.WORK_YEAR,
                            WorkMonth = g.Key.WORK_MONTH,
                            OperationId = g.Key.OPERATION_ID,
                            OperationName = g.Key.OPERATION_NAME,
                            WaitTimeAVG = Math.Round(g.Average(r => r.WaitTime), 1),
                            RunTimeAVG = Math.Round(g.Average(r => r.RunTime), 1),
                            LeadTimeAVG = Math.Round(g.Average(r => r.LeadTimeStartEnd), 1)
                        };

            // Month
            var lstLFEMMonth = (from lt in operationLeadTimeMonth.AsEnumerable()
                                group lt by new
                                {
                                    WORK_YEAR = lt.WorkYear,
                                    WORK_MONTH = lt.WorkMonth
                                } into g
                                select new ChartDataItem
                                {
                                    Label_x = g.Key.WORK_MONTH,
                                    Value_runtime = Math.Round(g.Sum(s => s.RunTimeAVG) / 24, 1),
                                    Value_waittime = Math.Round(g.Sum(s => s.WaitTimeAVG) / 24, 1),
                                    Value_leadtime = Math.Round((g.Sum(s => s.RunTimeAVG) + g.Sum(s => s.WaitTimeAVG)) / 24, 1)
                                }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            models.LFEM_LeadTimeByMonth = UpdateDataLost(lstLFEMMonth, "Month", models.Weeks_Lable);

            if (year == DateTime.Now.Year.ToString())
            {
                int monthNow = DateTime.Now.Month;
                models.LFEM_LeadTimeByMonth.RemoveAll(x => int.Parse(x.Label_x) > monthNow);
            }

            // week

            var operationLeadTimeWeek =
                        from leadTime in lstWeek.AsEnumerable()
                        group leadTime by new
                        {
                            WORK_YEAR = leadTime.WorkYear,
                            WORK_WEEK = leadTime.WorkWeek,
                            OPERATION_ID = leadTime.OperationID,
                            OPERATION_NAME = leadTime.Operation
                        } into g
                        select new
                        {
                            WorkYear = g.Key.WORK_YEAR,
                            WorkWeek = g.Key.WORK_WEEK,
                            OperationId = g.Key.OPERATION_ID,
                            OperationName = g.Key.OPERATION_NAME,
                            WaitTimeAVG = Math.Round(g.Average(r => r.WaitTime), 1),
                            RunTimeAVG = Math.Round(g.Average(r => r.RunTime), 1),
                            LeadTimeAVG = Math.Round(g.Average(r => r.LeadTimeStartEnd), 1)
                        };

            var  LFEM_LeadTimeByWeek = (from lt in operationLeadTimeWeek.AsEnumerable()
                                          group lt by new
                                          {
                                              WORK_YEAR = lt.WorkYear,
                                              WORK_WEEK = lt.WorkWeek,
                                          } into g
                                          select new ChartDataItem
                                          {
                                              Label_x = g.Key.WORK_WEEK,
                                              Value_runtime = Math.Round(g.Sum(s => s.RunTimeAVG) / 24, 1),
                                              Value_waittime = Math.Round(g.Sum(s => s.WaitTimeAVG) / 24, 1),
                                              Value_leadtime = Math.Round((g.Sum(s => s.RunTimeAVG) + g.Sum(s => s.WaitTimeAVG)) / 24, 1)
                                          }).OrderBy(x => int.Parse(x.Label_x)).ToList();

            foreach (var item in models.Weeks_Lable)
            {
                if(!LFEM_LeadTimeByWeek.Any(x=> int.Parse(x.Label_x) == int.Parse(item)))
                {
                    LFEM_LeadTimeByWeek.Add(new ChartDataItem()
                    {
                        Label_x = item,
                        Value_leadtime = 0,
                        Value_runtime = 0,
                        Value_waittime=0,
                    });
                }
            }

            models.LFEM_LeadTimeByWeek = LFEM_LeadTimeByWeek.OrderBy(x => int.Parse(x.Label_x)).ToList();

            if (day.NullString() != "" && month.NullString() != "" && year.NullString() != "")
            {
                var dailyLeadTime =
                       from leadTime in lst.AsEnumerable()
                       group leadTime by new
                       {
                           WORK_DATE = leadTime.WorkDate,
                           WORK_YEAR = leadTime.WorkYear,
                           WORK_MONTH = leadTime.WorkMonth,
                           WORK_WEEK = leadTime.WorkWeek,
                           OPERATION_ID = leadTime.OperationID,
                           OPERATION_NAME = leadTime.Operation,
                           DISPLAY_ORDER = leadTime.DisplayOrder,
                       } into g
                       select new
                       {
                           WorkDate = g.Key.WORK_DATE,
                           WorkYear = g.Key.WORK_YEAR,
                           WorkMonth = g.Key.WORK_MONTH,
                           WorkWeek = g.Key.WORK_WEEK,
                           OperationId = g.Key.OPERATION_ID,
                           OperationName = g.Key.OPERATION_NAME,
                           DisplayOrder = g.Key.DISPLAY_ORDER,
                           WaitTimeAVG = g.Average(r => r.WaitTime),
                           RunTimeAVG = g.Average(r => r.RunTime),
                           LeadTimeAVG = g.Average(r => r.LeadTimeStartEnd),
                           Capa = g.Average(r=>r.Capa)
                       };

                // runtime by operation
                models.LFEM_RuntimeByOperation = (from lt in dailyLeadTime.AsEnumerable()
                                                  group lt by new
                                                  {
                                                      OPERATION_ID = lt.OperationId,
                                                      OPERATION = lt.OperationName,
                                                      DISPLAY_ORDER = lt.DisplayOrder
                                                  } into g
                                                  select new ChartDataItem
                                                  {
                                                      Label_x = g.Key.OPERATION,
                                                      Value_runtime = Math.Round(g.Average(s => s.RunTimeAVG), 1),
                                                      Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                  }).OrderByDescending(x => x.Value_runtime).ToList();

               
                // wait time by operation
                models.LFEM_WaitTimeByOperation = (from lt in dailyLeadTime.AsEnumerable()
                                                   group lt by new
                                                   {
                                                       OPERATION_ID = lt.OperationId,
                                                       OPERATION = lt.OperationName,
                                                       DISPLAY_ORDER = lt.DisplayOrder
                                                   } into g
                                                   select new ChartDataItem
                                                   {
                                                       Label_x = g.Key.OPERATION,
                                                       Value_waittime = Math.Round(g.Average(s => s.WaitTimeAVG), 1),
                                                       Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                   }).OrderByDescending(x => x.Value_waittime).ToList();
            }
            else if (week.NullString() != "" && day.NullString() == "")
            {
                var weeklyLeadTime =
                        from leadTime in lst.AsEnumerable()
                        group leadTime by new
                        {
                            WORK_YEAR = leadTime.WorkYear,
                            WORK_WEEK = leadTime.WorkWeek,
                            OPERATION_ID = leadTime.OperationID,
                            OPERATION_NAME = leadTime.Operation,
                            DISPLAY_ORDER = leadTime.DisplayOrder
                        } into g
                        select new
                        {
                            WorkYear = g.Key.WORK_YEAR,
                            WorkWeek = g.Key.WORK_WEEK,
                            OperationId = g.Key.OPERATION_ID,
                            OperationName = g.Key.OPERATION_NAME,
                            DisplayOrder = g.Key.DISPLAY_ORDER,
                            WaitTimeAVG = g.Average(r => r.WaitTime),
                            RunTimeAVG = g.Average(r => r.RunTime),
                            LeadTimeAVG = g.Average(r => r.LeadTimeStartEnd),
                            Capa = g.Average(r => r.Capa)
                        };

                // runtime by operation
                models.LFEM_RuntimeByOperation = (from lt in weeklyLeadTime.AsEnumerable()
                                                  group lt by new
                                                  {
                                                      OPERATION_ID = lt.OperationId,
                                                      OPERATION = lt.OperationName,
                                                      DISPLAY_ORDER = lt.DisplayOrder
                                                  } into g
                                                  select new ChartDataItem
                                                  {
                                                      Label_x = g.Key.OPERATION,
                                                      Value_runtime = Math.Round(g.Average(s => s.RunTimeAVG), 1),
                                                      Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                  }).OrderByDescending(x => x.Value_runtime).ToList();

                // wait time by operation
                models.LFEM_WaitTimeByOperation = (from lt in weeklyLeadTime.AsEnumerable()
                                                   group lt by new
                                                   {
                                                       OPERATION_ID = lt.OperationId,
                                                       OPERATION = lt.OperationName,
                                                       DISPLAY_ORDER = lt.DisplayOrder
                                                   } into g
                                                   select new ChartDataItem
                                                   {
                                                       Label_x = g.Key.OPERATION,
                                                       Value_waittime = Math.Round(g.Average(s => s.WaitTimeAVG), 1),
                                                       Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                   }).OrderByDescending(x => x.Value_waittime).ToList();
            }
            else if (month.NullString() != "")
            {
                var mothlyLeadTime =
                        from leadTime in lst.AsEnumerable()
                        group leadTime by new
                        {
                            WORK_MONTH = leadTime.WorkMonth,
                            WORK_YEAR = leadTime.WorkYear,
                            WORK_WEEK = leadTime.WorkWeek,
                            OPERATION_ID = leadTime.OperationID,
                            OPERATION_NAME = leadTime.Operation,
                            DISPLAY_ORDER = leadTime.DisplayOrder
                        } into g
                        select new
                        {
                            WorkYear = g.Key.WORK_YEAR,
                            WorkMonth = g.Key.WORK_MONTH,
                            OperationId = g.Key.OPERATION_ID,
                            OperationName = g.Key.OPERATION_NAME,
                            DisplayOrder = g.Key.DISPLAY_ORDER,
                            WaitTimeAVG = g.Average(r => r.WaitTime),
                            RunTimeAVG = g.Average(r => r.RunTime),
                            LeadTimeAVG = g.Average(r => r.LeadTimeStartEnd),
                            Capa = g.Average(r => r.Capa)
                        };

                // runtime by operation
                models.LFEM_RuntimeByOperation = (from lt in mothlyLeadTime.AsEnumerable()
                                                  group lt by new
                                                  {
                                                      OPERATION_ID = lt.OperationId,
                                                      OPERATION = lt.OperationName,
                                                      DISPLAY_ORDER = lt.DisplayOrder
                                                  } into g
                                                  select new ChartDataItem
                                                  {
                                                      Label_x = g.Key.OPERATION,
                                                      Value_runtime = Math.Round(g.Average(s => s.RunTimeAVG), 1),
                                                      Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                  }).OrderByDescending(x => x.Value_runtime).ToList();

                // wait time by operation
                models.LFEM_WaitTimeByOperation = (from lt in mothlyLeadTime.AsEnumerable()
                                                   group lt by new
                                                   {
                                                       OPERATION_ID = lt.OperationId,
                                                       OPERATION = lt.OperationName,
                                                       DISPLAY_ORDER = lt.DisplayOrder
                                                   } into g
                                                   select new ChartDataItem
                                                   {
                                                       Label_x = g.Key.OPERATION,
                                                       Value_waittime = Math.Round(g.Average(s => s.WaitTimeAVG), 1),
                                                       Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                   }).OrderByDescending(x => x.Value_waittime).ToList();
            }
            else
            {
                var yearlyLeadTime =
                       from leadTime in lst.AsEnumerable()
                       group leadTime by new
                       {
                           WORK_YEAR = leadTime.WorkYear,
                           OPERATION_ID = leadTime.OperationID,
                           OPERATION_NAME = leadTime.Operation,
                           DISPLAY_ORDER = leadTime.DisplayOrder
                       } into g
                       select new
                       {
                           WorkYear = g.Key.WORK_YEAR,
                           OperationId = g.Key.OPERATION_ID,
                           OperationName = g.Key.OPERATION_NAME,
                           DisplayOrder = g.Key.DISPLAY_ORDER,
                           WaitTimeAVG = g.Average(r => r.WaitTime),
                           RunTimeAVG = g.Average(r => r.RunTime),
                           LeadTimeAVG = g.Average(r => r.LeadTimeStartEnd),
                           Capa = g.Average(r => r.Capa)
                       };

                // runtime by operation
                models.LFEM_RuntimeByOperation = (from lt in yearlyLeadTime.AsEnumerable()
                                                  group lt by new
                                                  {
                                                      OPERATION_ID = lt.OperationId,
                                                      OPERATION = lt.OperationName,
                                                      DISPLAY_ORDER = lt.DisplayOrder
                                                  } into g
                                                  select new ChartDataItem
                                                  {
                                                      Label_x = g.Key.OPERATION,
                                                      Value_runtime = Math.Round(g.Average(s => s.RunTimeAVG), 1),
                                                      Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                  }).OrderByDescending(x => x.Value_runtime).ToList();

                // wait time by operation
                models.LFEM_WaitTimeByOperation = (from lt in yearlyLeadTime.AsEnumerable()
                                                   group lt by new
                                                   {
                                                       OPERATION_ID = lt.OperationId,
                                                       OPERATION = lt.OperationName,
                                                       DISPLAY_ORDER = lt.DisplayOrder
                                                   } into g
                                                   select new ChartDataItem
                                                   {
                                                       Label_x = g.Key.OPERATION,
                                                       Value_waittime = Math.Round(g.Average(s => s.WaitTimeAVG), 1),
                                                       Value_target = Math.Round(g.Average(s => s.Capa), 1)
                                                   }).OrderByDescending(x => x.Value_waittime).ToList();
            }

            models.Operation1.AddRange(models.LFEM_RuntimeByOperation.Select(x => x.Label_x));
            models.Operation2.AddRange(models.LFEM_WaitTimeByOperation.Select(x => x.Label_x));

            return models;
        }
        #endregion
    }
}
