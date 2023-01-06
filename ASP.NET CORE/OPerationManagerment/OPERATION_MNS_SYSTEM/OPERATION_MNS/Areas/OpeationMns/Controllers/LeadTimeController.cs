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
                                Value_target = target
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
                            Value_target = target
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
                                Value_target = target
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
                            Value_target = target
                        });
                    }
                }
            }
            return chartDatas;
        }
    }
}
