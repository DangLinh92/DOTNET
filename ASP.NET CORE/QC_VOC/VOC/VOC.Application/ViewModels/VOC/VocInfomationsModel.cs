using System;
using System.Collections.Generic;
using System.Text;
using VOC.Data.EF.Extensions;

namespace VOC.Application.ViewModels.VOC
{
    public class VocInfomationsModel
    {
        public VocInfomationsModel()
        {
            vOC_MSTViews = new List<VOC_MSTViewModel>();
            vOC_DefectTypeViews = new List<VOC_DefectTypeViewModel>();
            vOCSiteModelByTimeLsts = new List<VOCSiteModelByTimeLst>();
        }
        public List<VOC_MSTViewModel> vOC_MSTViews { get; set; }
        public List<VOC_DefectTypeViewModel> vOC_DefectTypeViews { get; set; }
        public TotalVOCSiteModel totalVOCSitesView { get; set; }
        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeLsts { get; set; }
    }

    public class TotalVOCSiteModel
    {
        public TotalVOCSiteModel()
        {
            totalVOCSiteModelItems = new List<TotalVOCSiteModelItem>();
            Divisions = new List<string>();
            PartsClassification = new List<string>();
        }
        public string Year { get; set; }
        private int _total = 0;
        public int Total
        {
            get
            {
                foreach (var item in totalVOCSiteModelItems)
                {
                    _total += int.Parse(item.Qty.IfNullIsZero());
                }
                return _total;
            }
        }
        public List<string> Divisions { get; set; }
        public List<string> PartsClassification { get; set; }
        public List<TotalVOCSiteModelItem> totalVOCSiteModelItems { get; set; }
    }

    public class TotalVOCSiteModelItem
    {
        public string Classification { get; set; }
        public string Qty { get; set; }
        public string Division { get; set; } // week, year
    }

    public class VOCSiteModelByTimeLst
    {
        public VOCSiteModelByTimeLst()
        {
            vOCSiteModelByTimes = new List<VOCSiteModelByTime>();
            PartsClassifications = new List<string>();
            TimeHeader = new List<string>();
        }

        public string DivisionLst { get; set; }
        public List<string> PartsClassifications { get; set; }
        public List<string> TimeHeader { get; set; }
        public List<VOCSiteModelByTime> vOCSiteModelByTimes { get; set; }
    }

    public class VOCSiteModelByTime
    {
        public string Classification { get; set; }
        public string Qty { get; set; }
        public string Time { get; set; } // week, year
    }
}
