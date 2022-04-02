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
            paretoDataDefectName = new List<TotalVOCSiteModel>();
        }
        public List<VOC_MSTViewModel> vOC_MSTViews { get; set; }
        public List<VOC_DefectTypeViewModel> vOC_DefectTypeViews { get; set; }
        public TotalVOCSiteModel totalVOCSitesView { get; set; }
        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeLsts { get; set; }

        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeFinishedLsts { get; set; }

        public List<TotalVOCSiteModel> paretoDataDefectName { get; set; } // ve bieu do pareto cho defect

        public TotalVOC_ParetoView TotalVOC_ParetoView { get; set; }
    }

    public class TotalVOC_ParetoView
    {
        public TotalVOC_ParetoView()
        {
            paretoDataDefectName = new List<TotalVOCSiteModel>();
        }

        public TotalVOCSiteModel totalVOCSitesView { get; set; }
        public List<TotalVOCSiteModel> paretoDataDefectName { get; set; }
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


    public class VOCPPM_Ex
    {
        public VOCPPM_Ex()
        {
            vOCPPM_Customers = new List<VOCPPM_Customer>();
        }

        public string Module { get; set; } // csp/ lfem
        public string Year { get; set; }
        public List<VOCPPM_Customer> vOCPPM_Customers { get; set; }
    }

    public class VOCPPM_Customer
    {
        public VOCPPM_Customer()
        {
            vocPPMModels = new List<VocPPMViewModel>();
        }
        public string Customer { get; set; }
        public double ToTal_Input { get; set; }
        public double ToTal_Defect { get; set; }
        public double ToTal_PPM { get; set; }
        public double ToTal_PPM_Target { get; set; }
        public List<VocPPMViewModel> vocPPMModels { get; set; }
    }
}
