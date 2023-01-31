﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Utilities.Common
{
    public static class STD_DEV
    {
        public static double StdDev(this IEnumerable<double> values,bool as_sample)
        {
            // Get the mean.
            double mean = values.Sum() / values.Count();

            // Get the sum of the squares of the differences
            // between the values and the mean.
            var squares_query =
                from double value in values
                select (value - mean) * (value - mean);

            double sum_of_squares = squares_query.Sum();

            if (as_sample)
            {
                return Math.Sqrt(sum_of_squares / (values.Count() - 1));
            }
            else
            {
                return Math.Sqrt(sum_of_squares / values.Count());
            }
        }
    }
}