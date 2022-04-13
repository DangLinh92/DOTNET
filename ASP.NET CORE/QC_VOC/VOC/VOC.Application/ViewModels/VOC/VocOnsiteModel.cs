using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocOnsiteList
    {
        public VocOnsiteList()
        {
            Parts = new List<string>();
            lstOnsite = new List<List<VocOnsiteModel>>();
        }

        public string Customer { get; set; }
        public List<string> Parts { get; set; }
        public List<List<VocOnsiteModel>> lstOnsite { get; set; }
    }

    public class VocOnsiteModel
    {
        public string Customer { get; set; } // sev, sevt
        public string Part { get; set; } // csp, lfem
        public string Time { get; set; } // month , week
        public int OK { get; set; }
        public int NG { get; set; }
        public int NM { get; set; }//Can't measure
    }
}
