using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class LeadTimeViewModel
    {
        public string WorkDate { get; set; }
        public string WorkWeek { get; set; }
        public string WorkMonth { get; set; }
        public string WorkYear { get; set; }
        public double HoldTime { get; set; }
        public double WaitTime { get; set; }
        public double RunTime { get; set; }
        public double LeadTime { get; set; }
        public double LeadTimeMax { get; set; }
        public string WLP { get; set; }
        public string Ox { get; set; }
        public double Target { get; set; }
    }

    public class LeadTimeModel
    {
        public LeadTimeModel(string year)
        {
            WLP_LeadTimeByYear = new List<ChartDataItem>();
            WLP1_LeadTimeByMonth = new List<ChartDataItem>();
            WLP1_LeadTimeByWeek = new List<ChartDataItem>();
            WLP2_LeadTimeByMonth = new List<ChartDataItem>();
            WLP2_LeadTimeByWeek = new List<ChartDataItem>();
            WLP_LeadTimeByMonth = new List<ChartDataItem>();
            WLP_LeadTimeByWeek = new List<ChartDataItem>();
            WLP1_LeadTimeByDay = new List<ChartDataItem>();
            WLP2_LeadTimeByDay = new List<ChartDataItem>();

            Weeks = new List<string>();
            Months = new List<string>();
            Weeks_Lable = new List<string>();
            Days1 = new List<string>();
            Days2 = new List<string>();
            Year = year;
        }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Week { get; set; }
        public string Day { get; set; }
        public string Ox { get; set; }

        public List<ChartDataItem> WLP_LeadTimeByYear;
        public List<ChartDataItem> WLP1_LeadTimeByMonth;
        public List<ChartDataItem> WLP1_LeadTimeByWeek;
        public List<ChartDataItem> WLP2_LeadTimeByMonth;
        public List<ChartDataItem> WLP2_LeadTimeByWeek;
        public List<ChartDataItem> WLP_LeadTimeByMonth;
        public List<ChartDataItem> WLP_LeadTimeByWeek;

        public List<ChartDataItem> WLP1_LeadTimeByDay;
        public List<ChartDataItem> WLP2_LeadTimeByDay;

        public List<string> Weeks;
        public List<string> Weeks_Lable;
        public List<string> Months;
        public List<string> Days1;
        public List<string> Days2;

        public List<string> GetWeeks()
        {
            List<string> result = new List<string>();
            string beginYear = Year + "-01-01";
            string endYear = DateTime.Parse(beginYear).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear = DateTime.Parse(endYear).GetWeekOfYear();

            if (Year == DateTime.Now.Year.ToString())
            {
                weekOfYear = DateTime.Now.GetWeekOfYear();
            }

            for (int i = 1; i <= weekOfYear; i++)
            {
                result.Add(i + "");
            }
            return result;
        }

        public List<string> GetWeeksByMonth(string year,string month)
        {
            if (string.IsNullOrEmpty(month))
                return GetWeeks();

            List<string> result = new List<string>();
            string beginMonth = year + "-" + month + "-01";
            string endMonth = DateTime.Parse(beginMonth).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear;
            foreach (var day in EachDay.EachDays(DateTime.Parse(beginMonth),DateTime.Parse(endMonth)))
            {
                weekOfYear = day.GetWeekOfYear();
                if (!result.Contains(weekOfYear + "") && weekOfYear > 0)
                {
                    result.Add(weekOfYear + "");
                }
            }
            result.Sort((a, b) => int.Parse(a).CompareTo(int.Parse(b)));
            return result;
        }
    }

    public class ChartDataItem
    {
        public string Label_x {get; set; }
        public double Value_runtime { get; set; }
        public double Value_waittime { get; set; }
        public double Value_target { get; set; }
    }
}
