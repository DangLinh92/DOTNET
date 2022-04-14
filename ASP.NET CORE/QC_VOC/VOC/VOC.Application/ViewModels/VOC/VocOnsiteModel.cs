using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocOnsiteList
    {
        public VocOnsiteList()
        {
            vocOnsiteSumWeeks = new List<VocOnsiteSumWeek>();
            vocOnsiteModels = new List<VocOnsiteModel>();
        }

        public int Year { get; set; }
        public string Customer { get; set; }
        public string Part { get; set; }

        public List<VocOnsiteSumWeek> vocOnsiteSumWeeks { get; set; }
        public List<VocOnsiteModel> vocOnsiteModels { get; set; }
    }

    public class VocOnsiteSumWeek
    {
        public string Time { get; set; }
        public string Customer { get; set; }
        public int QTY { get; set; }
        public int OK { get; set; }
        public int NG { get; set; }
    }

    public class VocOnsiteModel
    {
        public VocOnsiteModel()
        {
            lstVocOnsite = new List<VocOnsiteViewModel>();
        }

        public string Customer { get; set; }
        public string Date { get; set; }
        public string Week { get; set; }
        public int Month { get; set; }
        public string Part { get; set; }
        public int Qty { get; set; }

        public int OK { get; set; }
        public int NG { get; set; }

        public string Customer_Code { get; set; }
        public string Wisol_Model { get; set; }
        public List<VocOnsiteViewModel> lstVocOnsite { get; set; }
    }
}
