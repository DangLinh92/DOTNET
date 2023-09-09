using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Lfem
{
    public class WipLotListLFEMViewModel
    {
        public string Model { get; set; }
        public string Size { get; set; }
        public double DAM_RUN { get; set; }
        public double MOLD_RUN { get; set; }
        public double GRINDING_RUN { get; set; }
        public double MARKING_RUN { get; set; }
        public double DICING_RUN { get; set; }
        public double TEST_RUN { get; set; }
        public double VI_RUN { get; set; }
        public double OQC_RUN { get; set; }

        public double DAM_WAIT { get; set; }
        public double MOLD_WAIT { get; set; }
        public double GRINDING_WAIT { get; set; }
        public double MARKING_WAIT { get; set; }
        public double DICING_WAIT { get; set; }
        public double TEST_WAIT { get; set; }
        public double VI_WAIT { get; set; }
        public double OQC_WAIT { get; set; }

        public double DAM_HOLD { get; set; }
        public double MOLD_HOLD { get; set; }
        public double GRINDING_HOLD { get; set; }
        public double MARKING_HOLD { get; set; }
        public double DICING_HOLD { get; set; }
        public double TEST_HOLD { get; set; }
        public double VI_HOLD { get; set; }
        public double OQC_HOLD { get; set; }
        public double DRY_HOLD { get; set; }

        public string LastUpdate { get; set; }
    }
}
