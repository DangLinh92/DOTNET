using OPERATION_MNS.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace OPERATION_MNS.Data.EF.Extensions
{
    public static class StringExtension
    {
        public static string NullString(this object result)
        {
            if (result == null)
                return string.Empty;
            else
                return result.ToString().Trim();
        }
        public static string IfNullIsZero(this object result)
        {
            if (result == null || result.NullString() == "-")
                return "0";
            else
                return result.ToString().Trim() == "" ? "0" : result.ToString().Trim();
        }

        public static string TimeHHMM(this string result)
        {
            TimeSpan time;
            if (TimeSpan.TryParse(result, out time))
            {
                return time.ToString(@"hh\:mm");
            }
            else
            {
                return "0:00";
            }
        }

        // Dao nguoc chuoi
        public static string Reverse(this string result)
        {
            char[] nameArray = result.ToCharArray();
            Array.Reverse(nameArray);
            string reverse = new string(nameArray);
            return reverse;
        }

        public static string ToYYYYMMDD(this string obj)
        {
            if (DateTime.TryParse(obj, out var date))
            {
                return date.ToString("yyyy-MM-dd");
            }
            else
            {
                string date1 = obj.Replace("/", "-");
                if (DateTime.TryParse(date1, out var date2))
                {
                    return date2.ToString("yyyy-MM-dd");
                }

                return "";
            }
        }

        /// <summary>
        /// so sanh 2 date time string 
        /// </summary>
        /// <param name="date1">yyyy-MM-dd</param>
        /// <param name="date2">yyyy-MM-dd</param>
        /// <returns>
        /// -1 : date1 < date2
        /// 0 : date1 = date2
        /// 1 : date1 > date2
        /// </returns>
        public static int CompareDateTime(this string date1, string date2)
        {
            return date1.CompareTo(date2);
        }

        /// <summary>
        /// Kiểm tra date1 có năm trong date2 và date3 không
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="date3"></param>
        /// <returns></returns>
        public static bool InRangeDateTime(this string date1, string date2, string date3)
        {
            if (date2.CompareTo(date1) <= 0 && date3.CompareTo(date1) >= 0)
            {
                return true;
            }

            return false;
        }

        public static int GetWeekOfYear(this DateTime date)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var formatRules = CultureInfo.CurrentCulture.DateTimeFormat;
            int week = calendar.GetWeekOfYear(date, formatRules.CalendarWeekRule, formatRules.FirstDayOfWeek) - 1;
            return week;
        }

        public static List<string> GetWeeks(string year)
        {
            List<string> result = new List<string>();
            string beginYear = year + "-01-01";
            string endYear = DateTime.Parse(beginYear).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear = DateTime.Parse(endYear).GetWeekOfYear() + 1;

            if (year == DateTime.Now.Year.ToString())
            {
                weekOfYear = DateTime.Now.GetWeekOfYear() + 1;
            }

            for (int i = 1; i <= weekOfYear; i++)
            {
                result.Add(i + "");
            }
            return result;
        }

        public static List<string> GetWeeksByMonth(string year, string month)
        {
            if (string.IsNullOrEmpty(month))
                return GetWeeks(year);

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
}
