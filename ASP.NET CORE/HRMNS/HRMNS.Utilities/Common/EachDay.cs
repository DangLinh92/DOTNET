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

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

    }

}
