﻿using System;
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
            vOCPPMs = new List<VOCPPM_Ex>();
            vOC_CHART = new VOC_CHART();
        }

        public List<VOC_MSTViewModel> vOC_MSTViews { get; set; }
        public List<VOC_DefectTypeViewModel> vOC_DefectTypeViews { get; set; }
        public TotalVOCSiteModel totalVOCSitesView { get; set; }
        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeLsts { get; set; }
        public List<TotalVOCSiteModel> paretoDataDefectName { get; set; } // ve bieu do pareto cho defect

        public PPMDataChartAll VocPPMView { get; set; }
        public List<VOCPPM_Ex> vOCPPMs { get; set; }
        public VOC_CHART vOC_CHART { get; set; }
    }

    public class VocProgessInfo
    {
        public VocProgessInfo()
        {
            lstVocProgress = new List<VOC_MSTViewModel>();
            lstVocComplete = new List<VOC_MSTViewModel>();
        }

        public int ReceiveCount { get; set; }
        public int CloseCount { get; set; }
        public int ProgressCount { get; set; }

        public List<VOC_MSTViewModel> lstVocProgress { get; set; }
        public List<VOC_MSTViewModel> lstVocComplete { get; set; }
    }

    public class VOC_CHART
    {
        public VOC_CHART()
        {
            vOCSiteModelByTimeLsts = new List<VOCSiteModelByTimeLst>();
            paretoDataDefectName = new List<TotalVOCSiteModel>();
        }

        public List<VOCSiteModelByTimeLst> vOCSiteModelByTimeLsts { get; set; }

        public TotalVOCSiteModel totalVOCSitesView { get; set; }

        public List<TotalVOCSiteModel> paretoDataDefectName { get; set; }

        public VocProgessInfo vocProgessInfo { get; set; }

        public int Year { get; set; }
        public string Customer { get; set; }
        public string Side { get; set; }
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
            pPMByMonths = new List<PPMByMonth>();
        }
        public string Customer { get; set; }
        public double ToTal_Input { get; set; }
        public double ToTal_Defect { get; set; }
        public double ToTal_PPM { get; set; }
        public double ToTal_PPM_Target { get; set; }
        public List<VocPPMViewModel> vocPPMModels { get; set; }
        public List<PPMByMonth> pPMByMonths { get; set; }
    }

    public class PPMByMonth
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public double PPM { get; set; }

        public double Target { get; set; }
    }

    public class PPMDataChart
    {
        public PPMDataChart()
        {
            lstData = new List<double>();
            dataTargetAll = new List<double>();
        }

        public string Module { get; set; } // csp/ lfem
        public string Customer { get; set; }
        public int Year { get; set; }

        public List<double> lstData { get; set; }

        public List<double> dataTargetAll { get; set; }
    }

    public class PPMDataChartAll
    {
        public PPMDataChartAll()
        {
            dataChartsItem = new List<List<PPMDataChart>>();
            dataChartsAll = new List<PPMDataChart>();

        }
        public int Year { get; set; }

        public string Module { get; set; }

        public List<List<PPMDataChart>> dataChartsItem { get; set; }

        public List<PPMDataChart> dataChartsAll { get; set; }
    }
}
