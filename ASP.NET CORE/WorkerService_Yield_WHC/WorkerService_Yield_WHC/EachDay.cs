using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService_Yield_WHC
{
    public static class EachDay
    {
        public static IEnumerable<DateTime> EachDays(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<DateTime> EachDayForWeeks(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(6))
                yield return day;
        }
    }
}
