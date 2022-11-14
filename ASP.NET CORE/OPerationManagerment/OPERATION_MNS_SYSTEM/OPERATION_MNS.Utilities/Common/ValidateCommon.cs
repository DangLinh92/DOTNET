﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Utilities.Common
{
    public static class ValidateCommon
    {
        public static bool DateTimeValid(string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    if (date.Contains("-"))
                    {
                        string year = date.Substring(0, 4);
                        string month = date.Substring(5, 2);
                        string day = date.Substring(8, 2);

                        string d1 = date.Substring(4, 1);
                        string d2 = date.Substring(7, 1);

                        if(d1 == d2 && d1 == "-" && int.TryParse(year,out _) && int.TryParse(month,out _) && int.TryParse(day,out _) && year.Length == 4 && month.Length == 2 && day.Length == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }
    }
}
