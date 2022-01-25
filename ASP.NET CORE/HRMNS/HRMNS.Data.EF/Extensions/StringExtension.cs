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
    }
}
