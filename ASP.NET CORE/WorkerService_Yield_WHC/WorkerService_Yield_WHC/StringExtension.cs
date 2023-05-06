﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace WorkerService_Yield_WHC
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
            if (result == null)
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

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
