using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Utilities.Common
{
    public static class EachDay
    {
        public static IEnumerable<DateTime> EachDays(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        ///  tính thâm niên, tính đến m1 của tháng tính lương
        /// </summary>
        /// <param name="startDate">thang tính lương</param>
        /// <param name="endDate">ngày vào</param>
        /// <returns></returns>
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;

            if (endDate.AddMonths(monthsApart).ToString("yyyy-MM-dd").CompareTo(startDate.ToString("yyyy-MM") + "-01") > 0)
            {
                return Math.Abs(monthsApart) - 1;
            }

            return Math.Abs(monthsApart);
        }
    }

}
