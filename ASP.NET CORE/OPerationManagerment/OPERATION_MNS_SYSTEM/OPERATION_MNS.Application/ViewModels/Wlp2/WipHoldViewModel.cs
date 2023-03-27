using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class WipHoldViewModel
    {
        public string SapCode { get; set; }
        public int HoldWafer { get; set; }
        public int HoldReel { get; set; }
        public int HoldOQC { get; set; }
        public int SmtReturn { get; set; }
    }
}
