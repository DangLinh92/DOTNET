using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocInfomationsModel
    {
        public VocInfomationsModel()
        {
            vOC_MSTViews = new List<VOC_MSTViewModel>();
            vOC_DefectTypeViews = new List<VOC_DefectTypeViewModel>();
            totalVOCSitesView = new List<TotalVOCSiteModel>();
            vOCSiteModelByTimeLsts = new List<VOCSiteModelByTimeLst>();
        }
        public List<VOC_MSTViewModel> vOC_MSTViews { get; set; }
        public List<VOC_DefectTypeViewModel> vOC_DefectTypeViews { get; set; }
        public List<TotalVOCSiteModel> totalVOCSitesView { get; set; }
        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeLsts { get; set; }
    }

    public class TotalVOCSiteModel
    {
        public string Division { get; set; }
        public string PartsClassification { get; set; }
        public float Qty { get; set; } 
        public int Year { get; set; }
    }

    public class VOCSiteModelByTimeLst
    {
        public VOCSiteModelByTimeLst()
        {
            vOCSiteModelByTimes = new List<VOCSiteModelByTime>();
        }
        public string DivisionLst { get; set; }
        List<VOCSiteModelByTime> vOCSiteModelByTimes { get; set; }
    }

    public class VOCSiteModelByTime
    {
        public string Division { get; set; }
        public string PartsClassification { get; set; }
        public float Qty { get; set; }
        public string Time { get; set; }
    }
}
