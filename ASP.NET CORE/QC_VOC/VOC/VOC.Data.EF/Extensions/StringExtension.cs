﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Extensions
{
    public static class StringExtension
    {
        public static string NullOtherString(this object result,string result2)
        {
            if (result == null)
                return result2;
            else
                return result.ToString().Trim();
        }

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
                return "";
            }
        }
    }
}