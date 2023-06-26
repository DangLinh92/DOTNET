using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Sameple
{
    public class LeadtimeViewModel
    {
        public string LotNo { get; set; }
        public string KeHoachIn { get; set; }
        public string ThucTeIn { get; set; }
        public string KeHoachOut { get; set; }
        public string ThucTeOut { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public string PLCode { get; set; }
        public string ModelRutGon { get; set; }
        public string Model { get; set; }
        public int SoTam { get; set; }
        public int Code_R { get; set; }
        public int Code_P { get; set; }
        public int Code_Z { get; set; }
        public int Code_H { get; set; }
        public int Code_M { get; set; }
        public float LTCode_PRZ { get; set; }
        public string NguoiChiuTrachNhiem { get; set; }
        public int LeadTimePlan { get; set; }
        public int LeadTimeActual { get; set; }
        public int Gap { get; set; }
        public string LyDoDelay { get; set; }
        public float DonViTinh { get; set; }
        public string PLHang { get; set; }
        public float RatioLT { get; set; }
    }

    public class LeadTimeSampleModel
    {
        public LeadTimeSampleModel(string year)
        {
            Sample_LeadTimeByYear = new List<ChartDataSampleItem>();
            Sample_LeadTimeByMonth = new List<ChartDataSampleItem>();
            Sample_LeadTimeByWeek = new List<ChartDataSampleItem>();
            Sample_WaferByMonth = new List<ChartDataSampleItem>();
            Sample_WaferByWeek = new List<ChartDataSampleItem>();
            SampleDetail = new List<LeadtimeViewModel>();
            SampleChiuTN = new List<ChartDataSampleItem>();
            Gaps = new List<int>();
            SampleGapLeadtime = new List<ChartDataSampleItem>();

            Weeks = new List<string>();
            Months = new List<string>();
            Weeks_Lable = new List<string>();
            Codes = new List<string>();
            NguoiPhuTrachs = new List<string>();
            Year = year;
        }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Week { get; set; }

        public string Code { get; set; }
        public string NguoiPhuTrach { get; set; }
        public int Gap { get; set; }
        public List<int> Gaps { get; set; }

        public float Total_Code_P { get; set; }
        public float Total_Code_R { get; set; }
        public float Total_Code_Z { get; set; }
        public float Total_Code_H { get; set; }
        public float Total_Code_M { get; set; }

        public List<ChartDataSampleItem> Sample_LeadTimeByYear;
        public List<ChartDataSampleItem> Sample_LeadTimeByMonth;
        public List<ChartDataSampleItem> Sample_LeadTimeByWeek;
        public List<ChartDataSampleItem> Sample_WaferByMonth;
        public List<ChartDataSampleItem> Sample_WaferByWeek;
        public List<ChartDataSampleItem> SampleChiuTN;
        public List<LeadtimeViewModel> SampleDetail;
        public List<ChartDataSampleItem> SampleGapLeadtime;

        public List<string> Weeks;
        public List<string> Weeks_Lable;
        public List<string> Months;
        public List<string> Codes { get; set; }
        public List<string> NguoiPhuTrachs { get; set; }

        public List<string> GetWeeks()
        {
            List<string> result = new List<string>();
            string beginYear = Year + "-01-01";
            string endYear = DateTime.Parse(beginYear).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear = DateTime.Parse(endYear).GetWeekOfYear() + 1;

            if (Year == DateTime.Now.Year.ToString())
            {
                weekOfYear = DateTime.Now.GetWeekOfYear() + 1;
            }

            for (int i = 1; i <= weekOfYear; i++)
            {
                result.Add(i + "");
            }
            return result;
        }

        public List<string> GetWeeksByMonth(string year, string month)
        {
            if (string.IsNullOrEmpty(month))
                return GetWeeks();

            List<string> result = new List<string>();
            string beginMonth = year + "-" + month + "-01";
            string endMonth = DateTime.Parse(beginMonth).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear;
            foreach (var day in EachDay.EachDays(DateTime.Parse(beginMonth), DateTime.Parse(endMonth)))
            {
                weekOfYear = day.GetWeekOfYear() + 1;
                if (!result.Contains(weekOfYear + "") && weekOfYear > 0)
                {
                    result.Add(weekOfYear + "");
                }
            }
            result.Sort((a, b) => int.Parse(a).CompareTo(int.Parse(b)));
            return result;
        }
    }

    public class ChartDataSampleItem
    {
        public string Label_x { get; set; }
        public double Code_P { get; set; }
        public double Code_H { get; set; }
        public double Code_R { get; set; }
        public double Code_Z { get; set; }
        public double Code_M { get; set; }
        public double Target_P { get; set; }
        public double Target_R { get; set; }
        public double Total { get; set; }

        public double Value { get; set; }
        public string Legend { get; set; }
        public double Rate { get; set; }

    }
}
