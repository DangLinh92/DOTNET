using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Extensions
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
        public static int CompareDateTime(this string date1,string date2)
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
        public static bool InRangeDateTime(this string date1,string date2,string date3)
        {
            if(date2.CompareTo(date1) <= 0 && date3.CompareTo(date1) >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
