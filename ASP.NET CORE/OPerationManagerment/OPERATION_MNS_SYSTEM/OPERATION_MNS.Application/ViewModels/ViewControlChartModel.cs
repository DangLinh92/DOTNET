using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class ViewControlChartModel
    {
        public string CHART_X { get; set; }
        public string DATE { get; set; }
        public string MATERIAL_ID { get; set; }
        public string LOT_ID { get; set; }
        public string CASSETTE_ID { get; set; }
        public string MAIN_OPERATION { get; set; }
        public string MAIN_OPERATION_ID { get; set; }
        public string MAIN_EQUIPMENT_ID { get; set; }
        public string MAIN_EQUIPMENT_NAME { get; set; }
        public string MAIN_CHARACTER { get; set; }
        public string MAIN_UNIT { get; set; }
        public double MAIN_TARGET_USL { get; set; }
        public double MAIN_FIXED_UCL { get; set; }
        public double MAIN_TARGET { get; set; }
        public double MAIN_FIXED_LCL { get; set; }
        public double MAIN_TARGET_LSL { get; set; }
        public double MAIN_TARGET_UCL { get; set; }
        public double MAIN_TARGET_LCL { get; set; }
        public double MAIN_VALUE_COUNT { get; set; }
        public double MAIN_VALUE1 { get; set; }
        public double MAIN_VALUE2 { get; set; }
        public double MAIN_VALUE3 { get; set; }
        public double MAIN_VALUE4 { get; set; }
        public double MAIN_VALUE5 { get; set; }
        public double MAIN_VALUE6 { get; set; }
        public double MAIN_VALUE7 { get; set; }
        public double MAIN_VALUE8 { get; set; }
        public double MAIN_VALUE9 { get; set; }
        public double MAIN_VALUE10 { get; set; }
        public double MAIN_VALUE11 { get; set; }
        public double MAIN_VALUE12 { get; set; }
        public double MAIN_VALUE13 { get; set; }
        public double MAIN_VALUE14 { get; set; }
        public double MAIN_VALUE15 { get; set; }
        public double MAIN_VALUE16 { get; set; }
        public double MAIN_VALUE17 { get; set; }
        public double MAIN_VALUE18 { get; set; }
        public double MAIN_VALUE19 { get; set; }
        public double MAIN_VALUE20 { get; set; }
        public double MAIN_VALUE21 { get; set; }
        public double MAIN_VALUE22 { get; set; }
        public double MAIN_VALUE23 { get; set; }
        public double MAIN_VALUE24 { get; set; }
        public double MAIN_VALUE25 { get; set; }
        public double MAIN_VALUE26 { get; set; }
        public double MAIN_VALUE27 { get; set; }
        public double MAIN_VALUE28 { get; set; }
        public double MAIN_VALUE29 { get; set; }
        public double MAIN_VALUE30 { get; set; }
        public double MAIN_MAX_VALUE { get; set; }
        public double MAIN_MIN_VALUE { get; set; }
        public double MAIN_AVG_VALUE { get; set; }
        public double MAIN_RANGE { get; set; }
        public string MAIN_JUDGE_FLAG { get; set; }

        public double LWL { get; set; }
        public double UWL { get; set; }

        public double Thicknet { get; set; }
    }

    public class ViewControlChartDataModel
    {
        public ViewControlChartDataModel()
        {
            lstData = new List<ViewControlChartModel>();
            lstDataErr = new List<ViewControlChartModel>();
            lstMaterialId = new List<string>();
        }

        //public string Year { get; set; }
        //public string Month { get; set; }
        //public string Day { get; set; }

        public string FromDay { get; set; }
        public string ToDay { get; set; }

        public string Operation { get; set; }
        public string MatertialID { get; set; }

        public double STDEV { get; set; }
        public double CPK_Thickness { get; set; }
        public double CPK_bst { get; set; }

        public double STDEV150 { get; set; }
        public double CPK_Thickness150 { get; set; }
        public double CPK_bst150 { get; set; }

        public double STDEV200 { get; set; }
        public double CPK_Thickness200 { get; set; }
        public double CPK_bst200 { get; set; }

        public List<ViewControlChartModel> lstData { get; set; }
        public List<ViewControlChartModel> lstDataErr { get; set; }
        public List<string> lstMaterialId { get; set; }
    }
}
