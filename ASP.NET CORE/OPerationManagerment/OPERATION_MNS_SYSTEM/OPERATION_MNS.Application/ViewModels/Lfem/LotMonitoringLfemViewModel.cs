using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Lfem
{
    public class LotMonitoringLfemViewModel
    {
        public LotMonitoringLfemViewModel()
        {
            TotalProduct = new TotalOperation();
            FinishProduct = new TotalOperation();
            SmtData = new List<StayDayLfemItem>();
            AssyLine = new List<StayDayLfemItem>();
            Test = new List<StayDayLfemItem>();
            FA = new List<StayDayLfemItem>();
            VI = new List<StayDayLfemItem>();
            OQC = new List<StayDayLfemItem>();
        }
        public TotalOperation TotalProduct { get; set; }
        public TotalOperation FinishProduct { get; set; }

        public List<StayDayLfemItem> SmtData { get; set; }
        public List<StayDayLfemItem> AssyLine { get; set; }
        public List<StayDayLfemItem> Test { get; set; }
        public List<StayDayLfemItem> FA { get; set; }
        public List<StayDayLfemItem> VI { get; set; }
        public List<StayDayLfemItem> OQC { get; set; }
    }

    public class TotalOperation
    {
        public TotalOperation()
        {
            Operation = new List<OperationItem>();
            Models = new List<string>();
            StayDays = new List<int>();
        }

        public List<OperationItem> Operation { get; set; }
        public List<string> Models { get; set; }
        public List<int> StayDays { get; set; }
        public decimal LotCount { get; set; }
        public decimal ChipQTYSum { get; set; }
        public double StayDayAvg { get; set; }

        public string ChipQTYSumStr { get; set; }

        public string OperationV { get; set; }
        public decimal StayDayV { get; set; }
        public string ModelV { get; set; }

        public List<StayDayLfemItem> StayDayLfemItems { get; set; }
        public List<StayDayLfemItem> StayDayLfemItems1 { get; set; }
        public List<StayDayLfemItem> StayDayLfemItems2 { get; set; }
    }

    public class OperationItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SMT_AssyLine_Test_Model
    {
        public List<StayDayLfemItem> SmtData { get; set; }
        public List<StayDayLfemItem> AssyLine { get; set; }
        public List<StayDayLfemItem> Test { get; set; }
    }

    public class StayDayLfemItem
    {
        public decimal LotIDCount { get; set; }
        public string OperationName { get; set;}
        public double StayDay { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public decimal Qty { get; set;}
        public string Status { get; set; }
        public string LotID { get; set; }
    }

    public class ViewWIpLfemData
    {
        public string MaterialCategory { get; set; }
        public string MaterialGroup { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string MaterialName { get; set; }
        public string LotID { get; set; }
        public string ProductOrder { get; set; }
        public string FAID { get; set; }
        public string AssyLotID { get; set; }
        public string MatVendor { get; set;}
        public string LotCategory { get; set; }
        public string LotType { get; set; }
        public string Date { get; set; }
        public string Operation { get; set; }
        public string OperationName { get; set; }
        public int StayDay { get; set; }
        public decimal ChipQty { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public string Equipment { get; set; }
        public string EquipmentName { get; set; }
        public string Worker { get; set; }
        public string Hold { get; set; }
        public string Rework { get; set; }
        public string ReworkCode { get; set; }
        public string Site { get; set; }
        public string Route { get; set; }
        public string RouteName { get; set; }
        public string MarkingNo { get; set; }
        public string TotalInspection { get; set; }
        public string IN_EX { get; set; }
        public string VIEnd { get; set; }
        public string ShipVendor { get; set; }
        public string Comment { get; set; }
        public string _Day { get; set; }
    }
}
