using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService_Yield_WHC.Models
{
    public class OutPutTrendDailyWlp
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }

        public double N_WLP_CumYield { get; set; }
        public double N_WLP_CumYield_Goals { get; set; }

        public double TC_WLP_CumYield { get; set; }
        public double TC_WLP_CumYield_Goals { get; set; }

        public double BDMP_CumYield { get; set; }
        public double BDMP_CumYield_Goals { get; set; }

        public string WorkDate { get; set; }
        public string DateModified { get; set; }

        public double Total_CumYield { get; set; }
        public double Total_CumYield_Goals { get; set; }
    }
}
